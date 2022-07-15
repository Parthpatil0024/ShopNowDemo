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
    public class UserRepo

    {
       public List<tblCustomer> lstCustomers()
        {
            List<tblCustomer> lst = new List<tblCustomer>();
            using (DBTContext context = new DBTContext())
            {
                lst=context.tblCustomers.ToList();
            }
            return lst;

        }

        public bool addCustomer(tblCustomer newCust)
        {
            bool result = false;
            using(DBTContext context = new DBTContext())
            {
                newCust.CreatedDate = DateTime.Now;
                newCust.CreatedBy = 2;
                context.tblCustomers.AddOrUpdate(newCust);
                context.SaveChanges();
                result = true;
            }

            return result;
        }

        public tblCustomer findCustomerById(int id)
        {
            
            tblCustomer customer;
            using(DBTContext context = new DBTContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                customer=context.tblCustomers.Where(x => x.Id==id).FirstOrDefault();
            }
            return customer;
        }

        public bool delCustomer(int id)
        {
            bool result = false;
            
            using(DBTContext context = new DBTContext())
            {
                tblCustomer customer =context.tblCustomers.Where(x=>x.Id==id).FirstOrDefault();
                context.tblCustomers.Remove(customer);
                context.SaveChanges();
                result=true;
            }
            return result;
            
        }

        public List<tblUser> listAdmins()
        {
            List<tblUser> lst = new List<tblUser>();
            using(DBTContext context = new DBTContext())
            {
                lst=context.tblUsers.Where(x=>x.RoleId==1).ToList();
            }

            return lst;
        }

       

        public List<tblUser> listCashiers()
        {
            List<tblUser> lst = new List<tblUser>();
            using (DBTContext context = new DBTContext())
            {
                lst = context.tblUsers.Where(x => x.RoleId == 2).ToList();
            }

            return lst;
        }

       

        public bool addUser(tblUser newUser)
        {
            bool result = false;
            using (DBTContext context = new DBTContext())
            {
                newUser.CreatedBy = 2;
                newUser.CreatedDate = DateTime.Now;
                

                context.tblUsers.AddOrUpdate(newUser);
                context.SaveChanges();
                result = true;
            }
            return result;
        }
        public tblUser findUserById(int id)
        {
            tblUser user;
            using (DBTContext context = new DBTContext())
            {
              user=context.tblUsers.Find(id);
              
            }
            return user;
        }
        public bool deleteUser(int id)
        {
            bool result = false;
            using(DBTContext context = new DBTContext())
            {
                tblUser user=context.tblUsers.Where(x=>x.Id==id).FirstOrDefault();
                context.tblUsers.Remove(user);
                context.SaveChanges();
                result=true;    
            }
            return result;

        }

        public tblUser authenticateUser(string emailId,string password)
        {
            tblUser user;
            using(DBTContext context=new DBTContext())
            {
                user= context.tblUsers.Where(x=>x.EmailId==emailId && x.Password==password).FirstOrDefault();
            }
            return user;
        }

        public List<tblUser> listUsers()
        {
            List<tblUser> usersList = new List<tblUser>();
            using (DBTContext context = new DBTContext())
            {
                usersList = context.tblUsers.ToList();
            }
            return usersList;

        }

    }
}
