using System;

namespace Diwen.Tiff.Tags
{
    [Serializable()]
    public class SoftwareTag : AsciiTag
    {
        public SoftwareTag(string value)
            : base(TagType.Software, value)
        {
        }

    }
}
