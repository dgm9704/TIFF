namespace Diwen.Tiff.Test
{
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using Diwen.Tiff.Tags;
    using System.Linq;
    using System;
    using Diwen.Tiff.TagValues;
    using Xunit;

    public class TiffFileTest
    {

        [Fact]
        public void CopyAndModifyPages()
        {
            var newTif = new Tif();
            var tif = Tif.Load(@"C:\lena_kodak.tif");
            newTif.Pages.Add(tif.Pages[0]);
            newTif.Pages.Add(tif.Pages[0]);

            var values = new ushort[] { 0, 2 };
            var tag = new Tag(TagType.PageNumber, DataType.Short, values);
            newTif.Pages[0].Tags.Add(tag);
            tag = new TiffTag(TagType.PageNumber, DataType.Short, new ushort[] { 1, 2 });
            newTif.Pages[1].Tags.Add(tag);

            newTif.Save(@"c:\copiedpages_2.tif");
        }


        [Fact]
        public void CombinePagesAndModifyTags()
        {
            var newFile = new Tif();
            var files = Directory.GetFiles(@"c:\img");

            TiffTag swTag = new SoftwareTag("Diwen.Tiff");

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

        [Fact]
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