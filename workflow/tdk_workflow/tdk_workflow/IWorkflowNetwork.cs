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
    public interface IWorkflowNetwork
    {
        void AddPolicy(IWorkflowPolicy policy);
        void RemovePolicy(IWorkflowPolicy policy);
        void RemovePolicy(string id);
        IWorkflowPolicy GetPolicy(string id);

        IWorkflowPolicy GetDefaultPolicy();
        void SetDefaultPolicy(IWorkflowPolicy policy);
        void SetDefaultPolicy(string id);

        void RegisterDocument(WorkflowDocument document);
    }
}
