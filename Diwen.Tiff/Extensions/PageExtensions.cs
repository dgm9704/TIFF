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
        {
            if (value == null)
            {
                value = string.Empty;
            }

            page.Add(tag, type, value.ToCharArray());
        }

        public static void Add(this Page page, TagType tag, string value)
        {
            page.Add(tag, FieldType.Ascii, value);
        }

        public static void Add(this Page page, TagType tag, URational32 value)
        {
            page.Add(tag, FieldType.Rational, new URational32[] { value });
        }

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
