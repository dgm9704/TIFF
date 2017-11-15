namespace Diwen.Tiff.Tags
{
    using System;

    [Serializable()]
    [Obsolete("NewSubfileTypeTag should be used instead.")]
    public class SubfileTypeTag : ShortTag
    {
        public SubfileTypeTag(TagValues.SubfileType type)
            : base((ushort)type)
        {
            this.Tag = Tag.SubfileType;
        }
    }
}
