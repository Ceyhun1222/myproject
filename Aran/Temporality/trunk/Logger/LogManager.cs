using Aran.Temporality.Internal.Logging;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Aran.Temporality.Common.Logging
{
    public class LogManager
    {
        private static readonly IDictionary<string, ILogger> Loggers = new Dictionary<string, ILogger>();

        public static ILogger GetLogger(Type type)
        {
            return GetLogger(type.Name);
        }

        public static ILogger GetLogger(object obj)
        {
            return GetLogger(obj.GetType());
        }

        public static ILogger GetLogger(string name)
        {
            if (!Configured)
                return new Logger(name, NLog.LogManager.CreateNullLogger());

            if (!Loggers.ContainsKey(name))
                Loggers.Add(name, new Logger(name));
            return Loggers[name];
        }

        //public static bool Configured => NLog.LogManager.Configuration != null;

        public static bool Configured => NLog.LogManager.Configuration != null;

        public static void Configure(string logFile = "logs.log", string errorLogFile = "errors.log", LogLevel level = LogLevel.Info)
        {
            var appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RISK");
            Configure(appDir, logFile, errorLogFile, level);
        }

        public static LogLevel GetLogLevel()
        {
            if(!Configured)
                return LogLevel.Fatal;

            return NLog.LogManager.Configuration.LoggingRules
                .Where(rule => rule.Levels.Contains(NLog.LogLevel.Debug) 
                || rule.Levels.Contains(NLog.LogLevel.Info) 
                || rule.Levels.Contains(NLog.LogLevel.Trace))
                .Select(rule => Logger.Convert(rule.Levels.FirstOrDefault())).FirstOrDefault();
        }

        public static bool SetLogLevel(LogLevel level)
        {
            if (!Configured)
                return false;
            if (level == LogLevel.Error || level == LogLevel.Fatal || level == LogLevel.Warn)
                return false;

            foreach (var rule in NLog.LogManager.Configuration.LoggingRules.Where(rule => rule.Levels.Contains(NLog.LogLevel.Debug) ||
                                                                                          rule.Levels.Contains(NLog.LogLevel.Info) || rule.Levels.Contains(NLog.LogLevel.Trace)))
            {
                if (level == LogLevel.Trace)
                {
                    rule.EnableLoggingForLevel(NLog.LogLevel.Debug);
                    rule.EnableLoggingForLevel(NLog.LogLevel.Trace);
                }
                if (level == LogLevel.Debug)
                {
                    rule.DisableLoggingForLevel(NLog.LogLevel.Trace);
                    rule.EnableLoggingForLevel(NLog.LogLevel.Debug);
                }

                if (level == LogLevel.Info)
                {
                    rule.DisableLoggingForLevel(NLog.LogLevel.Trace);
                    rule.DisableLoggingForLevel(NLog.LogLevel.Debug);
                }

                NLog.LogManager.ReconfigExistingLoggers();
                return true;
            }
            return false;
        }

        public static void Configure(string appDir, string logFile = "logs.log", string errorLogFile = "errors.log", LogLevel level = LogLevel.Info)
        {
            appDir = Path.Combine(appDir, "logs");
            if (!Directory.Exists(appDir))
                Directory.CreateDirectory(appDir);


            LoggingConfiguration loggingConf = new LoggingConfiguration();

            var mainTarget = new FileTarget();
            loggingConf.AddTarget("file", mainTarget);
            mainTarget.FileName = Path.Combine(appDir, logFile);
            mainTarget.Layout = "${machinename}|${windows-identity}|${assembly-version}|${logger}|${longdate}|${level:uppercase=true}|${message}";
            mainTarget.ArchiveDateFormat = "yyyy-MM-dd-HH-mm";
            mainTarget.ArchiveFileName = Path.Combine(appDir + "\\archive", GetArchiveLogfileName(logFile));
            mainTarget.ArchiveEvery = FileArchivePeriod.Day;
            mainTarget.ArchiveNumbering = ArchiveNumberingMode.Date;
            mainTarget.MaxArchiveFiles = 8;


            var exceptionTarget = new FileTarget();
            loggingConf.AddTarget("file", exceptionTarget);
            exceptionTarget.FileName = Path.Combine(appDir, errorLogFile);
            exceptionTarget.Layout = "${machinename}|${windows-identity}|${assembly-version}|${logger}|${longdate}|${level:uppercase=true}|${message}${exception:format=toString}";
            exceptionTarget.ArchiveDateFormat = "yyyy-MM-dd-HH-mm";
            exceptionTarget.ArchiveFileName = Path.Combine(appDir + "\\archive", GetArchiveLogfileName(errorLogFile));
            exceptionTarget.ArchiveEvery = FileArchivePeriod.Day;
            exceptionTarget.ArchiveNumbering = ArchiveNumberingMode.Date;
            exceptionTarget.MaxArchiveFiles = 8;


            var mainRule = new LoggingRule("*", Logger.Convert(level), mainTarget);
            loggingConf.LoggingRules.Add(mainRule);

            var exceptionsRule = new LoggingRule("*", NLog.LogLevel.Warn, exceptionTarget);
            loggingConf.LoggingRules.Add(exceptionsRule);

            NLog.LogManager.Configuration = loggingConf;
        }

        private static string GetArchiveLogfileName(string logFile)
        {
            var tmp = logFile.Split('.');
            var archLogFile = tmp[0] + ".{#}." + tmp[1];
            return archLogFile;
        }

        public static void Flush()
        {
            NLog.LogManager.Flush();
        }

    }
}
