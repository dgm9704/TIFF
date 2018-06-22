// Copyright (C) 2005-2018 by John Nordberg <john.nordberg@gmail.com>
// Free Public License 1.0.0

// Permission to use, copy, modify, and/or distribute this software for any purpose with or without fee is hereby granted.

// THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES
// OF MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR
// ANY DAMAGES WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS
// ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.

namespace Diwen.Tiff.Test
{
    using Diwen.Tiff;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class FieldCollectionTest
    {
        [Fact]
        public void FieldCollectionConstructorTest()
        {
            Ifd target = new Ifd();
        }

        [Fact]
        public void AddNullTest()
        {
            Ifd target = new Ifd();
            Field item = null;
            Assert.Throws<ArgumentNullException>(() => target.Add(item));
        }

        [Fact]
        public void AddRangeNullTest()
        {
            Ifd target = new Ifd();
            IEnumerable<Field> items = null;
            Assert.Throws<ArgumentNullException>(() => target.AddRange(items));
        }

        [Fact]
        public void AddTest()
        {
            Ifd target = new Ifd();
            Field item = new Field();
            target.Add(item);
        }

        [Fact]
        public void AddRangeTest()
        {
            Ifd target = new Ifd();
            IEnumerable<Field> items = new List<Field> { new Field(), new Field(), new Field() };
            target.AddRange(items);
        }

        [Fact]
        public void SortTest()
        {
            var target = new Ifd
            {
                new Field(TagType.Artist, FieldType.Short, new short[]{ }),
                new Field(TagType.BitsPerSample, FieldType.Short, new short[]{ }),
                new Field(TagType.CellLength, FieldType.Short, new short[]{ }),
            };
            target.Sort();
        }
    }
}
