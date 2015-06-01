namespace Diwen.Tiff.FieldValues
{
    public enum Compression : ushort
    {
        NoCompression = 1,
        CCITTRLE = 2,
        CCITT3 = 3,
        CCITT4 = 4,
        LZW = 5,
        OJPEG = 6,
        JPEG = 7,
        AdobeDeflateZlib = 8,
        Next = 32766,
        CCITTRLEW = 32771,
        PackBits = 32773,
        Thunderscan = 32809,
        IT8CTPAD = 32895,
        IT8LW = 32896,
        IT8MP = 32897,
        IT8BL = 32898,
        PixarFilm = 32908,
        PixarLog = 32909,
        Deflate = 32946,
        DCS = 32947,
        JBIG = 34661,
        SGILOG = 34676,
        SGILOG24 = 34677,
        JP2000 = 34712,
    }
}
