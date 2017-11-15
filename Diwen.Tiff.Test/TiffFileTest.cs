namespace Diwen.Tiff.Test
{
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using Diwen.Tiff.Tags;
    using System.Linq;
    using System;
    using Xunit;

    public class TiffFileTest
    {

        [Fact]
        public void CopyAndModifyPages()
        {
            var newTif = new TiffFile();
            var tif = TiffFile.Load(@"C:\lena_kodak.tif");
            newTif.Add(tif[0]);
            newTif.Add(tif[0]);

            var values = new ushort[] { 0, 2 };
            var tag = new TiffTag(Tag.PageNumber, TiffDataType.Short, values);
            newTif[0].Add(tag);
            tag = new TiffTag(Tag.PageNumber, TiffDataType.Short, new ushort[] { 1, 2 });
            newTif[1].Add(tag);

            newTif.Save(@"c:\copiedpages_2.tif");
        }


        [Fact]
        public void CombinePagesAndModifyTags()
        {
            var newFile = new Tif();
            var files = Directory.GetFiles(@"c:\img");

            TiffTag swTag = new SoftwareTag("Diwen.Tiff");

            //var comp = new CompressionTag(Compression.CCITT4);
            var comp = new TiffTag(Tag.Compression, TiffDataType.Ascii, new[]{ FieldValues.Compression.CCITT4});

            for (int i = 0; i < files.Length; i++)
            {
                var oldFile = Tif.Load(files[i]);
                Page page = oldFile[0];
                page.Add(swTag);
                TiffTag pageNumber = new PageNumberTag((ushort)i, (ushort)files.Length);
                TiffTag artist = new AsciiTag(Tag.Artist, "John Nordberg");
                var subfile = new SubfileTypeTag(TagValues.SubfileType.Page);
                var tags = new TiffTag[] { pageNumber, artist, new DateTimeTag(), subfile };
                page.AddRange(tags);
                newFile.Add(page);
            }

            newFile.Save(@"c:\combined_and_modified.tif");
        }

        [Fact]
        public void Tagging()
        {
            var files = Directory.GetFiles(@"c:\img");
            var tif = Tif.Load(files[0]);
            Page page = tif[0];
            page.Add(new SoftwareTag("Diwen.Tiff"));

            var tag = page[Tag.StripByteCounts];


            tif.Save(@"c:\tagged.tif");
        }
    }
}