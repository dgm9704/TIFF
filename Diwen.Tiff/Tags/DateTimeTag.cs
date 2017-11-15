namespace Diwen.Tiff.Tags
{
    using System;
    using System.Globalization;

    [Serializable()]
    public class DateTimeTag : AsciiTag
    {
        public DateTimeTag()
            : this(DateTime.Now)
        {
        }

        public DateTimeTag(DateTime value)
            : base(Tag.DateTime, value.ToString("yyyy:MM:dd hh:mm:ss", CultureInfo.InvariantCulture))
        {
        }

    }
}