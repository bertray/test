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
        private List<IWorkflowDocumentEventListener> listeners;
        private Dictionary<WorkflowDocumentReviewer, WorkflowReviewResult> results;

        public WorkflowDocument()
        {
            listeners = new List<IWorkflowDocumentEventListener>();
            results = new Dictionary<WorkflowDocumentReviewer, WorkflowReviewResult>();
            Status = WorkflowDocumentStatus.Transient;
        }

        public string Id { set; get; }
        public string PolicyId { set; get; }
        public User Author { set; get; }
        public DateTime DueDate { set; get; }
        public string Subject { set; get; }
        public bool Read { set; get; }

        private WorkflowDocumentReviewer reviewer;
        public WorkflowDocumentReviewer Reviewer
        {
            set
            {
                reviewer = value;
                NotifyEventChanged(WorkflowDocumentEventType.ReviewerChanged);
            }
            get
            {
                return reviewer;
            }
        }

        public void AddReviewResult(WorkflowReviewResult result)
        {
            if (results.ContainsKey(result.Reviewer))
            {
                results[result.Reviewer] = result;
            }
            else
            {
                results.Add(result.Reviewer, result);
            }
        }
        public void RemoveReviewResult(WorkflowReviewResult result)
        {
            if (results.ContainsKey(result.Reviewer))
            {
                results.Remove(result.Reviewer);
            }
        }
        IList<WorkflowDocumentReviewer> GetReviewers()
        {
            if (results.Count > 0)
            {
                return results.Keys.ToList().AsReadOnly();
            }
            return null;
        }
        IList<WorkflowReviewResult> GetReviewResults()
        {
            if (results.Count > 0)
            {
                return results.Values.ToList().AsReadOnly();
            }
            return null;
        }

        private WorkflowDocumentStatus status;
        public WorkflowDocumentStatus Status 
        {
            set
            {
                status = value;
                NotifyEventChanged(WorkflowDocumentEventType.StatusChanged);
            }

            get
            {
                return status;
            }
        }

        private object data;
        public object Data
        {
            set
            {
                data = value;
                NotifyEventChanged(WorkflowDocumentEventType.DataChanged);
            }

            get
            {
                return data;
            }
        }

        public void AddEventListener(IWorkflowDocumentEventListener listener)
        {
            if((listener != null) && !listeners.Contains(listener)) 
            {
                listeners.Add(listener);
            }
        }
        public void RemoveEventListener(IWorkflowDocumentEventListener listener)
        {
            listeners.Remove(listener);
        }

        protected void NotifyEventChanged(WorkflowDocumentEventType type)
        {
            foreach (IWorkflowDocumentEventListener l in GetEventListenersInArray())
            {
                l.WorkflowEventOccured(new WorkflowDocumentEvent()
                {
                    Source = this,
                    Type = type
                });
            }
        }
        protected IWorkflowDocumentEventListener[] GetEventListenersInArray()
        {
            IWorkflowDocumentEventListener[] listenerArray = new IWorkflowDocumentEventListener[0];
            if (listeners.Count > 0)
            {
                lock (listeners)
                {
                    listenerArray = listeners.ToArray();
                }
            }            

            return listenerArray;
        }
    }
}
