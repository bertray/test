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

namespace Toyota.Common.Workflow
{
    public interface IWorkflowPolicy
    {
        string GetId();
        string GetName();
        void SetDefault(bool defaultPolicy);
        bool IsDefault();

        WorkflowDocumentReviewer NextDocumentReviewer(WorkflowDocument document);
    }
}
