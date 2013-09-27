using System;
using System.Web.Optimization;

namespace FSIX.Web {
  public class DurandalBundleConfig {
    public static void RegisterBundles(BundleCollection bundles) {
      bundles.IgnoreList.Clear();
      AddDefaultIgnorePatterns(bundles.IgnoreList);

	  bundles.Add(
		new ScriptBundle("~/Scripts/vendor.js")
			.Include("~/Scripts/jquery-{version}.js")
			.Include("~/Scripts/bootstrap.js")
			.Include("~/Scripts/knockout-{version}.js")
            .Include("~/scripts/bootstrap.js")
            .Include("~/scripts/es5-shim.js")
            .Include("~/scripts/es5-sham.js")
            .Include("~/scripts/Q.js")
            .Include("~/scripts/breeze.debug.js")
        );

      bundles.Add(
        new StyleBundle("~/Content/css")
          //.Include("~/Content/ie10mobile.css")
          .Include("~/Content/bootstrap/bootstrap.min.css")
          //.Include("~/Content/bootstrap/bootstrap-theme.min.css")
          .Include("~/Content/font-awesome.min.css")
		  .Include("~/Content/durandal.css")
          //.Include("~/Content/starterkit.css")
          .Include("~/Content/toastr.css")
          .Include("~/Content/app.css")
        );
    }

    public static void AddDefaultIgnorePatterns(IgnoreList ignoreList) {
      if(ignoreList == null) {
        throw new ArgumentNullException("ignoreList");
      }

      ignoreList.Ignore("*.intellisense.js");
      ignoreList.Ignore("*-vsdoc.js");
      ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
      //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
      //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
    }
  }
}