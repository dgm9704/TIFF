﻿//
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

namespace Diwen.Tiff.Tags
{
    using System;

    [Serializable]
    public class AsciiTag : Tag
    {
        public AsciiTag(TagType type)
        {
            this.FieldType = FieldType.Ascii;
            this.TagType = type;
        }

        public AsciiTag(TagType type, string value)
            : this(type)
        {
            this.Values = (value + '\0').ToCharArray();
            this.ValueCount = (uint)Values.Length;
        }
    }
}
