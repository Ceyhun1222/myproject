using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.Parser.Document
{
    public class Lexer
    {
        private const int EOF = -1;
        private const int MAX_ID_LEN = 32;		// max identifier length

        private List<Token> _pcode;
        private int _pos;
        private int _line;
        private int _col;
        private int _ch;
        private string _buffer;
        private bool _buffered;
        private StreamReader _source;
        private ScanMode _mode;
        private Token _currToken;
        private Token _buffToken;
        private int _pc;				// current execution point in source code 
        private Dictionary<string, TokenId> _tokenTable;


        public Lexer()
        {
            _pcode = new List<Token>();

            _pos = 0;
            _col = 0;
            _line = 1;
            _ch = ' ';
            _buffer = "";
            _buffered = false;

            _tokenTable = new Dictionary<string, TokenId>();
        }


        public Token PeekToken()
        {
            if (_mode == ScanMode.Interpreting)
                return _pcode[_pc];

            if (!_buffered)
            {
                _buffToken = GetToken();
                _buffered = true;
            }

            return _buffToken;
        }

        public Token NextToken()
        {
            if (_mode == ScanMode.Interpreting)
            {
                if (_pc >= _pcode.Count)
                {
                    _currToken.Text = "";
                    _currToken.TokenId = TokenId.Eof;
                    _currToken.Line = _line;
                    _currToken.Column = _col;
                }
                else
                {
                    _currToken = _pcode[_pc++];
                }

                return _currToken;
            }

            if (_buffered)
            {
                _buffered = false;
                _currToken = _buffToken;
                return _currToken;
            }

            _currToken = GetToken();
            return _currToken;
        }

        public void Run(Stream source)
        {
            _source = new StreamReader(source);

            var token = NextToken();
        }


        private int GetChar()
        {
            if (_buffer.Length == 0)
            {
                _ch = _source.Read();
            }
            else
            {
                _ch = _buffer[0];
                _buffer = _buffer.Substring(1);
            }

            if (_ch == EOF)
                return _ch;

            _pos++;

            // start new line
            if (_ch == '\n')
            {
                _line++;
                _col = 0;
            }

            _col++;
            return _ch;
        }

        private int PeekChar()
        {
            if (_buffer.Length == 0)
                return _source.Peek();
            else
                return _buffer[0];
        }

        private bool IsDelimiter(int c)
        {
            return IsSpace(c) || "!:;,+-<>'/*%^=(){}&|\"".IndexOf((char)c) >= 0 || c == EOF;
        }

        private bool IsSpace(int c)
        {
            return c >= '\0' && c <= ' ';
        }

        private bool IsAlpha(int c)
        {
            return char.IsLetter((char)c) || c == '_';
        }

        private bool IsDigit(int c)
        {
            return char.IsDigit((char)c);
        }

        private int ScanCharacter(out bool isEscaped)
        {
            isEscaped = false;

            GetChar();

            if (_ch == '\\')
            {
                // It's an escape code
                isEscaped = true;
                GetChar();

                switch (_ch)
                {
                    case '\\':
                    case '\'':
                    case '"':
                        break;

                    case '0':
                        _ch = '\0';
                        break;

                    case 'n':
                        _ch = '\n';
                        break;

                    case 'r':
                        _ch = '\r';
                        break;

                    case 'b':
                        _ch = '\b';
                        break;

                    case 'a':
                        _ch = '\a';
                        break;

                    case 't':
                        _ch = '\t';
                        break;

                    case 'f':
                        _ch = '\f';
                        break;

                    case 'v':
                        _ch = '\v';
                        break;

                    default:
                        throw new SbvrException(
                            ErrorCode.InvalidEscapeSeq, 
                            new Token { Line = _line, Column = _col });
                }
            }

            return _ch;
        }

        private string ScanCharacterLiteral()
        {
            // The current character is the initiating '
            int ch = ScanCharacter(out bool isEscaped);
            return ((char)ch).ToString();
        }

        // Scan a string that starts with '"' and may contain escaped characters
        private string ScanStringLiteral()
        {
            // The current character is the initiating '"'

            var sb = new StringBuilder();

            int ch = ScanCharacter(out bool isEscaped);
            for (; ; )
            {
                //if (ch == EOF && !isEscaped)
                //	throw new InterpExc(ErrorMsg.QUOTE_EXPECTED);
                //"Unterminated String Literal"

                if ((ch == EOF || ch == '"') && !isEscaped)
                    break;

                sb.Append((char)ch);
                ch = ScanCharacter(out isEscaped);
            }

            return sb.ToString();
        }

        // Get a token.
        private Token GetToken()
        {
            // Skip over white space.
            while (IsSpace(_ch) && _ch > EOF)
                GetChar();

            //token.isBreakPt = false;
            //token.tokenId = TokenID.Undefined;

            var token = new Token
            {
                Text = "",
                Line = _line,
                Column = _col
            };

            // Check for end of program.
            if (_ch == EOF)
            {
                token.TokenId = TokenId.Eof;

                _pcode.Add(token);
                return token;
            }

            // Look for comments.
            if (_ch == '#')  //pragmas
            {
                GetChar();

                while (_ch != '\n' && _ch > EOF)  // find end of comment.
                    GetChar();

                return GetToken();
            }
            else if (_ch == '/')
                if (PeekChar() == '*')                // is a /* comment
                {
                    GetChar();
                    GetChar();

                    do                              // find end of comment
                    {
                        while (_ch != '*')
                            GetChar();
                        GetChar ();
                    } while (_ch != '/');
                    GetChar();

                    return GetToken();
                }
                else if (PeekChar() == '/')           // is a // comment
                {
                    GetChar();
                    GetChar();

                    while (_ch != '\n' && _ch > EOF)  // find end of comment.
                        GetChar();

                    return GetToken();
                }

            // Read a quoted char.
            if (_ch == '\'')
            {
                token.Text = ScanCharacterLiteral();
                GetChar();

                //Missing closing quote
                //"Unterminated Character Literal
                if (_ch != '\'')
                    throw new SbvrException(ErrorCode.QUOTE_EXPECTED, token);
                                                                            
                GetChar();

                token.TokenId = TokenId.CharLit;

                _pcode.Add(token);
                return token;
            }

            // Read a quoted string.
            if (_ch == '"')
            {
                token.Text = ScanStringLiteral();

                //Missing closing quote
                //"Parser_UnterminatedStringLiteral"
                if (_ch == '\n' || _ch == EOF)
                    throw new SbvrException(ErrorCode.QUOTE_EXPECTED, token);
                                                                            
                GetChar();

                token.TokenId = TokenId.StringLit;

                _pcode.Add(token);
                return token;
            }

            // Read a number.
            if (IsDigit(_ch))
            {
                while (!IsDelimiter(_ch))
                {
                    if (token.Text.Length < MAX_ID_LEN)
                        token.Text += (char)_ch;
                    GetChar();
                }

                token.TokenId = TokenId.NumberLit;

                _pcode.Add(token);
                return token;
            }

            // Read identifier or keyword.
            if (IsAlpha(_ch))
            {
                while (!IsDelimiter(_ch))
                {
                    if (token.Text.Length < MAX_ID_LEN)
                        token.Text += (char)_ch;
                    GetChar();
                }
                token.TokenId = TokenId.Ident;
            }

            // Determine if token is a keyword or identifier.
            if (token.TokenId == TokenId.Ident)
            {
                // Convert to lowercase.
                token.Text = token.Text.ToLower();
                var keyword = GetTokenId(token.Text);

                if (keyword > TokenId.Undefined)
                    token.TokenId = keyword;                // is a keyword

                _pcode.Add(token);
                return token;
            }

            throw new SbvrException(ErrorCode.SYNTAX, _currToken);
        }

        private TokenId GetTokenId(string text)
        {
            if (_tokenTable.Count == 0)
                InitTokenTable();

            return TokenId.Eof;
        }

        private void InitTokenTable()
        {
            var fields = typeof(TokenId).GetFields();
            foreach(var field in fields)
            {
                if (field.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is DescriptionAttribute desc)
                    _tokenTable[desc.Description] = (TokenId)field.GetValue(null);
            }
        }
    }

    public enum ScanMode
    {
        Scanning,
        Interpreting
    }

}
