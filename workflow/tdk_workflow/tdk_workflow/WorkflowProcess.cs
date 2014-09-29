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
using Toyota.Common.Credential;

namespace Toyota.Common.Workflow
{
    public class WorkflowProcess
    {
        public User Approver { set; get; }
        public WorkflowDocumentStatus Status { set; get; }
        public string Note { set; get; }
    }
}
