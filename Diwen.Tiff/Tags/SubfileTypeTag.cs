namespace Diwen.Tiff.Tags
{
    using System;
    using Diwen.Tiff.TagValues;

    [Serializable()]
    [Obsolete("NewSubfileTypeTag should be used instead.")]
    public class SubfileTypeTag : ShortTag
    {
        public SubfileTypeTag(SubfileType type)
            : base((ushort)type)
        {
            this.TagType = TagType.SubfileType;
        }
    }
}
