//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Pipes;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace AranUpdWinService
//{
//    class NamePipeServer
//    {
//        private NamedPipeServerStream _pipeServer;
//        private AutoResetEvent _connectEvent;
//        private ManualResetEvent _cancelEvent;
//        private Exception _waitException;
//        private bool _canSendPipiMessage;
//        private byte[] _buffer;
//        private DataReadState _bufferReadState;
//        private int _waitBufferCount;

//        public NamePipeServer()
//        {
//            _cancelEvent = new ManualResetEvent(false);
//            _canSendPipiMessage = false;
//            _buffer = new byte[0];
//            _bufferReadState = DataReadState.Count;
//        }

//        public event SettingsSettedEventHandler SettingsSetted;

//        public void Start()
//        {
//            _pipeServer = new NamedPipeServerStream("aran_updater_pipe",
//                PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

//            new Thread(PipeServerThread).Start();
//        }

//        public void Stop()
//        {
//            _cancelEvent.Set();
//            _pipeServer.Dispose();
//        }

//        public void SendCommand(CommandType cmd, string message = null)
//        {
//            if (!_canSendPipiMessage)
//                return;

//            if (message == null)
//                message = string.Empty;

//            try
//            {
//                var msgData = Encoding.UTF8.GetBytes(message);
//                var buffer = new byte[msgData.Length + 8];
//                var bw = new BinaryWriter(_pipeServer);
//                bw.Write(msgData.Length + 8);
//                bw.Write((int)cmd);
//                bw.Write(msgData, 0, msgData.Length);
//                bw.Flush();
//            }
//            catch { }
//        }


//        private void PipeServerThread()
//        {
//            _connectEvent = new AutoResetEvent(false);
//            _pipeServer.BeginWaitForConnection(StartedWait, true);

//            if (WaitHandle.WaitAny(new WaitHandle[] { _connectEvent, _cancelEvent }) == 1)
//                _pipeServer.Close();

//            if (_waitException != null)
//                throw _waitException;


//            var myByteArray = new byte[1024];
//            _pipeServer.BeginRead(myByteArray, 0, myByteArray.Length, ReadAsyncCallback, myByteArray);
//        }

//        private void StartedWait(IAsyncResult ar)
//        {
//            try
//            {
//                _pipeServer.EndWaitForConnection(ar);
//                _canSendPipiMessage = true;
//            }
//            catch (Exception er)
//            {
//                _waitException = er;
//            }

//            _connectEvent.Set();
//        }

//        private void OnBytesRead(byte[] data, int len)
//        {
//            var bufLen = _buffer.Length;
//            Array.Resize<byte>(ref _buffer, bufLen + len);
//            Array.Copy(data, 0, _buffer, bufLen, len);

//            if (_bufferReadState == DataReadState.Count)
//            {
//                if (_buffer.Length >= 4)
//                {
//                    _waitBufferCount = BitConverter.ToInt32(_buffer, 0);
//                    _bufferReadState = DataReadState.Data;
//                }
//            }
            
//            if (_bufferReadState == DataReadState.Data)
//            {
//                if (_buffer.Length >= _waitBufferCount + 4)
//                {
//                    _bufferReadState = DataReadState.Count;
//                    var cmd = (CommandType)BitConverter.ToInt32(_buffer, 4);
//                    var message = string.Empty;

//                    if (_waitBufferCount > 8)
//                        message = Encoding.UTF8.GetString(_buffer, 8, _waitBufferCount);

//                    _buffer = new byte[0];
//                    _waitBufferCount = 0;

//                    OnDataRead(cmd, message);
//                }
//            }
//        }

//        private void OnDataRead(CommandType cmd, string message)
//        {
//            if (cmd == CommandType.SettingsSaved)
//            {
//                if (SettingsSetted != null)
//                {
//                    try
//                    {
//                        var sa = message.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
//                        var server = sa[0];
//                        var port = int.Parse(sa[1]);
//                        SettingsSetted(this, new SettingsSettedEventArgs(server, port));
//                    }
//                    catch { }
//                }
//            }
//        }

//        private void ReadAsyncCallback(IAsyncResult ar)
//        {
//            var bytes = ar.AsyncState as byte[];
//            int amountRead = 0;

//            try
//            {
//                amountRead = _pipeServer.EndRead(ar);
//                if (amountRead == 0)
//                {
//                    Stop();
//                    Start();
//                }
                
//                OnBytesRead(bytes.Clone() as byte[], amountRead);

//            }
//            catch (IOException)
//            {
//                return;
//            }

//            _pipeServer.BeginRead(bytes, 0, bytes.Length, ReadAsyncCallback, bytes);
//        }
//    }
//}
