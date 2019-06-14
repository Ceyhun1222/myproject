using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AranUpdWinService
{
    class AgentCommServer
    {
        private TcpListener _listener;
        private Dictionary<TcpClient, ClientData> _clientDataDict;
        private string _initInfoCommand;

        public AgentCommServer()
        {
            _clientDataDict = new Dictionary<TcpClient, ClientData>();

            var ipAddressArr = Dns.GetHostByAddress("127.0.0.1").AddressList;
            
            if (ipAddressArr.Length == 0){
                Console.WriteLine("Error: {0}", ipAddressArr.Length);
                return;
            }

            _listener = new TcpListener(ipAddressArr[0], 3322);
        }

        public event SettingsSettedEventHandler SettingsSetted;

        public void Start()
        {
            new Thread(new ThreadStart(ListenForClients)).Start();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        public void SendCommand(CommandType cmd, string message = null)
        {
            if (message == null)
                message = string.Empty;

            var msgData = Encoding.UTF8.GetBytes(message);

            foreach (var client in _clientDataDict.Keys)
            {
                if (!client.Connected)
                    continue;

                SendCommand(client, cmd, msgData);
            }
        }

        public void SetInitInfoCommand(string server, int port, string userName)
        {
            _initInfoCommand = string.Format("{0};{1};{2}", server, port, userName);
        }


        private void SendCommand(TcpClient client, CommandType cmd, byte[] data)
        {
            var bw = new BinaryWriter(client.GetStream());
            bw.Write(data.Length + 8);
            bw.Write((int)cmd);
            bw.Write(data, 0, data.Length);
            bw.Flush();
        }

        private void ListenForClients()
        {
            _listener.Start();

            while (true)
            {
                try
                {
                    var client = _listener.AcceptTcpClient();
                    _clientDataDict.Add(client, new ClientData());

                    SendCommand(client, CommandType.InitInfo, Encoding.UTF8.GetBytes(_initInfoCommand ?? string.Empty));

                    new Thread(new ParameterizedThreadStart(HandleClientComm)).Start(client);
                }
                catch
                {
                    return;
                }
            }
        }

        private void HandleClientComm(object client)
        {
            var tcpClient = client as TcpClient;
            ClientData clientData;

            lock(_clientDataDict)
            {
                if (!_clientDataDict.TryGetValue(tcpClient, out clientData))
                    return;
            }

            var clientStream = tcpClient.GetStream();

            var data = new byte[1024];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;
                var buffer = clientData.Buffer;

                try
                {
                    var prevLen = buffer.Length;
                    bytesRead = clientStream.Read(data, 0, data.Length);
                    Array.Resize(ref buffer, buffer.Length + bytesRead);
                    Array.Copy(data, 0, buffer, prevLen, bytesRead);
                    clientData.Buffer = buffer;
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0)
                {
                    _clientDataDict.Remove(tcpClient);
                    break;
                }

                if (clientData.ReadState == DataReadState.Count)
                {
                    if (buffer.Length >= 4)
                    {
                        clientData.WaitBufferCount = BitConverter.ToInt32(buffer, 0);
                        clientData.ReadState = DataReadState.Data;
                    }
                }

                if (clientData.ReadState == DataReadState.Data)
                {
                    if (buffer.Length >= clientData.WaitBufferCount )
                    {
                        clientData.ReadState = DataReadState.Count;
                        var cmd = (CommandType)BitConverter.ToInt32(buffer, 4);
                        var message = string.Empty;

                        if (clientData.WaitBufferCount > 8)
                            message = Encoding.UTF8.GetString(buffer, 8, clientData.WaitBufferCount - 8);

                        clientData.Clear();

                        OnDataRead(cmd, message);
                    }
                }
            }
        }

        private void OnDataRead(CommandType cmd, string message)
        {
            if (cmd == CommandType.SettingsSaved)
            {
                if (SettingsSetted != null)
                {
                    try
                    {
                        var sa = message.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        var server = sa[0];
                        var port = int.Parse(sa[1]);
                        SettingsSetted(this, new SettingsSettedEventArgs(server, port));
                    }
                    catch { }
                }
            }
        }
    }

    delegate void SettingsSettedEventHandler(object sender, SettingsSettedEventArgs e);

    class SettingsSettedEventArgs : EventArgs
    {
        public SettingsSettedEventArgs(string server, int port)
        {
            Server = server;
            Port = port;
        }

        public string Server { get; private set; }

        public int Port { get; private set; }
    }

    enum DataReadState { Count, Data }

    enum CommandType { None, InitInfo, SettingsNotDefined, Offline, Online, Error, SettingsSaved }

    class ClientData
    {
        public ClientData()
        {
            Buffer = new byte[0];
            ReadState = DataReadState.Count;
            WaitBufferCount = 0;
        }

        public byte[] Buffer { get; set; }

        public DataReadState ReadState { get; set; }

        public int WaitBufferCount { get; set; }

        public void Clear()
        {
            Buffer = new byte[0];
            WaitBufferCount = 0;
        }
    }
}
