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

namespace Toyota.Common.Configuration
{
    /// <summary>
    /// Specification for a configuration cabinet.
    /// 
    /// Configuration Cabinet stores one or more Configuration Binder.
    /// Think it like an office drawer cabinet which stores paper binders.
    /// Configuration Binder is like a paper binder which group one or more
    /// Configuration Item (which, in this analogy, is a paper).
    /// </summary>
    public interface IConfigurationCabinet
    {
        /// <summary>
        /// Gets the label of this configuration cabiner
        /// </summary>
        /// <returns>Configuration binder's label</returns>
        string GetLabel();

        /// <summary>
        /// Adds a configuration binder
        /// </summary>
        /// <param name="binder">Configuration binder to be added</param>
        void AddBinder(IConfigurationBinder binder);

        /// <summary>
        /// Gets a configuration binder
        /// </summary>
        /// <param name="label">Configuration label</param>
        /// <returns>Matched configuration binder</returns>
        IConfigurationBinder GetBinder(string label);

        /// <summary>
        /// Gets configuration binders that have a configuration named by given key.
        /// </summary>
        /// <param name="key">Configuration binder</param>
        /// <returns>Matched configuration binders</returns>
        IConfigurationBinder[] GetBinderByConfigurationKey(string key);

        /// <summary>
        /// Gets available configuration binders
        /// </summary>
        /// <returns>Available binders</returns>
        IConfigurationBinder[] GetBinders();
    }
}
