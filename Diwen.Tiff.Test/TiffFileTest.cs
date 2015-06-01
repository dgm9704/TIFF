namespace Diwen.Tiff.Test
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using Diwen.Tiff;
    using NUnit.Framework;
    using Diwen.Tiff.TagValues;


    /// <summary>
    ///This is a test class for TiffFileTest and is intended
    ///to contain all TiffFileTest Unit Tests
    ///</summary>
    [TestFixture]
    public class TiffFileTest
    {
        /// <summary>
        ///A test for Load
        ///</summary>
        [Test]
        public void LoadFileII()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            TiffFile tiff;
            using (var stream = assembly.GetManifestResourceStream("Diwen.Tiff.Test.lenaII.tif"))
                tiff = TiffFile.Load(stream);

            Assert.IsNotNull(tiff);
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [Test]
        public void LoadFileMM()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            TiffFile tiff;
            using (var stream = assembly.GetManifestResourceStream("Diwen.Tiff.Test.lenaMM.tif"))
            {
                tiff = TiffFile.Load(stream);
            }

            Assert.IsNotNull(tiff);
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [Test]
        public void RoundtripII()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            TiffFile tiff;
            using (var stream = assembly.GetManifestResourceStream("Diwen.Tiff.Test.lenaII.tif"))
            {
                tiff = TiffFile.Load(stream);
            }

            var expected = tiff.ToString();
            tiff.Save("saved.tif");
            tiff = TiffFile.Load("saved.tif");
            var actual = tiff.ToString();
            var bitmap = Bitmap.FromFile("saved.tif");
            Assert.IsNotNull(bitmap);
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [Test]
        public void SaveMMAsII()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            TiffFile tiff;
            using (var stream = assembly.GetManifestResourceStream("Diwen.Tiff.Test.lenaMM.tif"))
            {
                tiff = TiffFile.Load(stream);
            }

            var expected = tiff.ToString();
            tiff.Save("II.tif");
            tiff = TiffFile.Load("II.tif");
            var actual = tiff.ToString();
            var bitmap = Bitmap.FromFile("II.tif");
            Assert.IsNotNull(bitmap);
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [Test]
        public void SaveMMAsII_2()
        {
            TiffFile tiff = TiffFile.Load(@"C:\lena.tif");

            var expected = tiff.ToString();
            tiff.Save(@"c:\lenaII.tif");
            tiff = TiffFile.Load(@"c:\lenaII.tif");
            var actual = tiff.ToString();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CombineFirstPages()
        {
            var newFile = new TiffFile();
            var files = Directory.GetFiles(@"c:\img");
            for (int i = 0; i < files.Length; i++)
            {
                var oldFile = TiffFile.Load(files[i]);
                newFile.Add(oldFile[0]);
            }
            newFile.Save(@"c:\combined.tif");
        }

        [Test]
        public void CombineAllPages()
        {
            var newTif = new TiffFile();
            for (int i = 0; i < 3; i++)
            {
                var tif = TiffFile.Load(@"C:\combined.tif");
                newTif.AddRange(tif);
            }
            newTif.Save(@"c:\combined_combined.tif");
        }

        [Test]
        public void CopyPages()
        {
            var newTif = new TiffFile();
            var tif = TiffFile.Load(@"C:\lena_kodak.tif");
            newTif.Add(tif[0]);
            newTif.Add(tif[0]);

            newTif.Save(@"c:\copiedpages.tif");
        }

        [Test]
        public void CopyAndModifyPages()
        {
            var newTif = new TiffFile();
            var tif = TiffFile.Load(@"C:\lena_kodak.tif");
            newTif.Add(tif[0]);
            newTif.Add(tif[0]);

            newTif[0].Add(new TiffTag{ TagType = TagType.PageNumber, DataType = TiffDataType.Short, Values = new ushort[]{ 1, 20 } });
            //newTif[1].Find(t=>t.TagType = TagValues.TagType.PageNumber)
            newTif.Save(@"c:\copiedpages.tif");
        }

        ///// <summary>
        /////A test for Load
        /////</summary>
        //[TestMethod()]
        //public void CombinePages()
        //{
        //    Assembly assembly = Assembly.GetExecutingAssembly();
        //    TiffFile tiff1;
        //    using (var stream = assembly.GetManifestResourceStream("Diwen.Tiff.Test.lenaII.tif"))
        //        tiff1 = TiffFile.Load(stream);

        //    TiffFile tiff2;
        //    using (var stream = assembly.GetManifestResourceStream("Diwen.Tiff.Test.lenaMM.tif"))
        //        tiff2 = TiffFile.Load(stream);

        //    TiffFile newTiff = new TiffFile();
        //    newTiff.Add(tiff2[0]);
        //    newTiff.Add(tiff1[0]);


        //    newTiff.Save(@"C:\combined.tif");
        //    var tiff3 = TiffFile.Load(@"C:\combined.tif");

        //    Assert.IsNotNull(tiff3);
        //}

        ///// <summary>
        /////A test for Load
        /////</summary>
        //[TestMethod()]
        //public void wtf()
        //{
        //    TiffFile tiff1 = TiffFile.Load(@"C:\Documents and Settings\John\My Documents\My Pictures\lena.tif");
        //    tiff1.Save(@"c:\lena_saved.tif");
        //    TiffFile tiff2 = TiffFile.Load(@"c:\lena_saved.tif");
        //    Assert.IsNotNull(tiff2);
        //}

        ///// <summary>
        /////A test for Load
        /////</summary>
        //[TestMethod()]
        //public void wtf2()
        //{
        //    TiffFile tiff1 = TiffFile.Load(@"C:\lena_kodak.tif");
        //    tiff1.Save(@"c:\lena_kodak_saved.tif");
        //    TiffFile tiff2 = TiffFile.Load(@"c:\lena_kodak_saved.tif");
        //    Assert.IsNotNull(tiff2);
        //}
    }
}


//using System;
//using System.Text;
//using System.IO;
//using Diwen.Tiff.TagValues;
//using System.Collections.ObjectModel;
//namespace Diwen.Tiff
//{
//    class Program
//    {

//        static void Main(string[] args)
//        {
//            var file = TiffFile.Load("base-2.tif");
//            Dump(file, "base-2.txt");
//            file.Save("mod.tif");
//            file = TiffFile.Load("mod.tif");
//            Dump(file, "mod.txt");


//            //var file = TiffFile.Load("scanned.tif");
//            //Dump(file, "scanned.txt");

//            //for (int i = 0; i < 1000; i++)
//            //{
//            //file.Save("mod.tif");
//            //file = TiffFile.Load("mod.tif");
//            //Dump(file, "mod.txt");
//            //}


//            //file.Save("mod.tif");
//            //file = TiffFile.Load("mod.tif");
//            //Dump(file, "mod.txt");

//            //Rational32 foo = new Rational32(1,1);
//            //Console.WriteLine(System.Runtime.InteropServices.Marshal.SizeOf(foo));

//            //var file = TiffFile.Load(args[0]);
//            //Dump(file, Path.ChangeExtension(args[0], "txt"));
//            //file.Save("mod.tif");
//            //file = TiffFile.Load("mod.tif");
//            //Dump(file, "mod.txt");

//            //TiffFile file = TiffFile.Load("color.tif");
//            //TiffPage page = file[0];
//            //var values = new Collection<object>();
//            //values.Add("String one");
//            //values.Add("String two");
//            //values.Add("String three");
//            //values.Add("String four");
//            //values.Add("String five");
//            //TiffTag tag = new TiffTag(TagType.PageNumber, TiffDataType.Ascii, values);
//            //values.Add((byte)1);
//            //values.Add((byte)2);
//            //values.Add((byte)3);
//            //values.Add((byte)4);
//            //values.Add((byte)5);
//            //TiffTag tag = new TiffTag(TagType.PageNumber, TiffDataType.Byte, values);

//            //values.Add(new URational32(0, 1));
//            //values.Add(new URational32(1, 2));
//            //values.Add(new URational32(2, 3));
//            //values.Add(new URational32(3, 4));
//            //values.Add(new URational32(4, 5));
//            //values.Add(new URational32(5, 6));
//            //values.Add(new URational32(6, 7));
//            //values.Add(new URational32(7, 8));
//            //values.Add(new URational32(8, 9));

//            //TiffTag tag = new TiffTag(TagType.PageNumber, TiffDataType.Rational, values);


//            //values.Add((sbyte)1);
//            //values.Add((sbyte)2);
//            //values.Add((sbyte)3);
//            //values.Add((sbyte)4);
//            //values.Add((sbyte)-127);
//            //TiffTag tag = new TiffTag(TagType.PageNumber, TiffDataType.SByte, values);

//            //page.Add(tag);
//            //file.Save("mod.tif");
//            //file = TiffFile.Load("mod.tif");
//            //Dump(file, "mod.txt");

//            //TiffPage page = new TiffPage { tag };
//            //TiffFile file = new TiffFile { page };
//            //file.Save("new.tif");
//            //TiffFile loaded = TiffFile.Load("new.tif");
//            //Dump(loaded, "loaded.txt");

//            //for (int i = 0; i < 1000; i++)
//            //{
//            //    TiffFile scanned = TiffFile.Load("scanned.tif");
//            //    //Dump(scanned, "scanned.txt");
//            //    //scanned.Save("mod.tif");
//            //    //TiffFile mod = TiffFile.Load("mod.tif");
//            //    //Dump(mod, "mod.txt");
//            //}
//        }

//        private static void Dump(TiffFile tif, string path)
//        {
//            File.WriteAllText(path, tif.ToString());
//        }
//    }
//}
