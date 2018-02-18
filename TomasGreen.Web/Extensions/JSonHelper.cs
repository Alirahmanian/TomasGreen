using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace TomasGreen.Web.Extensions
{
    public static class JSonHelper
    {
        public static string ToJSon(this object obj)
        {
            return (JsonConvert.SerializeObject(obj));
        }

        //public static string ToJSON(this object obj, int recursionDepth)
        //{
        //    var jsonConvert = new JsonConvert();
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    serializer.RecursionLimit = recursionDepth;
        //    return (JsonConvert.SerializeObject(obj,);
        //}
    }
}
