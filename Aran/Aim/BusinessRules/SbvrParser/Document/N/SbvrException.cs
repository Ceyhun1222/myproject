using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.Parser.Document
{
    public class SbvrException : Exception
    {
        public SbvrException(ErrorCode errorCode, Token token)
        {
            ErrorCode = errorCode;
            Token = token;
        }

        public Token Token { get; private set; }

        public ErrorCode ErrorCode { get; private set; }
    }

    public enum ErrorCode
    {
        InvalidEscapeSeq,
        QUOTE_EXPECTED,
        SYNTAX
    }
}
