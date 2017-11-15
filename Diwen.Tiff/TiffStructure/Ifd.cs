namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents an image tif directory
    /// </summary>
    [Serializable]
    public class Ifd : KeyedCollection<Tag, Field>
    {
        public List<Ifd> SubIfds { get; set; }

        public Ifd()
            : base(null, 0)
        {
        }

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
                            page.SubIfds.Add(Ifd.ReadSubIfd(field, data, flip));
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

            if (page.Contains(Tag.StripOffsets) && page.Contains(Tag.StripByteCounts))
            {
                offsetTag = page[Tag.StripOffsets];
                countTag = page[Tag.StripByteCounts];
            }
            else if (page.Contains(Tag.TileOffsets) && page.Contains(Tag.TileByteCounts))
            {
                offsetTag = page[Tag.TileOffsets];
                countTag = page[Tag.TileByteCounts];
            }

            if (offsetTag != null)
            {
                page.ImageData = GetImageData(data, offsetTag, countTag);
            }
            else
            {
                page.ImageData = new List<byte[]>();
            }

            return page;
        }

        private static Ifd ReadSubIfd(Field field, byte[] data, bool flip)
        {
            throw new NotImplementedException();
        }

        private static List<byte[]> GetImageData(byte[] data, Field stripOffsetTag, Field stripByteCountTag)
        {

            
            if (data == null || stripOffsetTag == null || stripByteCountTag == null || stripByteCountTag.Count != stripOffsetTag.Count)
            {
                return new List<byte[]>();
                ;
            }

            var stripData = new List<byte[]>();

            for (int i = 0; i < stripOffsetTag.Values.Length; i++)
            {
                long pos = Convert.ToInt64(stripOffsetTag.Values.GetValue(i));
                long count = Convert.ToInt64(stripByteCountTag.Values.GetValue(i));

                if (data.Length < pos + count)
                {
                    return new List<byte[]>();
                    ;
                }

                var strip = new byte[count];
                Array.Copy(data, pos, strip, 0, count);
                stripData.Add(strip);
            }

            return stripData;

        }

        public uint NextIfdAddress { get; set; }

        /// <summary>
        /// Adds a new tag to the collection
        /// </summary>
        /// <param name="item"></param>
        public new void Add(Field item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.Remove(item.Tag);
            base.Add(item);
        }

        /// <summary>
        /// Adds a range of tags to collection
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<Field> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            foreach (var item in items)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// Sorts the fields in the collection
        /// </summary>
        public void Sort()
        {
            ((List<Field>)Items).Sort((t1, t2) =>
                {
                    return t1.Tag.CompareTo(t2.Tag);
                });
        }

        /// <summary>
        /// Returns the key of an item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override Tag GetKeyForItem(Field item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return item.Tag;
        }
    }
}
