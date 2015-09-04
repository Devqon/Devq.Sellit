using System;
using System.Collections.Generic;
using Devq.Sellit.Models;
using Orchard;
using Orchard.ContentManagement;

namespace Devq.Sellit.Services {
    public interface IFeaturedProductService : IDependency {
        IContentQuery<FeaturedProductPart> GetFeaturedProductsToday();
        IContentQuery<FeaturedProductPart> GetFeaturedProductsByDate(DateTime date);
        IContentQuery<FeaturedProductPart, FeaturedProductPartRecord> GetFeaturedProductsQuery();
        IContent CreateFeaturedProduct(DateTime? date, int number);
        void HandleFeaturedProductWinners();
        void ScheduleNextTask(DateTime date);
        DateTime? GetNextTimeLimit();
        DateTime BuildTimeLimit(DateTime forDate, DateTime timeLimit);
    }
}