using Diwen.Tiff;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Diwen.Tiff.Test
{


    /// <summary>
    ///This is a test class for PageTest and is intended
    ///to contain all PageTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PageTest
    {

        string testFilePath = "TIFF_file_format_test.tif";
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for HasTag
        ///</summary>
        [TestMethod()]
        public void HasTagTestTrue()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif.Pages[0];
            Assert.AreEqual(true, page.HasTag(TagType.NewSubfileType));
        }

        /// <summary>
        ///A test for HasTag
        ///</summary>
        [TestMethod()]
        public void HasTagTestFalse()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif.Pages[0];
            Assert.AreEqual(false, page.HasTag(TagType.TargetPrinter));
        }

        [TestMethod()]
        public void ReadExistingTag()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif.Pages[0];
            var tag = page.Tags[TagType.ImageWidth];
            Assert.IsNotNull(tag);
            Assert.AreEqual(TiffDataType.Short, tag.DataType);
            Assert.AreEqual(1, tag.Values.Length);
            var values = (ushort[])tag.Values;
            Assert.AreEqual(288, values[0]);
        }

        [TestMethod()]
        [ExpectedException(typeof(System.Collections.Generic.KeyNotFoundException))]
        public void ReadNonExistingTag()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif.Pages[0];
            var tag = page.Tags[TagType.AliasLayerMetadata];
        }


        [TestMethod()]
        public void AddAsciiTag()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif.Pages[0];
            Assert.AreEqual(false, page.HasTag(TagType.Artist));
            var name = "Leonardo DaVinci";
            page.Add(TagType.Artist, TiffDataType.Ascii, name.ToCharArray());
            var tag = page[TagType.Artist];
            var values = (char[])tag.Values;
            string value = new String(values);
            Assert.AreEqual(name, value);
        }


        [TestMethod()]
        public void Artist()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif.Pages[0];
            Assert.AreEqual(false, page.HasTag(TagType.Artist));
            var name = "Leonardo DaVinci";
            page.Artist = name;
            var tag = page[TagType.Artist];
            var values = (char[])tag.Values;
            string value = new String(values);
            Assert.AreEqual(name, value);
        }

        [TestMethod()]
        public void PageNumber()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif.Pages[0];
            Assert.AreEqual(false, page.HasTag(TagType.PageNumber));
            ushort number = 42;
            ushort total = 666;
            page.PageNumber = number;
            page.PageTotal = total;
            var tag = page[TagType.PageNumber];
            var values = (ushort[])tag.Values;
            Assert.AreEqual(number, values[0]);
            Assert.AreEqual(total, values[1]);
        }

        [TestMethod()]
        public void BitsPerSample()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif.Pages[0];

            ushort bitsper = 42;
            
            page.BitsPerSample = bitsper;

            var tag = page[TagType.BitsPerSample];
            var values = (ushort[])tag.Values;
            Assert.AreEqual(bitsper, values[0]);
        }

        [TestMethod()]
        public void EnumTagValue()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif.Pages[0];


        }
    }
}
