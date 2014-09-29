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
using System.Xml;
using System.IO;
using Toyota.Common.Utilities;
using Toyota.Common.Credential;

namespace Toyota.Common.Workflow
{
    public class XmlWorkflowNetwork: WorkflowNetwork
    {
        private string path;

        public XmlWorkflowNetwork(string path, IWorkflowPolicy policy, IUserProvider userProvider): base(policy, userProvider)
        {
            this.path = path;
        }
        public XmlWorkflowNetwork(string path, IWorkflowPolicy policy): this(path, policy, null) { }
        public XmlWorkflowNetwork(string path) : this(null, null, null) { }

        public override bool SubmitDocument(WorkflowDocument doc)
        {
            throw new NotImplementedException();
        }

        public override bool ResetDocument(string id)
        {
            throw new NotImplementedException();
        }

        public override bool CancelDocument(string id)
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
            if (File.Exists(path))
            {
                Stream stream = null;
                XmlReader reader = null;
                XmlDocument doc = null;
                try
                {
                    stream = File.OpenRead(path);
                    reader = XmlReader.Create(stream, new XmlReaderSettings() { IgnoreComments = true });
                    doc = new XmlDocument();
                    doc.Load(reader);
                }
                finally
                {
                    if (!reader.IsNull())
                    {
                        reader.Close();
                    }
                    if (!stream.IsNull())
                    {
                        stream.Close();
                    }                    
                }

                if (approvals.IsNull())
                {
                    approvals = new List<WorkflowApproval>();
                }
                else
                {
                    approvals.Clear();
                }

                WorkflowApproval appr;
                XmlNodeList rootNodes = doc.GetElementsByTagName("approval.net");
                if ((rootNodes != null) && (rootNodes.Count > 0))
                {
                    XmlNode root = rootNodes.Item(0);
                    if (root.HasChildNodes)
                    {                        
                        foreach (XmlNode node in root.ChildNodes)
                        {
                            appr = _CreateApproval(node);
                            if (!appr.IsNull())
                            {
                                approvals.Add(appr);
                            }
                        }
                    }
                }

                if (!approvals.IsNullOrEmpty())
                {
                    int lastIndex = approvals.Count - 1;
                    int index;
                    for (int i = lastIndex; i >= 0; i--)
                    {
                        appr = approvals[i];
                        if (i == lastIndex)
                        {
                            index = i - 1;
                            if (index >= 0)
                            {
                                appr.Previous = approvals[index];
                            }                            
                            appr.Next = null;
                        }
                        else if (i == 0)
                        {
                            appr.Previous = null;
                            index = i + 1;
                            if (index >= 0)
                            {
                                appr.Next = approvals[index];
                            }                            
                        }
                        else
                        {
                            index = i - 1;
                            if (index > 0)
                            {
                                appr.Previous = approvals[index];
                            }
                            index = i + 1;
                            if (index > 0)
                            {
                                appr.Next = approvals[index];
                            }                            
                        }
                    }
                }
            }
        }
        private WorkflowApproval _CreateApproval(XmlNode node)
        {
            if (!node.IsNull())
            {
                if (node.Name.Equals("approval"))
                {
                    WorkflowApproval appr = new WorkflowApproval();
                    XmlAttributeCollection attributes = node.Attributes;
                    XmlAttribute attr = attributes["approver"];
                    if (!attr.IsNull())
                    {
                        if (!UserProvider.IsNull())
                        {
                            appr.Approver = UserProvider.GetUser(attr.Value);
                        }

                        if(appr.Approver.IsNull())
                        {
                            appr.Approver = new User() { Username = attr.Value };
                        }
                    }
                    return appr;
                }
            }
            return null;
        }

        public override void Save()
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            Stream stream = null;
            XmlWriter writer = null;
            try
            {
                stream = File.Create(path);
                writer = XmlWriter.Create(stream, new XmlWriterSettings() { Indent = true, IndentChars = "\t" });

                writer.WriteStartDocument();
                writer.WriteStartElement("approval.net");

                if (!approvals.IsNullOrEmpty())
                {
                    WorkflowApproval appr;
                    int count = approvals.Count;
                    string username;
                    for (int i = 0; i < count; i++)
                    {
                        appr = approvals[i];
                        writer.WriteStartElement("approval");
                        username = string.Empty;
                        if (!appr.Approver.IsNull() && !appr.Approver.Username.IsNullOrEmpty())
                        {
                            username = appr.Approver.Username;
                        }
                        writer.WriteAttributeString("approver", username);
                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            finally
            {
                if (!writer.IsNull())
                {
                    writer.Close();
                }
                if (!stream.IsNull())
                {
                    stream.Close();
                }                
            }
        }

        public override WorkflowDocument GetDocument(string id)
        {
            throw new NotImplementedException();
        }
    }
}
