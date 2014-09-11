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
using Toyota.Common.Utilities;

namespace Toyota.Common.Web.Platform
{
    public class UIComboBoxSettings: UIComponentSettings
    {
        public UIComboBoxSettings()
        {
            _items = new List<object>();
            NullText = "&lt; Select &gt;";
            ValueField = "Value";
            TextField = "Text";
            Type = UIComboBoxType.Standard;
            ClientSideEvent = new UIComboBoxClientSideEvent();
        }

        public object SelectedValue { set; get; }
        public string NullText { set; get; }
        public bool IsSelectedItemPrefixed { set; get; }
        public string TextField { set; get; }
        public string ValueField { set; get; }
        public UIComboBoxType Type { set; get; }

        private IList<object> _items;
        public void AddItem(object item)
        {
            if ((item != null) && !_items.Contains(item))
            {
                _items.Add(item);
            }
        }
        public IEnumerable<object> GetItems()
        {
            return _items.AsEnumerable<object>();
        }

        public UIComboBoxClientSideEvent ClientSideEvent { private set; get; }

        public override IList<UIJavascriptMapProperty> GetJsProperties()
        {
            return base.GetJsProperties().ExtendAsList(props =>
            {
                props.Merge(ClientSideEvent.GetJsProperties());
                props.AddIfConditionAchieved(new UIJavascriptMapProperty(true, "selectedValue", Convert.ToString(SelectedValue)), c => { return !SelectedValue.IsNull(); });
                props.AddIfAllowed(new UIJavascriptMapProperty(true, "selectedItemPrefix", NullText), IsSelectedItemPrefixed);
            });
        }
    }
}
