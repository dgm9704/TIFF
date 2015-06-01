using System;

namespace Diwen.Tiff.Tags
{
    [Serializable()]
    public class DateTimeTag : AsciiTag
    {
        public DateTimeTag()
            : this(DateTime.Now)
        {
        }

        public DateTimeTag(DateTime value)
            : base(value.ToString("yyyy:MM:dd hh:mm:ss"))
        {
            this.TagType = TagType.DateTime;
        }

    }
}