using System.Drawing;
using System.IO;
using System.Reflection;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace Diwen.Tiff.Test
{


    /// <summary>
    ///This is a test class for TiffFileTest and is intended
    ///to contain all TiffFileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TifTest
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
        public void LoadFromFile()
        {
            Tif tif;
            tif = Tif.Load(testFilePath);

            Assert.IsInstanceOfType(tif, typeof(Tif));
        }

        [TestMethod()]
        public void LoadFromStream()
        {
            Tif tif;

            using (var stream = new FileStream(testFilePath, FileMode.Open))
                tif = Tif.Load(stream);

            Assert.IsInstanceOfType(tif, typeof(Tif));
        }

        [TestMethod()]
        public void LoadFromBytes()
        {
            Tif tif;
            byte[] bytes = File.ReadAllBytes(testFilePath);

            tif = Tif.Load(bytes);

            Assert.IsInstanceOfType(tif, typeof(Tif));
        }

        [TestMethod()]
        public void SaveToFile()
        {
            Tif tif;
            tif = Tif.Load(testFilePath);

            string path = @"\TIFF_file_format_test.new";
            tif.Save(path);
            tif = Tif.Load(path);
            Assert.IsInstanceOfType(tif, typeof(Tif));
        }

        [TestMethod()]
        public void SaveToStream()
        {
            Tif tif;
            tif = Tif.Load(testFilePath);

            using (MemoryStream stream = new MemoryStream())
            {
                tif.Save(stream);
                stream.Position = 0;
                tif = Tif.Load(stream);
            }
            Assert.IsInstanceOfType(tif, typeof(Tif));
        }

        [TestMethod()]
        public void GetData()
        {
            Tif tif;
            tif = Tif.Load(testFilePath);
            byte[] buffer = tif.GetData();

            tif = Tif.Load(buffer);

            Assert.IsInstanceOfType(tif, typeof(Tif));
        }

        [TestMethod()]
        public void DuplicatePage()
        {
            var tif = Tif.Load(testFilePath);
            int numPages = tif.Pages.Count;
            tif.Pages.Add(tif.Pages[0]);

            Assert.AreEqual(numPages + 1, tif.Pages.Count);
        }

        [TestMethod()]
        public void RemovePage()
        {
            var tif = Tif.Load(testFilePath);
            int numPages = tif.Pages.Count;
            tif.Pages.RemoveAt(0);

            Assert.AreEqual(numPages - 1, tif.Pages.Count);
        }

        [TestMethod()]
        public void AddPages()
        {
            var tif1 = Tif.Load(testFilePath);
            var tif2 = Tif.Load(testFilePath);

            var newTif = new Tif();
            newTif.Pages.Add(tif1.Pages[0]);
            newTif.Pages.Add(tif2.Pages[0]);

            Assert.AreEqual(2, newTif.Pages.Count);

            using (var stream = new MemoryStream())
            {
                newTif.Save(stream);
                stream.Position = 0;
                newTif = Tif.Load(stream);
            }
            Assert.AreEqual(2, newTif.Pages.Count);
        }




        [TestMethod()]
        public void CopyAndModifyPages()
        {
            string originalName = @"C:\lena_kodak.tif";
            string newName = @"c:\copiedpages_2.tif";

            var newTif = new Tif();
            var tif = Tif.Load(originalName);
            newTif.Pages.Add(tif.Pages[0]);
            newTif.Pages.Add(tif.Pages[0]);

            var values = new ushort[] { 0, 2 };
            var tag = new Tag(TagType.PageNumber, TiffDataType.Short, values);
            newTif.Pages[0].Tags.Add(tag);
            tag = new Tag(TagType.PageNumber, TiffDataType.Short, new ushort[] { 1, 2 });
            newTif.Pages[1].Tags.Add(tag);

            newTif.Save(newName);

            newTif = Tif.Load(newName);

            Assert.AreEqual(2, newTif.Pages.Count);
            ushort[] pageNumber;

            pageNumber = (ushort[])newTif.Pages[0][TagType.PageNumber].Values;
            Assert.AreEqual(0, pageNumber[0]);
            Assert.AreEqual(2, pageNumber[1]);

            pageNumber = (ushort[])newTif.Pages[1][TagType.PageNumber].Values;
            Assert.AreEqual(1, pageNumber[0]);
            Assert.AreEqual(2, pageNumber[1]);

        }


        //[TestMethod()]
        //public void CombinePagesAndModifyTags()
        //{
        //    var newFile = new Tif();
        //    var files = Directory.GetFiles(@"c:\img");

        //    Tag swTag = new SoftwareTag("Diwen.Tiff");

        //    for (int i = 0; i < files.Length; i++)
        //    {
        //        var oldFile = Tif.Load(files[i]);
        //        Page page = oldFile.Pages[0];
        //        page.Tags.Add(swTag);
        //        Tag pageNumber = new PageNumberTag((ushort)i, (ushort)files.Length);
        //        Tag artist = new AsciiTag(TagType.Artist, "John Nordberg");
        //        var subfile = new SubfileTypeTag(SubfileType.Page);
        //        var tags = new Tag[] { pageNumber, artist, new DateTimeTag(), subfile };
        //        page.Tags.AddRange(tags);
        //        newFile.Pages.Add(page);
        //    }

        //    newFile.Save(@"c:\combined_and_modified.tif");
        //}
    }
}