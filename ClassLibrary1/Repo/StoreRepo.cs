using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ShopNowBL.Model;
using Microsoft.SqlServer;
using System.Data.Entity.Migrations;

namespace ShopNowBL.Repo
{
    public class StoreRepo
    {
        public List<tblStore> listStores()
        {
            List<tblStore> lst = new List<tblStore>();
            using (DBTContext context = new DBTContext())
            {
                lst = context.tblStores.ToList();
                lst.ForEach(x => x.StrStartedDate=x.StartedDate.ToString());
            }
            return lst;

        }

        public bool addStore(tblStore newStore)
        {
            bool result = false;
            using(DBTContext context = new DBTContext())
            {
               
                newStore.CreatedBy = 2;
                newStore.CreatedDate = DateTime.Now;
                
                context.tblStores.AddOrUpdate(newStore);
                context.SaveChanges();
                result = true;
            }
            return result;

        }

        public tblStore findStoreById(int id)
        {
            tblStore store;
            using(DBTContext context=new DBTContext())
            {
                store = context.tblStores.Where(x => x.Id == id).FirstOrDefault();  
            }
            return store;
        }

        public bool deleteStore(int id)
        {
            bool result=false;
            using(DBTContext context=new DBTContext())
            {
                tblStore store=context.tblStores.Where(x => x.Id == id).FirstOrDefault();
                context.tblStores.Remove(store);
                context.SaveChanges();
                result=true;
            }
            return result;
        }
    }
}
