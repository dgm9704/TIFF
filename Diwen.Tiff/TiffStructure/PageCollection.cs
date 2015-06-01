namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;

    [Serializable()]
    public class PageCollection : Collection<Page>
    {
        public PageCollection()
            : base(new List<Page>())
        {
        }

        public new void Add(Page page)
        {
            if (page == null)
            {
                throw new ArgumentNullException("page");
            }

            Items.Add(page.Copy());
        }

        public void AddRange(IEnumerable<Page> pages)
        {
            if (pages == null)
            {
                throw new ArgumentNullException("pages");
            }

            foreach (var page in pages)
            {
                this.Add(page);
            }
        }

        /// <summary>
        /// Returns a String with information about the Tif, it's pages and their tags
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var page in this)
            {
                sb.Append(page.ToString());
            }

            return sb.ToString();
        }
    }
}
