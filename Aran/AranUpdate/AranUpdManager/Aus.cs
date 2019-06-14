using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AranUpdateManager
{
    class Aus
    {
        public string VersionName { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string ChangesRTF { get; set; }

        public byte[] BinFile { get; set; }

        public void Pack(Stream output)
        {
            if (VersionName == null)
                VersionName = string.Empty;
            if (ChangesRTF == null)
                ChangesRTF = string.Empty;
            if (BinFile == null)
                BinFile = new byte[0];

            using (var bw = new BinaryWriter(output))
            {
                bw.Write(VersionName);
                bw.Write(ReleaseDate.Ticks);
                bw.Write(ChangesRTF);
                bw.Write(BinFile.Length);
                bw.Write(BinFile);
            }
        }

        public void Unpack(Stream input)
        {
            using (var br = new BinaryReader(input))
            {
                VersionName = br.ReadString();
                ReleaseDate = new DateTime(br.ReadInt64());
                ChangesRTF = br.ReadString();
                var count = br.ReadInt32();
                BinFile = br.ReadBytes(count);
            }
        }
    }
}
