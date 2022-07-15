using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopNowBL.Model;
using ShopNowBL.Repo;
using ShopNow.ViewModels1;
using Rotativa;
using System.IO;

namespace ShopNow.Controllers
{
    public class TransactionController : Controller
    {


        TransactionRepo transactionRepo= new TransactionRepo();
        UserRepo userRepo= new UserRepo();
        // GET: Transaction

        public ActionResult SelectProducts()
        {
            return View();
        }

        public ActionResult ajaxAddTransaction(string CustomerName,string MobileNo,int TotalQty,decimal InvoiceAmount,decimal GST,decimal TotalDiscount,string PaymentMethod,List<tblTransactionItem> TransactionItems)
        {

            tblCustomer customer=new tblCustomer();
            customer.CustomerName = CustomerName;
            customer.MobileNo = MobileNo;

            tblTransaction transaction = new tblTransaction();
            transaction.TotalQty=TotalQty;
            transaction.InvoiceAmount=InvoiceAmount;
            
            transaction.GST=GST;
            transaction.TotalDiscount=TotalDiscount;    
            transaction.PaymentMethod=PaymentMethod;

            var result = transactionRepo.CaptureTransaction(customer, transaction, TransactionItems);
            return Json(result,JsonRequestBehavior.AllowGet);
        }

       public ActionResult generateInvoice(string InvoiceNo,string hide)
        {
            InvoiceViewModel invoiceModel=new InvoiceViewModel();
            invoiceModel.listTransItems=transactionRepo.getTItemsByInvoice(InvoiceNo);
            invoiceModel.objTrans=transactionRepo.getTransByInvoice(InvoiceNo);
            invoiceModel.customer = userRepo.findCustomerById(invoiceModel.objTrans.CustomerId);
            ViewBag.Hide=hide;
         

            return PartialView("_generateInvoice",invoiceModel);
        }


        public ActionResult PrintInvoice(string InvoiceNo)
        {
            
            var a = new ViewAsPdf();
            a.ViewName = "_generateInvoice";
            InvoiceViewModel invoiceModel = new InvoiceViewModel();     
            
            invoiceModel.listTransItems=transactionRepo.getTItemsByInvoice(InvoiceNo);
            invoiceModel.objTrans=transactionRepo.getTransByInvoice(InvoiceNo);
            invoiceModel.customer=userRepo.findCustomerById(invoiceModel.objTrans.CustomerId);

          
            ViewBag.Hide = 1;

            a.Model = invoiceModel;
            var pdfBytes = a.BuildFile(this.ControllerContext);

            // Optionally save the PDF to server in a proper IIS location.
            var fileName = invoiceModel.customer.CustomerName+invoiceModel.objTrans.InvoiceNo + ".pdf";
            //path for storing pdf
            var path = Server.MapPath("~/Temp/" + fileName);
            
            System.IO.File.WriteAllBytes(path, pdfBytes);

            // return ActionResult
            MemoryStream ms = new MemoryStream(pdfBytes);
            return new FileStreamResult(ms, "application/pdf");
        }



    }
}