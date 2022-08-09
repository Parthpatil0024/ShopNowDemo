using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ShopNowBL.Model;
using Microsoft.SqlServer;
using System.Data.Entity.Migrations;
using System.Security.Cryptography;
using System.IO;

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
                
                newUser.CreatedDate = DateTime.Now;
                newUser.Password=encrypt(newUser.Password);
            
                

                context.tblUsers.AddOrUpdate(newUser);
                context.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool saveUserAfterEdit(tblUser user)
        {
            bool result = false;
            using (DBTContext context = new DBTContext())
            {
               
                context.tblUsers.AddOrUpdate(user);
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
                user.Password = user.Password;
              
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
            password=encrypt(password);
            
            using(DBTContext context=new DBTContext())
            {
                user= context.tblUsers.Where(x=>x.EmailId==emailId && x.Password==password).FirstOrDefault();
            }
            return user;
        }

        public tblUser VerifyEmail(string EmailId)
        {
            tblUser user;
            using (DBTContext context = new DBTContext())
            {
                 user = context.tblUsers.Where(u => u.EmailId == EmailId).FirstOrDefault();
            }
            return user;
        }


        public bool SaveOTP(tblOTP newOtp)
        {
            bool result=false;
            using (DBTContext context = new DBTContext())
            {
                context.tblOTPs.AddOrUpdate(newOtp);
                context.SaveChanges();
                result = true;
            }
            return (result);

        }

        public tblOTP getOtpByEmail(string EmailId)
        {  
            tblOTP newOtp;
            using(DBTContext context=new DBTContext())
            {
                newOtp=context.tblOTPs.Where(x=>x.EmailId==EmailId  ).OrderByDescending(x=>x.Created_DateTime).FirstOrDefault();
            }

            return newOtp;
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
        //--------------------------Encryption---------------------------
        public string encrypt(string encryptString)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        

    }
}
