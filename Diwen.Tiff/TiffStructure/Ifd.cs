//
//  This file is part of Diwen.Tiff.
//
//  Author:
//       John Nordberg <john.nordberg@gmail.com>
//
//  Copyright (c) 2005-2017 John Nordberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
                return null;

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

            var (offsetField, countField) = GetOffsetAndCount(page);

            page.ImageData = offsetField != null
                ? GetImageData(data, offsetField, countField)
                : new List<byte[]>();

            return page;
        }

        private static (Field offsetField, Field countField) GetOffsetAndCount(Page page)
        => IsStriped(page)
            ? StripedOffsetAndCount(page)
            : IsTiled(page)
                ? TiledOffsetAndCount(page)
                : (null, null);

        private static (Field offsetField, Field countField) StripedOffsetAndCount(Page page)
        => (page[TagType.StripOffsets],
            page[TagType.StripByteCounts]);

        private static (Field offsetField, Field countField) TiledOffsetAndCount(Page page)
        => (page[TagType.TileOffsets],
            page[TagType.TileByteCounts]);

        private static bool IsTiled(Page page)
        => page.Contains(TagType.TileOffsets)
        && page.Contains(TagType.TileByteCounts);

        private static bool IsStriped(Page page)
        => page.Contains(TagType.StripOffsets)
        && page.Contains(TagType.StripByteCounts);

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
                throw new ArgumentNullException(nameof(item));
            }

            this.Remove(item.TagType);
            base.Add(item);
        }

        public void AddRange(IEnumerable<Field> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
                this.Add(item);
        }

        public void AddRange(IEnumerable<Tag> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

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
