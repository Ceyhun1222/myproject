using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Web;
using ChartManagerWeb.ChartServiceReference;
using ChartManagerWeb.Helper;

namespace ChartManagerWeb
{
    //public static class Global
    //{
    //    //private static string _key4Client;
    //    //private static readonly Dictionary<string, ChartManagerServiceClient> _clientDict;
    //    private static ChartManagerServiceClient _client;

    //    static Global()
    //    {
    //        //_clientDict = new Dictionary<string, ChartManagerServiceClient>();
    //        RegisterServiceClient();            
    //    }

    //    public static ChartManagerServiceClient ChartManagerServiceClient
    //    {
    //        get
    //        {
    //            //var client = _clientDict[_key4Client];
    //            if (_client.State == CommunicationState.Faulted)
    //            {
    //                _client.Abort();                    
    //                //_clientDict.Remove(_key4Client);
    //                RegisterServiceClient();
    //            }
    //            else if (_client.State == CommunicationState.Closed || _client.State == CommunicationState.Closing)
    //            {
    //                //_clientDict.Remove(_key4Client);
    //                RegisterServiceClient();
    //            }

    //            return _client; //_clientDict[_key4Client];
    //        }
    //    }

    //    private static ChartManagerServiceClient RegisterServiceClient()
    //    {
    //        var callback = new ChartServiceCallBackListener();
    //        var instanceContext = new InstanceContext(callback);
    //        //_key4Client = Guid.NewGuid().ToString();
    //        _client = new ChartManagerServiceClient(instanceContext, Global.BindingName);
    //        _client.Endpoint.Address = new EndpointAddress(_client.Endpoint.Address.Uri,
    //            EndpointIdentity.CreateDnsIdentity(Global.CertificateName));
    //        _client.ClientCredentials.UserName.UserName = "admin";
    //        _client.ClientCredentials.UserName.Password = Global.Sha256_hash("admin");

    //        //return client;
    //        //_clientDict.Add(_key4Client, client);
    //    }

    //    public static String Sha256_hash(String value)
    //    {
    //        StringBuilder Sb = new StringBuilder();

    //        using (SHA256 hash = SHA256.Create())
    //        {
    //            Encoding enc = Encoding.UTF8;
    //            Byte[] result = hash.ComputeHash(enc.GetBytes(value));

    //            foreach (Byte b in result)
    //                Sb.Append(b.ToString("x2"));
    //        }

    //        return Sb.ToString();
    //    }

    //    public static string BindingName => "netTcp_ChartService";

    //    public static string CertificateName => "ChartManagerTempCert";

    //    //public static ChartUser User { get; set; }
    //}
}