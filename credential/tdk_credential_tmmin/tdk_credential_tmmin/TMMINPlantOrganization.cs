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
    public class TMMINPlantOrganization: PlantOrganization
    {
        public TMMINPlantOrganization()
        {
            _Shops = new Dictionary<string, ShopOrganization>();
        }
        
        private IDictionary<string, ShopOrganization> _Shops { set; get; }
        public override IList<ShopOrganization> Shops
        {
            get
            {
                return _Shops.Values.ToList().AsReadOnly();
            }
        }

        public override bool HasShop(string id)
        {
            return _Shops.ContainsKey(id);
        }
        public override ShopOrganization GetShop(string id)
        {
            if (_Shops.ContainsKey(id))
            {
                return _Shops[id];
            }
            return null;
        }
        public void AddShop(ShopOrganization shop)
        {
            if (!shop.IsNull())
            {
                string id = shop.Id;
                if (_Shops.ContainsKey(id))
                {
                    _Shops[id] = shop;
                }
                else
                {
                    _Shops.Add(id, shop);
                }
            }            
        }
    }
}
