namespace TiffDump
{
    using Diwen.Tiff;
    using System;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            if (args[0].EndsWith("tif", StringComparison.InvariantCultureIgnoreCase))
            {
                Dump(args[0]);
            }
            else
            {
                foreach (var f in Directory.GetFiles(args[0], "*.tif", SearchOption.TopDirectoryOnly))
                {
                    Dump(f);
                }
            }
        }

        private static void Dump(string file)
        {
            try
            {
                Console.WriteLine(Tif.Load(file));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Exception loading file: " + file);
                Console.Error.WriteLine(ex.ToString());
            }
        }
    }
}
