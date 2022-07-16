using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopNowBL.Model;
using System.Data.Entity.Migrations;


namespace ShopNowBL.Repo
{
     public class TransactionRepo
    {

        StockRepo stockRepo = new StockRepo();
            public string GenerateId()
            {
                long i = 1;
                foreach (byte b in Guid.NewGuid().ToByteArray())
                {
                    i *= ((int)b + 1);
                }
                return string.Format("{0:x}", i - DateTime.Now.Ticks);
            }


        public string CaptureTransaction(tblCustomer objCust,tblTransaction objTrans, List<tblTransactionItem> TransactionItems)
        {
            bool result = false;


            using (DBTContext context = new DBTContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        result = context.tblCustomers.Where(x => x.MobileNo == objCust.MobileNo).Any();

                        if (!result)
                        {
                            //Add Customer
                            tblCustomer customer = new tblCustomer();

                            customer.MobileNo = objCust.MobileNo;
                            customer.CustomerName = objCust.CustomerName;
                            customer.CreatedDate = DateTime.Now;
                            customer.CreatedBy = 1;

                            context.tblCustomers.Add(customer);
                            context.SaveChanges();
                            objTrans.CustomerId = customer.Id;

                        }
                        else
                        {

                            tblCustomer customer = context.tblCustomers.Where(x => x.MobileNo == objCust.MobileNo).FirstOrDefault();
                            objTrans.CustomerId = customer.Id;
                        }
                        //Capture Transaction
                        objTrans.CreatedDate = DateTime.Now;
                        objTrans.CreatedBy = 1;
                        objTrans.InvoiceNo = GenerateId();
                        objTrans.InvoiceDate = DateTime.Now;
                        context.tblTransactions.Add(objTrans);
                        context.SaveChanges();

                        //Capture Transaction Items
                        foreach (tblTransactionItem T in TransactionItems)
                        {
                            tblTransactionItem Item = new tblTransactionItem();
                            Item.InvoiceId = objTrans.InvoiceNo;
                            Item.Qty = T.Qty;
                            Item.ProductId = T.ProductId;
                            Item.Price = T.Price;
                            context.tblTransactionItems.Add(Item);
                            context.SaveChanges();


                            //Update Stock
                            tblStock stock = stockRepo.getProductById(Item.ProductId);
                            stock.ProductQty -= Convert.ToInt32(Item.Qty);
                            context.tblStocks.AddOrUpdate(stock);
                            context.SaveChanges();
                            

                        }

                        transaction.Commit();
                        result = true;








                    }


                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        result = false;
                        throw ex;

                    }


                }
            }


            return objTrans.InvoiceNo;
        }


        public List<tblTransactionItem> getTItemsByInvoice(string InvoiceNo)
        {

            List<tblTransactionItem> listTransItems;
            using(DBTContext context = new DBTContext())
            {
                
                listTransItems = context.tblTransactionItems.Where(x=>x.InvoiceId==InvoiceNo).ToList();

                foreach (var item in listTransItems)
                {
                    item.tblStock = context.tblStocks.Where(x => x.Id == item.ProductId).FirstOrDefault();
                }

            }
            
            return listTransItems;
        }

        public tblTransaction getTransByInvoice(string InvoiceNo)
        {
            tblTransaction objTrans;
            using(DBTContext context = new DBTContext())
            {
                objTrans=context.tblTransactions.Where(x=>x.InvoiceNo==InvoiceNo).FirstOrDefault();
            }
            return objTrans;
        }






    }
   
}
