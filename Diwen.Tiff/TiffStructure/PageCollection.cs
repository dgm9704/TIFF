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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;

    [Serializable]
    public class PageCollection : Collection<Page>
    {
        public PageCollection() : base(new List<Page>()) { }

        public new void Add(Page page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            Items.Add(page.Copy());
        }

        public void AddRange(IEnumerable<Page> pages)
        {
            if (pages == null)
                throw new ArgumentNullException(nameof(pages));

            foreach (var page in pages)
                this.Add(page);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var page in this)
            {
                sb.AppendLine(page.ToString());
            }
            return sb.ToString();
        }
    }
}
