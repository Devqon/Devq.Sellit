using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace Devq.Sellit.Models
{
    public class LevelTermsWidgetPart : ContentPart<LevelTermsWidgetPartRecord>
    {
        internal LazyField<IEnumerable<IContent>> _levelTermsField = new LazyField<IEnumerable<IContent>>();

        [Required]
        public string Taxonomy
        {
            get { return Retrieve(r => r.Taxonomy); }
            set { Store(r => r.Taxonomy, value); }
        }

        public IEnumerable<IContent> LevelTerms {
            get { return _levelTermsField.Value; }
        }
    }
}