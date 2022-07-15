using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopNowBL.Model;

namespace ShopNow.ViewModels1
{
    public class InvoiceViewModel
    {
       public  List<tblTransactionItem> listTransItems { get; set; }
        public tblTransaction objTrans { get; set; }
       public tblCustomer customer { get; set; }

    }
}