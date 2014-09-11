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

namespace Toyota.Common.Web.Platform
{
    public class UIDataTableColumn
    {
        public UIDataTableColumn(string name, string caption, string fieldName, bool asKey)
        {
            Name = name;
            Caption = caption;
            FieldName = fieldName;
            AsKey = asKey;
        }
        public UIDataTableColumn(string name, string caption, string fieldName): this(name, caption, fieldName, false) { }
        public UIDataTableColumn(string name, string caption) : this(name, caption, name, false) { }
        public UIDataTableColumn(string name, string caption, bool asKey) : this(name, caption, name, asKey) { }

        public string Name { set; get; }
        public string Caption { set; get; }
        public string FieldName { set; get; }
        public bool AsKey { set; get; }
        public UIHorizontalAlignment HorizontalAlignment { set; get; }
        public Func<object, string> DataItemTemplate { set; get; }
    }
}
