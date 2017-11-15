namespace Diwen.Tiff.Tags
{
    using System;

    [Serializable()]
    class SubFileTypeTag : ShortTag
    {
        public SubFileTypeTag(FieldValues.SubfileType type)
            : base((ushort)type)
        {
            this.Tag = Tag.SubfileType;
        }
    }
}
