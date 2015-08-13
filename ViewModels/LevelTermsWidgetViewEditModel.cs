using System.Collections.Generic;
using Orchard.Taxonomies.Models;

namespace Devq.Sellit.ViewModels
{
    public class LevelTermsWidgetViewEditModel
    {
        /// <summary>
        /// The Taxonomy to which this field is related to
        /// </summary>
        public string Taxonomy { get; set; }

        /// <summary>
        /// All existing taxonomies
        /// </summary>
        public IEnumerable<TaxonomyPart> Taxonomies { get; set; }
    }
}