namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Diwen.Tiff.TagValues;

    [Serializable]
    public class TagCollection : KeyedCollection<TagType, Tag>
    {
        internal TagCollection() : base(null, 0) 
        { 
        }

        public new void Add(Tag item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.Remove(item.TagType);
            base.Add(item);
        }

        public void AddRange(IEnumerable<Tag> items)
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

        public void Sort()
        {
            ((List<Tag>)Items).Sort((t1, t2) => { return t1.TagType.CompareTo(t2.TagType); });
        }

        protected override TagType GetKeyForItem(Tag item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return item.TagType;
        }
    }
}
