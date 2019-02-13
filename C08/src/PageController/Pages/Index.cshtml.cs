using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PageController.Pages
{
    public class IndexModel : PageModel
    {
        public string Toto { get; set; }

        public void OnGet(string toto)
        {
            Toto = toto;
        }
    }
}
