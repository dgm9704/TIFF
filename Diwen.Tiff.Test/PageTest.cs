namespace Diwen.Tiff.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Diwen.Tiff;
    using Diwen.Tiff.FieldValues;
    using Xunit;

    public class PageTest
    {
        private static string testFilePath = Path.Combine("testfiles", "TIFF_file_format_test.tif");

        [Fact]
        public void HasFieldTestTrue()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif[0];
            Assert.AreEqual(true, page.Contains(Tag.NewSubfileType));
        }

        [Fact]
        public void HasFieldTestFalse()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif[0];
            Assert.AreEqual(false, page.Contains(Tag.TargetPrinter));
        }

        [Fact]
        public void ReadExistingField()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif[0];
            var tag = page[Tag.ImageWidth];
            Assert.IsNotNull(tag);
            Assert.AreEqual(FieldType.Short, tag.FieldType);
            Assert.AreEqual(1, tag.Values.Length);
            var values = (ushort[])tag.Values;
            Assert.AreEqual(288, values[0]);
        }

        [Fact]
        [ExpectedException(typeof(System.Collections.Generic.KeyNotFoundException))]
        public void ReadNonExistingField()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif[0];
            var tag = page[Tag.AliasLayerMetadata];
        }

        [Fact]
        public void AddAsciiField()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif[0];
            Assert.AreEqual(false, page.Contains(Tag.Artist));
            var name = "Leonardo DaVinci";
            page.Add(Tag.Artist, FieldType.Ascii, name.ToCharArray());
            var tag = page[Tag.Artist];
            var values = (char[])tag.Values;
            string value = new String(values);
            Assert.AreEqual(name, value);
        }

        [Fact]
        public void Artist()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif[0];
            Assert.AreEqual(false, page.Contains(Tag.Artist));
            var name = "Leonardo DaVinci";
            page.Artist = name;

            tif.Save("modified.tif");
            tif = Tif.Load("modified.tif");
            page = tif[0];
            var tag = page[Tag.Artist];
            var values = (char[])tag.Values;
            string value = new String(values);
            Assert.AreEqual(name, value);
        }

        [Fact]
        public void PageNumber()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif[0];
            Assert.AreEqual(false, page.Contains(Tag.PageNumber));
            ushort number = 42;
            ushort total = 666;
            page.PageNumber = number;
            page.PageTotal = total;
            var tag = page[Tag.PageNumber];
            var values = (ushort[])tag.Values;
            Assert.AreEqual(number, values[0]);
            Assert.AreEqual(total, values[1]);
        }

        [Fact]
        public void BitsPerSample()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif[0];

            ushort bitsper = 42;

            page.BitsPerSample = bitsper;

            var tag = page[Tag.BitsPerSample];
            var values = (ushort[])tag.Values;
            Assert.AreEqual(bitsper, values[0]);
        }

        [Fact]
        public void BaselineAsciiFields()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif[0];
            Assert.AreEqual("Adobe Photoshop CS2 Windows", page.Software);
            Assert.AreEqual("2009:04:07 18:33:11", page.DateTime);
        }

        [Fact]
        public void AddTest()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif[0];
            page.Add(Tag.Predictor, Predictor.NoPredictionScheme);

            page.Add(Tag.NewSubfileType, NewSubfileType.Page | NewSubfileType.ReducedResolutionVersion | NewSubfileType.TransparencyMask);
        }

        [Fact]
        public void PageConstructorTest()
        {
            Page target = new Page();
        }

        [Fact]
        public void AddTest2()
        {
            Page target = new Page();
            Tag tagType = new Tag();
            uint value = 0;
            target.Add(tagType, value);
        }

        [Fact]
        public void AddTest3()
        {
            Page target = new Page();
            Tag tagType = new Tag();
            FieldType type = new FieldType();
            string value = string.Empty;
            target.Add(tagType, type, value);
        }

        [Fact]
        public void AddTest3withnull()
        {
            Page target = new Page();
            Tag tagType = new Tag();
            FieldType type = new FieldType();
            string value = null;
            target.Add(tagType, type, value);
        }

        [Fact]
        public void AddTest4()
        {
            Page target = new Page();
            Tag tagType = new Tag();
            URational32 value = new URational32();
            target.Add(tagType, value);
        }

        [Fact]
        public void AddTest5()
        {
            Page target = new Page();
            Tag tagType = new Tag();
            string value = string.Empty;
            target.Add(tagType, value);
        }

        [Fact]
        public void AddTest6()
        {
            Page target = new Page();
            Tag tagType = new Tag();
            ushort value = 0;
            target.Add(tagType, value);
        }

        [Fact]
        public void CopyTest()
        {
            var tif = Tif.Load(testFilePath);
            var original = tif[0];
            var copy = original.Copy();
            Assert.AreEqual(original.Count, copy.Count);
            foreach (var tag in original)
            {
                Assert.AreEqual(true, copy.Contains(tag.Tag));
            }

        }

        [Fact]
        public void ToStringTest()
        {
            Page target = new Page();
            string expected = string.Empty;
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void ArtistTest()
        {
            Page target = new Page();
            string expected = string.Empty;
            string actual;
            target.Artist = expected;
            actual = target.Artist;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void BitsPerSampleTest()
        {
            Page target = new Page();
            ushort expected = 0;
            ushort actual;
            target.BitsPerSample = expected;
            actual = target.BitsPerSample;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void CellLengthTest()
        {
            Page target = new Page();
            ushort expected = 0;
            ushort actual;
            target.CellLength = expected;
            actual = target.CellLength;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void CellWidthTest()
        {
            Page target = new Page();
            ushort expected = 0;
            ushort actual;
            target.CellWidth = expected;
            actual = target.CellWidth;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void ColorMapTest()
        {
            Page target = new Page();
            ushort[] expected = new ushort[] { 0, 0, 0 };
            ushort[] actual;
            target.ColorMap = expected;
            actual = target.ColorMap;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void CompressionTest()
        {
            Page target = new Page();
            Compression expected = new Compression();
            Compression actual;
            target.Compression = expected;
            actual = target.Compression;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void CopyrightTest()
        {
            Page target = new Page();
            string expected = string.Empty;
            string actual;
            target.Copyright = expected;
            actual = target.Copyright;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void DateTimeTest()
        {
            Page target = new Page();
            string expected = string.Empty;
            string actual;
            target.DateTime = expected;
            actual = target.DateTime;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void ExtraSamplesTest()
        {
            Page target = new Page();
            ExtraSampleType[] expected = new ExtraSampleType[] { ExtraSampleType.AssociatedAlphaData, ExtraSampleType.UnassociatedAlphaData, ExtraSampleType.UnspecifiedData };
            ExtraSampleType[] actual;
            target.ExtraSamples = expected;
            actual = target.ExtraSamples;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void FillOrderTest()
        {
            var tif = Tif.Load(testFilePath);
            Page target = tif[0];
            FillOrder expected = FillOrder.LowBitsFirst;
            FillOrder actual;
            target.FillOrder = expected;
            actual = target.FillOrder;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void FreeByteCountsTest()
        {
            Page target = new Page();
            uint expected = 0;
            uint actual;
            target.FreeByteCounts = expected;
            actual = target.FreeByteCounts;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void FreeOffsetsTest()
        {
            Page target = new Page();
            uint expected = 0;
            uint actual;
            target.FreeOffsets = expected;
            actual = target.FreeOffsets;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void GrayResponseCurveTest()
        {
            var tif = Tif.Load(testFilePath);
            Page target = tif[0];
            ushort[] expected = new ushort[8];
            ushort[] actual;
            target.GrayResponseCurve = expected;
            actual = target.GrayResponseCurve;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void GrayResponseUnitTest()
        {
            Page target = new Page();
            GrayResponseUnit expected = new GrayResponseUnit();
            GrayResponseUnit actual;
            target.GrayResponseUnit = expected;
            actual = target.GrayResponseUnit;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void HostComputerTest()
        {
            Page target = new Page();
            string expected = string.Empty;
            string actual;
            target.HostComputer = expected;
            actual = target.HostComputer;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void ImageDataTest()
        {
            Page target = new Page();
            List<byte[]> expected = null;
            List<byte[]> actual;
            target.ImageData = expected;
            actual = target.ImageData;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void ImageDescriptionTest()
        {
            Page target = new Page();
            string expected = string.Empty;
            string actual;
            target.ImageDescription = expected;
            actual = target.ImageDescription;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void ImageLengthTest()
        {
            Page target = new Page();
            uint expected = 0;
            uint actual;
            target.ImageLength = expected;
            actual = target.ImageLength;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void ImageWidthTest()
        {
            Page target = new Page();
            uint expected = 0;
            uint actual;
            target.ImageWidth = expected;
            actual = target.ImageWidth;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void MakeTest()
        {
            Page target = new Page();
            string expected = string.Empty;
            string actual;
            target.Make = expected;
            actual = target.Make;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void MaxSampleValueTest()
        {
            Page target = new Page();
            ushort expected = 0;
            ushort actual;
            target.MaxSampleValue = expected;
            actual = target.MaxSampleValue;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void MinSampleValueTest()
        {
            Page target = new Page();
            ushort expected = 0;
            ushort actual;
            target.MinSampleValue = expected;
            actual = target.MinSampleValue;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void ModelTest()
        {
            Page target = new Page();
            string expected = string.Empty;
            string actual;
            target.Model = expected;
            actual = target.Model;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void NewSubfileTypeTest()
        {
            Page target = new Page();
            NewSubfileType expected = new NewSubfileType();
            NewSubfileType actual;
            target.NewSubfileType = expected;
            actual = target.NewSubfileType;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void NextPageAddressTest()
        {
            Page target = new Page();
            uint expected = 0;
            uint actual;
            target.NextIfdAddress = expected;
            actual = target.NextIfdAddress;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void OrientationTest()
        {
            Page target = new Page();
            Orientation expected = new Orientation();
            Orientation actual;
            target.Orientation = expected;
            actual = target.Orientation;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void PageNumberTest()
        {
            Page target = new Page();
            ushort expected = 0;
            ushort actual;
            target.PageNumber = expected;
            actual = target.PageNumber;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void PageTotalTest()
        {
            Page target = new Page();
            ushort expected = 0;
            ushort actual;
            target.PageTotal = expected;
            actual = target.PageTotal;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void PhotometricInterpretationTest()
        {
            Page target = new Page();
            PhotometricInterpretation expected = new PhotometricInterpretation();
            PhotometricInterpretation actual;
            target.PhotometricInterpretation = expected;
            actual = target.PhotometricInterpretation;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void PlanarConfigurationTest()
        {
            Page target = new Page();
            PlanarConfiguration expected = new PlanarConfiguration();
            PlanarConfiguration actual;
            target.PlanarConfiguration = expected;
            actual = target.PlanarConfiguration;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void ResolutionUnitTest()
        {
            Page target = new Page();
            ResolutionUnit expected = new ResolutionUnit();
            ResolutionUnit actual;
            target.ResolutionUnit = expected;
            actual = target.ResolutionUnit;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void RowsPerStripTest()
        {
            Page target = new Page();
            uint expected = 0;
            uint actual;
            target.RowsPerStrip = expected;
            actual = target.RowsPerStrip;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void SamplesPerPixelTest()
        {
            Page target = new Page();
            ushort expected = 0;
            ushort actual;
            target.SamplesPerPixel = expected;
            actual = target.SamplesPerPixel;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void SoftwareTest()
        {
            Page target = new Page();
            string expected = string.Empty;
            string actual;
            target.Software = expected;
            actual = target.Software;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void StripByteCountsTest()
        {
            var tif = Tif.Load(testFilePath);
            Page target = tif[0];
            uint[] expected = new uint[] { 124416 };
            uint[] actual;
            target.StripByteCounts = expected;
            actual = target.StripByteCounts;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void StripOffsetsTest()
        {
            var tif = Tif.Load(testFilePath);
            Page target = tif[0];
            uint[] expected = new uint[] { 23860 };
            uint[] actual;
            target.StripOffsets = expected;
            actual = target.StripOffsets;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void StripsPerImageTest()
        {
            var tif = Tif.Load(testFilePath);
            Page target = tif[0];
            uint actual;
            actual = target.StripsPerImage;
            Assert.AreEqual((uint)1, actual);
        }

        [Fact]
        public void ThreshholdingTest()
        {
            Page target = new Page();
            Threshholding expected = new Threshholding();
            Threshholding actual;
            target.Threshholding = expected;
            actual = target.Threshholding;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void XResolutionTest()
        {
            Page target = new Page();
            URational32 expected = new URational32();
            URational32 actual;
            target.XResolution = expected;
            actual = target.XResolution;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void YResolutionTest()
        {
            Page target = new Page();
            URational32 expected = new URational32();
            URational32 actual;
            target.YResolution = expected;
            actual = target.YResolution;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void GetAsciiFieldValueTest()
        {
            var tif = Tif.Load(testFilePath);
            var page = tif[0];
            string expected = string.Empty;
            string actual;
            actual = page.GetAsciiFieldValue(Tag.PageName);
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void SetAsciiFieldValueNullTest()
        {
            Page target = new Page();
            Tag tag = Tag.Software;
            string value = null;
            target.SetAsciiFieldValue(tag, value);
            Assert.AreEqual(string.Empty, target.GetAsciiFieldValue(tag));
        }

        [Fact]
        public void ToStringTest1()
        {
            var tif = Tif.Load(testFilePath);
            string foo = tif[0].ToString();
            Assert.AreEqual(false, string.IsNullOrEmpty(foo));
        }

        [Fact]
        public void BitsPerSampleTest1()
        {
            Page target = new Page();
            ushort expected = 1;
            ushort actual;
            actual = target.BitsPerSample;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void CellLengthTest1()
        {
            Page target = new Page();
            ushort expected = 0;
            ushort actual;
            actual = target.CellLength;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void CellWidthTest1()
        {
            Page target = new Page();
            ushort expected = 0;
            ushort actual;
            actual = target.CellWidth;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void ColorMapTest1()
        {
            Page target = new Page();
            ushort[] expected = new ushort[]{ };
            ushort[] actual;
            actual = target.ColorMap;
            CollectionAssert.AreEqual(expected, actual);
        }

        [Fact]
        public void CompressionTest1()
        {
            Page target = new Page();
            Compression expected = Compression.NoCompression;
            Compression actual;
            actual = target.Compression;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void ExtraSamplesTest1()
        {
            Page target = new Page();
            ExtraSampleType[] expected = new ExtraSampleType[]{ };
            ExtraSampleType[] actual;
            actual = target.ExtraSamples;
            CollectionAssert.AreEqual(expected, actual);
        }

        [Fact]
        public void FillOrderTest1()
        {
            Page target = new Page();
            FillOrder expected = FillOrder.LowBitsFirst;
            FillOrder actual;
            actual = target.FillOrder;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void FreeByteCountsTest1()
        {
            Page target = new Page();
            uint expected = 0;
            uint actual;
            actual = target.FreeByteCounts;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void FreeOffsetsTest1()
        {
            Page target = new Page();
            uint expected = 0;
            uint actual;
            actual = target.FreeOffsets;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void GrayResponseCurveTest1()
        {
            Page target = new Page();
            ushort[] expected = new ushort[] { };
            ushort[] actual;
            actual = target.GrayResponseCurve;
            CollectionAssert.AreEqual(expected, actual);
        }

        [Fact]
        public void GrayResponseUnitTest1()
        {
            Page target = new Page();
            GrayResponseUnit expected = GrayResponseUnit.Hundredths;
            GrayResponseUnit actual;
            actual = target.GrayResponseUnit;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void ImageLengthTest1()
        {
            Page target = new Page();
            uint expected = 0;
            uint actual;
            actual = target.ImageLength;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void ImageWidthTest1()
        {
            Page target = new Page();
            uint expected = 0;
            uint actual;
            actual = target.ImageWidth;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void MaxSampleValueTest1()
        {
            Page target = new Page();
            ushort expected = 1;
            ushort actual;
            actual = target.MaxSampleValue;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void MinSampleValueTest1()
        {
            Page target = new Page();
            ushort expected = 0;
            ushort actual;
            actual = target.MinSampleValue;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void NewSubfileTypeTest1()
        {
            Page target = new Page();
            NewSubfileType expected = new NewSubfileType();
            NewSubfileType actual;
            actual = target.NewSubfileType;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void OrientationTest1()
        {
            Page target = new Page();
            Orientation expected = Orientation.TopLeft;
            Orientation actual;
            actual = target.Orientation;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void PageNumberTest1()
        {
            Page target = new Page();
            ushort expected = 0;
            ushort actual;
            actual = target.PageNumber;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void PageTotalTest1()
        {
            Page target = new Page();
            ushort expected = 0;
            ushort actual;
            actual = target.PageTotal;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void PhotometricInterpretationTest1()
        {
            Page target = new Page();
            PhotometricInterpretation expected = new PhotometricInterpretation();
            PhotometricInterpretation actual;
            actual = target.PhotometricInterpretation;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void PlanarConfigurationTest1()
        {
            Page target = new Page();
            PlanarConfiguration expected = PlanarConfiguration.Chunky;
            PlanarConfiguration actual;
            actual = target.PlanarConfiguration;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void ResolutionUnitTest1()
        {
            Page target = new Page();
            ResolutionUnit expected = ResolutionUnit.Inch;
            ResolutionUnit actual;
            actual = target.ResolutionUnit;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void RowsPerStripTest1()
        {
            Page target = new Page(); 
            uint expected = uint.MaxValue; 
            uint actual;
            actual = target.RowsPerStrip;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void SamplesPerPixelTest1()
        {
            Page target = new Page(); 
            ushort expected = 1; 
            ushort actual;
            actual = target.SamplesPerPixel;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void StripByteCountsTest1()
        {
            Page target = new Page(); 
            uint[] expected = new uint[]{ }; 
            uint[] actual;
            actual = target.StripByteCounts;
            CollectionAssert.AreEqual(expected, actual);
        }

        [Fact]
        public void StripOffsetsTest1()
        {
            Page target = new Page(); 
            uint[] expected = new uint[]{ }; 
            uint[] actual;
            actual = target.StripOffsets;
            CollectionAssert.AreEqual(expected, actual);
        }

        [Fact]
        public void ThreshholdingTest1()
        {
            Page target = new Page();
            Threshholding expected = Threshholding.None;
            Threshholding actual;
            actual = target.Threshholding;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void XResolutionTest1()
        {
            Page target = new Page(); 
            URational32 expected = new URational32(); 
            URational32 actual;
            actual = target.XResolution;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void YResolutionTest1()
        {
            Page target = new Page(); 
            URational32 expected = new URational32(); 
            URational32 actual;
            actual = target.YResolution;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void PageNumberTest2()
        {
            Page target = new Page();
            Assert.AreEqual(target.PageNumber, 0);
            Assert.AreEqual(target.PageTotal, 0);
            target.PageNumber = 5;
            target.PageTotal = 7;
            Assert.AreEqual(target.PageNumber, 5);
            Assert.AreEqual(target.PageTotal, 7);
            target.PageTotal = 8;
            Assert.AreEqual(target.PageNumber, 5);
            Assert.AreEqual(target.PageTotal, 8);
            target.PageNumber = 3;
            Assert.AreEqual(target.PageNumber, 3);
            Assert.AreEqual(target.PageTotal, 8);
        }

        [Fact]
        public void StripByteCountsTest2()
        {
            Page target = new Page(); 
            uint[] expected = new uint[]{ }; 
            uint[] actual;
            target.StripByteCounts = null;
            actual = target.StripByteCounts;
            CollectionAssert.AreEqual(expected, actual);
        }

        [Fact]
        public void StripOffsetsTest2()
        {
            Page target = new Page();
            uint[] expected = new uint[] { };
            uint[] actual;
            target.StripOffsets = null;
            actual = target.StripOffsets;
            CollectionAssert.AreEqual(expected, actual);
        }

        [Fact]
        public void XResolutionTest2()
        {
            Page target = new Page(); 
            URational32 expected = new URational32(); 
            URational32 actual;
            actual = target.XResolution;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void YResolutionTest2()
        {
            Page target = new Page(); 
            URational32 expected = new URational32(); 
            URational32 actual;

            actual = target.YResolution;
            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void ExtraSamplesTest2()
        {
            Page target = new Page(); 
            ExtraSampleType[] expected = new ExtraSampleType[]{ }; 
            ExtraSampleType[] actual;
            target.ExtraSamples = null;
            actual = target.ExtraSamples;
            CollectionAssert.AreEqual(expected, actual);
        }

        [Fact]
        public void PageNumberTest3()
        {
            Page target = new Page(); 
            ushort expected = 0; 
            ushort actual;
            actual = target.PageNumber;
            Assert.AreEqual(expected, actual);
        }
    }
}
