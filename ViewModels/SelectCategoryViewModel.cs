using System.Collections.Generic;

namespace Devq.Sellit.ViewModels
{
    public class SelectCategoryViewModel
    {
        public string SelectedCategory { get; set; }
        public IDictionary<int, string> Categories { get; set; } 
    }
}