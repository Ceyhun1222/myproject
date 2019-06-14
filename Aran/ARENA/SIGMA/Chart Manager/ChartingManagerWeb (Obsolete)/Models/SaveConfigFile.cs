using ChartingManagerWeb.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChartingManagerWeb.Models
{
    public class SaveConfigFile
    {
        public byte[] ConfigSaveAs(long ID, ChartManagerServiceClient client)
        {
            byte[] fileBytes = client.GetConfigFile(ID);
            return fileBytes;
        }
    }
}