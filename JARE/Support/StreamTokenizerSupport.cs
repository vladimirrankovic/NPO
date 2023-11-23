using System;
using System.Collections.Generic;
using System.Text;

namespace JARE.support
{
    /*******************************/

    /// <summary>
    /// The StreamTokenizerSupport class takes an input stream and parses it into "tokens".
    /// The stream tokenizer can recognize identifiers, numbers, quoted strings, and various comment styles. 
    /// </summary>
    public class StreamTokenizerSupport
    {

        /// <summary>
        /// Internal constants and fields
        /// </summary>

        private const System.String TOKEN = "Token[";
        private const System.String NOTHING = "NOTHING";
        private const System.String NUMBER = "number=";
        private const System.String EOF = "EOF";
        private const System.String EOL = "EOL";
        private const System.String QUOTED = "quoted string=";
        private const System.String LINE = "], Line ";
        private const System.String DASH = "-.";
        private const System.String DOT = ".";

        private const int TT_NOTHING = -4;

        private const sbyte ORDINARYCHAR = 0x00;
        private const sbyte WORDCHAR = 0x01;
        private const sbyte WHITESPACECHAR = 0x02;
        private const sbyte COMMENTCHAR = 0x04;
        private const sbyte QUOTECHAR = 0x08;
        private const sbyte NUMBERCHAR = 0x10;

        private const int STATE_NEUTRAL = 0;
        private const int STATE_WORD = 1;
        private const int STATE_NUMBER1 = 2;
        private const int STATE_NUMBER2 = 3;
        private const int STATE_NUMBER3 = 4;
        private const int STATE_NUMBER4 = 5;
        private const int STATE_STRING = 6;
        private const int STATE_LINECOMMENT = 7;
        private const int STATE_DONE_ON_EOL = 8;

        private const int STATE_PROCEED_ON_EOL = 9;
        private const int STATE_POSSIBLEC_COMMENT = 10;
        private const int STATE_POSSIBLEC_COMMENT_END = 11;
        private const int STATE_C_COMMENT = 12;
        private const int STATE_STRING_ESCAPE_SEQ = 13;
        private const int STATE_STRING_ESCAPE_SEQ_OCTAL = 14;

        private const int STATE_DONE = 100;

        private sbyte[] attribute = new sbyte[256];
        private bool eolIsSignificant = false;
        private bool slashStarComments = false;
        private bool slashSlashComments = false;
        private bool lowerCaseMode = false;
        private bool pushedback = false;
        private int lineno = 1;

        private BackReader inReader;
        private BackStringReader inStringReader;
        private BackInputStream inStream;
        private System.Text.StringBuilder buf;


        /// <summary>
        /// Indicates that the end of the stream has been read.
        /// </summary>
        public const int TT_EOF = -1;

        /// <summary>
        /// Indicates that the end of the line has been read.
        /// </summary>
        public const int TT_EOL = '\n';

        /// <summary>
        /// Indicates that a number token has been read.
        /// </summary>
        public const int TT_NUMBER = -2;

        /// <summary>
        /// Indicates that a word token has been read.
        /// </summary>
        public const int TT_WORD = -3;

        /// <summary>
        /// If the current token is a number, this field contains the value of that number.
        /// </summary>
        public double nval;

        /// <summary>
        /// If the current token is a word token, this field contains a string giving the characters of the word 
        /// token.
        /// </summary>
        public System.String sval;

        /// <summary>
        /// After a call to the nextToken method, this field contains the type of the token just read.
        /// </summary>
        public int ttype;


        /// <summary>
        /// Internal methods
        /// </summary>

        private int read()
        {
            if (this.inReader != null)
                return this.inReader.Read();
            else if (this.inStream != null)
                return this.inStream.Read();
            else
                return this.inStringReader.Read();
        }

        private void unread(int ch)
        {
            if (this.inReader != null)
                this.inReader.UnRead(ch);
            else if (this.inStream != null)
                this.inStream.UnRead(ch);
            else
                this.inStringReader.UnRead(ch);
        }

        private void init()
        {
            this.buf = new System.Text.StringBuilder();
            this.ttype = StreamTokenizerSupport.TT_NOTHING;

            this.WordChars('A', 'Z');
            this.WordChars('a', 'z');
            this.WordChars(160, 255);
            this.WhitespaceChars(0x00, 0x20);
            this.CommentChar('/');
            this.QuoteChar('\'');
            this.QuoteChar('\"');
            this.ParseNumbers();
        }

        private void setAttributes(int low, int hi, sbyte attrib)
        {
            int l = System.Math.Max(0, low);
            int h = System.Math.Min(255, hi);
            for (int i = l; i <= h; i++)
                this.attribute[i] = attrib;
        }

        private bool isWordChar(int data)
        {
            char ch = (char)data;
            return (data != -1 && (ch > 255 || this.attribute[ch] == StreamTokenizerSupport.WORDCHAR || this.attribute[ch] == StreamTokenizerSupport.NUMBERCHAR));
        }

        /// <summary>
        /// Creates a StreamToknizerSupport that parses the given string.
        /// </summary>
        /// <param name="reader">The System.IO.StringReader that contains the String to be parsed.</param>
        public StreamTokenizerSupport(System.IO.StringReader reader)
        {
            string s = "";
            for (int i = reader.Read(); i != -1; i = reader.Read())
            {
                s += (char)i;
            }
            reader.Close();
            this.inStringReader = new BackStringReader(s);
            this.init();
        }

        /// <summary>
        /// Creates a StreamTokenizerSupport that parses the given stream.
        /// </summary>
        /// <param name="reader">Reader to be parsed.</param>
        public StreamTokenizerSupport(System.IO.StreamReader reader)
        {
            this.inReader = new BackReader(new System.IO.StreamReader(reader.BaseStream, reader.CurrentEncoding).BaseStream, 2, reader.CurrentEncoding);
            this.init();
        }

        /// <summary>
        /// Creates a StreamTokenizerSupport that parses the given stream.
        /// </summary>
        /// <param name="stream">Stream to be parsed.</param>
        public StreamTokenizerSupport(System.IO.Stream stream)
        {
            this.inStream = new BackInputStream(new System.IO.BufferedStream(stream), 2);
            this.init();
        }

        /// <summary>
        /// Specified that the character argument starts a single-line comment.
        /// </summary>
        /// <param name="ch">The character.</param>
        public virtual void CommentChar(int ch)
        {
            if (ch >= 0 && ch <= 255)
                this.attribute[ch] = StreamTokenizerSupport.COMMENTCHAR;
        }

        /// <summary>
        /// Determines whether or not ends of line are treated as tokens.
        /// </summary>
        /// <param name="flag">True indicates that end-of-line characters are separate tokens; False indicates 
        /// that end-of-line characters are white space.</param>
        public virtual void EOLIsSignificant(bool flag)
        {
            this.eolIsSignificant = flag;
        }

        /// <summary>
        /// Return the current line number.
        /// </summary>
        /// <returns>Current line number</returns>
        public virtual int Lineno()
        {
            return this.lineno;
        }

        /// <summary>
        /// Determines whether or not word token are automatically lowercased.
        /// </summary>
        /// <param name="flag">True indicates that all word tokens should be lowercased.</param>
        public virtual void LowerCaseMode(bool flag)
        {
            this.lowerCaseMode = flag;
        }

        /// <summary>
        /// Parses the next token from the input stream of this tokenizer.
        /// </summary>
        /// <returns>The value of the ttype field.</returns>
        public virtual int NextToken()
        {
            char prevChar = (char)(0);
            char ch = (char)(0);
            char qChar = (char)(0);
            int octalNumber = 0;
            int state;

            if (this.pushedback)
            {
                this.pushedback = false;
                return this.ttype;
            }

            this.ttype = StreamTokenizerSupport.TT_NOTHING;
            state = StreamTokenizerSupport.STATE_NEUTRAL;
            this.nval = 0.0;
            this.sval = null;
            this.buf.Length = 0;

            do
            {
                int data = this.read();
                prevChar = ch;
                ch = (char)data;

                switch (state)
                {
                    case StreamTokenizerSupport.STATE_NEUTRAL:
                        {
                            if (data == -1)
                            {
                                this.ttype = TT_EOF;
                                state = StreamTokenizerSupport.STATE_DONE;
                            }
                            else if (ch > 255)
                            {
                                this.buf.Append(ch);
                                this.ttype = StreamTokenizerSupport.TT_WORD;
                                state = StreamTokenizerSupport.STATE_WORD;
                            }
                            else if (this.attribute[ch] == StreamTokenizerSupport.COMMENTCHAR)
                            {
                                state = StreamTokenizerSupport.STATE_LINECOMMENT;
                            }
                            else if (this.attribute[ch] == StreamTokenizerSupport.WORDCHAR)
                            {
                                this.buf.Append(ch);
                                this.ttype = StreamTokenizerSupport.TT_WORD;
                                state = StreamTokenizerSupport.STATE_WORD;
                            }
                            else if (this.attribute[ch] == StreamTokenizerSupport.NUMBERCHAR)
                            {
                                this.ttype = StreamTokenizerSupport.TT_NUMBER;
                                this.buf.Append(ch);
                                if (ch == '-')
                                    state = StreamTokenizerSupport.STATE_NUMBER1;
                                else if (ch == '.')
                                    state = StreamTokenizerSupport.STATE_NUMBER3;
                                else
                                    state = StreamTokenizerSupport.STATE_NUMBER2;
                            }
                            else if (this.attribute[ch] == StreamTokenizerSupport.QUOTECHAR)
                            {
                                qChar = ch;
                                this.ttype = ch;
                                state = StreamTokenizerSupport.STATE_STRING;
                            }
                            else if ((this.slashSlashComments || this.slashStarComments) && ch == '/')
                                state = StreamTokenizerSupport.STATE_POSSIBLEC_COMMENT;
                            else if (this.attribute[ch] == StreamTokenizerSupport.ORDINARYCHAR)
                            {
                                this.ttype = ch;
                                state = StreamTokenizerSupport.STATE_DONE;
                            }
                            else if (ch == '\n' || ch == '\r')
                            {
                                this.lineno++;
                                if (this.eolIsSignificant)
                                {
                                    this.ttype = StreamTokenizerSupport.TT_EOL;
                                    if (ch == '\n')
                                        state = StreamTokenizerSupport.STATE_DONE;
                                    else if (ch == '\r')
                                        state = StreamTokenizerSupport.STATE_DONE_ON_EOL;
                                }
                                else if (ch == '\r')
                                    state = StreamTokenizerSupport.STATE_PROCEED_ON_EOL;
                            }
                            break;
                        }
                    case StreamTokenizerSupport.STATE_WORD:
                        {
                            if (this.isWordChar(data))
                                this.buf.Append(ch);
                            else
                            {
                                if (data != -1)
                                    this.unread(ch);
                                this.sval = this.buf.ToString();
                                state = StreamTokenizerSupport.STATE_DONE;
                            }
                            break;
                        }
                    case StreamTokenizerSupport.STATE_NUMBER1:
                        {
                            if (data == -1 || this.attribute[ch] != StreamTokenizerSupport.NUMBERCHAR || ch == '-')
                            {
                                if (this.attribute[ch] == StreamTokenizerSupport.COMMENTCHAR && System.Char.IsNumber(ch))
                                {
                                    this.buf.Append(ch);
                                    state = StreamTokenizerSupport.STATE_NUMBER2;
                                }
                                else
                                {
                                    if (data != -1)
                                        this.unread(ch);
                                    this.ttype = '-';
                                    state = StreamTokenizerSupport.STATE_DONE;
                                }
                            }
                            else
                            {
                                this.buf.Append(ch);
                                if (ch == '.')
                                    state = StreamTokenizerSupport.STATE_NUMBER3;
                                else
                                    state = StreamTokenizerSupport.STATE_NUMBER2;
                            }
                            break;
                        }
                    case StreamTokenizerSupport.STATE_NUMBER2:
                        {
                            if (data == -1 || this.attribute[ch] != StreamTokenizerSupport.NUMBERCHAR || ch == '-')
                            {
                                if (System.Char.IsNumber(ch) && this.attribute[ch] == StreamTokenizerSupport.WORDCHAR)
                                {
                                    this.buf.Append(ch);
                                }
                                else if (ch == '.' && this.attribute[ch] == StreamTokenizerSupport.WHITESPACECHAR)
                                {
                                    this.buf.Append(ch);
                                }

                                else if ((data != -1) && (this.attribute[ch] == StreamTokenizerSupport.COMMENTCHAR && System.Char.IsNumber(ch)))
                                {
                                    this.buf.Append(ch);
                                }
                                else
                                {
                                    if (data != -1)
                                        this.unread(ch);
                                    try
                                    {
                                        this.nval = System.Double.Parse(this.buf.ToString());
                                    }
                                    catch (System.FormatException) { }
                                    state = StreamTokenizerSupport.STATE_DONE;
                                }
                            }
                            else
                            {
                                this.buf.Append(ch);
                                if (ch == '.')
                                    state = StreamTokenizerSupport.STATE_NUMBER3;
                            }
                            break;
                        }
                    case StreamTokenizerSupport.STATE_NUMBER3:
                        {
                            if (data == -1 || this.attribute[ch] != StreamTokenizerSupport.NUMBERCHAR || ch == '-' || ch == '.')
                            {
                                if (this.attribute[ch] == StreamTokenizerSupport.COMMENTCHAR && System.Char.IsNumber(ch))
                                {
                                    this.buf.Append(ch);
                                }
                                else
                                {
                                    if (data != -1)
                                        this.unread(ch);
                                    System.String str = this.buf.ToString();
                                    if (str.Equals(StreamTokenizerSupport.DASH))
                                    {
                                        this.unread('.');
                                        this.ttype = '-';
                                    }
                                    else if (str.Equals(StreamTokenizerSupport.DOT) && !(StreamTokenizerSupport.WORDCHAR != this.attribute[prevChar]))
                                        this.ttype = '.';
                                    else
                                    {
                                        try
                                        {
                                            this.nval = System.Double.Parse(str);
                                        }
                                        catch (System.FormatException) { }
                                    }
                                    state = StreamTokenizerSupport.STATE_DONE;
                                }
                            }
                            else
                            {
                                this.buf.Append(ch);
                                state = StreamTokenizerSupport.STATE_NUMBER4;
                            }
                            break;
                        }
                    case StreamTokenizerSupport.STATE_NUMBER4:
                        {
                            if (data == -1 || this.attribute[ch] != StreamTokenizerSupport.NUMBERCHAR || ch == '-' || ch == '.')
                            {
                                if (data != -1)
                                    this.unread(ch);
                                try
                                {
                                    this.nval = System.Double.Parse(this.buf.ToString());
                                }
                                catch (System.FormatException) { }
                                state = StreamTokenizerSupport.STATE_DONE;
                            }
                            else
                                this.buf.Append(ch);
                            break;
                        }
                    case StreamTokenizerSupport.STATE_LINECOMMENT:
                        {
                            if (data == -1)
                            {
                                this.ttype = StreamTokenizerSupport.TT_EOF;
                                state = StreamTokenizerSupport.STATE_DONE;
                            }
                            else if (ch == '\n' || ch == '\r')
                            {
                                this.unread(ch);
                                state = StreamTokenizerSupport.STATE_NEUTRAL;
                            }
                            break;
                        }
                    case StreamTokenizerSupport.STATE_DONE_ON_EOL:
                        {
                            if (ch != '\n' && data != -1)
                                this.unread(ch);
                            state = StreamTokenizerSupport.STATE_DONE;
                            break;
                        }
                    case StreamTokenizerSupport.STATE_PROCEED_ON_EOL:
                        {
                            if (ch != '\n' && data != -1)
                                this.unread(ch);
                            state = StreamTokenizerSupport.STATE_NEUTRAL;
                            break;
                        }
                    case StreamTokenizerSupport.STATE_STRING:
                        {
                            if (data == -1 || ch == qChar || ch == '\r' || ch == '\n')
                            {
                                this.sval = this.buf.ToString();
                                if (ch == '\r' || ch == '\n')
                                    this.unread(ch);
                                state = StreamTokenizerSupport.STATE_DONE;
                            }
                            else if (ch == '\\')
                                state = StreamTokenizerSupport.STATE_STRING_ESCAPE_SEQ;
                            else
                                this.buf.Append(ch);
                            break;
                        }
                    case StreamTokenizerSupport.STATE_STRING_ESCAPE_SEQ:
                        {
                            if (data == -1)
                            {
                                this.sval = this.buf.ToString();
                                state = StreamTokenizerSupport.STATE_DONE;
                                break;
                            }

                            state = StreamTokenizerSupport.STATE_STRING;
                            if (ch == 'a')
                                this.buf.Append(0x7);
                            else if (ch == 'b')
                                this.buf.Append('\b');
                            else if (ch == 'f')
                                this.buf.Append(0xC);
                            else if (ch == 'n')
                                this.buf.Append('\n');
                            else if (ch == 'r')
                                this.buf.Append('\r');
                            else if (ch == 't')
                                this.buf.Append('\t');
                            else if (ch == 'v')
                                this.buf.Append(0xB);
                            else if (ch >= '0' && ch <= '7')
                            {
                                octalNumber = ch - '0';
                                state = StreamTokenizerSupport.STATE_STRING_ESCAPE_SEQ_OCTAL;
                            }
                            else
                                this.buf.Append(ch);
                            break;
                        }
                    case StreamTokenizerSupport.STATE_STRING_ESCAPE_SEQ_OCTAL:
                        {
                            if (data == -1 || ch < '0' || ch > '7')
                            {
                                this.buf.Append((char)octalNumber);
                                if (data == -1)
                                {
                                    this.sval = buf.ToString();
                                    state = StreamTokenizerSupport.STATE_DONE;
                                }
                                else
                                {
                                    this.unread(ch);
                                    state = StreamTokenizerSupport.STATE_STRING;
                                }
                            }
                            else
                            {
                                int temp = octalNumber * 8 + (ch - '0');
                                if (temp < 256)
                                    octalNumber = temp;
                                else
                                {
                                    buf.Append((char)octalNumber);
                                    buf.Append(ch);
                                    state = StreamTokenizerSupport.STATE_STRING;
                                }
                            }
                            break;
                        }
                    case StreamTokenizerSupport.STATE_POSSIBLEC_COMMENT:
                        {
                            if (ch == '*')
                                state = StreamTokenizerSupport.STATE_C_COMMENT;
                            else if (ch == '/')
                                state = StreamTokenizerSupport.STATE_LINECOMMENT;
                            else
                            {
                                if (data != -1)
                                    this.unread(ch);
                                this.ttype = '/';
                                state = StreamTokenizerSupport.STATE_DONE;
                            }
                            break;
                        }
                    case StreamTokenizerSupport.STATE_C_COMMENT:
                        {
                            if (ch == '*')
                                state = StreamTokenizerSupport.STATE_POSSIBLEC_COMMENT_END;
                            if (ch == '\n')
                                this.lineno++;
                            else if (data == -1)
                            {
                                this.ttype = StreamTokenizerSupport.TT_EOF;
                                state = StreamTokenizerSupport.STATE_DONE;
                            }
                            break;
                        }
                    case StreamTokenizerSupport.STATE_POSSIBLEC_COMMENT_END:
                        {
                            if (data == -1)
                            {
                                this.ttype = StreamTokenizerSupport.TT_EOF;
                                state = StreamTokenizerSupport.STATE_DONE;
                            }
                            else if (ch == '/')
                                state = StreamTokenizerSupport.STATE_NEUTRAL;
                            else if (ch != '*')
                                state = StreamTokenizerSupport.STATE_C_COMMENT;
                            break;
                        }
                }
            }
            while (state != StreamTokenizerSupport.STATE_DONE);

            if (this.ttype == StreamTokenizerSupport.TT_WORD && this.lowerCaseMode)
                this.sval = this.sval.ToLower();

            return this.ttype;
        }

        /// <summary>
        /// Specifies that the character argument is "ordinary" in this tokenizer.
        /// </summary>
        /// <param name="ch">The character.</param>
        public virtual void OrdinaryChar(int ch)
        {
            if (ch >= 0 && ch <= 255)
                this.attribute[ch] = StreamTokenizerSupport.ORDINARYCHAR;
        }

        /// <summary>
        /// Specifies that all characters c in the range low less-equal c less-equal high are "ordinary" in this 
        /// tokenizer.
        /// </summary>
        /// <param name="low">Low end of the range.</param>
        /// <param name="hi">High end of the range.</param>
        public virtual void OrdinaryChars(int low, int hi)
        {
            this.setAttributes(low, hi, StreamTokenizerSupport.ORDINARYCHAR);
        }

        /// <summary>
        /// Specifies that numbers should be parsed by this tokenizer.
        /// </summary>
        public virtual void ParseNumbers()
        {
            for (int i = '0'; i <= '9'; i++)
                this.attribute[i] = StreamTokenizerSupport.NUMBERCHAR;
            this.attribute['.'] = StreamTokenizerSupport.NUMBERCHAR;
            this.attribute['-'] = StreamTokenizerSupport.NUMBERCHAR;
        }

        /// <summary>
        /// Causes the next call to the nextToken method of this tokenizer to return the current value in the 
        /// ttype field, and not to modify the value in the nval or sval field.
        /// </summary>
        public virtual void PushBack()
        {
            if (this.ttype != StreamTokenizerSupport.TT_NOTHING)
                this.pushedback = true;
        }

        /// <summary>
        /// Specifies that matching pairs of this character delimit string constants in this tokenizer.
        /// </summary>
        /// <param name="ch">The character.</param>
        public virtual void QuoteChar(int ch)
        {
            if (ch >= 0 && ch <= 255)
                this.attribute[ch] = QUOTECHAR;
        }

        /// <summary>
        /// Resets this tokenizer's syntax table so that all characters are "ordinary." See the ordinaryChar 
        /// method for more information on a character being ordinary.
        /// </summary>
        public virtual void ResetSyntax()
        {
            this.OrdinaryChars(0x00, 0xff);
        }

        /// <summary>
        /// Determines whether or not the tokenizer recognizes C++-style comments.
        /// </summary>
        /// <param name="flag">True indicates to recognize and ignore C++-style comments.</param>
        public virtual void SlashSlashComments(bool flag)
        {
            this.slashSlashComments = flag;
        }

        /// <summary>
        /// Determines whether or not the tokenizer recognizes C-style comments.
        /// </summary>
        /// <param name="flag">True indicates to recognize and ignore C-style comments.</param>
        public virtual void SlashStarComments(bool flag)
        {
            this.slashStarComments = flag;
        }

        /// <summary>
        /// Returns the string representation of the current stream token.
        /// </summary>
        /// <returns>A String representation of the current stream token.</returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder(StreamTokenizerSupport.TOKEN);

            switch (this.ttype)
            {
                case StreamTokenizerSupport.TT_NOTHING:
                    {
                        buffer.Append(StreamTokenizerSupport.NOTHING);
                        break;
                    }
                case StreamTokenizerSupport.TT_WORD:
                    {
                        buffer.Append(this.sval);
                        break;
                    }
                case StreamTokenizerSupport.TT_NUMBER:
                    {
                        buffer.Append(StreamTokenizerSupport.NUMBER);
                        buffer.Append(this.nval);
                        break;
                    }
                case StreamTokenizerSupport.TT_EOF:
                    {
                        buffer.Append(StreamTokenizerSupport.EOF);
                        break;
                    }
                case StreamTokenizerSupport.TT_EOL:
                    {
                        buffer.Append(StreamTokenizerSupport.EOL);
                        break;
                    }
            }

            if (this.ttype > 0)
            {
                if (this.attribute[this.ttype] == StreamTokenizerSupport.QUOTECHAR)
                {
                    buffer.Append(StreamTokenizerSupport.QUOTED);
                    buffer.Append(this.sval);
                }
                else
                {
                    buffer.Append('\'');
                    buffer.Append((char)this.ttype);
                    buffer.Append('\'');
                }
            }

            buffer.Append(StreamTokenizerSupport.LINE);
            buffer.Append(this.lineno);
            return buffer.ToString();
        }

        /// <summary>
        /// Specifies that all characters c in the range low less-equal c less-equal high are white space 
        /// characters.
        /// </summary>
        /// <param name="low">The low end of the range.</param>
        /// <param name="hi">The high end of the range.</param>
        public virtual void WhitespaceChars(int low, int hi)
        {
            this.setAttributes(low, hi, StreamTokenizerSupport.WHITESPACECHAR);
        }

        /// <summary>
        /// Specifies that all characters c in the range low less-equal c less-equal high are word constituents.
        /// </summary>
        /// <param name="low">The low end of the range.</param>
        /// <param name="hi">The high end of the range.</param>
        public virtual void WordChars(int low, int hi)
        {
            this.setAttributes(low, hi, StreamTokenizerSupport.WORDCHAR);
        }
    }
}
