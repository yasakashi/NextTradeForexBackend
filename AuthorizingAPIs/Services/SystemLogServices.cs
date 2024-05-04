using Newtonsoft.Json;
using DataLayers;
using Entities.Systems;

namespace NextTradeAPIs.Services
{
    /// <summary>
    /// ثبت لاگ در سیستم
    /// </summary>
    public class SystemLogServices
    {
        LogSBbContext _Context { get; set; }
        private string _LogPath = string.Empty;

        public SystemLogServices(LogSBbContext contex)
        {
            _Context = contex;
            _LogPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\logs\\";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logdesc"></param>
        /// <returns></returns>
        public async Task InsertLogs(string logdesc)
        {
            try
            {
                _Context.SystemLogs.Add(new SystemLog() { Id = Guid.NewGuid(), logDescription = logdesc, LogDatetime = DateTime.Now });

                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string logPath = _LogPath + GlobalFunctions._GenerateRequestID() + ".txt";

                string logtext = $"systemlog:{logdesc},error:{JsonConvert.SerializeObject(ex)}";

                using (StreamWriter outputFile = new StreamWriter(logPath, true))
                {
                    outputFile.WriteLine(logtext);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logdesc"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        public async Task InsertLogs(string logdesc, string processId)
        {
            try
            {
                _Context.SystemLogs.Add(new SystemLog() { Id = Guid.NewGuid(), logDescription = logdesc, ProcessId = processId, LogDatetime = DateTime.Now });

                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string logPath = _LogPath + GlobalFunctions._GenerateRequestID() + ".txt";

                string logtext = $"systemlog:{logdesc},error:{JsonConvert.SerializeObject(ex)}";

                using (StreamWriter outputFile = new StreamWriter(logPath, true))
                {
                    outputFile.WriteLine(logtext);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logdesc"></param>
        /// <param name="processId"></param>
        /// <param name="remoteip"></param>
        /// <param name="logLocation"></param>
        /// <param name="logtypeid"></param>
        /// <param name="token"></param>
        /// <param name="hosturl"></param>
        /// <returns></returns>
        public async Task InsertLogs(string logdesc, string processId, string remoteip, string logLocation, long logtypeid, string token="",string hosturl ="")
        {
            SystemLog log = new SystemLog()
            {
                Id = Guid.NewGuid(),
                LogDatetime = DateTime.Now,
                logDescription = logdesc,
                LogLocation = logLocation,
                ClientId = remoteip,
                LogTypeId = logtypeid,
                ProcessId = processId,
                Token = token
            };
            try
            {
                _Context.SystemLogs.Add(log);

                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                /// در صورت وقوع خطا و عدم دسترسی به دیتا بیس لاگ در فایل ذخیره می شود
                string logPath = _LogPath + GlobalFunctions._GenerateRequestID() + ".txt";

                string logtext = $"systemlog:{JsonConvert.SerializeObject(log)},error:{JsonConvert.SerializeObject(ex)}";

                using (StreamWriter outputFile = new StreamWriter(logPath, true))
                {
                    outputFile.WriteLine(logtext);
                }
            }
        }
    }
}
