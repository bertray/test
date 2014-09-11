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
    public class UISummaryListSettings: UIComponentSettings
    {
        public UISummaryListSettings()
        {
            ClientSideEvent = new UISummaryListClientSideEvent();
            TitleFieldName = "Title";
            SubtitleFieldName = "Subtitle";
            TextFieldName = "Text";
            KeyFieldName = "Id";
            MissingTitleText = "&lt;No Title&gt;";
            PageSize = 5;
        }

        public UISummaryListClientSideEvent ClientSideEvent { private set; get; }
        public string TitleFieldName { set; get; }
        public Func<object, string> CustomTitle { set; get; }
        public string SubtitleFieldName { set; get; }
        public Func<object, string> CustomSubtitle { set; get; }
        public string TextFieldName { set; get; }
        public Func<object, string> CustomText { set; get; }
        public string KeyFieldName { set; get; }
        public Func<object, string> CustomKey { set; get; }
        public string MissingTitleText { set; get; }
        public object SelectedKey { set; get; }
        public string CallbackRoute { set; get; }
        public bool UseReadLabel { set; get; }

        public IEnumerable DataSource { set; get; }
        public int PageSize { set; get; }
        public bool EnableBackendProcessing { set; get; }

        public override IList<UIJavascriptMapProperty> GetJsProperties()
        {
            return base.GetJsProperties().ExtendAsList(props =>
            {
                props.Merge(ClientSideEvent.GetJsProperties());
                props.AddIfAllowed(new UIJavascriptMapProperty(false, "enabledBackendProcessing", EnableBackendProcessing), EnableBackendProcessing);
                props.AddIfAllowed(new UIJavascriptMapProperty(false, "pageSize", PageSize), PageSize > 0);
                props.Add(new UIJavascriptMapProperty(false, "totalData", DataSource.Count()));
            });
        }
    }
}
