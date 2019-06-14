using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AranUpdAgent
{
    class Client
    {
        TcpClient _client;
        byte[] _buffer;
        DataReadState _bufferReadState;
        int _waitBufferCount;

        public Client()
        {
            _client = new TcpClient();

            _buffer = new byte[0];
            _bufferReadState = DataReadState.Count;
        }

        public event DataReadEventHandler DataRead;
        public event EventHandler Disconnected;

        public bool Start()
        {
            try
            {
                _client.Connect("127.0.0.1", 3322);
            }
            catch { return false; }

            var buffer = new byte[1024];
            var stream = _client.GetStream();
            stream.BeginRead(buffer, 0, buffer.Length, ReadClientData, buffer);

            return true;
        }

        public void Stop()
        {
            _client.Close();
        }

        public void SendCommand(CommandType cmd, string message)
        {
            if (!_client.Connected)
                return;

            var stream = _client.GetStream();

            var msgData = Encoding.UTF8.GetBytes(message);
            var buffer = new byte[msgData.Length + 8];
            var bw = new BinaryWriter(stream);
            bw.Write(msgData.Length + 8);
            bw.Write((int)cmd);
            bw.Write(msgData, 0, msgData.Length);
            bw.Flush();
        }


        private void ReadClientData(IAsyncResult ar)
        {
            var data = ar.AsyncState as byte[];
            int amountRead = 0;

            try
            {
                var stream = _client.GetStream();
                amountRead = stream.EndRead(ar);

                if (amountRead == 0)
                {
                    stream.Close();
                    if (Disconnected != null)
                        Disconnected(this, new EventArgs());
                    return;
                }

                OnBytesRead(data, amountRead);

                stream.BeginRead(data, 0, data.Length, ReadClientData, data);
            }
            catch (IOException) { }
        }

        private void OnBytesRead(byte[] data, int len)
        {
            var bufLen = _buffer.Length;
            Array.Resize<byte>(ref _buffer, bufLen + len);
            Array.Copy(data, 0, _buffer, bufLen, len);

            if (_bufferReadState == DataReadState.Count)
            {
                if (_buffer.Length >= 4)
                {
                    _waitBufferCount = BitConverter.ToInt32(_buffer, 0);
                    _bufferReadState = DataReadState.Data;
                }
            }

            if (_bufferReadState == DataReadState.Data)
            {
                if (_buffer.Length >= _waitBufferCount)
                {
                    _bufferReadState = DataReadState.Count;
                    var cmd = (CommandType)BitConverter.ToInt32(_buffer, 4);
                    var message = string.Empty;

                    if (_waitBufferCount > 8)
                        message = Encoding.UTF8.GetString(_buffer, 8, _waitBufferCount - 8);

                    _buffer = new byte[0];
                    _waitBufferCount = 0;

                    OnDataRead(cmd, message);
                }
            }
        }

        private void OnDataRead(CommandType cmd, string message)
        {
            if (DataRead != null)
                DataRead(this, new DataReaderEventArgs(cmd, message));
        }
    }

    enum DataReadState { Count, Data }

    enum CommandType { None, InitInfo, SettingsNotDefined, Offline, Online, Error, SettingsSaved }


    delegate void DataReadEventHandler(object sender, DataReaderEventArgs e);

    class DataReaderEventArgs : EventArgs
    {
        public DataReaderEventArgs(CommandType cmd, string message)
        {
            CmdType = cmd;
            Message = message;
        }

        public CommandType CmdType { get; private set; }

        public string Message { get; private set; }
    }
}
