namespace Diwen.Tiff.Test
{
    using Diwen.Tiff;
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    /// <summary>
    ///This is a test class for FieldCollectionTest and is intended
    ///to contain all FieldCollectionTest Unit Tests
    ///</summary>
    [TestFixture]
    public class FieldCollectionTest
    {
    

        /// <summary>
        ///A test for Ifd Constructor
        ///</summary>
        [Test]
        public void FieldCollectionConstructorTest()
        {
            Ifd target = new Ifd();
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNullTest()
        {
            Ifd target = new Ifd();
            Field item = null;
            target.Add(item);
        }

        /// <summary>
        ///A test for AddRange
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddRangeNullTest()
        {
            Ifd target = new Ifd();
            IEnumerable<Field> items = null;
            target.AddRange(items);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [Test]
        public void AddTest()
        {
            Ifd target = new Ifd();
            Field item = new Field();
            target.Add(item);
        }

        /// <summary>
        ///A test for AddRange
        ///</summary>
        [Test]
        public void AddRangeTest()
        {
            Ifd target = new Ifd();
            IEnumerable<Field> items = new List<Field> { new Field(), new Field(), new Field() };
            target.AddRange(items);
        }

        ///// <summary>
        /////A test for GetKeyForItem
        /////</summary>
        //[Test]
        //[DeploymentItem("Diwen.Tiff.dll")]
        //public void GetKeyForItemTest()
        //{
        //    FieldCollection_Accessor target = new FieldCollection_Accessor();
        //    Field item = new Field(Tag.AntiAliasStrength, FieldType.Short, new short[] { });
        //    Tag expected = Tag.AntiAliasStrength;
        //    Tag actual;
        //    actual = target.GetKeyForItem(item);
        //    Assert.AreEqual(expected, actual);
        //}

        ///// <summary>
        /////A test for GetKeyForItem
        /////</summary>
        //[Test]
        //[DeploymentItem("Diwen.Tiff.dll")]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void GetKeyForItemNullTest()
        //{
        //    FieldCollection_Accessor target = new FieldCollection_Accessor();
        //    Field item = null;
        //    Tag actual;
        //    actual = target.GetKeyForItem(item);
        //}

        /// <summary>
        ///A test for Sort
        ///</summary>
        [Test]
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
