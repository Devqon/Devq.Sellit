using System;
using Devq.Bids.Models;
using Devq.Sellit.Services;
using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.Logging;
using Orchard.Tasks.Scheduling;

namespace Devq.Sellit.Handlers {
    [UsedImplicitly]
    public class FeaturedProductsTaskHandler : IScheduledTaskHandler {
        private readonly IFeaturedProductService _featuredProductService;

        public FeaturedProductsTaskHandler(IFeaturedProductService featuredProductService) {
            _featuredProductService = featuredProductService;
            Logger = NullLogger.Instance;

            try {
            }
            catch (Exception e) {
                Logger.Error(e, e.Message);
            }
        }

        public ILogger Logger { get; set; }

        public void Process(ScheduledTaskContext context) {
            if (context.Task.TaskType == Constants.FeaturedProductWinnersTask) {
                Logger.Information("Handling daily featured products at {0}",
                    context.Task.ScheduledUtc);

                // Handle the task
                _featuredProductService.HandleFeaturedProductWinners();
                
                // Get all current featured products
                var featured = _featuredProductService
                    .GetFeaturedProductsByDate(DateTime.UtcNow.AddDays(1))
                    .List();

                // Disable the bids for the featured products
                foreach (var ft in featured) {
                    ft.As<BidsPart>().BidsActive = false;
                }

                // Schedule next
                NextTaskDate();
            }
        }

        private void NextTaskDate() {
            var nextDate = _featuredProductService.GetNextTimeLimit();
            if (nextDate.HasValue) {
                _featuredProductService.ScheduleNextTask(nextDate.Value);
            }
        }
    }
}