// Copyright (C) 2005-2017 by John Nordberg <john.nordberg@gmail.com>
// Permission to use, copy, modify, and/or distribute this software for any purpose with or without fee is hereby granted. 

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
