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
