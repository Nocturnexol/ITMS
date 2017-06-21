using System;
using System.IO;

namespace Bitshare.DataDecision.Common
{
    /// <summary>
    /// 日志写入类
    /// </summary>
    public class LogManager
    {
        private static object lock_info = new object();
        private static object lock_error = new object();
        private static object lock_log = new object();
        private static object lock_logName = new object();
        private static object lock_hour = new object();

        /// <summary>
        /// 写入日志(每天一个文件)
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="folder">日志文件所在文件件,默认“Log”</param>
        public static void Log(string content, string folder = "Log")
        {
            lock (lock_log)
            {
                string fileDir = string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'), folder);
                // 创建目录
                CreateDirectory(fileDir);
                string filePath = string.Format("{0}\\{1:yyyy-MM-dd}.log", fileDir, DateTime.Now);
                try
                {
                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "]消息：" + content);
                        sw.Close();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 写入日志(每天一个文件)
        /// 文件名带后缀
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="postfix">文件名后缀</param>
        /// <param name="folder">日志文件所在文件件,默认“Log”</param>
        public static void LogWithPostfix(string content, string postfix, string folder = "Log")
        {
            lock (lock_logName)
            {
                string fileDir = string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'), folder);
                // 创建目录
                CreateDirectory(fileDir);
                string filePath = string.Format("{0}\\{1:yyyy-MM-dd}_{2}.log", fileDir, DateTime.Now, postfix);
                try
                {
                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "]消息：" + content);
                        sw.Close();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 写入日志(每小时一个文件)
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="folder">日志文件所在文件件,默认“Log”</param>
        public static void LogHour(string content, string folder = "Log")
        {
            lock (lock_hour)
            {
                string fileDir = string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'), folder);
                // 创建目录
                CreateDirectory(fileDir);
                string filePath = string.Format("{0}\\{1:yyyyMMddHH}.log", fileDir, DateTime.Now);
                try
                {
                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "]消息：" + content);
                        sw.Close();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 写入信息(每天一个文件)
        /// </summary>
        /// <param name="content">内容</param>
        public static void Info(string content)
        {
            lock (lock_info)
            {
                string fileDir = string.Format("{0}\\MessageLog", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'));
                // 创建目录
                CreateDirectory(fileDir);
                string filePath = string.Format("{0}\\{1:yyyy-MM-dd}.log", fileDir, DateTime.Now);
                try
                {
                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "]消息：" + content);
                        sw.Close();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 写入Exception信息(每天一个文件)
        /// </summary>
        /// <param name="content">错误文本</param>
        /// <param name="ex">异常信息</param>
        public static void Error(string content, Exception ex)
        {
            lock (lock_error)
            {
                string fileDir = string.Format("{0}\\ErrorLog", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'));
                // 创建目录
                CreateDirectory(fileDir);
                string filePath = string.Format("{0}\\{1:yyyy-MM-dd}.log", fileDir, DateTime.Now);
                try
                {
                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine(string.Format("[{0:HH:mm:ss}]{1}", DateTime.Now, ex.ToString()));
                        sw.Close();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dir"></param>
        private static void CreateDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}
