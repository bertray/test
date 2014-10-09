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
using Toyota.Common.Credential;

namespace Toyota.Common.Web.Platform
{
    public class Menu: IHierarchyData
    {
        private MenuList children;

        public Menu()
        {
            children = new MenuList();
            Parent = this;
            Separator = MenuSeparator.None;
            Roles = new List<Role>();
        }

        public string Id { set; get; }
        public string Text { set; get; }
        public string Description { set; get; }
        public string NavigateUrl { set; get; }
        public string Callback { set; get; }
        public string OpeningTarget { set; get; }
        public string IconUrl { set; get; }
        public string Glyph { set; get; }
        public Menu Parent { set; get; }
        public MenuList Children 
        {
            get
            {
                return children;
            }
        }

        public MenuSeparator Separator { set; get; }
        public bool Enabled { set; get; }
        public bool Visible { set; get; }
        public void AddChildren(Menu menu)
        {
            if (!children.Contains(menu))
            {
                children.Add(menu);
            }
        }
        public void RemoveChildren(Menu menu)
        {
            if (children.Contains(menu))
            {
                children.Remove(menu);
            }
        }
        public void RemoveChildren(string id)
        {
            if (children.Count > 0)
            {
                Menu found = null;
                foreach (Menu menu in children)
                {
                    if (menu.Id.Equals(id))
                    {
                        found = menu;
                    }
                }

                if (found != null)
                {
                    children.Remove(found);
                }
            }
        }
        public bool HasChildren()
        {
            return children.Count > 0;
        }
        public Menu GetChildren(string id)
        {
            if (children.Count > 0)
            {
                foreach(Menu menu in children) 
                {
                    if (menu.Id.Equals(id))
                    {
                        return menu;
                    }
                }
            }

            return null;
        }
        public IList<Menu> GetChildrenByUrl(string url)
        {
            if (children.Count > 0)
            {
                List<Menu> result = new List<Menu>();
                foreach (Menu menu in children)
                {
                    if (menu.NavigateUrl.Contains(url))
                    {
                        result.Add(menu);
                    }
                }

                if (result.Count > 0)
                {
                    return result;
                }
            }

            return null;
        }
        public IList<Menu> GetChildrenByText(string text)
        {
            if (children.Count > 0)
            {
                List<Menu> result = new List<Menu>();
                foreach (Menu menu in children)
                {
                    if (menu.Text.Contains(text))
                    {
                        result.Add(menu);
                    }
                }

                if (result.Count > 0)
                {
                    return result;
                }
            }

            return null;
        }

        public IHierarchicalEnumerable GetChildren()
        {
            return Children;
        }
        public IHierarchyData GetParent()
        {
            return Parent;
        }
        bool IHierarchyData.HasChildren
        {
            get { return HasChildren(); }
        }
        public object Item
        {
            get { return this; }
        }
        public string Path
        {
            get { return Id; }
        }
        public string Type
        {
            get { return GetType().ToString(); }
        }

        public bool IsRestricted { set; get; }
        public IList<Role> Roles { private set; get; }

        public Menu Clone()
        {
            Menu menu = new Menu();
            menu.Id = Id;
            menu.Text = Text;
            menu.Description = Description;
            menu.NavigateUrl = NavigateUrl;
            menu.Callback = Callback;
            menu.OpeningTarget = OpeningTarget;
            menu.IconUrl = IconUrl;
            menu.Glyph = Glyph;
            menu.Parent = Parent;
            menu.Separator = Separator;
            menu.Enabled = Enabled;
            menu.Visible = Visible;
            menu.IsRestricted = IsRestricted;
            foreach (Menu m in Children)
            {
                menu.AddChildren(m);
            }
            foreach (Role role in Roles)
            {
                menu.Roles.Add(role);
            }

            return menu;
        }
    }
}
