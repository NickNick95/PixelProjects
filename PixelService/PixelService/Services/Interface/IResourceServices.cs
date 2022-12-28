namespace PixelService.Services.Interface
{
    public interface IResourceServices
    {
        public Task<byte[]> GetGifByDefaultPathAsync();
    }
}
