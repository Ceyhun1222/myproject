using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Consistency;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Extensions;
using Aran.Temporality.CommonUtil.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TossConsistency
{
    class Program
    {
        const string recalculateCommand = "recalculate";
        const string mongoRepositoryCommand = "mongo";
        const string noDeleteRepositoryCommand = "file";

        static void HelpMessage()
        {
            Console.WriteLine($"3 parameters are required:");
            Console.WriteLine($"1. server config name");
            Console.WriteLine($"2. repository type ({mongoRepositoryCommand}, {noDeleteRepositoryCommand})");
            Console.WriteLine($"3. storage name");
            Console.WriteLine($"4th parameter '{recalculateCommand}' is optional");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {

            if (args.Length < 3)
            {
                HelpMessage();
                return;
            }

            var configName = args[0];

            RepositoryType repositoryType;
            if (args[1].Equals(mongoRepositoryCommand, StringComparison.CurrentCultureIgnoreCase))
                repositoryType = RepositoryType.MongoRepository;
            else if (args[1].Equals(noDeleteRepositoryCommand, StringComparison.CurrentCultureIgnoreCase))
                repositoryType = RepositoryType.NoDeleteRepository;
            else
            {
                Console.WriteLine("Incorrect repository type");
                Console.WriteLine();
                HelpMessage();
                return;
            }

            var storageName = args[2];

            var date = DateTime.Now.ToString("yy.MM.dd_H.mm.ss");
            var rootDirectory = "C:\\TOSS\\logs";
            var consistencyReportsDirectory = "consistency";
            var directory = $"{rootDirectory}\\{consistencyReportsDirectory}";

            try
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }
            catch (Exception)
            {
                directory = ".";
            }

            var fileName = $"{directory}\\{date}_{configName}_{repositoryType}_{storageName}";

            try
            {
                ConnectionProvider.InitServerSettings(configName);

                var consistencyManager = new EventConsistencyManager(repositoryType, storageName)
                {
                    Logger = Console.WriteLine
                };

                Console.WriteLine($"Server config name: {configName}");
                Console.WriteLine($"Repository type: {repositoryType}");
                Console.WriteLine($"Storage name: {storageName}");
                Console.WriteLine();

                if (args.Length > 3 && args[3] == recalculateCommand)
                {
                    Console.WriteLine("Clear existing consistencies and recalculate them? (y/n)");
                    var res = Console.ReadLine();

                    if (res.Equals("y", StringComparison.CurrentCultureIgnoreCase) || res.Equals("yes", StringComparison.CurrentCultureIgnoreCase))
                        consistencyManager.ReCalculateConsistencies();

                    Console.WriteLine("Completed");
                }
                else
                {
                    var reports = consistencyManager.CheckConsistencies();

                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("Total:");
                    sb.AppendLine("Category,Type,Count");

                    foreach (var errorType in EventConsistencyReportModel.ErrorTypes)
                    {
                        sb.AppendLine($"Error,{errorType},{reports.Count(x => x.ErrorType == errorType)}");
                    }

                    foreach (var errorType in EventConsistencyReportModel.WarningTypes)
                    {
                        sb.AppendLine($"Warning,{errorType},{reports.Count(x => x.ErrorType == errorType)}");
                    }

                    sb.AppendLine();
                    sb.AppendLine(EventConsistencyReportModel.GetTitles());
                    
                    Console.WriteLine("Errors:");

                    foreach (EventConsistencyErrorType errorType in EventConsistencyReportModel.ErrorTypes)
                    {
                        var count = reports.Count(x => x.ErrorType == errorType);
                        
                        Console.WriteLine($"    {errorType}: {count}");

                        foreach (var report in reports.Where(x => x.ErrorType == errorType))
                        {
                            sb.AppendLine(report.ToString());
                        }
                    }

                    Console.WriteLine("Warnings:");

                    foreach (EventConsistencyErrorType errorType in EventConsistencyReportModel.WarningTypes)
                    {
                        var count = reports.Count(x => x.ErrorType == errorType);

                        Console.WriteLine($"    {errorType}: {count}");

                        foreach (var report in reports.Where(x => x.ErrorType == errorType))
                        {
                            sb.AppendLine(report.ToString());
                        }
                    }

                    File.AppendAllText(fileName + ".csv", sb.ToString());

                    Console.WriteLine("Completed");
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Console.Write(ex.ToString());
                    File.WriteAllText(fileName + "_error.txt", ex.ToString());
                }
                catch
                {
                    Console.Write(ex.ToString());
                }
            }
        }
    }
}
