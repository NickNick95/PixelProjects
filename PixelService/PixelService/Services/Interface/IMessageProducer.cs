namespace PixelService.Services.Interface
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
    }
}
