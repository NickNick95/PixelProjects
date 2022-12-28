using Storage.Data.Model;

namespace Storage.Data.Repositories
{
    public interface IFileRepository
    {
        void SaveToTmpFile(string fileName, TrackModel track);

        string GetLogsFromTmpFile(string fileName);
    }
}
