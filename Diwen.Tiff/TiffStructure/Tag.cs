using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Diwen.Tiff;

namespace Diwen.Tiff
{
    [Serializable]
    public class Tag //: IComparable<Tag>
    {
        public Array Values { get; internal set; }

        public TagType TagType { get; set; }

        public TiffDataType DataType { get; set; }

        internal uint ValueCount { get; set; }

        internal uint ValueOffset { get; set; }

        public Tag()
        {
        }

        public Tag(TagType tagType, TiffDataType dataType, Array values)
        {
            this.TagType = tagType;
            this.DataType = dataType;
            this.Values = values;
            this.ValueCount = (uint)values.Length;
        }

        internal static Tag Read(byte[] data, int startPosition)
        {
            var tag = new Tag
            {
                TagType = (TagType)BitConverter.ToUInt16(data, startPosition),
                DataType = (TiffDataType)BitConverter.ToUInt16(data, startPosition + 2),
                ValueCount = BitConverter.ToUInt32(data, startPosition + 4),
                ValueOffset = BitConverter.ToUInt32(data, startPosition + 8),
            };

            byte[] valuebytes = GetValueBytes(data, tag);
            tag.Values = Tif.ReadValues(valuebytes, tag.DataType, (int)tag.ValueCount);

            return tag;
        }


        internal static Tag ReadMM(byte[] data, int startPosition)
        {
            var tag = new Tag
            {
                TagType = (TagType)(ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(data, startPosition)),
                DataType = (TiffDataType)(ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(data, startPosition + 2)),
                ValueCount = (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, startPosition + 4)),

            };
            if ((int)tag.ValueCount * Tif.ValueLength[tag.DataType] > 4)
                tag.ValueOffset = (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, startPosition + 8));
            else
                tag.ValueOffset = (uint)BitConverter.ToInt32(data, startPosition + 8);

            byte[] valuebytes = Tif.SwitchEndian(GetValueBytes(data, tag), Tif.ValueLength[tag.DataType]);

            tag.Values = Tif.ReadValues(valuebytes, tag.DataType, (int)tag.ValueCount);

            return tag;
        }

        private static byte[] GetValueBytes(byte[] data, Tag tag)
        {
            byte[] valuebytes;
            int valueLength = (int)tag.ValueCount * Tif.ValueLength[tag.DataType];
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
            sb.AppendFormat("{0:D}({0})", this.TagType);
            sb.Append("[");

            if (this.DataType == TiffDataType.Ascii)
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
        {
            Type enumType = System.Type.GetType(name);

            if (enumType != null)
                return (Enum)Enum.Parse(enumType, value);
            else
                return null;
        }

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

        //#region comparison and operators

        //public int CompareTo(Tag other)
        //{
        //    if (this.TagType < other.TagType)
        //        return -1;

        //    if (this.TagType > other.TagType)
        //        return 1;

        //    return 0;
        //}

        //public override bool Equals(object obj)
        //{
        //    var tag = obj as Tag;

        //    if (obj == null)
        //        return false;
        //    return (this.CompareTo(tag) == 0);
        //}

        //public override int GetHashCode()
        //{
        //    return (int)this.TagType;
        //}

        //public static bool operator ==(Tag tag1, Tag tag2)
        //{
        //    try
        //    {
        //        return tag1.CompareTo(tag2) == 0;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return false;
        //}

        //public static bool operator !=(Tag tag1, Tag tag2)
        //{
        //    return !(tag1 == tag2);
        //}

        //public static bool operator <(Tag tag1, Tag tag2)
        //{
        //    return (tag1.CompareTo(tag2) < 0);
        //}

        //public static bool operator >(Tag tag1, Tag tag2)
        //{
        //    return (tag1.CompareTo(tag2) > 0);
        //}

        //#endregion
    }
}