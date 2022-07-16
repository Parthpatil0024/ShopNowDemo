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
using System.Net.Mail;
using System.Net;

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

       public ActionResult generateInvoice(string InvoiceNo,string hide,string Email)
        {
            InvoiceViewModel invoiceModel=new InvoiceViewModel();
            invoiceModel.listTransItems=transactionRepo.getTItemsByInvoice(InvoiceNo);
            invoiceModel.objTrans=transactionRepo.getTransByInvoice(InvoiceNo);
            invoiceModel.customer = userRepo.findCustomerById(invoiceModel.objTrans.CustomerId);
            ViewBag.Hide=hide;
            ViewBag.Email = Email;
         

            return PartialView("_generateInvoice",invoiceModel);
        }


        public ActionResult PrintInvoice(string InvoiceNo,string Email)
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
            var fileName = invoiceModel.customer.CustomerName+"-"+invoiceModel.objTrans.InvoiceNo + ".pdf";
            //path for storing pdf
            var path = Server.MapPath("~/Temp/" + fileName);
            
            System.IO.File.WriteAllBytes(path, pdfBytes);

            MemoryStream ms = new MemoryStream(pdfBytes);

            try
            {

                var senderEmail = new MailAddress("prp9096@gmail.com", "Parth");
                var receiverEmail = new MailAddress(Email, "Receiver");
                var password = "vmfhxvhenhoarjgx";
                var sub = "Invoice For Your Purchase dated "+ invoiceModel.objTrans.InvoiceDate;
                var body = "Thank you for your purchase "+ invoiceModel.customer.CustomerName+". Your Invoice is here. Visit Again!!";

                MailMessage message = new MailMessage();
                message.To.Add(Email);// Email-ID of Receiver  
                message.Subject = sub;// Subject of Email  
                message.From = senderEmail;// Email-ID of Sender  
                message.IsBodyHtml = true;
                Attachment data = new Attachment(ms, fileName, "application/pdf");
                message.Attachments.Add(data);
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





            return RedirectToAction("SelectProducts", "Transaction");
        }



    }
}