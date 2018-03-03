using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArmyBuilder.Web.Pages
{
    public class IndexModel : PageModel
    {
        public string Title { get; } = "ArmyBuilder";

        public void OnGet()
        {
        }
    }
}