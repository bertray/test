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
    public class UILayout
    {
        public UILayout()
        {
            FeatureStates = new Dictionary<string, bool>();
        }

        private IDictionary<string, bool> FeatureStates { set; get; }
        public bool HasFeature(string id)
        {
            if (FeatureStates.ContainsKey(id))
            {
                return FeatureStates[id];
            }
            return false;
        }
        public void AddFeature(string id)
        {
            if (FeatureStates.ContainsKey(id))
            {
                FeatureStates[id] = true;
            }
            else
            {
                FeatureStates.Add(id, true);
            }
        }
        public void RemoveFeature(string id)
        {
            if (FeatureStates.ContainsKey(id))
            {
                FeatureStates.Remove(id);
            }
        }

        private WeakReference Model { set; get; }
        public void SetModel(object model)
        {
            Model = null;
            if (model != null)
            {
                Model = new WeakReference(model);
            }
        }
        public object GetModel()
        {
            if ((Model != null) && Model.IsAlive)
            {
                return Model.Target;
            }
            return null;
        }
        public T GetModel<T>()
        {
            object model = GetModel();
            if ((model != null) && (model is T))
            {
                return (T)model;
            }
            return default(T);
        }
    }

    public static class UILayoutExtension 
    {
        public static bool IsNotNullAndHasFeature(this UILayout layout, string featureId)
        {
            return !layout.IsNull() && layout.HasFeature(featureId);
        }
    }
}
