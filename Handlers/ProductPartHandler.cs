using System;
using System.Collections.Generic;
using Devq.Sellit.Models;
using Devq.Sellit.Settings;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Tasks.Scheduling;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;

namespace Devq.Sellit.Handlers
{
    public class ProductPartHandler : ContentHandler {

        private readonly ITaxonomyService _taxonomyService;
        private readonly IScheduledTaskManager _scheduledTaskManager;
        private readonly IWorkContextAccessor _workContextAccessor;

        public ProductPartHandler(IRepository<ProductPartRecord> repository, 
            ITaxonomyService taxonomyService, IScheduledTaskManager scheduledTaskManager, IWorkContextAccessor workContextAccessor) {

            _taxonomyService = taxonomyService;
            _scheduledTaskManager = scheduledTaskManager;
            _workContextAccessor = workContextAccessor;

            Filters.Add(StorageFilter.For(repository));

            OnCreated<ProductPart>(SetCategory);
            OnPublished<ProductPart>(ScheduleUnpublish);
        }

        private void ScheduleUnpublish(PublishContentContext ctx, ProductPart part) {
            _scheduledTaskManager.DeleteTasks(part.ContentItem, t => t.TaskType == Constants.UnpublishTaskName);

            var settings = _workContextAccessor.GetContext().CurrentSite.Get<ProductSettingsPart>();
            if (settings.HideProductDelay > 0) {
                // Schedule the unpublish moment
                var dateToUnpublish = DateTime.UtcNow.AddDays(settings.HideProductDelay);
                _scheduledTaskManager.CreateTask(Constants.UnpublishTaskName, dateToUnpublish, part.ContentItem);
            }
        }

        private void SetCategory(CreateContentContext ctx, ProductPart part) {

            var settings = part.Settings.GetModel<ProductPartSettings>();

            var category = settings.CategoryId;
            int categoryId;
            if (category == "0" || !int.TryParse(category, out categoryId))
                return;

            var term = _taxonomyService.GetTerm(categoryId);

            // Store category in part
            part.Category = term.Name;

            // As well as in the termspart, to make use of the taxonomies
            _taxonomyService.UpdateTerms(part.ContentItem, new List<TermPart>{term}, Constants.CategoryTaxonomyName);
        }
    }
}