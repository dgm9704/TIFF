using System;
using System.Collections.Generic;
using System.Text;
using Diwen.Tiff.TagValues;
using System.Net;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Diwen.Tiff
{
    [Serializable()]
    public class TiffPage : KeyedCollection<TagType, TiffTag>
    {
        public int Number { get; set; }
        internal uint NextPageAddress { get; set; }
        internal List<byte[]> ImageData { get; set; }

        private TiffPage() : base() { }

        protected override TagType GetKeyForItem(TiffTag item)
        {
            return item.TagType;
        }

        internal static TiffPage Read(byte[] data, int pos)
        {
            var page = new TiffPage();
            ushort tagCount = BitConverter.ToUInt16(data, pos);
            pos += 2;
            for (int i = 0; i < tagCount; i++)
            {
                page.Add(TiffTag.Read(data, pos));
                pos += 12;
            }
            page.NextPageAddress = BitConverter.ToUInt32(data, pos);

            var offsetTag = page.Find(t => t.TagType == TagType.StripOffsets || t.TagType == TagType.TileOffsets);
            var countTag = page.Find(t => t.TagType == TagType.StripByteCounts || t.TagType == TagType.TileByteCounts);
            page.ImageData = GetImageData(data, offsetTag, countTag);

            return page;
        }

        internal static TiffPage ReadMM(byte[] data, int pos)
        {
            var page = new TiffPage();

            ushort tagCount = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(data, pos));
            pos += 2;
            for (int i = 0; i < tagCount; i++)
            {
                page.Add(TiffTag.ReadMM(data, pos));
                pos += 12;
            }
            page.NextPageAddress = (uint)IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(data, pos));

            var offsetTag = page.Find(t => t.TagType == TagType.StripOffsets || t.TagType == TagType.TileOffsets);
            var countTag = page.Find(t => t.TagType == TagType.StripByteCounts || t.TagType == TagType.TileByteCounts);
            page.ImageData = GetImageData(data, offsetTag, countTag);

            return page;
        }

        private static List<byte[]> GetImageData(byte[] data, TiffTag stripOffsetTag, TiffTag stripByteCountTag)
        {
            var stripData = new List<byte[]>();
            for (int i = 0; i < stripOffsetTag.Values.Length; i++)
            {
                long pos = (long)(uint)stripOffsetTag.Values.GetValue(i);
                long count = (long)(uint)stripByteCountTag.Values.GetValue(i);
                var strip = new byte[count];
                try
                {
                    Array.Copy(data, pos, strip, 0, count);
                    stripData.Add(strip);
                }
                catch (Exception)
                {
                    throw;
                }

            }
            return stripData;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("-Page {0}: \r\n", this.Number);
            foreach (var tag in this)
                sb.AppendLine(tag.ToString());

            return sb.ToString();
        }

        //public new void Add(TiffTag tag)
        //{
        //    if (null == tag)
        //        throw new ArgumentNullException("tag");

        //    Items.Add(tag.Copy());
        //}

        public void AddRange(IEnumerable<TiffTag> tags)
        {
            //if (tags == null)
            //    throw new ArgumentNullException("tags");

            foreach (var tag in tags)
                Add(tag);
        }

        public TiffPage Copy()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (TiffPage)formatter.Deserialize(stream);
            }
        }

        public TiffTag Find(Predicate<TiffTag> match)
        {
            return ((List<TiffTag>)Items).Find(match);
        }

        public void Sort()
        {
            ((List<TiffTag>)Items).Sort();
        }
    }
}
