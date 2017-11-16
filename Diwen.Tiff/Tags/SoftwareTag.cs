namespace Diwen.Tiff.Tags
{
    using System;

    [Serializable]
    public class SoftwareTag : AsciiTag
    {
        public SoftwareTag(string value)
            : base(TagType.Software, value)
        {
        }

    }
}
