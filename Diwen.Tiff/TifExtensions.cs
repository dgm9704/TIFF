namespace Diwen.Tiff
{
    public static class TifExtensions
    {
        public static void SetPageNumbers(this Tif tif)
        {
            ushort total = (ushort)tif.Count;
            for (ushort i = 0; i < total; i++)
            {
                tif[i].PageNumber = i;
                tif[i].PageTotal = total;
            }
        }
    }
}
