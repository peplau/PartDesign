using Sitecore.Data.Items;
using Sitecore.Mvc.Pipelines.Response.BuildPageDefinition;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Feature.PartDesign.Mvc.Services
{
  public interface IPartDesignService
  {
    bool IsPartDesign(Sitecore.Mvc.Presentation.Rendering rendering);
    Item GetDatasourceFromRules(BuildPageDefinitionArgs args, Rendering r, Item ds);
    IEnumerable<Rendering> GetRenderings(XElement layoutDefinition);
    Rendering GetRendering(XElement renderingNode, Guid deviceId, Guid layoutId, string renderingType, XmlBasedRenderingParser parser);
  }
}
