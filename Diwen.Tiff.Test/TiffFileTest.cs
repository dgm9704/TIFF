using System.Drawing;
using System.IO;
using System.Reflection;
using Diwen.Tiff.Tags;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Diwen.Tiff.TagValues;


namespace Diwen.Tiff.Test
{


    /// <summary>
    ///This is a test class for TiffFileTest and is intended
    ///to contain all TiffFileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TiffFileTest
    {


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


        [TestMethod()]
        public void CopyAndModifyPages()
        {
            var newTif = new Tif();
            var tif = Tif.Load(@"C:\lena_kodak.tif");
            newTif.Pages.Add(tif.Pages[0]);
            newTif.Pages.Add(tif.Pages[0]);

            var values = new ushort[] { 0, 2 };
            var tag = new Tag(TagType.PageNumber, DataType.Short, values);
            newTif.Pages[0].Tags.Add(tag);
            tag = new Tag(TagType.PageNumber, DataType.Short, new ushort[] { 1, 2 });
            newTif.Pages[1].Tags.Add(tag);

            newTif.Save(@"c:\copiedpages_2.tif");
        }


        [TestMethod()]
        public void CombinePagesAndModifyTags()
        {
            var newFile = new Tif();
            var files = Directory.GetFiles(@"c:\img");

            Tag swTag = new SoftwareTag("Diwen.Tiff");

            var comp = new CompressionTag(Compression.CCITT4);

            for (int i = 0; i < files.Length; i++)
            {
                var oldFile = Tif.Load(files[i]);
                Page page = oldFile.Pages[0];
                page.Tags.Add(swTag);
                Tag pageNumber = new PageNumberTag((ushort)i, (ushort)files.Length);
                Tag artist = new AsciiTag(TagType.Artist, "John Nordberg");
                var subfile = new SubfileTypeTag(SubfileType.Page);
                var tags = new Tag[] { pageNumber, artist, new DateTimeTag(), subfile };
                page.Tags.AddRange(tags);
                newFile.Pages.Add(page);
            }

            newFile.Save(@"c:\combined_and_modified.tif");
        }

        [TestMethod()]
        public void Tagging()
        {
            var files = Directory.GetFiles(@"c:\img");
            var tif = Tif.Load(files[0]);
            Page page = tif[0];
            page.Add(new SoftwareTag("Diwen.Tiff"));

            var tag = page[TagType.StripByteCounts];


            tif.Save(@"c:\tagged.tif");
        }
    }
}