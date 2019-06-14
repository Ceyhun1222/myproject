using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace AerodromeServices.Helpers
{
    public static class HelperMethods
    {
        public static String Sha256_hash(String value)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        public static string GetLocalIpAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            var t = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            return t.ToString();
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        //public static string[] GetAllLocalIPv4(NetworkInterfaceType _type)
        //{
        //    List<string> ipAddrList = new List<string>();
        //    foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        //    {
        //        if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
        //        {
        //            foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
        //            {
        //                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
        //                {
        //                    ipAddrList.Add(ip.Address.ToString());
        //                }
        //            }
        //        }
        //    }
        //    return ipAddrList.ToArray();
        //}

        public static string Beautify(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = Environment.NewLine,
                NewLineHandling = NewLineHandling.Replace,
                Encoding = new UTF8Encoding(false)
            };

            using (var ms = new MemoryStream())
            using (var writer = XmlWriter.Create(ms, settings))
            {
                doc.Save(writer);
                var xmlString = Encoding.UTF8.GetString(ms.ToArray());
                return xmlString;
            }
        }

    }

}