using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopNowBL.Repo;
using ShopNowBL.Model;
using Newtonsoft.Json;

namespace ShopNow.Controllers
{
    public class StockController : Controller
    {
        StockRepo stockRepo = new StockRepo();
        // GET: Stock
        public ActionResult listProducts()
        {
            var products=stockRepo.listProducts();
            return View(products);
        }

        public ActionResult addProduct()
        {
            tblStock newProduct=new tblStock();
            return View(newProduct);
        }

        public ActionResult saveProduct(tblStock newProduct)
        {
            bool result = stockRepo.addProduct(newProduct);
            if (result)
            {
                return RedirectToAction("listProducts");
            }
            return RedirectToAction("addProduct");
        }

        public ActionResult editProduct(int id)
        {
            tblStock product=stockRepo.getProductById(id);
            return View("addProduct",product);
        }

        public ActionResult deleteProduct(int id)
        {
            bool result = stockRepo.deleteProduct(id);
            if (result)
            {
                return RedirectToAction("listProducts");
            }
            return RedirectToAction("Home","Index");
        }

        public ActionResult ajaxAddProductListing()
        {
            return View();
        }

        public ActionResult ajaxListProducts()
        {
            
            var productList=stockRepo.listProducts();
            return PartialView("_ajaxListProducts", productList);
            
           
        }
        public ActionResult ajaxAddProduct( string ProductName,int ProductQty,decimal BasePrice,decimal SellingPrice,int Discount,string Id)
        {
            tblStock newProduct = new tblStock();

            if (!string.IsNullOrEmpty(Id))
            {
                newProduct.Id = Convert.ToInt32(Id);
            }
           
            newProduct.ProductName = ProductName;
            newProduct.ProductQty = ProductQty; 
            newProduct.BasePrice = BasePrice;
            newProduct.SellingPrice = SellingPrice;
            newProduct.Discount = Discount;
            var result = stockRepo.addProduct(newProduct);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ajaxGetProdById(int Id)
        {
            tblStock product=stockRepo.getProductById(Id);
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        
             public ActionResult ajaxDeleteProduct(int Id)
        {
            var result = stockRepo.deleteProduct(Id);
            if (result)
                return Json(result, JsonRequestBehavior.AllowGet);
            else
            {
                result = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ajaxGetProductList()
        {
            var productList = stockRepo.listProducts();
            return Json(productList, JsonRequestBehavior.AllowGet);
        }






      
    }

}