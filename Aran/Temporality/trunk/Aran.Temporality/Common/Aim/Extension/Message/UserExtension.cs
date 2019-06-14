using System;

namespace Aran.Temporality.Common.Aim.Extension.Message
{
    [Serializable]
    public class UserExtension : MessageExtension
    {
        public string User { get; set; }
        public string Application { get; set; }
    }
}
