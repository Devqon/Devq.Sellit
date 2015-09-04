using System;
using System.Collections.Generic;

namespace Devq.Sellit.ViewModels
{
    public class FeaturedProductsIndexViewModel
    {
        public DateTime ForDate { get; set; }
        public DateTime TimeLimit { get; set; }
        public IEnumerable<dynamic> FeaturedProducts { get; set; } 
    }
}