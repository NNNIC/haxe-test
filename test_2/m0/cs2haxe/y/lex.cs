using cs2haxe.y;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * プリミティブ構文解析 
 *  .lexファイルを作らずに処理自体をここに記載。
 * 
  */
namespace cs2haxe
{
    public class lexFirst
    {
        //string m_src;
        //string[] m_lines;
        SrcReader m_sr;

        int m_l { get { return m_sr.m_line;  } }
        int m_c { get { return m_sr.m_index; } set { m_sr.m_index = value; } }

        public List<VALUE> m_value_list;

        public void Init(string src)
        {
            m_sr = new SrcReader();
            m_sr.Init(src);
            m_value_list = new List<VALUE>();
        }

        void add(VALUE v)
        {
            m_value_list.Add(v);
        }

        void end_line()
        {
        }

        public bool CheckOne() //no more then return false;
        {
            var s = m_sr.get();

            if (m_sr.m_err == SrcReader.ERR.eof)
            {
                var vtmp = new VALUE();
                vtmp.type = YDEF.EOF;
                add(vtmp);
                end_line();
                return false;
            }
            
            int wdlen;
            var v = lexUtil.GetWord(out wdlen, m_c, m_sr.m_src,m_l);
            m_c += wdlen;

            add(v);

            if (v.type == YDEF.ERROR)
            {
                sys.error(string.Format("L:{0}C:{1}>{2}",v.dbg_line,v.dbg_col,v.s));
                return false;
            }

            if (v.type == YDEF.EOL)
            {
                end_line();
                //m_l++;
                //m_c = 0;
            }

            return true;
        }
    }

    public class lexUtil //汎用で使える。
    {
        public static List<VALUE> lexSource(string src)
        {
            const int LOOPMAX = (int)1E+8;

            var lex = new lexFirst();
            lex.Init(src);
            for(var loop = 0; loop<=LOOPMAX; loop++)
            {
                if (loop == LOOPMAX)  sys.error("lexSource LOOP MAX");
                if (!lex.CheckOne())
                { 
                    break;
                }
            }

            return lex.m_value_list;
        }

        public static VALUE GetWord(out int wdlen, int col, string i_line, int dbg_line = -1)
        {
            var v= new VALUE();

            v.type = YDEF.UNKNOWN;
            v.dbg_col  = col;
            v.dbg_line = dbg_line;

            wdlen = 0;
            if (string.IsNullOrEmpty(i_line)) { wdlen = 1; return any_return(v, YDEF.EOL); }

            var line = i_line.TrimEnd();
            if (string.IsNullOrEmpty(line) || col >= line.Length) { wdlen = 1; return any_return(v, YDEF.EOL); }

            var ls = i_line.Substring(col);
            if (string.IsNullOrEmpty(ls)) { wdlen = 1;  return any_return(v, YDEF.EOL); }

            //コメントは全部
            if (ls.StartsWith(YDEF.CMTSTR))
            {
                wdlen = ls.Length;
                return any_return(v,YDEF.CMT,null,ls);
            }

            //コメント /* */で囲まれた部分
            

            //ダブルクォーテーションで囲まれた文字列はそのまま
            if (ls.StartsWith(YDEF.DQ))
            {
                if (ls.Length > 1)
                {
                    var idx = ls.IndexOf(YDEF.DQ, 1);
                    if (idx < 0)
                    {
                        return err_return(v, "End of Double Quatation is not found:1");
                    }
                    wdlen = idx + 1;
                    return any_return(v,YDEF.QSTR,null,ls.Substring(0, idx + 1));
                }
                else
                {
                    return err_return(v,"End of Double Quation is not found:2");
                }
            }

            //連続したスペース・タブはそのまま
            if (ls[0] <= ' ')
            {
                for (var i = 0; i < ls.Length; i++)
                {
                    if (ls[i] == '\x0a')
                    {
                        if (i > 0)
                        {
                            wdlen = i;
                            return any_return(v, YDEF.SP, null, " ");
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (ls[i] > ' ')
                    {
                        wdlen = i;
                        return any_return(v,YDEF.SP,null," ");
                    }
                }
                if (wdlen == 0) wdlen = 1;
                return any_return(v,YDEF.EOL);
            }

            //数字
            if (IsNumberElement(ls[0], true))
            {
                wdlen = 0;
                string s = "" + ls[0];
                for (int i = 1; i < ls.Length; i++)
                {
                    if (IsNumberElement(ls[i]))
                    {
                        s += ls[i];
                    }
                    else
                    {
                        wdlen = i;
                        break;
                    }
                }
                if (wdlen == 0) wdlen = ls.Length;
                double d;
                if (double.TryParse(s, out d))
                {
                    return any_return(v, YDEF.NUM, d, s);
                }
                else
                {
                    if (s == ".")
                    {
                        return any_return(v, YDEF.SYM, null, s);
                    }
                    return err_return(v,s);
                }
            }

            //名前
            if (IsNameElement(ls[0]))
            {
                wdlen = 0;
                string s = "" + ls[0];
                for (int i = 1; i < ls.Length; i++)
                {
                    if (IsNameElement(ls[i], true))
                    {
                        s += ls[i];
                    }
                    else
                    {
                        wdlen = i;
                        break;
                    }
                }
                if (wdlen == 0) wdlen = ls.Length;
                return any_return(v,YDEF.STR,null,s);
            }
            if (ls[0] == '@' || ls[0] == '#')
            {
                if (ls.Length == 1 || !IsNameElement(ls[1]))
                {
                    return err_return(v,"variable or predefine name error");
                }
                wdlen = 0;
                string s = "" + ls[0] + ls[1];
                for (int i = 2; i < ls.Length; i++)
                {
                    if (IsNameElement(ls[i], true))
                    {
                        s += ls[i];
                    }
                    else
                    {
                        wdlen = i;
                        break;
                    }
                }
                if (wdlen == 0) wdlen = ls.Length;
                return any_return(v, (ls[0] == '@') ? YDEF.VAR : YDEF.PRE, null,s);
            }

            //その他　記号とみなす
            wdlen = 1;

            return any_return(v,YDEF.SYM,null,""+ls[0]);
        }

        // -- util for this clas --
        static VALUE err_return(VALUE v, string s)
        {
            v.type = YDEF.ERROR;
            v.o = v.s = s;
            return v;
        }
        static VALUE any_return(VALUE v, int type, double? n=null, string s=null)
        {
            v.type = type;
            if (n!=null)
            {
                v.o = v.n = (double)n;
            }
            if (s!=null)
            {
                v.s = s;
                if (v.o==null) v.o = v.s;
            }
            if (v.o==null) v.o = type;
            return v;
        }

        static bool IsNumberElement(char c, bool bHeadIsMinus = false)
        {
            if (bHeadIsMinus)
            {
                if (c == '-' || c == '+') return true;
            }
            return (c >= '0' && c <= '9') || (c == '.');
        }
        static bool IsNameElement(char c, bool bNumberInclude = false)
        {
            if (c == '_') return true;
            if (c >= 'a' && c <= 'z') return true;
            if (c >= 'A' && c <= 'Z') return true;
            if (bNumberInclude && c >= '0' && c <= '9') return true;

            return false;
        }
    }
}
