namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    /// <summary>
    /// Represents a TIF file
    /// </summary>
    [Serializable()]
    public class Tif : PageCollection
    {

        internal static ASCIIEncoding Ascii = new ASCIIEncoding();

        private static Dictionary<FieldType, ValueBytesMethod> valueByteMethods =
            new Dictionary<FieldType, ValueBytesMethod> 
            { 
                { FieldType.Byte, GetBytes },
                { FieldType.SByte, GetBytes },
                { FieldType.Undefined, GetBytes },
                { FieldType.Ascii, GetAsciiBytes },
                { FieldType.Short, GetShortBytes },
                { FieldType.SShort, GetSShortBytes },
                { FieldType.Long, GetLongBytes },
                { FieldType.SLong, GetSLongBytes },
                { FieldType.Rational, GetRationalBytes },
                { FieldType.SRational, GetSRationalBytes },
                { FieldType.Float, GetFloatBytes },
                { FieldType.Double, GetDoubleBytes },
            };

        private static ASCIIEncoding ascii = new ASCIIEncoding();

        private static Dictionary<FieldType, int> valueLength = new Dictionary<FieldType, int> 
        { 
            { FieldType.Ascii, 1 }, 
            { FieldType.Byte, 1 }, 
            { FieldType.SByte, 1 }, 
            { FieldType.Undefined, 1 },
            { FieldType.Short, 2 }, 
            { FieldType.SShort, 2 }, 
            { FieldType.Long, 4 }, 
            { FieldType.SLong, 4 }, 
            { FieldType.Float, 4 },
            { FieldType.Rational, 8 },
            { FieldType.SRational, 8 },
            {FieldType.Double,8},
        };

        /// <summary>
        /// Creates a new instance of the Tif class
        /// </summary>
        public Tif()
            : base()
        {

        }

        private delegate byte[] ValueBytesMethod(Array values);

        ///// <summary>
        ///// Gets the collection of pages contained within the file
        ///// </summary>
        //public PageCollection Pages { get; internal set; }

        ///// <summary>
        ///// Gets or sets the page at the specified position in the file
        ///// </summary>
        ///// <param name="index">A zero-based page position in the current Tif object</param>
        ///// <returns></returns>
        //public Page this[int index]
        //{
        //    get
        //    {
        //        return this.Pages[index];
        //    }

        //    set
        //    {
        //        this.Pages[index] = value;
        //    }
        //}

        /// <summary>
        /// Creates a Tif from the specified stream
        /// </summary>
        /// <param name="stream">Stream containing a TIFF file</param>
        /// <returns></returns>
        public static Tif Load(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length - 1);
            return Load(bytes);
        }

        /// <summary>
        /// Creates a Tif from the specified file
        /// </summary>
        /// <param name="path">A path to a TIF file</param>
        /// <returns></returns>
        public static Tif Load(string path)
        {
            return Load(File.ReadAllBytes(path));
        }

        /// <summary>
        /// Creates a Tif from the specified bytes
        /// </summary>
        /// <param name="data">Array of bytes representing a TIF file</param>
        /// <returns></returns>
        public static Tif Load(byte[] data)
        {
            Tif file;
            int pos;
            switch (ascii.GetString(data, 0, 2))
            {
                case "II":
                    pos = (int)BitConverter.ToUInt32(data, 4);
                    file = ReadPages(data, pos);
                    break;
                default:
                    throw new NotSupportedException("Sorry, can't read this file.");
            }

            return file;
        }

        /// <summary>
        /// Writes the TIFF data into a file
        /// </summary>
        /// <param name="path">File to write the data to</param>
        public void Save(string path)
        {
            File.WriteAllBytes(path, this.GetData());
        }

        /// <summary>
        /// Returns the contents of the Tif as a byte array
        /// </summary>
        /// <returns></returns>
        public byte[] GetData()
        {
            var pageDatas = new List<List<byte>>();
            var fileData = new List<byte>();
            int filelen = 8;

            for (int i = 0; i < this.Count; i++)
            {
                filelen = GeneratePageData(pageDatas, filelen, i);
            }

            WriteHeader(pageDatas, fileData);
            this.WritePageDatas(pageDatas, fileData);
            return fileData.ToArray();
        }

        /// <summary>
        /// Creates a deep copy of the current Tif
        /// </summary>
        /// <returns></returns>
        public Tif Copy()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (Tif)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Saves the Tif to a stream
        /// </summary>
        /// <param name="stream">An open Stream object</param>
        public void Save(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            byte[] buffer = this.GetData();
            stream.Write(buffer, 0, buffer.Length);
        }

        internal static int GetValueLength(FieldType type)
        {
            int len = 0;
            valueLength.TryGetValue(type, out len);
            return len;
        }

        private static int GenerateTagData(int filelen, List<byte> pageData, Page page)
        {
            int valueLength;
            foreach (var tag in page)
            {
                valueLength = tag.Values.Length * GetValueLength(tag.FieldType);
                if (valueLength > 4)
                {
                    tag.ValueOffset = (uint)filelen;
                    pageData.AddRange(ValueBytes(tag.Values, tag.FieldType));
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

        private static int GeneratePageData(int filelen, List<byte> pageData, Page page)
        {
            foreach (var sd in page.ImageData)
            {
                pageData.AddRange(sd);
                filelen += sd.Length;
            }

            return filelen;
        }

        private static uint GenerateValueOffset(Field offsetTag, Field byteCountTag, List<byte> offsetData, uint offset, long valueLength)
        {
            if (valueLength > 4)
            {
                for (int o = 0; o < offsetTag.Count; o++)
                {
                    if (offsetTag.FieldType == FieldType.Short)
                    {
                        offsetData.AddRange(BitConverter.GetBytes((ushort)offset));
                    }
                    else
                    {
                        offsetData.AddRange(BitConverter.GetBytes((uint)offset));
                    }

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

        private static void WriteIfd(List<byte> fileData, Page page)
        {
            fileData.AddRange(BitConverter.GetBytes((ushort)page.Count));

            foreach (var tag in page)
            {
                fileData.AddRange(BitConverter.GetBytes((ushort)tag.Tag));
                fileData.AddRange(BitConverter.GetBytes((ushort)tag.FieldType));
                fileData.AddRange(BitConverter.GetBytes((uint)tag.Count));
                fileData.AddRange(BitConverter.GetBytes((uint)tag.ValueOffset));
            }
        }

        private static Tif ReadPages(byte[] data, int pos)
        {
            var file = new Tif();
            Page page;
            do
            {
                page = Page.Read(data, pos);
                //page.Number = file.Count + 1;
                file.Add(page);
                pos = (int)page.NextPageAddress;
            }
            while (pos != 0);
            return file;
        }

        private static byte[] ValueBytes(Array values, FieldType type)
        {
            if (valueByteMethods.ContainsKey(type))
            {
                return valueByteMethods[type](values);
            }
            else
            {
                return null;
            }
        }

        private static byte[] GetDoubleBytes(Array values)
        {
            var bytes = new byte[values.Length * 8];
            Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static byte[] GetFloatBytes(Array values)
        {
            var bytes = new byte[values.Length * 4];
            Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static byte[] GetSRationalBytes(Array values)
        {
            var rationalArray = values as Rational32[];
            var bytes = new byte[values.Length * 8];
            for (int i = 0; i < values.Length; i++)
            {
                Buffer.BlockCopy(rationalArray[i].GetBytes(), 0, bytes, i * 8, 8);
            }

            return bytes;
        }

        private static byte[] GetRationalBytes(Array values)
        {
            var rationals = values as URational32[];
            var bytes = new byte[values.Length * 8];
            for (int i = 0; i < values.Length; i++)
            {
                Buffer.BlockCopy(rationals[i].GetBytes(), 0, bytes, i * 8, 8);
            }

            return bytes;
        }

        private static byte[] GetSLongBytes(Array values)
        {
            var bytes = new byte[values.Length * 4];
            Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static byte[] GetLongBytes(Array values)
        {
            var bytes = new byte[values.Length * 4];
            Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static byte[] GetSShortBytes(Array values)
        {
            var bytes = new byte[values.Length * 2];
            Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static byte[] GetShortBytes(Array values)
        {
            var bytes = new byte[values.Length * 2];
            Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static byte[] GetAsciiBytes(Array values)
        {
            var bytes = ascii.GetBytes((char[])values);
            return bytes;
        }

        private static byte[] GetBytes(Array values)
        {
            var bytes = values as byte[];
            return bytes;
        }

        private int GeneratePageData(List<List<byte>> pageDatas, int filelen, int i)
        {
            var pageData = new List<byte>();
            var page = this[i];
            page.Sort();
            int ifdlen = 2 + (this[i].Count * 12) + 4;

            var offsetTag = page[Tag.StripOffsets] ?? page[Tag.TileOffsets];
            var countTag = page[Tag.StripByteCounts] ?? page[Tag.TileByteCounts];

            var offsetData = new List<byte>();
            uint offset = (uint)filelen;

            long valueLength = offsetTag.Count * GetValueLength(offsetTag.FieldType);
            offset = GenerateValueOffset(offsetTag, countTag, offsetData, offset, valueLength);

            filelen = GeneratePageData(filelen, pageData, page);

            filelen = GenerateTagData(filelen, pageData, page);

            pageDatas.Add(pageData);
            filelen += ifdlen;
            return filelen;
        }

        private void WritePageDatas(List<List<byte>> pageDatas, List<byte> fileData)
        {
            for (int i = 0; i < this.Count; i++)
            {
                var pd = pageDatas[i];
                fileData.AddRange(pd);
                WriteIfd(fileData, this[i]);

                if (i < this.Count - 1)
                {
                    fileData.AddRange(BitConverter.GetBytes((uint)(fileData.Count + pageDatas[i + 1].Count + 4)));
                }
                else
                {
                    fileData.AddRange(new byte[] { 0, 0, 0, 0 });
                }
            }
        }
    }
}