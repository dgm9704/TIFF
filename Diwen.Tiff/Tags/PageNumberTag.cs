namespace Diwen.Tiff.Tags
{
    using System;

    [Serializable]
    public class PageNumberTag : ShortTag
    {
        public PageNumberTag(ushort pageNumber, ushort totalPages)
            : base(pageNumber, totalPages)
        {
            this.TagType = TagType.PageNumber;
        }

    }
}
