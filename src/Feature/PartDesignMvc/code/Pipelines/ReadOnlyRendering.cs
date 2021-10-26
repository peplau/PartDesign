using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Feature.PartDesign.Mvc.Pipelines
{
  public class ReadOnlyRendering : RenderingItem
  {
    public ReadOnlyRendering(Item innerItem) : base(innerItem) { }

    public override bool Editable => false;
  }
}