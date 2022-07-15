using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopNowBL.Repo;
using ShopNowBL.Model;
using System.Data.Entity.Migrations;

namespace ShopNow.Controllers
{
    public class StoreController : Controller
    {

        StoreRepo storeRepo = new StoreRepo();
        // GET: Store

        public ActionResult listStores()
        {
            var stores = storeRepo.listStores();
            

            return View(stores);
        }

        public ActionResult getStoreById(int Id)
        {
            tblStore store=storeRepo.findStoreById(Id);
            store.StrCreatedDate = store.CreatedDate.ToString();
            store.StrStartedDate = store.StartedDate.ToString();

            return Json(store,JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult addStore()
        {
            tblStore newStore = new tblStore();
            
            return View(newStore);
        }

        public ActionResult saveStore(tblStore newStore)
        {
            bool result = storeRepo.addStore(newStore);
            if (result)
            {
                return RedirectToAction("listStores");
            }
            return RedirectToAction("addStore");
        }

        public ActionResult editStore(int id)
        {
            tblStore store = storeRepo.findStoreById(id);
            return View("addStore", store);
        }

        public ActionResult deleteStore(int id)
        {
            bool result = storeRepo.deleteStore(id);
            if (result)
            {
                return RedirectToAction("listStores");
            }
            return RedirectToAction("Home", "Index");
        }

        public ActionResult ajaxAddStoreListing()
        {
            return View();
        }

        public ActionResult ajaxListStores()
        {

            var storesList = storeRepo.listStores();
            return PartialView("_ajaxListStores", storesList);


        }
        public ActionResult ajaxAddStore(string StoreName, string Address, string City, string ContactNo,DateTime StartedDate, string Id)
        {
          
            
                tblStore newStore = new tblStore();
            if (!string.IsNullOrEmpty(Id))
            { 

                newStore.Id=Convert.ToInt32(Id);
            }

                newStore.StoreName = StoreName;
                newStore.Address = Address;
                newStore.City = City;
                newStore.ContactNo = ContactNo;
            newStore.StartedDate = StartedDate;

                var result = storeRepo.addStore(newStore);
                return Json(result, JsonRequestBehavior.AllowGet);
            
            
            
            
        }

        public ActionResult ajaxDeleteStore(int Id)
        {
            var result = storeRepo.deleteStore(Id);
            if(result)
            return Json(result,JsonRequestBehavior.AllowGet);
            else
            {
                result = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
                
        }
    }
}