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

namespace Diwen.Tiff
{
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    [Serializable]
    public class Tag : IComparable<Tag>
    {
        public Array Values { get; internal set; }
        public TagType TagType { get; set; }
        public FieldType FieldType { get; set; }
        internal uint ValueCount { get; set; }
        internal uint ValueOffset { get; set; }

        public Tag() { }

        public Tag(TagType tagType, FieldType fieldType, Array values)
        {
            TagType = tagType;
            FieldType = fieldType;
            Values = values;
        }

        internal static Tag Read(byte[] data, int startPosition)
        {
            var tag = new Tag
            {
                TagType = (TagType)BitConverter.ToUInt16(data, startPosition),
                FieldType = (FieldType)BitConverter.ToUInt16(data, startPosition + 2),
                ValueCount = BitConverter.ToUInt32(data, startPosition + 4),
                ValueOffset = BitConverter.ToUInt32(data, startPosition + 8),
            };

            byte[] valuebytes = GetValueBytes(data, tag);
            tag.Values = Tif.ReadValues(valuebytes, tag.FieldType, (int)tag.ValueCount);

            return tag;
        }

        internal static Tag ReadMM(byte[] data, int startPosition)
        {
            var tag = new Tag
            {
                TagType = (TagType)(ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(data, startPosition)),
                FieldType = (FieldType)(ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(data, startPosition + 2)),
                ValueCount = (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, startPosition + 4)),

            };
            tag.ValueOffset = GetValueOffset(data, startPosition, tag.ValueCount, tag.FieldType);

            byte[] valuebytes = Tif.SwitchEndian(GetValueBytes(data, tag), Tif.ValueLength[tag.FieldType]);

            tag.Values = Tif.ReadValues(valuebytes, tag.FieldType, (int)tag.ValueCount);

            return tag;
        }

        private static uint GetValueOffset(byte[] data, int startPosition, uint valueCount, FieldType fieldType)
        => (int)valueCount * Tif.ValueLength[fieldType] > 4
            ? (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, startPosition + 8))
            : (uint)BitConverter.ToInt32(data, startPosition + 8);

        private static byte[] GetValueBytes(byte[] data, Tag tag)
        {
            byte[] valuebytes;
            int valueLength = (int)tag.ValueCount * Tif.ValueLength[tag.FieldType];
            if (valueLength > 4)
            {
                valuebytes = new byte[valueLength];
                Buffer.BlockCopy(data, (int)tag.ValueOffset, valuebytes, 0, valueLength);
            }
            else
            {
                valuebytes = BitConverter.GetBytes(tag.ValueOffset);
            }
            return valuebytes;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"{TagType:D}({TagType})");
            sb.Append("[");

            if (this.FieldType == FieldType.Ascii)
            {
                sb.Append(Values as char[]);
                sb.Append("]");
                return sb.ToString();
            }

            foreach (var value in this.Values)
            {
                string v = value.ToString();
                Enum temp = EnumeratedTagValue("Tiff.TagValues." + this.TagType, v);
                if (temp != null)
                    v = temp.ToString();
                sb.Append(v);
                sb.Append(",");
            }
            return sb.ToString().Remove(sb.Length - 1) + "]";
        }

        internal static Enum EnumeratedTagValue(string name, string value)
        => EnumeratedTagValue(Type.GetType(name), value);

        private static Enum EnumeratedTagValue(Type enumType, string value)
        => enumType != null
            ? (Enum)Enum.Parse(enumType, value)
            : null;

        public Tag Copy()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (Tag)formatter.Deserialize(stream);
            }
        }

        #region comparison and operators

        public int CompareTo(Tag other)
        => TagType < other.TagType
            ? -1
            : TagType > other.TagType
                ? 1
                : 0;

        public override bool Equals(object obj)
        => this.CompareTo(obj as Tag) == 0;

        public bool Equals(Tag other)
        => CompareTo(other) == 0;

        public override int GetHashCode()
        => (int)TagType;

        public static bool operator ==(Tag tag1, Tag tag2)
        => tag1.CompareTo(tag2) == 0;

        public static bool operator !=(Tag tag1, Tag tag2)
        => !(tag1 == tag2);

        public static bool operator <(Tag tag1, Tag tag2)
        => (tag1.CompareTo(tag2) < 0);

        public static bool operator >(Tag tag1, Tag tag2)
        => (tag1.CompareTo(tag2) > 0);

        #endregion
    }
}