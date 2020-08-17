using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs2haxe
{
    public class yanalyzer
    {
        public static bool Analyze(List<VALUE> src, out List<VALUE> dst)
        {
            const int LOOPMAX = 10000;

            dst = new List<VALUE>(src);

            var list = new List<VALUE>(src);

            for(int loop = 0; loop <= LOOPMAX; loop++)
            {
                if (loop == LOOPMAX) sys.error("yanalyzer Analyze LoopMax:1"); 

                var syntax_order = YDEF.get_syntax_order();

                bool bNeedLoop=false;
                for(int i = 0; i< syntax_order.Count; i++)
                {
                    var syntax = syntax_order[i];
                    var tslist = YDEF.get_syntax_set(syntax);

                    foreach (var ts in tslist)
                    {
                        if (_check_syntax(dst,ts))
                        {
                            bNeedLoop = true;
                            break;
                        }
                    }
                    if (bNeedLoop) break; //最初から
                }

                if (!bNeedLoop) break;
            }

            return true;
        }

        private static bool _check_syntax(List<VALUE> list, YDEF.TreeSet ts)
        {
            const int LOOPMAX = 10000;

            bool bModified = false;

            for (int loop = 0; loop <= LOOPMAX; loop++)
            {
                if (loop == LOOPMAX) sys.error("yanalyzer Analyze LoopMax:2"); 

                bool bNeedLoop = false;
                
                for(int i = 0; i<list.Count; i++)
                {
                    if (_isMatchAndMake(list,i,ts))
                    {
                        bModified = true;
                        bNeedLoop = true;
                        break;
                    }
                }
                if (!bNeedLoop) break;
            }

            return bModified;
        }
        private static bool _isMatchAndMake(List<VALUE> list, int index, YDEF.TreeSet ts)
        {
            Func<int,VALUE> get = (n) => {
                if (n >= list.Count) return null;
                return list[n];
            };
            List<VALUE> args = new List<VALUE>();
            int removelength = ts.list.Count;

            for (int i = 0; i<ts.list.Count; i++)
            {
                var v = get(index + i);
                if (v==null) return false;

                if (i==0 && ts.list.Count==1 && v.IsType(ts.type)) return false; //既に変換済み

                var o = ts.list[i];
                if (o.GetType() == typeof(string))
                {
                    if (v.GetString() != (string)o) return false;
                }
                else
                {
                    var tstype = (int)o;
                    if (tstype == YDEF.REST) // ※RESTは特殊処理。EOLまでのすべて(除EOL)が入る
                    {
                        removelength--; //本VALUE分を事前に引く

                        var restv = new VALUE();
                        restv.type = YDEF.REST;
                        restv.list = new List<VALUE>();

                        for (int j = index + i; j < list.Count; j++)
                        {
                            var v2 = get(j);
                            if (v2 != null && !v2.IsType(YDEF.EOL))
                            {
                                restv.list.Add(v2);
                                removelength++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        args.Add(restv);
                        break;
                    }
                    else
                    {
                        if (!v.IsType(tstype)) return false;
                    }
                }
                args.Add(v);
            }

            //Yes, they match! then Make it.

            var makefunc = (Func<int, VALUE[], int[], VALUE>)ts.make_func;

            var newv = ts.make_func(ts.type,args.ToArray(),ts.make_index.ToArray());

            list.RemoveRange(index, removelength);
            list.Insert(index,newv);

            return true;
        }
    }
}
