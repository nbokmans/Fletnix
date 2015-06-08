using System.Linq;
using System.Web;
using System.Web.Mvc;
using FletnixDatabase.Models;

namespace Fletnix
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
