using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TeknikServis.UI.Filters
{
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var path = httpContext.Request.Path.Value;

            //// ✅ API yolları filtrelenmesin (sonsuz döngüyü önler)
            //if (path != null && path.StartsWith("/api", StringComparison.OrdinalIgnoreCase))
            //{
            //    base.OnActionExecuting(context);
            //    return;
            //}

            // ✅ Login ve kayıt sayfasına filtre uygulama
            if (path != null && (path.Contains("/Kullanici/Login") || path.Contains("/Kullanici/Kayit")))
            {
                //base.OnActionExecuting(context);
                return;
            }
               

            var adSoyad = httpContext.Session.GetString("adSoyad");

            if (string.IsNullOrEmpty(adSoyad))
            {
                context.Result = new RedirectToActionResult("Login", "Kullanici", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
