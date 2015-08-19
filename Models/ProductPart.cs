﻿using System;
using Orchard.ContentManagement;

namespace Devq.Sellit.Models
{
    public class ProductPart : ContentPart<ProductPartRecord> {

        public DateTime? CreatedUtc {
            get { return Retrieve(r => r.CreatedUtc); }
            set { Store(r => r.CreatedUtc, value); }
        }

        public ProductStatus Status {
            get { return Retrieve(r => r.Status); }
            set { Store(r => r.Status, value); }
        }
    }
}