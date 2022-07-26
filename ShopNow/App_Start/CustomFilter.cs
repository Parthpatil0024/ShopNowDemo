using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopNow { 
    public class CustomFilter:ActionFilterAttribute
    {
  
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sessionUser = filterContext.HttpContext.Session["User"];
            var routeValues = filterContext.RequestContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.RequestContext.RouteData.Values["action"].ToString();
            if (sessionUser == null && !actionName.Equals("Login") && !actionName.Equals("authenticateUser") && 
                !actionName.Equals("RegisterAdmin") && !actionName.Equals("ForgotPassword") &&
                !actionName.Equals("SaveAdmin") && !actionName.Equals("VerifyEmail") && 
                !actionName.Equals("VerifyOtp") && !actionName.Equals("ResetPassword") && 
                !actionName.Equals("SavePassword"))
            {
                filterContext.Result = new RedirectResult("~/User/Login");
            }
            
                
               
            
            base.OnActionExecuting(filterContext);
        }
    }

}

