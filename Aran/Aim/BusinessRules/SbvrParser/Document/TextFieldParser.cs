using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.Parser
{
    public class TextFieldParser : IDisposable
    {
        private StreamReader _streamReader;

        public TextFieldParser(Stream stream)
        {
            _streamReader = new StreamReader(stream);
        }

        ~TextFieldParser()
        {
            Dispose();
        }

        public void Dispose()
        {
            _streamReader.BaseStream.Dispose();
        }

        public bool EndOfData
        {
            get
            {
                return _streamReader.EndOfStream;
            }
        }
    }
}
