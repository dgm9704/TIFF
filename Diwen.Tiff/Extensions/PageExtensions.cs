//
//  This file is part of Diwen.Tiff.
//
//  Author:
//       John Nordberg <john.nordberg@gmail.com>
//
//  Copyright (c) 2005-2017 John Nordberg
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

namespace Diwen.Tiff
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public static class PageExtensions
    {
        public static void Add(this Page page, TagType tag, ushort value)
        => page.Add(tag, FieldType.Short, new ushort[] { value });

        public static void Add(this Page page, TagType tag, uint value)
        => page.Add(tag, FieldType.Long, new uint[] { value });

        public static void Add(this Page page, TagType tag, Enum value)
        {
            Type underType = Enum.GetUnderlyingType(value.GetType());

            switch (underType.Name)
            {
                case "UInt16":
                    page.Add(tag, (ushort)Convert.ChangeType(value, underType));
                    break;

                case "UInt32":
                default:
                    page.Add(tag, (uint)Convert.ChangeType(value, underType));
                    break;
            }
        }

        public static void Add(this Page page, TagType tag, FieldType type, string value)
        => page.Add(tag, type, (value ?? "").ToCharArray());

        public static void Add(this Page page, TagType tag, string value)
        => page.Add(tag, FieldType.Ascii, value);

        public static void Add(this Page page, TagType tag, URational32 value)
        => page.Add(tag, FieldType.Rational, new URational32[] { value });

        public static Page Copy(this Page page)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, page);
                stream.Position = 0;
                return (Page)formatter.Deserialize(stream);
            }
        }

        public static T[] TagValuesOrDefault<T>(this Page page, TagType tagType, T[] defaultValue)
        => page.Contains(tagType)
            ? (T[])page[tagType].Values
            : defaultValue;

        public static T TagValueOrDefault<T>(this Page page, TagType tagType, T defaultValue)
        => page.Contains(tagType)
            ? (T)page[tagType].Value
            : defaultValue;

        public static T TagValueOrDefault<T>(this Page page, TagType tagType)
        => page.Contains(tagType)
            ? (T)page[tagType].Value
            : default(T);

    }
}
