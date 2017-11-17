namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    [Serializable]
    public class Ifd : KeyedCollection<TagType, Field>
    {
        public List<Ifd> SubIfds { get; set; }

        public Ifd() : base(null, 0) { }

        internal static Page Read(byte[] data, int pos, bool flip)
        {
            var page = new Page();
            ushort tagCount;

            tagCount = BitConverter.ToUInt16(Tif.GetBytes(data, pos, 2, flip), 0);
            if (tagCount == 0)
            {
                return null;
            }

            pos += 2;
            for (int i = 0; i < tagCount && pos < data.Length; i++)
            {
                try
                {
                    var field = Field.Read(data, pos, flip);
                    if (field != null)
                    {
                        if (field.IsIfdField())
                        {
                            var subIfd = Ifd.ReadSubIfd(field, data, flip);
                            if (subIfd != null)
                                page.SubIfds.Add(subIfd);
                        }

                        page.Add(field);
                    }
                }
                catch (OutOfMemoryException)
                {
                    //The field data is fucked
                }

                pos += 12;
            }

            page.NextIfdAddress = BitConverter.ToUInt32(Tif.GetBytes(data, pos, 4, flip), 0);

            Field offsetTag = null;
            Field countTag = null;

            if (page.Contains(TagType.StripOffsets) && page.Contains(TagType.StripByteCounts))
            {
                offsetTag = page[TagType.StripOffsets];
                countTag = page[TagType.StripByteCounts];
            }
            else if (page.Contains(TagType.TileOffsets) && page.Contains(TagType.TileByteCounts))
            {
                offsetTag = page[TagType.TileOffsets];
                countTag = page[TagType.TileByteCounts];
            }

            page.ImageData = offsetTag != null
                ? GetImageData(data, offsetTag, countTag)
                : new List<byte[]>();

            return page;
        }

        private static Ifd ReadSubIfd(Field field, byte[] data, bool flip)
        => null;

        private static List<byte[]> GetImageData(byte[] data, Field stripOffsetTag, Field stripByteCountTag)
        {

            if (data == null || stripOffsetTag == null || stripByteCountTag == null || stripByteCountTag.Count != stripOffsetTag.Count)
            {
                return new List<byte[]>();
            }

            var stripData = new List<byte[]>();

            for (int i = 0; i < stripOffsetTag.Values.Length; i++)
            {
                long pos = Convert.ToInt64(stripOffsetTag.Values.GetValue(i));
                long count = Convert.ToInt64(stripByteCountTag.Values.GetValue(i));

                if (data.Length < pos + count)
                {
                    return new List<byte[]>();
                }

                var strip = new byte[count];
                Array.Copy(data, pos, strip, 0, count);
                stripData.Add(strip);
            }

            return stripData;

        }

        public uint NextIfdAddress { get; set; }

        public new void Add(Field item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.Remove(item.TagType);
            base.Add(item);
        }

        public void AddRange(IEnumerable<Field> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items)
                this.AddRange(items);
        }

        public void AddRange(IEnumerable<Tag> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items)
                this.Add(new Field(item.TagType, item.FieldType, item.Values));
        }

        public void Sort()
        => ((List<Field>)Items).Sort((t1, t2) => { return t1.TagType.CompareTo(t2.TagType); });

        protected override TagType GetKeyForItem(Field item)
        => item != null
            ? item.TagType
            : throw new ArgumentException(nameof(item));
    }
}
