using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopNowBL.Repo;
using ShopNowBL.Model;
using System.Data.Entity.Migrations;
using ShopNow.ViewModels1;


namespace ShopNow.Controllers
{
    public class UserController : Controller
    {
        // GET: Customer
        UserRepo userRepo = new UserRepo();


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
            tblUser newUser = new tblUser();
            return View(newUser);
        }

        public ActionResult saveUser(tblUser newUser)
        {
            bool result = userRepo.addUser(newUser);
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
            tblUser user = userRepo.findUserById(id);

            return View("addUser", user);
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