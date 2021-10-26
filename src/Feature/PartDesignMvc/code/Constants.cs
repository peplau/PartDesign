using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Feature.PartDesign.Mvc
{
  public static class Constants
  {
    public static ID PartDesignRenderingId = new ID("{0897A26E-283C-4278-A78A-DD2112972CF1}");
    public static ID PartDesignEndRenderingId = new ID("{EDE387DB-37EE-4FDB-9609-C9F59B0B9650}");
    public const string PartDesignIdParameter = "partDesignId";
    public const string PartDesignPlaceholder = "part-design";

    // Messages
    public const string EditPartDesignConfirmMessage = "If you have unsaved changes, they will be lost. Are you sure you want to proceed?";
    public const string EditPartDesignLabel = "Edit PartDesign";
    public const string EmptyDatasourceMessage = "PartDesign has no Datasource - Please, make sure to select a valid item as Datasource";
    public const string ComponentErrorMessage = "Error rendering PartDesign (check Sitecore logs for details)";
  }
}