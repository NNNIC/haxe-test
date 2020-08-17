using System;
using System.Collections.Generic;
using System.Text;

namespace cs2haxe.y
{
    public class SrcReader
    {
        public enum ERR { none, unknown, eof, nobuf, indexminus };

        public int m_index;
        public int m_line;

        public ERR m_err;  //UNKNOWN,NONE,or EOF
        public string m_src { get; private set; }

        public void Init(string src)
        {
            m_src = src;
        }
        public string get()
        {
            m_err = ERR.none;
            m_line = count_eol();
            if (m_src == null) { m_err = ERR.nobuf;  return null; }
            if (m_index < 0) { m_err = ERR.indexminus; return null; }
            if (m_index >= m_src.Length) { m_err = ERR.eof; return null; };
            return m_src.Substring(m_index);
        }

        //public void inc(int n = 1)
        //{
        //    m_index++;
        //}

        //public void dec(int n = 1)
        //{
        //    m_index--;
        //}

        private int count_eol()
        {
            if (m_src==null || m_index < 0) return -1;
            var num = 0;
            for (var n = 0; n < m_index; n++)
            {
                if (n >= m_src.Length)
                {
                    num++;
                    break;
                }
                var c = m_src[n];
                if (c == 0x0a)
                {
                    num++;
                }
            }
            return num;
        }
    }
}
