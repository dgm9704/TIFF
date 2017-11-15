namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text;
    using Diwen.Tiff.Extensions;
    using Diwen.Tiff.TagValues;
    using System.Net;
    using System.Runtime.Serialization.Formatters.Binary;
    public class TiffFile : Collection<TiffPage>
    {
        internal static ASCIIEncoding Ascii = new ASCIIEncoding();
        internal static Dictionary<TiffDataType, int> ValueLength = new Dictionary<TiffDataType, int> {
            { TiffDataType.Ascii, 1 },
            { TiffDataType.Byte, 1 },
            { TiffDataType.Long, 4 },
            { TiffDataType.Short, 2 },
            { TiffDataType.Rational, 8 }, };

        public TiffFile()
            : base(new List<TiffPage>())
        {
        }

        public static TiffFile Load(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length - 1);
            return Load(bytes);
        }

        public static TiffFile Load(string path)
        {
            return Load(File.ReadAllBytes(path));
        }

        public static TiffFile Load(byte[] data)
        {
            TiffFile file;
            int pos;
            switch (Ascii.GetString(data, 0, 2))
            {
                case "II":
                    pos = (int)BitConverter.ToUInt32(data, 4);
                    file = ReadPages(data, pos);
                    break;
                case "MM":
                    pos = (int)IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(data, 4));
                    file = ReadPagesMM(data, pos);
                    break;
                default:
                    throw new NotSupportedException("Sorry, can't read this file.");
            }
            return file;
        }

        public void Save(string path)
        {
            File.WriteAllBytes(path, this.GetData());
        }

        private byte[] GetData()
        {
            var pageDatas = new List<List<byte>>();
            var fileData = new List<Byte>();
            int filelen = 8;

            for (int i = 0; i < this.Count; i++)
                filelen = GeneratePageData(pageDatas, filelen, i);

            WriteHeader(pageDatas, fileData);
            WritePageDatas(pageDatas, fileData);

            return fileData.ToArray();
        }

        private int GeneratePageData(List<List<byte>> pageDatas, int filelen, int i)
        {
            var pageData = new List<byte>();
            var page = this[i];
            page.Sort();
            int ifdlen = 2 + this[i].Count * 12 + 4;

            var offsetTag = page.Find(t => t.TagType == TagType.StripOffsets || t.TagType == TagType.TileOffsets);
            var countTag = page.Find(t => t.TagType == TagType.StripByteCounts || t.TagType == TagType.TileByteCounts);

            var offsetData = new List<byte>();
            uint offset = (uint)filelen;

            long valueLength = offsetTag.ValueCount * ValueLength[offsetTag.DataType];
            offset = GenerateValueOffset(offsetTag, countTag, offsetData, offset, valueLength);

            filelen = GeneratePageData(filelen, pageData, page);

            filelen = GenerateTagData(filelen, pageData, page);

            pageDatas.Add(pageData);
            filelen += ifdlen;
            return filelen;
        }

        private static int GenerateTagData(int filelen, List<byte> pageData, TiffPage page)
        {
            int valueLength;
            foreach (var tag in page)
            {
                valueLength = tag.Values.Length * ValueLength[tag.DataType];
                if (valueLength > 4)
                {
                    tag.ValueOffset = (uint)(filelen);
                    pageData.AddRange(ValueBytes(tag.Values, tag.DataType));
                    filelen += valueLength;
                }
                else
                {
                    byte[] value = new byte[4];
                    Buffer.BlockCopy(tag.Values, 0, value, 0, valueLength);
                    tag.ValueOffset = BitConverter.ToUInt32(value, 0);
                }
            }
            return filelen;
        }

        private static int GeneratePageData(int filelen, List<byte> pageData, TiffPage page)
        {
            foreach (var sd in page.ImageData)
            {
                pageData.AddRange(sd);
                filelen += sd.Length;
            }
            return filelen;
        }

        private static uint GenerateValueOffset(TiffTag offsetTag, TiffTag byteCountTag, List<byte> offsetData, uint offset, long valueLength)
        {
            if (valueLength > 4)
            {

                for (int o = 0; o < offsetTag.ValueCount; o++)
                {
                    if (offsetTag.DataType == TiffDataType.Short)
                        offsetData.AddRange(BitConverter.GetBytes((ushort)offset));
                    else
                        offsetData.AddRange(BitConverter.GetBytes((uint)offset));

                    offsetTag.Values.SetValue(offset, o);
                    offset += (uint)byteCountTag.Values.GetValue(o);
                }
            }
            else
            {
                offsetTag.Values.SetValue(offset, 0);
            }
            return offset;
        }

        private static void WriteHeader(List<List<byte>> pageDatas, List<byte> fileData)
        {
            fileData.AddRange(new byte[] { 73, 73, 42, 00 });
            fileData.AddRange(BitConverter.GetBytes((uint)(8 + pageDatas[0].Count)));
        }

        private void WritePageDatas(List<List<byte>> pageDatas, List<byte> fileData)
        {
            for (int i = 0; i < this.Count; i++)
            {
                var pd = pageDatas[i];
                fileData.AddRange(pd);
                WriteIfd(fileData, this[i]);

                if (i < this.Count - 1)
                    fileData.AddRange(BitConverter.GetBytes((uint)(fileData.Count + pageDatas[i + 1].Count + 4)));
                else
                    fileData.AddRange(new byte[] { 0, 0, 0, 0 });
            }
        }

        private static void WriteIfd(List<byte> fileData, TiffPage page)
        {
            fileData.AddRange(BitConverter.GetBytes((ushort)page.Count));

            foreach (var tag in page)
            {
                fileData.AddRange(BitConverter.GetBytes((ushort)tag.TagType));
                fileData.AddRange(BitConverter.GetBytes((ushort)tag.DataType));
                fileData.AddRange(BitConverter.GetBytes((uint)tag.ValueCount));
                fileData.AddRange(BitConverter.GetBytes((uint)tag.ValueOffset));
            }
        }

        public static TiffFile ReadPages(byte[] data, int pos)
        {
            var file = new TiffFile();
            TiffPage page;
            do
            {
                page = TiffPage.Read(data, pos);
                page.Number = file.Count + 1;
                file.Add(page);
                pos = (int)page.NextPageAddress;
            } while (pos != 0);
            return file;
        }

        public static TiffFile ReadPagesMM(byte[] data, int pos)
        {
            var file = new TiffFile();
            TiffPage page;
            do
            {
                page = TiffPage.ReadMM(data, pos);
                page.Number = file.Count + 1;
                file.Add(page);
                pos = (int)page.NextPageAddress;
            } while (pos != 0);
            return file;
        }


        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var page in this)
                sb.Append(page.ToString());

            return sb.ToString();
        }

        internal static Array ReadValues(byte[] data, TiffDataType type, int count)
        {
            Array values = null;

            switch (type)
            {
                case TiffDataType.Byte:
                case TiffDataType.Undefined:
                    values = data;
                    break;
                case TiffDataType.SByte:
                    values = new sbyte[data.Length];
                    Buffer.BlockCopy(data, 0, values, 0, count);
                    break;
                case TiffDataType.Ascii:
                    values = TiffFile.Ascii.GetString(data).ToCharArray();
                    break;
                case TiffDataType.Short:
                    values = new ushort[count];
                    Buffer.BlockCopy(data, 0, values, 0, count * 2);
                    break;
                case TiffDataType.SShort:
                    values = new short[count];
                    Buffer.BlockCopy(data, 0, values, 0, count * 2);
                    break;
                case TiffDataType.Long:
                    values = new uint[count];
                    Buffer.BlockCopy(data, 0, values, 0, count * 4);
                    break;
                case TiffDataType.SLong:
                    values = new int[count];
                    Buffer.BlockCopy(data, 0, values, 0, count * 4);
                    break;
                case TiffDataType.Rational:
                    values = new URational32[count];
                    for (int i = 0; i < count; i++)
                        values.SetValue(new URational32(data, i * 4), i);
                    break;
                case TiffDataType.SRational:
                    values = new Rational32[count];
                    for (int i = 0; i < count; i++)
                        values.SetValue(new Rational32(data, i * 4), i);
                    break;
                case TiffDataType.Float:
                    values = new float[count];
                    Buffer.BlockCopy(data, 0, values, 0, count * 4);
                    break;
                case TiffDataType.Double:
                    values = new double[count];
                    Buffer.BlockCopy(data, 0, values, 0, count * 8);
                    break;
                default:
                    break;
            }

            return values;
        }

        internal static byte[] ValueBytes(Array values, TiffDataType type)
        {
            byte[] bytes = null;
            switch (type)
            {
                case TiffDataType.Byte:
                case TiffDataType.Undefined:
                case TiffDataType.SByte:
                    bytes = values as byte[];
                    break;
                case TiffDataType.Ascii:
                    bytes = Ascii.GetBytes((char[])values);
                    break;
                case TiffDataType.Short:
                    bytes = new byte[values.Length * 2];
                    Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
                    break;
                case TiffDataType.SShort:
                    bytes = new byte[values.Length * 2];
                    Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
                    break;
                case TiffDataType.Long:
                    bytes = new byte[values.Length * 4];
                    Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
                    break;
                case TiffDataType.SLong:
                    bytes = new byte[values.Length * 4];
                    Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
                    break;
                case TiffDataType.Rational:
                    var uRationalArray = values as URational32[];
                    bytes = new byte[values.Length * 8];
                    for (int i = 0; i < values.Length; i++)
                        Buffer.BlockCopy(BitConverterExtensions.GetBytes(uRationalArray[i]), 0, bytes, i * 8, 8);
                    break;
                case TiffDataType.SRational:
                    var rationalArray = values as Rational32[];
                    bytes = new byte[values.Length * 8];
                    for (int i = 0; i < values.Length; i++)
                        Buffer.BlockCopy(BitConverterExtensions.GetBytes(rationalArray[i]), 0, bytes, i * 8, 8);
                    break;
                case TiffDataType.Float:
                    bytes = new byte[values.Length * 4];
                    Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
                    break;
                case TiffDataType.Double:
                    bytes = new byte[values.Length * 8];
                    Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
                    break;
                default:
                    break;
            }
            return bytes;
        }

        internal static byte[] SwitchEndian(byte[] data, int typeLength)
        {
            byte[] temp = new byte[typeLength];
            byte[] switched = new byte[data.Length];
            for (int i = 0; i < data.Length; i += typeLength)
            {
                Buffer.BlockCopy(data, i, temp, 0, typeLength);
                Array.Reverse(temp);
                Buffer.BlockCopy(temp, 0, switched, i, typeLength);
            }

            return switched;
        }

        public new void Add(TiffPage page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            Items.Add(page.Copy());
        }

        public void AddRange(IEnumerable<TiffPage> pages)
        {
            if (pages == null)
                throw new ArgumentNullException("pages");

            foreach (var page in pages)
                Add(page);
        }

        public TiffFile Copy()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (TiffFile)formatter.Deserialize(stream);
            }
        }

    }
}