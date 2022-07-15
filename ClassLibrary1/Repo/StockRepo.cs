using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopNowBL.Model;

namespace ShopNowBL.Repo
{
    public class StockRepo
    {
       
        public List<tblStock> listProducts()
        {
            List<tblStock> list = new List<tblStock>();
            using(DBTContext context = new DBTContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                list=context.tblStocks.ToList();
            }
            return list;

        }

        public bool addProduct(tblStock newProduct)
        {
            bool result=false;
            using(DBTContext context = new DBTContext())
            {
                newProduct.CreatedBy = 1;
                newProduct.CreatedDate = DateTime.Now;
                context.tblStocks.AddOrUpdate(newProduct);
                context.SaveChanges();
                result=true;
            }
            return result;

        }

        public tblStock getProductById(int id)
        {
            tblStock product;
            
            using(DBTContext context =new DBTContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
               

                product=context.tblStocks.Find(id);
            }
            return product;
        }

        public bool deleteProduct(int id)
        {
            bool result = false;
            using(DBTContext context =new DBTContext())
            {
                tblStock product = context.tblStocks.Find(id);
                context.tblStocks.Remove(product);
                context.SaveChanges();
                result = true;
            }
            return result;
        }
    }
}
