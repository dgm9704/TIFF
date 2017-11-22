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
