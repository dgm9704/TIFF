namespace Diwen.Tiff.Test
{
    using Diwen.Tiff;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class PageCollectionTest
    {
        [Fact]
        public void PageCollectionConstructorTest()
        {
            PageCollection target = new PageCollection();
        }

        [Fact]
        public void AddTest()
        {
            PageCollection target = new PageCollection();
            Page page = new Page();
            target.Add(page);
        }

        [Fact]
        public void AddRangeTest()
        {
            PageCollection target = new PageCollection();
            IEnumerable<Page> pages = new List<Page> { new Page(), new Page(), new Page() };
            target.AddRange(pages);
        }

        [Fact]
        public void ToStringTest()
        {
            PageCollection target = new PageCollection();
            string expected = string.Empty;
            string actual = target.ToString();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddTest1()
        {
            PageCollection target = new PageCollection();
            Page page = null;
            Assert.Throws<ArgumentNullException>(() => target.Add(page));
        }

        [Fact]
        public void AddRangeTest1()
        {
            PageCollection target = new PageCollection();
            IEnumerable<Page> pages = null;
            Assert.Throws<ArgumentNullException>(() => target.AddRange(pages));
        }

        [Fact]
        public void ToStringTest1()
        {
            PageCollection target = new PageCollection { new Page { new Field(TagType.Software, FieldType.Ascii, new string[] { "Diwen.Tiff" }) } };
            Assert.False(string.IsNullOrEmpty(target.ToString()));
        }
    }
}
