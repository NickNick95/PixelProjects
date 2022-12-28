using PixelService.Services.Interface;

namespace PixelService.Services
{
    public class ResourceServices : IResourceServices
    {
        private readonly IConfiguration _config;

        public ResourceServices(IConfiguration config)
        {
            _config = config;
        }

        public async Task<byte[]> GetGifByDefaultPathAsync()
        {
            var defPath = Path.Combine("Source", _config.GetValue<string>("GifName"));
            var gif = await File.ReadAllBytesAsync(defPath);

            return gif;
        }
    }
}
