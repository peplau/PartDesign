using Feature.PartDesign.Mvc.ViewModels;
using Sitecore.Configuration;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using System;
using System.Web.Mvc;

namespace Feature.PartDesign.Mvc.Controllers
{
  public class PartDesignController : Controller
  {
    public ActionResult Index()
    {
      try
      {
        // Empty Datasource validation
        var datasourceItem = Sitecore.Context.Database.GetItem(RenderingContext.Current.Rendering.DataSource);
        if (datasourceItem == null)
        {
          if (!Sitecore.Context.PageMode.IsExperienceEditor)
            return null;
          var errorMessage = Settings.GetSetting("PartDesign.EmptyDatasourceMessage", Constants.EmptyDatasourceMessage);
          return View("Error", new ErrorViewModel(errorMessage));
        }

        // Build model and pass ahead to the View
        var model = new PartDesignViewModel
        {
          EditButtonLabel = Settings.GetSetting("PartDesign.EditPartDesignLabel", Constants.EditPartDesignLabel),
          EditButtonUrl = LinkManager.GetItemUrl(datasourceItem)
        };
        return View("PartDesign", model);
      }
      catch (Exception e)
      {
        Sitecore.Diagnostics.Log.Error("Error rendering PartDesign", e, GetType());
        var errorMessage = Settings.GetSetting("PartDesign.ComponentErrorMessage", Constants.ComponentErrorMessage);
        return View("Error", new ErrorViewModel(errorMessage));
      }
    }

    public ActionResult PartDesignEnd()
    {
      // Scape if no Datasource is selected
      var datasourceItem = Sitecore.Context.Database.GetItem(RenderingContext.Current.Rendering.DataSource);
      if (datasourceItem == null)
        return null;

      try
      {
        // Pass the PartDesignId to the view
        var rendering = RenderingContext.Current.Rendering;
        var partDesignId = rendering.Parameters.Contains(Constants.PartDesignIdParameter) ? rendering.Parameters[Constants.PartDesignIdParameter] : string.Empty;
        return View(new PartDesignEndViewModel()
        {
          PartDesignId = partDesignId
        });
      }
      catch (Exception e)
      {
        Sitecore.Diagnostics.Log.Error("Error rendering PartDesign End", e, GetType());
        var errorMessage = Settings.GetSetting("PartDesign.ComponentErrorMessage", Constants.ComponentErrorMessage);
        return View("Error", new ErrorViewModel(errorMessage));
      }
    }
  }
}