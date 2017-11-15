namespace Diwen.Tiff.Test
{
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Linq;
    using System;
    using Diwen.Tiff;
    using System.Diagnostics;
    using Xunit;

    public class TifTest
    {
        private static string testFilePath = Path.Combine("testfiles", "TIFF_file_format_test.tif");

        [Fact]
        public void LoadFromFile()
        {
            Tif tif;
            tif = Tif.Load(testFilePath);

            Assert.IsInstanceOfType(typeof(Tif), tif);
        }

        [Fact]
        public void LoadFromStream()
        {
            Tif tif;

            using (var stream = new FileStream(testFilePath, FileMode.Open))
                tif = Tif.Load(stream);

            Assert.IsInstanceOfType(typeof(Tif), tif);
        }

        [Fact]
        public void LoadFromStreamNull()
        {
            Tif tif;
            FileStream stream = null;
            using (stream)
                Assert.Throws<ArgumentNullException>(() => Tif.Load(stream));
        }

        [Fact]
        public void LoadFromBytes()
        {
            Tif tif;
            byte[] bytes = File.ReadAllBytes(testFilePath);

            tif = Tif.Load(bytes);

            Assert.IsInstanceOfType(typeof(Tif), tif);
        }

        [Fact]
        public void SaveToFile()
        {
            Tif tif;
            tif = Tif.Load(testFilePath);

            string path = @"\TIFF_file_format_test.new";
            tif.Save(path);
            tif = Tif.Load(path);
            Assert.IsInstanceOfType(typeof(Tif), tif);
        }

        [Fact]
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
            Assert.IsInstanceOfType(typeof(Tif), tif);
        }

        [Fact]
        public void SaveToStreamNull()
        {
            Tif tif;
            tif = Tif.Load(testFilePath);
            MemoryStream stream = null;

            using (stream)
            {
                Assert.Throws<ArgumentNullException>(() => tif.Save(stream));
                stream.Position = 0;
                tif = Tif.Load(stream);
            }
        }

        [Fact]
        public void GetData()
        {
            Tif tif;
            tif = Tif.Load(testFilePath);
            byte[] buffer = tif.GetData();

            tif = Tif.Load(buffer);

            Assert.IsType(typeof(Tif), tif);
        }

        [Fact]
        public void DuplicatePage()
        {
            var tif = Tif.Load(testFilePath);
            int numPages = tif.Count;
            tif.Add(tif[0]);

            Assert.Equal(numPages + 1, tif.Count);
        }

        [Fact]
        public void RemovePage()
        {
            var tif = Tif.Load(testFilePath);
            int numPages = tif.Count;
            tif.RemoveAt(0);

            Assert.Equal(numPages - 1, tif.Count);
        }

        [Fact]
        public void AddPages()
        {
            var tif1 = Tif.Load(testFilePath);
            var tif2 = Tif.Load(testFilePath);

            var newTif = new Tif();
            newTif.Add(tif1[0]);
            newTif.Add(tif2[0]);

            Assert.Equal(2, newTif.Count);

            using (var stream = new MemoryStream())
            {
                newTif.Save(stream);
                stream.Position = 0;
                newTif = Tif.Load(stream);
            }
            Assert.Equal(2, newTif.Count);
        }

        [Fact]
        public void CopyAndModifyPages()
        {
            string originalName = @"C:\lena_kodak.tif";
            string newName = @"c:\copiedpages_2.tif";

            var newTif = new Tif();
            var tif = Tif.Load(originalName);
            newTif.Add(tif[0]);
            newTif.Add(tif[0]);

            var values = new ushort[] { 0, 2 };
            var tag = new Field(Tag.PageNumber, FieldType.Short, values);
            newTif[0].Add(tag);
            tag = new Field(Tag.PageNumber, FieldType.Short, new ushort[] { 1, 2 });
            newTif[1].Add(tag);

            newTif.Save(newName);

            newTif = Tif.Load(newName);

            Assert.Equal(2, newTif.Count);
            ushort[] pageNumber;

            pageNumber = (ushort[])newTif[0][Tag.PageNumber].Values;
            Assert.Equal(0, pageNumber[0]);
            Assert.Equal(2, pageNumber[1]);

            pageNumber = (ushort[])newTif[1][Tag.PageNumber].Values;
            Assert.Equal(1, pageNumber[0]);
            Assert.Equal(2, pageNumber[1]);

        }


        //[Fact]
        //public void CombinePagesAndModifyTags()
        //{
        //    var newFile = new Tif();
        //    var files = Directory.GetFiles(@"c:\img");

        //    Tag swTag = new SoftwareTag("Diwen.Tiff");

        //    for (int i = 0; i < files.Length; i++)
        //    {
        //        var oldFile = Tif.Load(files[i]);
        //        Page page = oldFile.Pages[0];
        //        page.Add(swTag);
        //        Tag pageNumber = new PageNumberTag((ushort)i, (ushort)files.Length);
        //        Tag artist = new AsciiTag(Tag.Artist, "John Nordberg");
        //        var subfile = new SubfileTypeTag(SubfileType.Page);
        //        var tags = new Tag[] { pageNumber, artist, new DateTimeTag(), subfile };
        //        page.AddRange(tags);
        //        newFile.Pages.Add(page);
        //    }

        //    newFile.Save(@"c:\combined_and_modified.tif");
        //}

        [Fact]
        public void TiledTiffTest()
        {
            var tif = Tif.Load("tiled.tif");
            var page = tif[0];

        }

        [Fact]
        public void SetPagenumbersTest()
        {
            var tif = Tif.Load(@"c:\pruned.tif");
            tif.SetPageNumbers();
            tif.Save(@"C:\paged.tif");
        }

        /// <summary>
        ///A test for Copy
        ///</summary>
        [Fact]
        public void CopyTest()
        {
            Tif original = Tif.Load(testFilePath);
            Tif copy = original.Copy();
            Assert.NotSame(original, copy);
        }


        //[Fact]
        //public void LibTiffPicTest()
        //{
        //    string folder = @"C:\Documents and Settings\John\Desktop\pics-3.8.0.tar\pics-3.8.0\libtiffpic";
        //    foreach (var file in Directory.EnumerateFiles(folder, "*.tif", SearchOption.TopDirectoryOnly))
        //    {
        //        Console.WriteLine(Tif.Load(file));
        //    }
        //}

        //[Fact]
        //public void LibTiffPicTest1()
        //{
        //    Console.WriteLine(Tif.Load(@"C:\Documents and Settings\John\Desktop\pics-3.8.0.tar\pics-3.8.0\libtiffpic\cramps-tile.tif"));
        //}

        //[Fact]
        //public void LibTiffPicTest2()
        //{
        //    Console.WriteLine(Tif.Load(@"C:\Documents and Settings\John\Desktop\pics-3.8.0.tar\pics-3.8.0\libtiffpic\dscf0013.tif"));
        //}

        ////C:\Documents and Settings\John\Desktop\pics-3.8.0.tar\pics-3.8.0\libtiffpic\dscf0013.tif

        //[Fact]
        //public void MMTest1()
        //{
        //    Console.WriteLine(Tif.Load(@"c:\lena.tif"));
        //}

        [Fact]
        public void TiffTestSuiteComplete()
        {
            string folder = @"D:\tiff_test_images";
            foreach (var file in Directory.EnumerateFiles(folder, "*.tif", SearchOption.TopDirectoryOnly))
            {
                Console.WriteLine(Tif.Load(file));
            }
        }
    }
}