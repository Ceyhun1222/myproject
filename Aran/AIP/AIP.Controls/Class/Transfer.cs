using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIP.BaseLib.Struct;
using WinSCP;

namespace AIP.BaseLib.Class
{
    public static class Transfer
    {
        public static TransferParams TransferParams;
        public static Queue<Output> OutputQueue = new Queue<Output>();

        private static SessionOptions sessionOptions;

        public static void Initialize()
        {
            try
            {
                sessionOptions = new SessionOptions
                {
                    Protocol = TransferParams.Protocol,
                    HostName = TransferParams.HostName,
                    UserName = TransferParams.UserName,
                    Password = TransferParams.Password,
                    SshHostKeyFingerprint = TransferParams.SshHostKeyFingerprint,
                    GiveUpSecurityAndAcceptAnySshHostKey = true // Todo: make false and check hostkey
                };
            }
            catch (Exception ex)
            {
                Report.Write($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex.GetBaseException()}");
                throw;
            }
        }

        public static bool IsConnected()
        {
            try
            {
                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Non Sync - just upload all files over old
        /// </summary>
        public static void Run()
        {
            try
            {
                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult =
                        session.PutFiles(TransferParams.LocalDirectory, TransferParams.RemoteDirectory, false, transferOptions);

                    // Throw on any error
                    transferResult.Check();

                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        SendOutput($@"Upload of {transfer.FileName} succeeded");
                    }
                }
            }
            catch (Exception ex)
            {
                Report.Write($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex.GetBaseException()}");
                throw;
            }
        }

        /// <summary>
        /// Sync upload
        /// </summary>
        public static void Sync()
        {
            try
            {
                using (Session session = new Session())
                {
                    // Will continuously report progress of synchronization
                    session.FileTransferred += FileTransferred;

                    // Connect
                    session.Open(sessionOptions);

                    if (!session.FileExists(TransferParams.RemoteDirectory))
                    {
                        session.CreateDirectory(TransferParams.RemoteDirectory);
                    }

                    // Synchronize files
                    SynchronizationResult synchronizationResult;
                    synchronizationResult =
                        session.SynchronizeDirectories(
                            SynchronizationMode.Remote, TransferParams.LocalDirectory, TransferParams.RemoteDirectory, false);

                    // Throw on any error
                    synchronizationResult.Check();

                    SendOutput($@"Upload to website successfully completed", Color.Green);
                }
            }
            catch (Exception ex)
            {
                SendOutput($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex.GetBaseException()}");
                Report.Write($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex.GetBaseException()}");
                throw;
            }
        }

        private static void FileTransferred(object sender, TransferEventArgs e)
        {
            try
            {
                if (e.Error == null)
                    SendOutput($@"Upload of {e.FileName} succeeded");
                else
                    SendOutput($@"Upload of {e.FileName} failed: {e.Error}");

                if (e.Chmod != null)
                {
                    if (e.Chmod.Error == null)
                        SendOutput($@"Permissions of {e.Chmod.FileName} set to {e.Chmod.FilePermissions}");
                    else
                        SendOutput($@"Setting permissions of {e.Chmod.FileName} failed: {e.Chmod.Error}");
                }
                else
                    SendOutput($@"Permissions of {e.Destination} kept with their defaults");

                if (e.Touch != null)
                {
                    if (e.Touch.Error == null)
                        SendOutput($@"Timestamp of {e.Touch.FileName} set to {e.Touch.LastWriteTime}");
                    else
                        SendOutput($@"Setting timestamp of {e.Touch.FileName} failed: {e.Touch.Error}");
                }
                else
                {
                    // This should never happen during "local to remote" synchronization
                    SendOutput($@"Timestamp of {e.Destination} kept with its default (current time)");
                }
            }
            catch (Exception ex)
            {
                Report.Write($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex.GetBaseException()}");
                throw;
            }
        }

        private static void SendOutput(string Message, Color? Color = null, int? Percent = null)
        {
            try
            {
                BaseLib.Struct.Output output = new BaseLib.Struct.Output();
                output.Message = Message;
                output.Color = Color ?? System.Drawing.Color.Black;
                output.Percent = Percent;
                OutputQueue.Enqueue(output);
            }
            catch (Exception ex)
            {
                Report.Write($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex.GetBaseException()}");
                throw;
            }
        }
    }
}
