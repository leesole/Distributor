using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Distributor.Helpers
{
    public static class GeneralHelpers
    {
        #region Get

        public static Guid GetGuidFromStringId(string stringId)
        {
            Guid guidId;
            Guid.TryParse(stringId, out guidId);

            return guidId;
        }

        #endregion
    }
}