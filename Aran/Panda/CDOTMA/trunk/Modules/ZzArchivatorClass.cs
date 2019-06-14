using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace ZzArchivator	//CDOTMA
{
	public static class ZzArchivatorClass
	{
		/// <summary>
		/// Создает архив archiveName , содержащий файлы fileNames
		/// </summary>
		/// <param name="archiver">файл архиватора вместе с полным путем</param>
		/// <param name="FileName">Файлы для запаковки (можно с маской *)</param>
		/// <param name="archiveName">Имя архива с полным путем</param>
		public static void AddFileToArchive(string archiver, string FileName, string archiveName)
		{
			try
			{
				// Предварительные проверки
				if (!File.Exists(archiver))
					throw new Exception("Архиватор 7z по пути \"" + archiver + "\" не найден");

				// Формируем параметры вызова 7z
				ProcessStartInfo startInfo = new ProcessStartInfo();
				startInfo.FileName = archiver;
				// добавить в архив с максимальным сжатием
				startInfo.Arguments = " a -mx9 ";
				// имя архива
				startInfo.Arguments += "\"" + archiveName + "\"";
				// файлы для запаковки
				startInfo.Arguments += " \"" + FileName + "\"";
				startInfo.WindowStyle = ProcessWindowStyle.Hidden;
				int sevenZipExitCode = 0;

				using (System.Diagnostics.Process sevenZip = System.Diagnostics.Process.Start(startInfo))
				{
					sevenZip.WaitForExit();
					sevenZipExitCode = sevenZip.ExitCode;
				}

				// Если с первого раза не получилось,
				// пробуем еще раз через 1 секунду
				if (sevenZipExitCode != 0 && sevenZipExitCode != 1)
				{
					using (System.Diagnostics.Process sevenZip = System.Diagnostics.Process.Start(startInfo))
					{
						Thread.Sleep(1000);
						sevenZip.WaitForExit();
						switch (sevenZip.ExitCode)
						{
							case 0: return; // Без ошибок и предупреждений
							case 1: return; // Есть некритичные предупреждения
							case 2: throw new Exception("Фатальная ошибка");
							case 7: throw new Exception("Ошибка в командной строке");
							case 8:
								throw new Exception("Недостаточно памяти для выполнения операции");
							case 225:
								throw new Exception("Пользователь отменил выполнение операции");
							default:
								throw new Exception("Архиватор 7z вернул недокументированный код ошибки: " + sevenZip.ExitCode.ToString());
						}
					}
				}
			}
			catch (Exception e)
			{
				throw new Exception("SevenZip.AddToArchive: " + e.Message);
			}
		}

		/// <summary>
		/// Создает архив archiveName , содержащий файлы fileNames
		/// </summary>
		/// <param name="archiver">файл архиватора вместе с полным путем</param>
		/// <param name="FolderName">Файлы для запаковки (можно с маской *)</param>
		/// <param name="archiveName">Имя архива с полным путем</param>
		public static void AddFolderToArchive(string archiver, string archiveName, string FolderName)
		{
			try
			{
				// Предварительные проверки
				if (!File.Exists(archiver))
					throw new Exception("Архиватор 7z по пути \"" + archiver + "\" не найден");

				// Формируем параметры вызова 7z
				ProcessStartInfo startInfo = new ProcessStartInfo();
				startInfo.FileName = archiver;
				// добавить в архив с максимальным сжатием
				startInfo.Arguments = " a -mx9 ";
				// имя архива
				startInfo.Arguments += "\"" + archiveName + "\"";
				// файлы для запаковки
				startInfo.Arguments += " \"" + FolderName + "\"";
				startInfo.WindowStyle = ProcessWindowStyle.Hidden;
				int sevenZipExitCode = 0;

				using (System.Diagnostics.Process sevenZip = System.Diagnostics.Process.Start(startInfo))
				{
					sevenZip.WaitForExit();
					sevenZipExitCode = sevenZip.ExitCode;
				}

				// Если с первого раза не получилось,
				// пробуем еще раз через 1 секунду
				if (sevenZipExitCode != 0 && sevenZipExitCode != 1)
				{
					using (System.Diagnostics.Process sevenZip = System.Diagnostics.Process.Start(startInfo))
					{
						Thread.Sleep(1000);
						sevenZip.WaitForExit();
						switch (sevenZip.ExitCode)
						{
							case 0: return; // Без ошибок и предупреждений
							case 1: return; // Есть некритичные предупреждения
							case 2: throw new Exception("Фатальная ошибка");
							case 7: throw new Exception("Ошибка в командной строке");
							case 8:
								throw new Exception("Недостаточно памяти для выполнения операции");
							case 225:
								throw new Exception("Пользователь отменил выполнение операции");
							default:
								throw new Exception("Архиватор 7z вернул недокументированный код ошибки: " + sevenZip.ExitCode.ToString());
						}
					}
				}
			}
			catch (Exception e)
			{
				throw new Exception("SevenZip.AddToArchive: " + e.Message);
			}
		}

		/// <summary>
		/// Распаковывает архив archiveName в каталог outputFolder
		/// </summary>
		/// <param name="archiver">файл архиватора вместе с полным путем</param>
		/// <param name="archiveName">Имя архива с полным путем</param>
		/// <param name="outputFolder">Каталог для распаковки</param>
		public static void ExtractFromArchive(string archiver, string archiveName, string outputFolder)
		{
			try
			{
				// Предварительные проверки
				if (!File.Exists(archiver))
					throw new Exception("Архиватор 7z по пути \"" + archiver + "\" не найден");

				if (!File.Exists(archiveName))
					throw new Exception("Файл архива \"" + archiveName + "\" не найден");

				if (!Directory.Exists(outputFolder))
					Directory.CreateDirectory(outputFolder);

				// Формируем параметры вызова 7z
				ProcessStartInfo startInfo = new ProcessStartInfo();
				startInfo.FileName = archiver;
				// Распаковать (для полных путей - x)
				startInfo.Arguments = " e";
				// На все отвечать yes
				startInfo.Arguments += " -y";
				// Файл, который нужно распаковать
				startInfo.Arguments += " " + "\"" + archiveName + "\"";
				// Папка распаковки
				startInfo.Arguments += " -o" + "\"" + outputFolder + "\"";
				startInfo.WindowStyle = ProcessWindowStyle.Hidden;
				int sevenZipExitCode = 0;

				using (System.Diagnostics.Process sevenZip = System.Diagnostics.Process.Start(startInfo))
				{
					sevenZip.WaitForExit();
					sevenZipExitCode = sevenZip.ExitCode;
				}

				// Если с первого раза не получилось,
				// пробуем еще раз через 1 секунду
				if (sevenZipExitCode != 0 && sevenZipExitCode != 1)
				{
					using (System.Diagnostics.Process sevenZip = System.Diagnostics.Process.Start(startInfo))
					{
						Thread.Sleep(1000);
						sevenZip.WaitForExit();
						switch (sevenZip.ExitCode)
						{
							case 0: return; // Без ошибок и предупреждений
							case 1: return; // Есть некритичные предупреждения
							case 2: throw new Exception("Фатальная ошибка");
							case 7: throw new Exception("Ошибка в командной строке");
							case 8:
								throw new Exception("Недостаточно памяти для выполнения операции");
							case 225:
								throw new Exception("Пользователь отменил выполнение операции");
							default: throw new Exception("Архиватор 7z вернул недокументированный код ошибки: " + sevenZip.ExitCode.ToString());
						}
					}
				}
			}
			catch (Exception e)
			{
				throw new Exception("SevenZip.ExtractFromArchive: " + e.Message);
			}
		}

	}
}
