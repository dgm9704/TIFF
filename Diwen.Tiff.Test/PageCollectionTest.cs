// Copyright (C) 2005-2018 by John Nordberg <john.nordberg@gmail.com>
// Free Public License 1.0.0

// Permission to use, copy, modify, and/or distribute this software for any purpose with or without fee is hereby granted.

// THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES
// OF MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR
// ANY DAMAGES WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS
// ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.

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
