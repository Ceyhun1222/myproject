using WinSCP;

namespace AIP.BaseLib.Class
{
    public class TransferParams
    {
        public Protocol Protocol { get; set; }
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SshHostKeyFingerprint { get; set; }
        public string LocalDirectory { get; set; }
        public string RemoteDirectory { get; set; }
    }
}
