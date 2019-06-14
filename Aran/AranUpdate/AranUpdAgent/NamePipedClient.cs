//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Pipes;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace AranUpdAgent
//{
//    class NamePipedClient
//    {
//        private byte[] _buffer;
//        private DataReadState _bufferReadState;
//        private int _waitBufferCount;
//        private NamedPipeClientStream _pipeClientStream;

//        public NamePipedClient()
//        {
//            _buffer = new byte[0];
//            _bufferReadState = DataReadState.Count;
//            _waitBufferCount = 0;
//        }

//        public void Start()
//        {
//            _pipeClientStream = new NamedPipeClientStream(".", "aran_updater_pipe", PipeDirection.InOut);
//            _pipeClientStream.Connect(10);

//            var myByteArray = new Byte[1024];
//            _pipeClientStream.BeginRead(myByteArray, 0, myByteArray.Length, ReadAsyncCallback, myByteArray);
//        }

//        public void Stop()
//        {
//            _pipeClientStream.Close();
//        }

//        public void SendCommand(CommandType cmd, string message = null)
//        {
//            if (_pipeClientStream == null || !_pipeClientStream.IsConnected)
//                return;

//            if (message == null)
//                message = string.Empty;

//            try
//            {
//                var msgData = Encoding.UTF8.GetBytes(message);
//                var buffer = new byte[msgData.Length + 8];
//                var bw = new BinaryWriter(_pipeClientStream);
//                bw.Write(msgData.Length + 8);
//                bw.Write((int)cmd);
//                bw.Write(msgData, 0, msgData.Length);
//                bw.Flush();
//            }
//            catch { }
//        }

//        public event DataReadEventHandler DataRead;



//        private void ReadAsyncCallback(IAsyncResult ar)
//        {
//            var data = ar.AsyncState as byte[];
//            int amountRead = 0;

//            try
//            {
//                amountRead = _pipeClientStream.EndRead(ar);

//                if (amountRead == 0)
//                {
//                    _pipeClientStream.Close();
//                    return;
//                }

//                OnBytesRead(data, amountRead);

//                _pipeClientStream.BeginRead(data, 0, data.Length, ReadAsyncCallback, data);
//            }
//            catch (IOException) { }
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
//            if (DataRead != null)
//                DataRead(this, new DataReaderEventArgs(cmd, message));
//        }
//    }
//}
