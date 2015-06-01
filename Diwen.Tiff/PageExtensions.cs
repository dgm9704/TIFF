using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Diwen.Tiff
{
    public static class PageExtensions
    {
        /// <summary>
        /// Adds a new tag to the page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="tag">tag type</param>
        /// <param name="value">field data</param>
        public static void Add(this Page page, Tag tag, ushort value)
        {
            page.Add(tag, FieldType.Short, new ushort[] { value });
        }

        /// <summary>
        /// Adds a new tag to the page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="tag">tag type</param>
        /// <param name="value">field data</param>
        public static void Add(this Page page, Tag tag, uint value)
        {
            page.Add(tag, FieldType.Long, new uint[] { value });
        }

        /// <summary>
        /// Adds a new tag to the page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="tag">tag type</param>
        /// <param name="value">field data</param>
        public static void Add(this Page page, Tag tag, Enum value)
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

        /// <summary>
        /// Adds a new tag to the page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="tag">tag type</param>
        /// <param name="type">type of data contained in the field</param>
        /// <param name="value">field data</param>
        public static void Add(this Page page, Tag tag, FieldType type, string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            page.Add(tag, type, value.ToCharArray());
        }

        /// <summary>
        /// Adds a new tag to the page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="tag">tag type</param>
        /// <param name="value">field data</param>
        public static void Add(this Page page, Tag tag, string value)
        {
            page.Add(tag, FieldType.Ascii, value);
        }

        /// <summary>
        /// Adds a new tag to the page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="tag">tag type</param>
        /// <param name="value">field data</param>
        public static void Add(this Page page, Tag tag, URational32 value)
        {
            page.Add(tag, FieldType.Rational, new URational32[] { value });
        }

        /// <summary>
        /// Creates a deep copy of the Page object
        /// </summary>
        /// <returns></returns>
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


    }
}
