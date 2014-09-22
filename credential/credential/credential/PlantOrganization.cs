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

namespace Toyota.Common.Credential
{
    public class PlantOrganization: OrganizationStructure
    {
        public PlantOrganization()
        {
            Shops = new List<ShopOrganization>();
        }

        public virtual IList<ShopOrganization> Shops { private set; get; }
        public virtual bool HasShop(string id)
        {
            return Shops.IsElementExists(shop => {
                return shop.Id.Equals(id);
            });
        }
        public virtual ShopOrganization GetShop(string id)
        {
            return Shops.FindElement(shop => {
                return shop.Id.Equals(id);
            });
        }
    }
}
