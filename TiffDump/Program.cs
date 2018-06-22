// Copyright (C) 2005-2018 by John Nordberg <john.nordberg@gmail.com>
// Free Public License 1.0.0

// Permission to use, copy, modify, and/or distribute this software for any purpose with or without fee is hereby granted.

// THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES
// OF MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR
// ANY DAMAGES WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS
// ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.

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
