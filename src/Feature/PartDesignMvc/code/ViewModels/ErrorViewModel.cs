using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Feature.PartDesign.Mvc.ViewModels
{
  public class ErrorViewModel
  {
    public ErrorViewModel(string message)
    {
      ErrorMessage = message;
    }
    public string ErrorMessage { get; set; }
  }
}