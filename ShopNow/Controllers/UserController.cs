using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopNowBL.Repo;
using ShopNowBL.Model;
using System.Data.Entity.Migrations;
using ShopNow.ViewModels1;
using System.Net.Mail;
using System.Net;

namespace ShopNow.Controllers
{
    public class UserController : Controller
    {
        // GET: Customer
        UserRepo userRepo = new UserRepo();
        StoreRepo storeRepo= new StoreRepo();


        //-----------------------Customer_________________----------------
        public ActionResult listCustomers()
        {
            var customers = userRepo.lstCustomers();

            return View(customers);
        }

        public ActionResult addCustomer()
        {

            tblCustomer newCust = new tblCustomer();


            return View(newCust);
        }



        public ActionResult editCustomer(int id)
        {
           
            tblCustomer customer = userRepo.findCustomerById(id);
                
            
            return View("addCustomer", customer);

        }

        public ActionResult deleteCustomer(int id)
        {
            bool result=userRepo.delCustomer(id);
            if (result)
            {
                return RedirectToAction("listCustomers");
            }
            return RedirectToAction("Index","Home");
        }

        public ActionResult saveCustomer(tblCustomer newCust)
        {
            
            bool result=userRepo.addCustomer(newCust);
            if (result)
            {
                return RedirectToAction("listCustomers");
            }

            return RedirectToAction("addCustomer");
        }
        //************AJAX-CUSTOMER**************
        public ActionResult ajaxAddCustomer(string CustomerName, string MobileNo, string Id)
        {
            tblCustomer newCustomer = new tblCustomer();
            if (!string.IsNullOrEmpty(Id))
            {
                newCustomer.Id = Convert.ToInt32(Id);
            }
            newCustomer.CustomerName = CustomerName;
            newCustomer.MobileNo = MobileNo;
            var result = userRepo.addCustomer(newCustomer);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ajaxGetCustById(int Id)
        {
            var cust = userRepo.findCustomerById(Id);
            return Json(cust,JsonRequestBehavior.AllowGet);
        }

        public ActionResult ajaxDeleteCustomer(int Id)
        {
            var result = userRepo.delCustomer(Id);
            if (result)
                return Json(result, JsonRequestBehavior.AllowGet);
            else
            {
                result = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }



        //--------------------------ADMIN----------------------------------------
        public ActionResult listAdmins()
        {
            var admins = userRepo.listAdmins();
            return View(admins);

        }
        public ActionResult deleteAdmin(int id)
        {
            bool result = userRepo.deleteUser(id);
            if (result)
            {
                return RedirectToAction("listAdmins");
            }
            return RedirectToAction("Index", "Home");
        }
        //--------------------------------USER__________________________________________
        public ActionResult addUser()
        {
            UserAndStores userAndStores=new UserAndStores();
            userAndStores.lstStores = storeRepo.listStores();
            return View(userAndStores);
        }
        public ActionResult addUserAfterEdit()
        {
            tblUser user =new tblUser();
            return View(user);
        }

        public ActionResult saveUserAfterEdit(UserAndStores userAndStores)
        {
            bool result = userRepo.saveUserAfterEdit(userAndStores.user);
            if(result)
                return RedirectToAction("listUsers");

            return RedirectToAction("addUserAfterEdit");
        }


        public ActionResult saveUser(UserAndStores userAndStores)
        {
            var loggedInUser = (tblUser)HttpContext.Session["User"];
            userAndStores.user.CreatedBy = loggedInUser.Id;
            bool result = userRepo.addUser(userAndStores.user);
            if (result)
            {
                return RedirectToAction("listUsers");
            }
            return RedirectToAction("addUser");
        }

        public ActionResult listUsers()
        {
            var users = userRepo.listUsers();
            return View(users);
        }
        
        public ActionResult editUser(int id)
        {
            UserAndStores userAndStores = new UserAndStores();
             userAndStores.user= userRepo.findUserById(id);
            userAndStores.lstStores= storeRepo.listStores();    

            return View("addUserAfterEdit", userAndStores);
        }
        public ActionResult deleteUser(int id)
        {
            bool result = userRepo.deleteUser(id);
            if (result)
            {
                return RedirectToAction("listUsers");
            }
            return RedirectToAction("Index", "Home");
        }

        
             public ActionResult ajaxAddUser(string UserName, string EmailId,string MobileNo,int RoleId,string Password, string City,int StoreId, string Id)
        {
            tblUser newUser = new tblUser();
            if (!string.IsNullOrEmpty(Id))
            {
                newUser.Id = Convert.ToInt32(Id);
            }
            newUser.UserName = UserName;
            newUser.EmailId = EmailId;
            newUser.MobileNo = MobileNo;
            newUser.RoleId = RoleId;
            newUser.Password = Password;
            newUser.City = City;
            newUser.StoreId = StoreId;

            var result = userRepo.addUser(newUser);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ajaxGetUserById(int Id)
        {
            var user=userRepo.findUserById(Id);
            return Json(user, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ajaxDeleteUser(int Id)
        {
            var result = userRepo.deleteUser(Id);
            if (result)
                return Json(result, JsonRequestBehavior.AllowGet);
            else
            {
                result = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        //-----------------------------Cashier-----------------------------------


        public ActionResult listCashiers()
        {
            var cashiers = userRepo.listCashiers();
            return View(cashiers);
        }
         
        public ActionResult deleteCashier(int id)
        {
            bool result = userRepo.deleteUser(id);
            if (result)
            {
                return RedirectToAction("listCashiers");
            }
            return RedirectToAction("Index", "Home");
        }



        public ActionResult Login()
        {
            return View();
        }

        public ActionResult RegisterAdmin()
        {
            UserAndStores userAndStores = new UserAndStores();  
            userAndStores.lstStores=storeRepo.listStores();
            return View(userAndStores);
        }
        public ActionResult SaveAdmin(UserAndStores userAndStores)
        {
            userAndStores.user.RoleId = 1;
            bool result = userRepo.addUser(userAndStores.user);

            if (result) 
            return RedirectToAction("Login","User");
           
            return RedirectToAction("RegisterAdmin", "User");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }


        public ActionResult VerifyEmail(string EmailId)
        {
          tblUser user=userRepo.VerifyEmail(EmailId);
            tblOTP objOTP = new tblOTP();
            bool result = false;
            if (user != null)
            {
                Random r=new Random();
                int otp=r.Next(100000,999999);
                objOTP.OTP =Convert.ToString(otp);
                objOTP.Created_DateTime=DateTime.Now;
                objOTP.IsUsed = 0;
                objOTP.EmailId = user.EmailId;
               result= userRepo.SaveOTP(objOTP);

                if (result)
                {
                    try
                    {

                        var senderEmail = new MailAddress("prp9096@gmail.com", "ShopNow");
                        var receiverEmail = new MailAddress(user.EmailId, "Receiver");
                        var password = "vmfhxvhenhoarjgx";
                        var sub = "OTP for Password Reset";
                        var body = "Your OTP is "+objOTP.OTP+" valid for 5 minutes.";

                        MailMessage message = new MailMessage();
                        message.To.Add(user.EmailId);// Email-ID of Receiver  
                        message.Subject = sub;// Subject of Email  
                        message.From = senderEmail;// Email-ID of Sender  
                        message.IsBodyHtml = true;
                        
                       
                        message.Body = body;
                        SmtpClient SmtpMail = new SmtpClient();

                        SmtpMail.Host = "smtp.gmail.com";
                        SmtpMail.Port = 587;
                        SmtpMail.EnableSsl = true;
                        SmtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
                        SmtpMail.UseDefaultCredentials = false;
                        SmtpMail.Credentials = new NetworkCredential(senderEmail.Address, password);
                        SmtpMail.Send(message);

                    }
                    catch (Exception)
                    {
                        ViewBag.Error = " Error Sending Email";
                    }
                }

            }
            
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerifyOtp(string Otp, string EmailId)
        {
            tblOTP objOTP=userRepo.getOtpByEmail(EmailId);
        
            string result="";


            if (objOTP.IsUsed == 0)
            {
                TimeSpan ts = DateTime.Now - objOTP.Created_DateTime;
                if (ts.Minutes <= 5)
                {
                    if (objOTP.OTP == Otp)
                    {
                        objOTP.IsUsed = 1;
                        userRepo.SaveOTP(objOTP);
                        result = "Valid OTP";
                    }
                    else
                    {
                        result = "Invalid OTP";
                    }
                }
                else
                {
                    result = "OTP expired";
                }
            }
            else
            {
                result = "OTP Already Used";
            }




            

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResetPassword(string EmailId)
        {
            tblUser user=userRepo.VerifyEmail(EmailId);
            ViewBag.EmailId=user.EmailId;

            return PartialView("_ResetPassword");
        }

        public ActionResult SavePassword(string email,string pass1)
        {
          tblUser user = userRepo.VerifyEmail(email);
            user.Password = pass1;
            userRepo.addUser(user);

            
           return View();
        }



        public ActionResult authenticateUser(tblUser obj)
        {
            var user = userRepo.authenticateUser(obj.EmailId, obj.Password);
            if(user != null)
            {
                Session["User"] = user;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.InvalidUser = "InvalidUser";
                return View("Login");
            }
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Login");
        }

        //-------------------------USING-VIEW-MODEL-----------------------------
        public ActionResult listUserCustomer()
        {
            UserCustomerModel userCustomerModel = new UserCustomerModel();
            userCustomerModel.listUser = userRepo.listUsers();
            userCustomerModel.listCustomer = userRepo.lstCustomers();
            userCustomerModel.UserCount=userCustomerModel.listUser.Count;
            userCustomerModel.CustomerCount=userCustomerModel.listCustomer.Count;


            return View(userCustomerModel);
        }
        //--------------------------AJAX-CUSTOMER&USER_LIST------------------------------
        public ActionResult ajaxAddCustomerListing()
        {
            return View();
        }

        

        public ActionResult ajaxListUserCustomer()
        {
            UserCustomerModel userCustomerModel = new UserCustomerModel();
            userCustomerModel.listUser = userRepo.listUsers();
            userCustomerModel.listCustomer = userRepo.lstCustomers();
            userCustomerModel.UserCount = userCustomerModel.listUser.Count;
            userCustomerModel.CustomerCount = userCustomerModel.listCustomer.Count;


            return PartialView("_AjaxUserCustomer",userCustomerModel);
        }





    }
}