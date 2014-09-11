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

namespace Toyota.Common.Credential.TMMIN
{
    class Gentani
    {
        public string Key { get; set; }
        public string Username { get; set; }
        public string PlantCode { get; set; }
        public string ShopCode { get; set; }
        public string SectionCode { set; get; }
        public string SectionDescription { set; get; }
        public string LineCode { set; get; }
        public string LineDescription { set; get; }
        public string Shift { set; get; }
    }
}
