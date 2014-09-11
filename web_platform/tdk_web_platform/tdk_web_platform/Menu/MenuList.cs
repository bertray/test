///
/// <author>lufty.abdillah@gmail.com</author>
/// <summary>
/// Toyota .Net Development Kit
/// Copyright (c) Toyota Motor Manufacturing Indonesia, All Right Reserved.
/// </summary>
/// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Toyota.Common.Utilities;

namespace Toyota.Common.Web.Platform
{
    public class MenuList : List<Menu>, IHierarchicalEnumerable
    {
        public MenuList() { }
        public MenuList(ICollection<Menu> menus)
        {
            if (!menus.IsNullOrEmpty())
            {
                AddRange(menus);
            }
        }

        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return (IHierarchyData)enumeratedItem;
        }
    }
}
