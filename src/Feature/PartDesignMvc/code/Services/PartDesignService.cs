using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;
using Sitecore.Analytics;
using Sitecore.Analytics.Core;
using Sitecore.ContentTesting;
using Sitecore.ContentTesting.Model.Data.Items;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Layouts;
using Sitecore.Mvc.Configuration;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Pipelines.Response.BuildPageDefinition;
using Sitecore.Mvc.Presentation;
using Sitecore.Rules;
using Sitecore.Rules.ConditionalRenderings;

namespace Feature.PartDesign.Mvc.Services
{
  public class PartDesignService : IPartDesignService
  {
    /// <summary>
    /// Checks if the passed rendering is a PartDesign rendering
    /// </summary>
    /// <param name="rendering"></param>
    /// <returns></returns>
    public bool IsPartDesign(Sitecore.Mvc.Presentation.Rendering rendering)
    {
      return rendering.RenderingItemPath == Constants.PartDesignRenderingId.ToString();
    }


    /// <summary>
    /// Process datasources if any personalization is in place
    /// </summary>
    /// <param name="args"></param>
    /// <param name="r"></param>
    /// <param name="ds"></param>
    /// <returns></returns>
    public Item GetDatasourceFromRules(BuildPageDefinitionArgs args, Rendering r, Item ds)
    {
      // Gets rendering XML
      var xml = r.Properties["RenderingXml"];
      // No XML? Returns the item itself
      if (string.IsNullOrEmpty(xml))
        return ds;

      // Instantiates renderingReference, from there obtain the rules
      var renderingReference = new RenderingReference(XElement.Parse(xml).ToXmlNode(), Sitecore.Context.Language, args.PageContext.Database);
      var rules = renderingReference.Settings.Rules;
      // No rules? Returns the item itself
      if (rules.Count == 0)
        return ds;

      // Instantiates the ruleContext to run the rule
      var renderingsRuleContext =
          new ConditionalRenderingsRuleContext(new List<RenderingReference> { renderingReference },
              renderingReference)
          { Item = args.PageContext.Item };
      rules.Evaluated += RulesEvaluatedHandler;
      rules.RunFirstMatching(renderingsRuleContext);

      var newReference = renderingsRuleContext.References.Find(r1 => r1.UniqueId == renderingsRuleContext.Reference.UniqueId);
      var dataSource = newReference.Settings.DataSource;
      if (string.IsNullOrEmpty(dataSource))
        return ds;
      var obj = newReference.Database.GetItem(dataSource);
      if (obj != null)
        ds = obj;
      return ds;
    }

    private void RulesEvaluatedHandler(RuleList<ConditionalRenderingsRuleContext> ruleList, ConditionalRenderingsRuleContext ruleContext, Rule<ConditionalRenderingsRuleContext> rule)
    {
      if (!ruleContext.IsTesting)
        return;
      Item testDefinitionItem = ruleContext.Item.Database.GetItem(ruleContext.TestId);
      if (testDefinitionItem == null || this.ShouldAllowRule(rule, testDefinitionItem, ruleContext))
        return;
      ruleContext.SkipRule = true;
    }


    private bool ShouldAllowRule(Rule<ConditionalRenderingsRuleContext> rule, Item testDefinitionItem, ConditionalRenderingsRuleContext ruleContext)
    {
      TestDefinitionItem testDefinition = TestDefinitionItem.Create(testDefinitionItem);
      if (testDefinition == null || !testDefinition.IsRunning)
        return true;

      var tracker = ContentTestingFactory.Instance.GetTestingTracker();
      var testId = tracker.GetTestId();
      if (testId.IsNull)
        return true;

      var combination = tracker.GetTestCombination(testId.Guid);
      if (combination == null)
        return true;

      if (!Tracker.Current.CurrentPage.IsTestSetIdNull() && Tracker.Current.CurrentPage.MvTest.Id == testDefinition.ID.ToGuid())
        return combination.GetValue(Guid.Parse(ruleContext.Reference.UniqueId)).Id == rule.UniqueId.ToGuid();

      if (Tracker.Current.CurrentPage.IsTestSetIdNull())
        combination.SaveTo((ISupportsTesting)Tracker.Current.CurrentPage);

      return combination.GetValue(Guid.Parse(ruleContext.Reference.UniqueId)).Id == rule.UniqueId.ToGuid();
    }

    /// <summary>
    /// Recursively retrieves all renderings from a given layoutDefinition
    /// </summary>
    /// <param name="layoutDefinition"></param>
    /// <returns></returns>
    public IEnumerable<Rendering> GetRenderings(XElement layoutDefinition)
    {
      var parser = MvcSettings.GetRegisteredObject<XmlBasedRenderingParser>();
      foreach (var xelement in layoutDefinition.Elements("d"))
      {
        var deviceId = xelement.GetAttributeValueOrEmpty("id").ToGuid();
        var layoutId = xelement.GetAttributeValueOrEmpty("l").ToGuid();
        yield return GetRendering(xelement, deviceId, layoutId, "Layout", parser);
        foreach (var renderingNode in xelement.Elements("r"))
          yield return GetRendering(renderingNode, deviceId, layoutId, renderingNode.Name.LocalName, parser);
      }
    }

    /// <summary>
    /// Retrieves a rendering from a given Node, allowing to pass some filters
    /// </summary>
    /// <param name="renderingNode"></param>
    /// <param name="deviceId"></param>
    /// <param name="layoutId"></param>
    /// <param name="renderingType"></param>
    /// <param name="parser"></param>
    /// <returns></returns>
    public Rendering GetRendering(XElement renderingNode, Guid deviceId, Guid layoutId, string renderingType, XmlBasedRenderingParser parser)
    {
      var rendering = parser.Parse(renderingNode, false);
      rendering.DeviceId = deviceId;
      rendering.LayoutId = layoutId;
      if (renderingType != null)
        rendering.RenderingType = renderingType;
      return rendering;
    }
  }
}