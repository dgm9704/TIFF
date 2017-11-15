namespace Diwen.Tiff.Tags
{
    using System;
    using Diwen.Tiff.TagValues;

    [Serializable()]
    class SubFileTypeTag : ShortTag
    {
        public SubFileTypeTag(SubfileType type)
            : base((ushort)type)
        {
            this.TagType = TagType.SubfileType;
        }
    }
}
