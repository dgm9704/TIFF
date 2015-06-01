namespace Diwen.Tiff.Test
{
    using Diwen.Tiff;
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    /// <summary>
    ///This is a test class for PageCollectionTest and is intended
    ///to contain all PageCollectionTest Unit Tests
    ///</summary>
    [TestFixture]
    public class PageCollectionTest
    {

        /// <summary>
        ///A test for PageCollection Constructor
        ///</summary>
        [Test]
        public void PageCollectionConstructorTest()
        {
            PageCollection target = new PageCollection();
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [Test]
        public void AddTest()
        {
            PageCollection target = new PageCollection();
            Page page = new Page();
            target.Add(page);
        }

        /// <summary>
        ///A test for AddRange
        ///</summary>
        [Test]
        public void AddRangeTest()
        {
            PageCollection target = new PageCollection();
            IEnumerable<Page> pages = new List<Page> { new Page(), new Page(), new Page() };
            target.AddRange(pages);
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [Test]
        public void ToStringTest()
        {
            PageCollection target = new PageCollection();
            string expected = string.Empty;
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddTest1()
        {
            PageCollection target = new PageCollection();
            Page page = null;
            target.Add(page);
        }

        /// <summary>
        ///A test for AddRange
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddRangeTest1()
        {
            PageCollection target = new PageCollection();
            IEnumerable<Page> pages = null;
            target.AddRange(pages);
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [Test]
        public void ToStringTest1()
        {
            PageCollection target = new PageCollection { new Page { new Field(Tag.Software, FieldType.Ascii, new string[] { "Diwen.Tiff" }) } };
            string actual;
            actual = target.ToString();
            Assert.AreEqual(false, string.IsNullOrEmpty(actual));
        }
    }
}
