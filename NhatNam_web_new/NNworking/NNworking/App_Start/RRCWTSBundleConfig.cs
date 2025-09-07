using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace NNworking
{
    public class RRCWTSBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var scriptRRCWTSBundle = new ScriptBundle("~/Content/wts/RRCWTSCustomJS");
            var styleRRCWTSBundle = new StyleBundle("~/Content/wts/RRCWTSCustomCSS");

            //// All css file
            styleRRCWTSBundle.Include("~/Content/wts/css/jquery-ui.css")
                            //// Changable css
                            .Include("~/Content/wts/css/flaty.css")
                            //// Custom css
                            .Include("~/Content/wts/css/Style.css")
                            .Include("~/Content/wts/css/examples.css")
                            .Include("~/Content/wts/css/flaty-responsive.css")
                            .Include("~/Content/wts/css/font-awesome.css")
                            .Include("~/Content/wts/css/bootstrap.min.css");

            scriptRRCWTSBundle.Include("~/Content/wts/js/jszip.min.js")
                            .Include("~/Scripts/wts/jquery.min.js")
                            .Include("~/Content/wts/js/bootstrap.min.js")
                            .Include("~/Content/wts/js/flaty.js")
                            .Include("~/Content/wts/js/jquery-3.3.1.min.js")
                            .Include("~/Content/wts/jquery.cookie.js");
                            //// signalR - hubs js
                            //.Include("~/Scripts/wts/jquery.signalR-2.3.0.min.js")
                            //.Include("~/signalr/hubs")
                            ////// Custom js
                            //.Include("~/Content/wts/js/CustomJS/HeaderJS.js")
                            //.Include("~/Content/wts/js/CustomJS/NotificationJS.js");


            bundles.Add(scriptRRCWTSBundle);
            bundles.Add(styleRRCWTSBundle);
#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}