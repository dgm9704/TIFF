namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;

    [Serializable]
    public class PageCollection : Collection<Page>
    {
        public PageCollection() : base(new List<Page>()) { }

        public new void Add(Page page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            Items.Add(page.Copy());
        }

        public void AddRange(IEnumerable<Page> pages)
        {
            if (pages == null)
                throw new ArgumentNullException(nameof(pages));

            foreach (var page in pages)
                this.Add(page);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var page in this)
            {
                sb.AppendLine(page.ToString());
            }
            return sb.ToString();
        }
    }
}
