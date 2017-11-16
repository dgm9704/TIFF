// namespace Diwen.Tiff.Test
// {
//     using System.Drawing;
//     using System.IO;
//     using System.Reflection;
//     using Diwen.Tiff.Tags;
//     using System.Linq;
//     using System;
//     using Xunit;

//     public class TiffFileTest
//     {

//         [Fact]
//         public void CopyAndModifyPages()
//         {
//             var newTif = new TiffFile();
//             var tif = TiffFile.Load(Path.Combine("testfiles","lena_kodak.tif"));
//             newTif.Add(tif[0]);
//             newTif.Add(tif[0]);

//             var values = new ushort[] { 0, 2 };
//             var tag = new Tag(TagType.PageNumber, DataType.Short, values);
//             newTif[0].Add(tag);
//             tag = new Tag(TagType.PageNumber, DataType.Short, new ushort[] { 1, 2 });
//             newTif[1].Add(tag);

//             newTif.Save(Path.Combine("output","copiedpages_2.tif"));
//         }


//         [Fact]
//         public void CombinePagesAndModifyTags()
//         {
//             var newFile = new Tif();
//             var files = Directory.GetFiles(Path.Combine("testfiles","img"));

//             Tag swTag = new SoftwareTag("Diwen.Tiff");

//             //var comp = new CompressionTag(Compression.CCITT4);
//             var comp = new Tag(TagType.Compression, DataType.Ascii, new[]{ FieldValues.Compression.CCITT4});

//             for (int i = 0; i < files.Length; i++)
//             {
//                 var oldFile = Tif.Load(files[i]);
//                 Page page = oldFile[0];
//                 page.Add(swTag);
//                 Tag pageNumber = new PageNumberTag((ushort)i, (ushort)files.Length);
//                 Tag artist = new AsciiTag(TagType.Artist, "John Nordberg");
//                 var subfile = new SubfileTypeTag(TagValues.SubfileType.Page);
//                 var tags = new Tag[] { pageNumber, artist, new DateTimeTag(), subfile };
//                 page.AddRange(tags);
//                 newFile.Add(page);
//             }

//             newFile.Save(Path.Combine("output","combined_and_modified.tif"));
//         }

//         [Fact]
//         public void Tagging()
//         {
//             var files = Directory.GetFiles(Path.Combine("testfiles","img"));
//             var tif = Tif.Load(files[0]);
//             Page page = tif[0];
//             page.Add(new SoftwareTag("Diwen.Tiff"));

//             var tag = page[TagType.StripByteCounts];

//             tif.Save(Path.Combine("output","tagged.tif"));
//         }
//     }
// }