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
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.ComponentModel.DataAnnotations;
using Toyota.Common.Utilities;

namespace Toyota.Common.Web.Platform
{
    public class UIRazorExtension: BaseRazorExtension
    {
        public UIRadio Radio(Action<UIRadioSettings> action)
        {
            UIRadioSettings settings = new UIRadioSettings();
            if (action != null)
            {
                action.Invoke(settings);
            }
            _Validate(settings);
            return new UIRadio(Helper, settings);
        }

        public UICheckBox CheckBox(Action<UICheckBoxSettings> action)
        {
            UICheckBoxSettings settings = new UICheckBoxSettings();
            if (action != null)
            {
                action.Invoke(settings);
            }
            _Validate(settings);
            return new UICheckBox(Helper ,settings);
        }

        public UIDataTable DataTable(Action<UIDataTableSettings> action)
        {
            UIDataTableSettings settings = new UIDataTableSettings();
            if (action != null)
            {
                action.Invoke(settings);
            }
            _Validate(settings);
            return new UIDataTable(Helper, settings);           
        }

        public UIComboBox ComboBox(Action<UIComboBoxSettings> action)
        {
            UIComboBoxSettings settings = new UIComboBoxSettings();
            if (action != null)
            {
                action.Invoke(settings);
            }
            _Validate(settings);
            return new UIComboBox(Helper, settings);
        }

        public UIPaginator Paginator(Action<UIPaginatorSettings> action)
        {
            UIPaginatorSettings settings = new UIPaginatorSettings();
            if (action != null)
            {
                action.Invoke(settings);
            }
            _Validate(settings);
            return new UIPaginator(Helper, settings);
        }

        public UISummaryList SummaryList(Action<UISummaryListSettings> action)
        {
            UISummaryListSettings settings = new UISummaryListSettings();
            if (action != null)
            {
                action.Invoke(settings);
            }
            _Validate(settings);
            return new UISummaryList(Helper, settings);
        }

        private void _Validate(object obj)
        {
            ValidationContext context = new ValidationContext(obj, null, null);
            IList<ValidationResult> validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(obj, context, validationResults);
            if (!validationResults.IsNullOrEmpty())
            {
                StringBuilder messages = new StringBuilder();
                foreach (ValidationResult vr in validationResults)
                {
                    messages.AppendLine(vr.ErrorMessage);
                }
                throw new Exception(messages.ToString());
            }
        }
    }
}
