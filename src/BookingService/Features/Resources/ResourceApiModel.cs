using BookingService.Data.Model;

namespace BookingService.Features.Resources
{
    public class ResourceApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }

        public static TModel FromResource<TModel>(Resource resource) where
            TModel : ResourceApiModel, new()
        {
            var model = new TModel();
            model.Id = resource.Id;
            model.TenantId = resource.TenantId;
            model.Name = resource.Name;
            return model;
        }

        public static ResourceApiModel FromResource(Resource resource)
            => FromResource<ResourceApiModel>(resource);

    }
}
