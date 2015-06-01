namespace Diwen.Tiff
{
    using System;

    [Flags()]
    public enum T6Options : uint
    {
        None = 0,
        AllowUncompressed = 2,
    }
}
