namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents a collection of tags
    /// </summary>
    [Serializable()]
    public class FieldCollection : KeyedCollection<Tag, Field>
    {
        internal FieldCollection() : base(null, 0) 
        { 
        }

        //public Tag this[Tag tag]
        //{
        //    get
        //    {
        //        return base[tag];
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
        public new void Add(Field item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.Remove(item.Tag);
            base.Add(item);
        }

        /// <summary>
        /// Adds a range of tags to collection
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<Field> items)
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
        /// Sorts the fields in the collection
        /// </summary>
        public void Sort()
        {
            ((List<Field>)Items).Sort((t1, t2) => { return t1.Tag.CompareTo(t2.Tag); });
        }

        /// <summary>
        /// Returns the key of an item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override Tag GetKeyForItem(Field item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return item.Tag;
        }
    }
}
