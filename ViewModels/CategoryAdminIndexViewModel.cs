using System.Collections.Generic;

namespace Devq.Sellit.ViewModels
{
    public class CategoryAdminIndexViewModel
    {
        public IList<CategoryEntry> Categories { get; set; }
        public dynamic Pager { get; set; }
        public CategoryAdminIndexBulkAction BulkAction { get; set; }
    }

    public class CategoryEntry
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }

    public enum CategoryAdminIndexBulkAction
    {
        None,
        Delete,
    }
}