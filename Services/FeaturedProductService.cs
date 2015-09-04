using System;
using System.Collections.Generic;
using System.Linq;
using Devq.Bids.Models;
using Devq.Bids.Services;
using Devq.Sellit.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Messaging.Services;
using Orchard.Tasks.Scheduling;

namespace Devq.Sellit.Services
{
    public class FeaturedProductService : IFeaturedProductService {

        private readonly IContentManager _contentManager;
        private readonly IMessageService _messageService;
        private readonly IBidService _bidService;
        private readonly IScheduledTaskManager _scheduledTaskManager;
        private readonly IWorkContextAccessor _workContextAccessor;

        public FeaturedProductService(IContentManager contentManager, IBidService bidService, IMessageService messageService, IShapeFactory shapeFactory, IScheduledTaskManager scheduledTaskManager, IWorkContextAccessor workContextAccessor) {
            _contentManager = contentManager;
            _bidService = bidService;
            _messageService = messageService;

            Shape = shapeFactory;
            _scheduledTaskManager = scheduledTaskManager;
            _workContextAccessor = workContextAccessor;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        public dynamic Shape { get; set; }

        /// <summary>
        /// The query
        /// </summary>
        /// <returns></returns>
        public IContentQuery<FeaturedProductPart, FeaturedProductPartRecord> GetFeaturedProductsQuery()
        {
            return _contentManager
                .Query<FeaturedProductPart, FeaturedProductPartRecord>(VersionOptions.Published);
        } 

        public IContentQuery<FeaturedProductPart> GetFeaturedProductsToday()
        {
            return GetFeaturedProductsByDate(DateTime.Now);
        }

        /// <summary>
        /// Get featured products by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public IContentQuery<FeaturedProductPart> GetFeaturedProductsByDate(DateTime date) {
            return GetFeaturedProductsQuery()
                .Where<FeaturedProductPartRecord>(f => f.Date >= date.Date && f.Date < date.Date.AddDays(1));
        }

        /// <summary>
        /// Create a featured product item
        /// </summary>
        /// <param name="date"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public IContent CreateFeaturedProduct(DateTime? date, int number) {
            var newFeatured = _contentManager.New<FeaturedProductPart>("FeaturedProduct");
            newFeatured.Date = date;
            newFeatured.Number = number;
            _contentManager.Create(newFeatured);

            return newFeatured;
        }

        public void HandleFeaturedProductWinners() {

            var featuredProducts = GetFeaturedProductsToday().List();
            foreach (var fp in featuredProducts) {
                var heighestBid = _bidService.GetHeighestBid(fp.Id);
                // No bids so nothing to handle
                if (heighestBid == null)
                    continue;

                var winner = heighestBid.Owner;
                var template = Shape.Create("Template_FeaturedProductsWinner_Notification", Arguments.From(new
                {
                    BidPart = heighestBid
                }));

                var parameters = new Dictionary<string, object> {
                    {"Subject", T("Bid notification").Text},
                    {"Body", Shape.Display(template)},
                    {"Recipients", winner.Email}
                };

                _messageService.Send("Email", parameters);
            }
        }

        public void ScheduleNextTask(DateTime date) {
            if (date > DateTime.UtcNow)
            {
                var tasks = _scheduledTaskManager.GetTasks(Constants.FeaturedProductWinnersTask).ToList();
                if (tasks.Any()) {
                    // Delete any existing
                    _scheduledTaskManager.DeleteTasks(tasks.First().ContentItem);
                }
                _scheduledTaskManager.CreateTask(Constants.FeaturedProductWinnersTask, date, null);
            }
        }

        public DateTime? GetNextTimeLimit() {
            var timeLimit = _workContextAccessor.GetContext().CurrentSite.Get<FeaturedProductsSettingsPart>().TimeLimit;
            if (!timeLimit.HasValue)
            {
                return null;
            }

            var now = DateTime.UtcNow;
            if (now.TimeOfDay > timeLimit.Value.TimeOfDay)
                now = now.AddDays(1);

            var nextTask = BuildTimeLimit(now, timeLimit.Value);
            return nextTask;
        }

        public DateTime BuildTimeLimit(DateTime forDate, DateTime timeLimit)
        {
            return new DateTime(forDate.Year, forDate.Month, forDate.Day, timeLimit.Hour, timeLimit.Minute, timeLimit.Second);
        }
    }
}