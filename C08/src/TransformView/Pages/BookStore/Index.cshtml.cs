using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TransformView.Models;
using TransformView.Services;

namespace TransformView.Pages.BookStore
{
    public class IndexModel : PageModel
    {
        private readonly ICorporationFactory _corporationFactory;

        public IndexModel(ICorporationFactory corporationFactory)
        {
            _corporationFactory = corporationFactory ?? throw new ArgumentNullException(nameof(corporationFactory));
        }

        public ReadOnlyCollection<IComponent> Components { get; private set; }

        public void OnGet()
        {
            var corporation = _corporationFactory.Create();
            Components = new ReadOnlyCollection<IComponent>(new IComponent[] { corporation });
        }
    }
}