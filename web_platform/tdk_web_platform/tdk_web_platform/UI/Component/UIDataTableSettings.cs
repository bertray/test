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
using System.Collections;

namespace Toyota.Common.Web.Platform
{
    public class UIDataTableSettings: UIComponentSettings
    {
        public UIDataTableSettings()
        {
            Columns = new List<UIDataTableColumn>();
            ClientSideEvent = new UIDataTableClientSideEvent();
            PageSizes = new List<long>() { 10, 20, 30, 50 };
            PageSize = 10;
        }
        
        public IList<UIDataTableColumn> Columns { private set; get; }
        public UIDataTableClientSideEvent ClientSideEvent { private set; get; }
        public IList<long> PageSizes { private set; get; }
        public long PageSize { set; get; }
        public bool EnablePaging { set; get; }
        public string KeyFieldName { set; get; }
        public IEnumerable DataSource { set; get; }

        public override IList<UIJavascriptMapProperty> GetJsProperties()
        {
            return base.GetJsProperties().ExtendAsList(props =>
            {
                props.Merge(ClientSideEvent.GetJsProperties());
                props.AddIfAllowed(new UIJavascriptMapProperty(false, "pageSize", PageSize), PageSize > 0);
                props.AddIfAllowed(new UIJavascriptMapProperty(false, "dataCount", DataSource.Count()), !DataSource.IsNull());
            });
        }
    }
}
