using MicroMotel.Web.Models.BaseModels;
using Microsoft.Extensions.Options;

namespace MicroMotel.Web.Helpers
{
    public class PhotoHelper
    {
        private readonly ServiceURLs _serviceurls;

        public PhotoHelper(IOptions<ServiceURLs> serviceurls)
        {
            _serviceurls = serviceurls.Value;
        }

        public string GetPhotoStockURL(string photourl)
        {
            return $"{_serviceurls.PhotoStockURL}/{_serviceurls.photoget.Path}/photos/{photourl}";
        }
    }
}
