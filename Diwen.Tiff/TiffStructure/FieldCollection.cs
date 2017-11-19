namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    [Serializable]
    public class FieldCollection : KeyedCollection<TagType, Field>
    {
        internal FieldCollection() : base(null, 0) { }

        public new void Add(Field item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.Remove(item.TagType);
            base.Add(item);
        }

        public void AddRange(IEnumerable<Field> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
                this.Add(item);
        }

        public void Sort()
        => ((List<Field>)Items).Sort((t1, t2) => { return t1.TagType.CompareTo(t2.TagType); });

        protected override TagType GetKeyForItem(Field item)
        => item != null
            ? item.TagType
            : throw new ArgumentNullException(nameof(item));

    }
}
