using Storage.Data.Model;

namespace Storage.Data.Repositories
{
    public class FileRepository : IFileRepository
    {
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

        public string GetLogsFromTmpFile(string fileName)
        {
           var logs = string.Empty;

            _readWriteLock.EnterReadLock();
            try
            {
                using (StreamReader sr = File.OpenText($"{Path.GetTempPath()}{fileName}"))
                {
                    logs = sr.ReadToEnd();
                    sr.Close();
                }
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }

            return logs;
        }

        public void SaveToTmpFile(string fileName, TrackModel track)
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                using (StreamWriter sw = File.AppendText($"{Path.GetTempPath()}{fileName}"))
                {
                    var log = $"{DateTime.UtcNow} | {track.ReferrerHeader} | {track.UserAgentHeader} | {track.VisitorIPAddress}";
                    sw.WriteLine(log);
                    sw.Close();
                }
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }
    }
}
