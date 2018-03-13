using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Web.BaseModels;

namespace TomasGreen.Web.ExceptionsHandlers
{
    public static class CustomException
    {
        public static string Filter(IHostingEnvironment hostingEnvironment, IStringLocalizer<BaseController> localizer, string controllerName, string actionName, Exception exception)
        {

            return "";
        }
    }
}
