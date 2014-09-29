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
    public class WorkflowDocument
    {
        public WorkflowDocument()
        {
            Process = new List<WorkflowProcess>();
            Status = WorkflowDocumentStatus.Pending;
        }

        public string Id { set; get; }
        public string Name { set; get; }
        public string Subject { set; get; }
        public User Submitter { set; get; }
        public WorkflowApproval UpcomingApproval { set; get; }
        public WorkflowDocumentStatus Status { set; get; }
        public IList<WorkflowProcess> Process { set; get; }
    }
}
