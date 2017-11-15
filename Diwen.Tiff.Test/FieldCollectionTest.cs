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

        //[Fact]
        //[DeploymentItem("Diwen.Tiff.dll")]
        //public void GetKeyForItemTest()
        //{
        //    FieldCollection_Accessor target = new FieldCollection_Accessor();
        //    Field item = new Field(Tag.AntiAliasStrength, FieldType.Short, new short[] { });
        //    Tag expected = Tag.AntiAliasStrength;
        //    Tag actual;
        //    actual = target.GetKeyForItem(item);
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //[DeploymentItem("Diwen.Tiff.dll")]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void GetKeyForItemNullTest()
        //{
        //    FieldCollection_Accessor target = new FieldCollection_Accessor();
        //    Field item = null;
        //    Tag actual;
        //    actual = target.GetKeyForItem(item);
        //}

        [Fact]
        public void SortTest()
        {
            var target = new Ifd
            {
                new Field(Tag.Artist, FieldType.Short, new short[]{ }),
                new Field(Tag.BitsPerSample, FieldType.Short, new short[]{ }),
                new Field(Tag.CellLength, FieldType.Short, new short[]{ }),
            };
            target.Sort();
        }
    }
}
