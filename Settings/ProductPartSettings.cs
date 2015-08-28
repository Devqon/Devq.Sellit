using System.Collections.Generic;
using Orchard.Taxonomies.Models;

namespace Devq.Sellit.Settings
{
    public class ProductPartSettings
    {
        public decimal PostCosts { get; set; }
        public string CategoryId { get; set; }
        public IEnumerable<TermPart> Categories { get; set; }
    }
}