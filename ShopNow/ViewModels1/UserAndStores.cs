using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopNowBL.Model;
using System.Data.Entity.Migrations;

namespace ShopNow.ViewModels1
{
    public class UserAndStores
    {
        public tblUser user {get; set;}
        public List<tblStore> lstStores {get; set;}
    }
}
