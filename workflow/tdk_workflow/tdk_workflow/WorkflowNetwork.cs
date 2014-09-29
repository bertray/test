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
using Toyota.Common.Credential;

namespace Toyota.Common.Workflow
{
    public abstract class WorkflowNetwork
    {
        public WorkflowNetwork(IWorkflowPolicy policy, IUserProvider userProvider)
        {
            this.policy = policy;
            UserProvider = userProvider;
            approvals = new List<WorkflowApproval>();
        }
        public WorkflowNetwork(IWorkflowPolicy policy) : this(policy, null) { }
        public WorkflowNetwork() : this(null, null) { }

        public string Id { set; get; }
        public string Name { set; get; }
        protected IUserProvider UserProvider { set; get; }

        private IWorkflowPolicy policy;
        public void SetPolicy(IWorkflowPolicy policy)
        {
            this.policy = policy;
        }

        protected List<WorkflowApproval> approvals;
        public void AddApproval(WorkflowApproval approval)
        {
            if (approval.IsNull() || approval.Approver.IsNull())
            {
                return;
            }

            int _ap = -1;
            int _apPrev = -1;
            int _apNext = -1;

            if (!approval.Approver.IsNull())
            {
                _ap = approvals.FindIndex(p => { return p.Approver.Username.Equals(approval.Approver.Username); });
            }
            if (!approval.Previous.IsNull() && !approval.Previous.Approver.IsNull())
            {
                _apPrev = approvals.FindIndex(p => { return p.Approver.Username.Equals(approval.Previous.Approver.Username); });
            }
            if (!approval.Next.IsNull() && !approval.Next.Approver.IsNull())
            {
                _apNext = approvals.FindIndex(p => { return p.Approver.Username.Equals(approval.Next.Approver.Username); });
            }
            
            if (_ap < 0)
            {
                int lastIndex = approvals.Count - 1;
                if ((_apPrev < 0) && (_apNext >= 0))
                {
                    if (_apNext == 0)
                    {
                        approvals.Insert(0, approval);
                    }
                    else
                    {
                        approvals.Insert(_apNext, approval);
                    }
                }
                else if ((_apPrev >= 0) && (_apNext < 0))
                {
                    if (_apPrev == lastIndex)
                    {
                        approvals.Add(approval);
                    }
                    else if (_apPrev == 0)
                    {
                        approvals.Insert(0, approval);
                    }
                    else
                    {
                        approvals.Insert(_apPrev + 1, approval);
                    }
                }
                else if((_apPrev >= 0) && (_apNext <= lastIndex))
                {
                    if ((_apNext - _apPrev) == 1)
                    {
                        approvals.Insert(_apPrev + 1, approval);
                    }
                }
            }
        }
        public void RemoveApproval(WorkflowApproval approval)
        {
            if (approval.IsNull())
            {
                return;
            }
            string username = approval.Approver.Username;
            WorkflowApproval _ap = approvals.FindElement(p => { return p.Approver.Username.Equals(username); });
            if (!_ap.IsNull())
            {
                approvals.Remove(approval);
            }
        }
        public IList<WorkflowApproval> GetApprovals()
        {
            return approvals.AsReadOnly();
        }

        public abstract bool SubmitDocument(WorkflowDocument doc);
        public abstract bool ResetDocument(string id);
        public abstract bool CancelDocument(string id);
        public abstract WorkflowDocument GetDocument(string id);

        public abstract void Load();
        public abstract void Save();
    }
}
