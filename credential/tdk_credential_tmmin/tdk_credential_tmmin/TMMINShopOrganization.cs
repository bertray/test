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

namespace Toyota.Common.Credential.TMMIN
{
    public class TMMINShopOrganization: ShopOrganization
    {
        public TMMINShopOrganization()
        {
            _Lines = new Dictionary<string, LineOrganization>();
        }

        private IDictionary<string, LineOrganization> _Lines { set; get; }

        public override IList<LineOrganization> Lines
        {
            get
            {
                return _Lines.Values.ToList().AsReadOnly();
            }
        }
        public override bool HasLine(string id)
        {
            return _Lines.ContainsKey(id);
        }
        public override LineOrganization GetLine(string id)
        {
            if (_Lines.ContainsKey(id))
            {
                return _Lines[id];
            }
            return null;
        }
        public void AddLine(LineOrganization line)
        {
            if (!line.IsNull())
            {
                string id = line.Id;
                if (_Lines.ContainsKey(id))
                {
                    _Lines[id] = line;
                }
                else
                {
                    _Lines.Add(id, line);
                }
            }
        }
    }
}
