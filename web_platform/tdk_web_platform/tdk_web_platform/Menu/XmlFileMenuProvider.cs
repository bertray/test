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
using System.Web;
using Toyota.Common.Configuration;
using System.IO;
using System.Xml;
using Toyota.Common.Utilities;

namespace Toyota.Common.Web.Platform
{
    public class XmlFileMenuProvider: IMenuProvider
    {
        private MenuList menus;

        public XmlFileMenuProvider()
        {
            menus = new MenuList();            
            Load();
            if (menus.Count == 0)
            {
                _GenerateSampleMenu();
            }
        }

        public Menu Get(string id)
        {
            if (menus.Count > 0)
            {
                foreach (Menu menu in menus)
                {
                    if (menu.Id.Equals(id))
                    {
                        return menu;
                    }
                }
            }

            return null;
        }

        public MenuList GetByUrl(string url)
        {
            if (menus.Count > 0)
            {
                MenuList result = new MenuList();
                foreach (Menu menu in menus)
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

        public MenuList GetByText(string text)
        {
            if (menus.Count > 0)
            {
                MenuList result = new MenuList();
                foreach (Menu menu in menus)
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

        public MenuList GetAll()
        {
            return menus;
        }

        public void Save()
        {
            Save(menus);
        }

        private void Save(MenuList menuList)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            string path = _GetConfigurationPath();
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            FileStream stream = File.Create(path);
            XmlWriter writer = XmlWriter.Create(stream, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("menu");

            foreach (Menu menu in menuList)
            {
                _WriteMenu(menu, writer);
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Close();
            stream.Close();
        }

        public void Load()
        {
            menus.Clear();
            string path = _GetConfigurationPath();
            if (!File.Exists(path))
            {
                return;
            }

            XmlDocument doc = new XmlDocument();
            
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;
            Stream stream = File.OpenRead(path);
            XmlReader reader = XmlReader.Create(stream, readerSettings);
            
            doc.Load(reader);
            reader.Close();
            stream.Close();

            XmlNodeList rootNodes = doc.GetElementsByTagName("menu");
            if((rootNodes != null) && (rootNodes.Count > 0)) 
            {
                XmlNode rootNode = rootNodes.Item(0);
                Menu menu;
                if (rootNode.HasChildNodes)
                {
                    ILocalizedWordCollection localization = ProviderRegistry.Instance.Get<ILocalizedWordCollection>();
                    foreach (XmlNode menuNode in rootNode.ChildNodes)
                    {
                        menu = CreateMenuFromNode(menuNode);
                        menus.Add(menu);                        
                    }
                }
            }
        }

        private void _WriteMenu(Menu menu, XmlWriter writer)
        {
            if (menu.IsNull())
            {
                return;
            }

            writer.WriteStartElement("menu-item");
            writer.WriteAttributeString("Id", menu.Id.ToEmptyStringIfNull());
            writer.WriteAttributeString("Text", menu.Text.ToEmptyStringIfNull());
            writer.WriteAttributeString("Description", menu.Description.ToEmptyStringIfNull());
            writer.WriteAttributeString("Url", menu.NavigateUrl.ToEmptyStringIfNull());
            writer.WriteAttributeString("Callback", menu.Callback.ToEmptyStringIfNull());
            writer.WriteAttributeString("Target", menu.OpeningTarget.ToEmptyStringIfNull());
            writer.WriteAttributeString("IconUrl", menu.IconUrl.ToEmptyStringIfNull());
            writer.WriteAttributeString("Glyph", menu.Glyph.ToEmptyStringIfNull());
            writer.WriteAttributeString("Separator", Convert.ToString(menu.Separator).ToLower());  
            writer.WriteAttributeString("Enabled", Convert.ToString(menu.Enabled));
            writer.WriteAttributeString("Visible", Convert.ToString(menu.Visible));

            if (menu.HasChildren())
            {
                foreach (Menu submenu in menu.Children)
                {
                    _WriteMenu(submenu, writer);
                }
            }

            writer.WriteEndElement();
        }

        private Menu CreateMenuFromNode(XmlNode node)
        {
            Menu menu = new Menu();
            XmlAttributeCollection attributes = node.Attributes;
            XmlAttribute attribute = attributes["Id"];
            if (attribute != null)
            {
                menu.Id = attribute.Value;
            }            
            attribute = attributes["Enabled"];
            if (attribute != null)
            {
                menu.Enabled = Convert.ToBoolean(attribute.Value.ToLower());
            }
            attribute = attributes["Visible"];
            if (attribute != null)
            {
                menu.Visible = Convert.ToBoolean(attribute.Value.ToLower());
            }
            attribute = attributes["Url"];
            if (attribute != null)
            {
                menu.NavigateUrl = attribute.Value;
            }
            attribute = attributes["Text"];
            if (attribute != null)
            {
                menu.Text = attribute.Value;                  
            }
            attribute = attributes["Callback"];
            if (attribute != null)
            {
                menu.Callback = attribute.Value;
            }
            attribute = attributes["Description"];
            if (attribute != null)
            {
                menu.Description = attribute.Value;
            }
            attribute = attributes["Target"];
            if (attribute != null)
            {
                menu.OpeningTarget = attribute.Value;
            }
            attribute = attributes["IconUrl"];
            if (attribute != null)
            {
                menu.IconUrl = attribute.Value;
            }
            attribute = attributes["Glyph"];
            if (attribute != null)
            {
                menu.Glyph = attribute.Value;
            }
            attribute = attributes["Separator"];
            if ((attribute != null) && !string.IsNullOrEmpty(attribute.Value))
            {
                string lowered = attribute.Value.ToLower();
                if (lowered.Equals("before"))
                {
                    menu.Separator = MenuSeparator.Before;
                }
                else if (lowered.Equals("after"))
                {
                    menu.Separator = MenuSeparator.After;
                }
            }

            if (node.HasChildNodes)
            {
                Menu submenu;
                foreach (XmlNode submenuNode in node.ChildNodes)
                {
                    submenu = CreateMenuFromNode(submenuNode);
                    submenu.Parent = menu;
                    menu.AddChildren(submenu);
                }
            }

            return menu;
        }

        public void AttachBaseUrl(string prefix)
        {
            if (!menus.IsNullOrEmpty())
            {
                Menu menu;
                int menuCount = menus.Count;
                for (int i = 0; i < menus.Count; i++ )
                {
                    menu = menus[i];
                    _AppendBaseUrl(ref menu, prefix);
                }

                IsBaseUrlAttached = true;
            }
        }

        private void _AppendBaseUrl(ref Menu menu, string baseUrl)
        {
            if (menu != null)
            {
                string url = menu.NavigateUrl;
                if (string.IsNullOrEmpty(url))
                {
                    menu.NavigateUrl = "#";
                    return;
                }

                url = url.Trim();

                if (url.Equals("/"))
                {
                    menu.NavigateUrl = baseUrl;
                    return;
                } else if (!url.Equals("#") && (!url.StartsWith("http:") || !url.StartsWith("www."))) {
                    if (url.StartsWith("/"))
                    {
                        menu.NavigateUrl = baseUrl + menu.NavigateUrl;
                    }
                    else
                    {
                        menu.NavigateUrl = baseUrl + "/" + menu.NavigateUrl;
                    }
                } else if (menu.HasChildren()) {
                    Menu submenu;
                    MenuList childrens = menu.Children;
                    int childrenCount = childrens.Count;
                    for (int i = 0; i < childrenCount; i++)
                    {
                        submenu = childrens[i];
                        _AppendBaseUrl(ref submenu, baseUrl);
                    }
                }
            }
        }

        private string _GetConfigurationPath()
        {            
            return GlobalConstants.Instance.Location.Configuration + "/Menu-" + ApplicationSettings.Instance.Development.Stage.Code + ".xml";
        }

        private void _GenerateSampleMenu()
        {
            Menu menu;
            for (int i = 1; i < 6; i++)
            {
                menu = new Menu()
                {
                    Visible = true,
                    Enabled = true,
                    Id = "menu-" + i,
                    Text = "Menu " + i,
                    Description = "Menu description " + i,
                    NavigateUrl = "relative-path-from-base-Url"
                };

                for (int j = 1; j < 6; j++)
                {
                    menu.AddChildren(new Menu()
                    {
                        Visible = true,
                        Enabled = true,
                        Id = "submenu-" + j,
                        Text = "Sub Menu " + j,
                        Description = "Menu description " + j,
                        NavigateUrl = "relative-path-from-base-Url"
                    });
                }

                menus.Add(menu);
            }

            Save(menus);
        }

        private bool _baseUrlAttached;
        public bool IsBaseUrlAttached
        {
            private set { _baseUrlAttached = value; }
            get { return _baseUrlAttached; }
        }
    }
}
