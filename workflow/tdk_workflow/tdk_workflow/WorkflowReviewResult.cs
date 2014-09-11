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
    public class WorkflowReviewResult
    {
        public WorkflowReviewResultType Type { set; get; }
        public WorkflowDocumentReviewer Reviewer { set; get; }
        public string Note { set; get; }
    }
}
