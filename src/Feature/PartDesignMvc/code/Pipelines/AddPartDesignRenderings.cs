using Feature.PartDesign.Mvc.Services;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Pipelines.Response.BuildPageDefinition;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Feature.PartDesign.Mvc.Pipelines
{
  public class AddPartDesignRenderings : BuildPageDefinitionProcessor
  {
    private IPartDesignService _partDesignService;
    public AddPartDesignRenderings(IPartDesignService partDesignService)
    {
      _partDesignService = partDesignService;
    }

    public override void Process(BuildPageDefinitionArgs args)
    {
      // Null Validations
      Assert.ArgumentNotNull(args, "args");
      if (args.Result == null)
        return;

      try
      {
        // Get all PartDesigns at the current page
        var partDesigns = args.Result.Renderings.Where(p => _partDesignService.IsPartDesign(p) && !string.IsNullOrEmpty(p.DataSource)).ToArray();
        if (!partDesigns.Any())
          return;

        foreach (var partDesign in partDesigns)
        {
          var loopSum = 1;

          // Skip this loop if the PartDesign has no Datasource
          var datasourceItem = partDesign.RenderingItem.Database.GetItem(partDesign.DataSource);
          if (datasourceItem == null)
            continue;
          // If the PartDesign has personalization, then update the data source
          datasourceItem = _partDesignService.GetDatasourceFromRules(args, partDesign, datasourceItem);
          if (datasourceItem == null)
            continue;

          // When the presentation details is loading from a datasource
          var fieldValue = LayoutField.GetFieldValue(new LayoutField(datasourceItem).InnerField);
          if (fieldValue.IsWhiteSpaceOrNull())
            return;
          var layoutFromField = XDocument.Parse(fieldValue).Root;
          var renderings = _partDesignService.GetRenderings(layoutFromField);
          var index = args.Result.Renderings.FindIndex(f => f.Id == partDesign.Id);

          // Loop throught the injected renderings to make them Read-only
          foreach (var cbRend in renderings.Where(p => p.RenderingType != "Layout"))
          {
            // Re-work the placeholder name, so it falls under the same placeholder where our PartDesign is located
            cbRend.Placeholder = cbRend.Placeholder.Replace(Constants.PartDesignPlaceholder, partDesign.Placeholder).Replace("//", "/");
            cbRend.Caching.Cacheable = false;

            // Make renderings non-editable
            var nonEditableRenderingItem = new ReadOnlyRendering(cbRend.RenderingItem.InnerItem);
            cbRend.RenderingItem = nonEditableRenderingItem;
            var field = cbRend.GetType().GetField("renderingItem", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null)
              field.SetValue(cbRend, nonEditableRenderingItem);

            args.Result.Renderings.Insert(index + loopSum, cbRend);
            loopSum++;
          }

          // Add PartDesign End - allowing us to know where it starts and finishes
          if (!Sitecore.Context.PageMode.IsExperienceEditorEditing)
            continue;
          var endRendering = GetPartDesignEnd(partDesign);
          if (endRendering == null)
            continue;
          args.Result.Renderings.Insert(index + loopSum, endRendering);
        }
      }
      catch (Exception e)
      {
        Log.Error(
            $"Error showing PartDesign components on item '{args.PageContext.Item.Paths.Path}'",
            e, GetType());
      }
    }

    /// <summary>
    /// Get instance pf PartDesignEnd
    /// </summary>
    /// <param name="rendering"></param>
    /// <param name="endRenderingItem"></param>
    /// <returns></returns>
    private static Rendering GetPartDesignEnd(Rendering rendering)
    {
      var endRenderingItem = rendering.Item.Database.GetItem(Constants.PartDesignEndRenderingId);
      if (endRenderingItem == null)
        return null;

      return new Rendering
      {
        UniqueId = ID.NewID.ToGuid(),
        Item = rendering.Item,
        RenderingItem = endRenderingItem,
        RenderingItemPath = endRenderingItem.ID.ToString(),
        RenderingType = rendering.RenderingType,
        Placeholder = rendering.Placeholder,
        DataSource = rendering.DataSource,
        Parameters = new RenderingParameters($"{Constants.PartDesignIdParameter}={rendering.UniqueId}"),
        DeviceId = rendering.DeviceId,
        LayoutId = rendering.LayoutId,
        Caching = { Cacheable = false },
        Renderer = new ControllerRenderer
        {
          ControllerName = endRenderingItem.Fields["Controller"].Value,
          ActionName = endRenderingItem.Fields["Controller Action"].Value
        },
      };
    }
  }
}