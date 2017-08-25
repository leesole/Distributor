using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Distributor.Models
{
    public class BaseViewWithCallingFields
    {
        public string CallingController { get; set; }

        public string CallingAction { get; set; }
    }
}