using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArenaStatic;
using NLog.Config;
using NLog.Targets;

namespace ChartServices.Logging
{
    public static class LogManager
    {
        private static readonly IDictionary<string, ILogger> Loggers = new Dictionary<string, ILogger>();

        private static ILogger GetLogger(Type type)
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

        private static bool Configured => NLog.LogManager.Configuration != null;

        public static void Configure(string logFile = "logs.log", string errorLogFile = "errors.log", LogLevel level = LogLevel.Info)
        {
            var appDir = ArenaStaticProc.GetMainFolder();
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

        private static void Configure(string appDir, string logFile = "logs.log", string errorLogFile = "errors.log", LogLevel level = LogLevel.Info)
        {
            appDir = Path.Combine(appDir, "Service logs");
            if (!Directory.Exists(appDir))
                Directory.CreateDirectory(appDir);


            LoggingConfiguration loggingConf = new LoggingConfiguration();
            
            var mainTarget = new FileTarget();
            loggingConf.AddTarget("file", mainTarget);
            mainTarget.FileName = Path.Combine(appDir, logFile);
            mainTarget.Layout = "${level:uppercase=true}|${machinename}|${windows-identity}|${longdate}|${logger}|${message}";
            mainTarget.ArchiveDateFormat = "yyyy-MM-dd-HH-mm";
            mainTarget.ArchiveFileName = Path.Combine(appDir + "\\archive", GetArchiveLogfileName(logFile));
            mainTarget.ArchiveEvery = FileArchivePeriod.Day;
            mainTarget.ArchiveNumbering = ArchiveNumberingMode.Date;
            mainTarget.MaxArchiveFiles = 8;
           
            var exceptionTarget = new FileTarget();
            loggingConf.AddTarget("file", exceptionTarget);
            exceptionTarget.FileName = Path.Combine(appDir, errorLogFile);
            exceptionTarget.Layout = "${level:uppercase=true}|${machinename}|${windows-identity}|${longdate}|${logger}|${message}|${exception:format=toString}";
            exceptionTarget.ArchiveDateFormat = "yyyy-MM-dd-HH-mm";
            exceptionTarget.ArchiveFileName = Path.Combine(appDir + "\\archive", GetArchiveLogfileName(errorLogFile));
            exceptionTarget.ArchiveEvery = FileArchivePeriod.Day;
            exceptionTarget.ArchiveNumbering = ArchiveNumberingMode.Date;
            exceptionTarget.MaxArchiveFiles = 8;

            var consoleTarget = new ColoredConsoleTarget();
            loggingConf.AddTarget("console", consoleTarget);
            consoleTarget.Layout = "${level:uppercase=true}|${date:format=HH\\:mm\\:ss}|${logger}|${message}";


            var mainRule = new LoggingRule("*", Logger.Convert(level), mainTarget);
            loggingConf.LoggingRules.Add(mainRule);

            var exceptionsRule = new LoggingRule("*", NLog.LogLevel.Warn, exceptionTarget);
            loggingConf.LoggingRules.Add(exceptionsRule);

            var consoleRule = new LoggingRule("*", Logger.Convert(level), consoleTarget);
            loggingConf.LoggingRules.Add(consoleRule);

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