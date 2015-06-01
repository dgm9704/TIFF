using System;

namespace Diwen.Tiff.Tags
{
    [Serializable()]
    public class PageNumberTag : ShortTag
    {
        public PageNumberTag(ushort pageNumber, ushort totalPages)
            : base(pageNumber, totalPages)
        {
            this.TagType = Tiff.TagType.PageNumber;
        }

    }
}
