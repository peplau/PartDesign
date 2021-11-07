using System.Linq;
using System.Web;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Shell.Applications.WebEdit.Commands;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Feature.PartDesign.Mvc.Commands
{
  public class EditPartDesign : WebEditCommand
  {
    public override void Execute(CommandContext context)
    {
      // Validations
      Assert.ArgumentNotNull(context, nameof(context));

      var datasourceItem = context.Items.FirstOrDefault();
      if (datasourceItem == null)
        return;

      var targetUrl = LinkManager.GetItemUrl(datasourceItem);

      // Window URL
      //var windowUrl = $"/?sc_mode=edit&sc_itemid={HttpUtility.UrlEncode(datasourceItem.ID.ToString())}&sc_site={Sitecore.Context.Site.Name}";
      //var fullScript = "top.location.href='" + windowUrl + "';";
      var fullScript = "top.location.href='" + targetUrl + "';";

      // Confirm is only necessary if the item was modified
      //if (SheerResponse.CheckModified())
      //{
        var confirmMessage = Settings.GetSetting("PartDesign.EditConfirmMessage", Constants.EditPartDesignConfirmMessage);
        fullScript = "if (confirm('" + confirmMessage + "')) { " + fullScript + " }";
      //}

      // Triggering the window
      SheerResponse.Eval(fullScript);
    }
  }
}