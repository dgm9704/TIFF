using System;

namespace Diwen.Tiff.Tags
{
    [Serializable()]
    public class SoftwareTag : AsciiTag
    {
        public SoftwareTag(string value)
            : base(value)
        {
            this.TagType = TagType.Software;
       }

    }
}
