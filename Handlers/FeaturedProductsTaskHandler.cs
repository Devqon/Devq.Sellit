using System;
using System.Linq;
using Devq.Sellit.Models;
using Devq.Sellit.Services;
using JetBrains.Annotations;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Logging;
using Orchard.Tasks.Scheduling;

namespace Devq.Sellit.Handlers {
    [UsedImplicitly]
    public class FeaturedProductsTaskHandler : IScheduledTaskHandler {
        private readonly IFeaturedProductService _featuredProductService;
        private readonly IScheduledTaskManager _scheduledTaskManager;
        private readonly IWorkContextAccessor _workContextAccessor;

        public FeaturedProductsTaskHandler(IFeaturedProductService featuredProductService, IWorkContextAccessor workContextAccessor, IScheduledTaskManager scheduledTaskManager) {
            _featuredProductService = featuredProductService;
            _scheduledTaskManager = scheduledTaskManager;
            _workContextAccessor = workContextAccessor;
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
                Logger.Information("Handling featured products winners at {0}",
                    context.Task.ScheduledUtc);

                // Handle the task
                _featuredProductService.HandleFeaturedProductWinners();

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
