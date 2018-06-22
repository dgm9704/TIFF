//
//  This file is part of Diwen.Tiff.
//
//  Author:
//       John Nordberg <john.nordberg@gmail.com>
//
//  Copyright (c) 2005-2018 John Nordberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
