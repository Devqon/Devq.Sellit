using System.Collections.Generic;

namespace Devq.Sellit.ViewModels
{
    public class SelectCategoryViewModel
    {
        public string SelectedCategory { get; set; }
        public IDictionary<string, string> Categories { get; set; } 
    }
}