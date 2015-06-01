using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Diwen.Tiff
{
    [Serializable()]
    public class Page
    {
        public int Number { get; set; }
        internal uint NextPageAddress { get; set; }
        internal List<byte[]> ImageData { get; set; }

        public TagCollection Tags { get; internal set; }

        public Page()
        {
            Tags = new TagCollection();
        }

        public Tag this[TagType tagType]
        {
            get
            {
                return Tags[tagType];
            }
            set
            {
                Tags.Insert(0, value);
            }
        }

        internal static Page Read(byte[] data, int pos)
        {
            var page = new Page();
            ushort tagCount = BitConverter.ToUInt16(data, pos);
            pos += 2;
            for (int i = 0; i < tagCount; i++)
            {
                page.Tags.Add(Tag.Read(data, pos));
                pos += 12;
            }
            page.NextPageAddress = BitConverter.ToUInt32(data, pos);

            var offsetTag = page.Tags[TagType.StripOffsets] ?? page.Tags[TagType.TileOffsets];
            var countTag = page.Tags[TagType.StripByteCounts] ?? page.Tags[TagType.TileByteCounts];
            page.ImageData = GetImageData(data, offsetTag, countTag);

            return page;
        }

        internal static Page ReadMM(byte[] data, int pos)
        {
            var page = new Page();

            ushort tagCount = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(data, pos));
            pos += 2;
            for (int i = 0; i < tagCount; i++)
            {
                page.Tags.Add(Tag.ReadMM(data, pos));
                pos += 12;
            }
            page.NextPageAddress = (uint)IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(data, pos));

            var offsetTag = page.Tags[TagType.StripOffsets] ?? page.Tags[TagType.TileOffsets];
            var countTag = page.Tags[TagType.StripByteCounts] ?? page.Tags[TagType.TileByteCounts];
            page.ImageData = GetImageData(data, offsetTag, countTag);

            return page;
        }

        private static List<byte[]> GetImageData(byte[] data, Tag stripOffsetTag, Tag stripByteCountTag)
        {
            var stripData = new List<byte[]>();
            for (int i = 0; i < stripOffsetTag.Values.Length; i++)
            {
                long pos = (long)(uint)stripOffsetTag.Values.GetValue(i);
                long count = (long)(uint)stripByteCountTag.Values.GetValue(i);
                var strip = new byte[count];
                Array.Copy(data, pos, strip, 0, count);
                stripData.Add(strip);
            }
            return stripData;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("-Page {0}: \r\n", this.Number);
            foreach (var tag in this.Tags)
                sb.AppendLine(tag.ToString());

            return sb.ToString();
        }

        public Page Copy()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (Page)formatter.Deserialize(stream);
            }
        }

    }
}
