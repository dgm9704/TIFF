namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Diwen.Tiff.TagValues;

    /// <summary>
    /// Represents a collection of tags
    /// </summary>
    [Serializable()]
    public class TagCollection : KeyedCollection<TagType, TiffTag>
    {
        internal TagCollection() : base(null, 0) 
        { 
        }

        //public Tag this[TagType tagType]
        //{
        //    get
        //    {
        //        return base[tagType];
        //    }

        //    set
        //    {
        //        base.Insert(0, value);
        //    }
        //}

        /// <summary>
        /// Adds a new tag to the collection
        /// </summary>
        /// <param name="item"></param>
        public new void Add(TiffTag item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.Remove(item.TagType);
            base.Add(item);
        }

        /// <summary>
        /// Adds a range of tags to collection
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<TiffTag> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            foreach (var item in items)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// Sorts the tags in the collection
        /// </summary>
        public void Sort()
        {
            ((List<TiffTag>)Items).Sort((t1, t2) => { return t1.TagType.CompareTo(t2.TagType); });
        }

        /// <summary>
        /// Returns the key of an item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override TagType GetKeyForItem(TiffTag item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return item.TagType;
        }
    }
}
