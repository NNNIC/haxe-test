using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lextool
{
    public class YCODE
    {
        //汎用 DO_NEW DO_ADD
        public static Func<int,VALUE[],int[],VALUE> DO_NEW = (type, args,idx) => {
            var v = new VALUE();
            v.type = type;
            v.list = new List<VALUE>();
            foreach(var i in idx)
            {
                if (i>=0&&i<args.Length) v.list.Add(args[i]);
            }
            return v;
        };

        public static Func<int, VALUE[],int[], VALUE> DO_ADD = (type, args,idx) => {
            var v = args[idx[0]];
            if (v.list==null) v.list = new List<VALUE>();
            for(int i=1; i<idx.Length; i++)
            {
                var n = idx[i];
                if (n>=0 && n<args.Length) v.list.Add(args[idx[i]]);
            }            
            return v;
        };

        // tools for this class

        static double _getnum(object[] o,int n)
        {
            if (n>=0 &&  n < o.Length)
            {
                var v = (VALUE)o[n];
                return v.n;
            }
            return 0;
        }
        static string _getstr(object[] o, int n)
        {
            if (n>=0 && n < o.Length )
            {
                var v = (VALUE)o[n];
                return v.s;
            }
            return null;
        }
    }
}
