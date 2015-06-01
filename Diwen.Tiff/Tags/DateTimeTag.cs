using System;
using System.Globalization;

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
            : base(TagType.DateTime, value.ToString("yyyy:MM:dd hh:mm:ss",CultureInfo.InvariantCulture))
        {
        }

    }
}