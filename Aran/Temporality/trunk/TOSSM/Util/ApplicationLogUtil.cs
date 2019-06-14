using System;
using System.Collections;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;

namespace TOSSM.Util
{

    public enum ApplicationExitCode
    {
        Success = 0,
        Failure = 1,
        CantWriteToApplicationLog = 2,
        CantPersistApplicationState = 3
    }

    public class ApplicationLogUtil : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            try
            {
                // Write entry to application log 
                WriteApplicationLogEntry(
                    e.ApplicationExitCode == (int)ApplicationExitCode.Success ? "Failure" : "Success",
                    e.ApplicationExitCode);
            }
            catch
            {
                // Update exit code to reflect failure to write to application log
                e.ApplicationExitCode = (int)ApplicationExitCode.CantWriteToApplicationLog;
            }

            // Persist application state 
            try
            {
                PersistApplicationState();
            }
            catch
            {
                // Update exit code to reflect failure to persist application state
                e.ApplicationExitCode = (int)ApplicationExitCode.CantPersistApplicationState;
            }
        }

        static void WriteApplicationLogEntry(string message, int exitCode)
        {
            // Write log entry to file in isolated storage for the user
            var store = IsolatedStorageFile.GetUserStoreForAssembly();
            using (Stream stream = new IsolatedStorageFileStream("log.txt", FileMode.Append, FileAccess.Write, store))
            using (var writer = new StreamWriter(stream))
            {
                string entry = string.Format("{0}: {1} - {2}", message, exitCode, DateTime.Now);
                writer.WriteLine(entry);
            }
        }

        void PersistApplicationState()
        {
            // Persist application state to file in isolated storage for the user
            var store = IsolatedStorageFile.GetUserStoreForAssembly();
            using (Stream stream = new IsolatedStorageFileStream("state.txt", FileMode.Create, store))
            using (var writer = new StreamWriter(stream))
            {
                foreach (DictionaryEntry entry in Properties)
                {
                    writer.WriteLine(entry.Value);
                }
            }
        }
    
    }
}
