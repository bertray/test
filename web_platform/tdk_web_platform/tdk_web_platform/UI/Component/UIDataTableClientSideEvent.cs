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
    public class UIDataTableClientSideEvent: UIClientSideEvent
    {
        public string OnPageSizeChanged { set; get; }
        public string OnActivePageChanged { set; get; }
        public string OnRowSelected { set; get; }
        public string OnColumnHidden { set; get; }
        public string OnColumnVisible { set; get; }
        public string OnRowDeleted { set; get; }
        public string OnUploadInitiated { set; get; }
        public string OnDownloadInitiated { set; get; }
        public string OnAdditionPerformed { set; get; }
        public string OnDeletionPerformed { set; get; }

        public override IList<UIJavascriptMapProperty> GetJsProperties()
        {
            return base.GetJsProperties().ExtendAsList(props =>
            {
                props.AddIfStringIsNotNullOrEmpty(new UIJavascriptMapProperty(false, "onPageSizeChanged", NormalizeCallback(OnPageSizeChanged)), OnPageSizeChanged);
                props.AddIfStringIsNotNullOrEmpty(new UIJavascriptMapProperty(false, "onActivePageChanged", NormalizeCallback(OnActivePageChanged)), OnActivePageChanged);
                props.AddIfStringIsNotNullOrEmpty(new UIJavascriptMapProperty(false, "onRowSelected", NormalizeCallback(OnRowSelected)), OnRowSelected);
                props.AddIfStringIsNotNullOrEmpty(new UIJavascriptMapProperty(false, "onColumnHidden", NormalizeCallback(OnColumnHidden)), OnColumnHidden);
                props.AddIfStringIsNotNullOrEmpty(new UIJavascriptMapProperty(false, "onColumnVisible", NormalizeCallback(OnRowSelected)), OnColumnVisible);
                props.AddIfStringIsNotNullOrEmpty(new UIJavascriptMapProperty(false, "onRowDeleted", NormalizeCallback(OnRowDeleted)), OnRowDeleted);
                props.AddIfStringIsNotNullOrEmpty(new UIJavascriptMapProperty(false, "onUploadInitiated", NormalizeCallback(OnUploadInitiated)), OnUploadInitiated);
                props.AddIfStringIsNotNullOrEmpty(new UIJavascriptMapProperty(false, "onDownloadInitiated", NormalizeCallback(OnDownloadInitiated)), OnDownloadInitiated);
                props.AddIfStringIsNotNullOrEmpty(new UIJavascriptMapProperty(false, "onAdditionPerformed", NormalizeCallback(OnAdditionPerformed)), OnAdditionPerformed);
                props.AddIfStringIsNotNullOrEmpty(new UIJavascriptMapProperty(false, "onDeletionPerformed", NormalizeCallback(OnDeletionPerformed)), OnDeletionPerformed); 
            });
        }
    }
}
