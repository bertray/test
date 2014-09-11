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

namespace Toyota.Common.Messaging
{
    public class Annoucement: Message
    {
        public Annoucement()
        {
            Priority = AnnoucementPriority.Normal;
        }

        public AnnoucementPriority Priority { set; get; }
        public string Title { set; get; }
    }
}
