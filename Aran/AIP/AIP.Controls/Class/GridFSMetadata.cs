using Aran.Temporality.CommonUtil.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace AIP.BaseLib.Class
{
    public class GridFSMetaData
    {
        public GridFSMetaData()
        {
            Version = 1;
        }
        //public ObjectId Id { get; set; }
        public string Username { get; set; }
        public int Version { get; set; }
    }
}
