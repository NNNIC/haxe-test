using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs2haxe
{
    public class yengine
    {
        string dbg_src;                 // デバッグ用途
        List<List<VALUE>> dbg_src_list; //     〃
        List<List<VALUE>> dbg_out_list; //     〃

        public List<List<VALUE>> Lex(string src)
        {
            dbg_src = src;
            dbg_out_list = lexUtil.lexSource(src);
            return dbg_out_list;
        }

        public void Normalize(ref List<List<VALUE>> src_list)
        {
            const int LOOPMAX = 10000;

            dbg_src_list = src_list;

            //無意味な行の削除
            for (int loop = 0; loop <= LOOPMAX; loop++)
            {
                if (loop == LOOPMAX) sys.error("Normalize LOOPMAX");

                bool bNeedLoop = false;
                for (int n = 0; n < src_list.Count; n++)
                {
                    var l = dbg_out_list[n];
                    if (l.TrueForAll(i => i.IsType(YDEF.EOL) || i.IsType(YDEF.SP) || i.IsType(YDEF.CMT)))
                    {
                        dbg_out_list.RemoveAt(n);
                        bNeedLoop = true;
                        break;
                    }
                }
                if (!bNeedLoop) break;
            }

            //ダブルクォーテーションで囲まれた文字以外、すべて大文字へ
            //スペースの全削除
            foreach (var l in src_list)
            {
                for (int loop = 0; loop <= LOOPMAX; loop++)
                {
                    if (loop == LOOPMAX) sys.error("Normalize LOOPMAX:2");

                    bool bNeedLoop = false;
                    for (int n = 0; n < l.Count; n++)
                    {
                        var v = l[n];
                        if (!v.IsType(YDEF.QSTR))
                        {
                            if (v.s != null) v.s = v.s.ToUpper();
                        }

                        if (v.IsType(YDEF.SP))
                        {
                            l.RemoveAt(n);
                            bNeedLoop = true;
                            break;
                        }

                        if (v.IsType(YDEF.EOF))
                        {
                            l.RemoveAt(n);
                            bNeedLoop = true;
                            break;
                        }
                    }
                    if (!bNeedLoop) break;
                }
            }

            return;
        }

        public List<List<VALUE>> Interpret(List<List<VALUE>> src)
        {
            dbg_src_list = src;
            var output = new List<List<VALUE>>();
            foreach (var l in src)
            {
                var oline = new List<VALUE>();
                if (!yanalyzer.Analyze(l, out oline)) return null;
                output.Add(oline);
            }

            dbg_out_list = output;

            return output;
        }

        public void DumpList(List<List<VALUE>> list, bool bDetail = false)
        {
            if (bDetail)
            {
                foreach(var l in list)
                {
                    DumpLine_detail(l);
                }
            }
            else
            {
                foreach (var l in list)
                {
                    foreach (var v in l)
                    {
                        sys.log(v.ToString() + ",");
                    }
                }
            }
        }
        public void DumpLine(List<VALUE> l)
        {
            string s = null;
            foreach(var v in l)
            {
                var types = v.get_ascent_types();
                var terms = v.get_all_terminals();

                s+= string.Format("[{0}:{1}] ",types,terms);
            }
            sys.logline(s);
        }
        public void DumpLine_detail(List<VALUE> l)
        {
            // [type>|?|0[]1[]2[]
            string s =null;
            Action<VALUE> work = null;
            work = (v) => {
                s += "[";
                var tm = v.GetTerminal();
                s += v.get_type_name() + ">" + (tm!=null ? ("`" + tm + "`") :"");
                if (v.list!=null)
                {
                    for(int i = 0; i<v.list.Count; i++)
                    {
                        s+=i.ToString();
                        work(v.list[i]);
                    }
                }
                s+="]";
            };
            
            l.ForEach(i=>work(i));

            sys.logline(s);
        }

        public List<List<VALUE>> PreProcess(List<List<VALUE>> src)
        {
            Hashtable ht = new Hashtable(); //変数用

            var out_list = new List<List<VALUE>>();

            bool bIfZone = false;
            bool? bIfTree = null;
            foreach (var l in src)
            {
                if (l.Count == 0) continue;
                VALUE v0 = l[0];
                if (v0.IsType(getyp(YDEF.sx_prepro_setence))) //#if #elif #else #endif
                {
                    if (v0.IsType(getyp(YDEF.sx_pif_sentence))) //#if
                    {
                        if (bIfZone) sys.error("Not supported Dual #if");
                        bIfZone = true;
                        var func = find(v0,YDEF.sx_function);
                        if (func==null) sys.error("Can not find function for #if",v0);
                        bIfTree = runtime.PreProcessFunction.Execute(func);
                        continue;
                    }
                    if (v0.IsType(getyp(YDEF.sx_pelif_sentence)))//#elif
                    {
                        if (!bIfZone) sys.error("Preceed #if does not exist",v0);
                        if (bIfTree == true) bIfTree = null;
                        if (bIfTree==null)
                        {
                            sys.logline("skip");
                        }
                        var func = find(v0,YDEF.sx_function);
                        if (func==null) sys.error("Can not find function for #elif",v0);
                        bIfTree = runtime.PreProcessFunction.Execute(func);
                        continue;
                    }
                    if (v0.IsType(getyp(YDEF.sx_pelse_sentence)))//else
                    {
                        if (!bIfZone) sys.error("Preceed #if does not exist",v0);
                        if (bIfTree == true) bIfTree = null;
                        if (bIfTree == null)
                        {
                            sys.logline("skip");
                        }
                        else
                        {
                            bIfTree = true;
                        }
                        continue;
                    }
                    if (v0.IsType(getyp(YDEF.sx_pendif_sentence)))//endif
                    {
                        if (!bIfZone) sys.error("Preceed #if does not exist",v0);
                        bIfZone = false;
                        bIfTree = null;
                        continue;
                    }

                    if (v0.IsType(getyp(YDEF.sx_pset_sentence)))//set
                    {
                        //#set will process later.
                    }
                    else //以外はエラー
                    {
                        sys.error("Unexpected",v0);
                    }

                }

                if (bIfZone==false || (bIfZone==true && bIfTree==true) )
                {
                    if (v0.IsType(getyp(YDEF.sx_pset_sentence)))
                    {
                        var varv = find(v0,YDEF.VAR);
                        if (varv==null) sys.error("Cannot find set variable",v0);
                        var rest = find(v0,YDEF.REST);
                        if (rest == null) sys.error("Cannot find set rest",v0);
                        ht[varv.GetString()] = rest;
                        continue;
                    }
                    var findvar = find_l(l,YDEF.VAR);
                    if (findvar!=null)
                    {
                        var key = findvar.GetString();
                        if (ht.ContainsKey(key))
                        {
                            if (!replace_l(l,YDEF.VAR, (VALUE)ht[key]))
                            {
                                sys.error("Cannot converted",v0);
                            }
                        }
                        else
                        {
                            sys.error(key + " has not been defined",v0);
                        }
                    }

                    var newl = l;
                    YDEF.insert_rest_children_if_use(ref newl);
                    out_list.Add(newl);
                }

            }
            dbg_out_list = out_list;
            return out_list;
        }

        public bool IsExecuable(List<List<VALUE>> list, out int errorline)
        {
            errorline = -1;
            foreach(var l in list)
            {
                if (l.Count>0)
                {
                    var typ = l[0].type;
                    if (typ == getyp(YDEF.sx_sentence)) continue; //最終形態でＯＫ

                    errorline = l[0].get_dbg_line();
                    return false;
                }
            }
            return true;
        }

        // -- util --
        private static string gn(object[] o)
        {
            return YDEF.get_name(o);
        }
        private static int getyp(object[] o)
        {
            return YDEF.get_type(o);
        }
        private static VALUE find(VALUE v, object[] o)
        {
            var t = getyp(o);
            var fv = v.FindValueByTravarse(t);
            return fv;
        }
        private static VALUE find(VALUE v, int typ)
        {
            var fv = v.FindValueByTravarse(typ);
            return fv;
        }
        private static bool replace(VALUE src, int typ, VALUE dst)
        {
            return src.ReplaceValueByTravarse(typ,dst);
        }
        //ライン
        private static VALUE find_l(List<VALUE> l, int typ)
        {

            for (int i = 0; i < l.Count; i++)
            {
                var v = l[i];
                if (v.type == typ)
                {
                    return v;
                }
            }
            for (int i = 0; i < l.Count; i++)
            {
                var v = l[i];
                var f = find(v, typ);
                if (f != null) return f;
            }
            return null;
        }
        private static bool replace_l(List<VALUE> l, int typ, VALUE dst)
        {
            for (int i = 0; i < l.Count; i++)
            {
                var v = l[i];
                if (v.type == typ)
                {
                    l[i] = dst;
                    return true;
                }
            }
            for (int i = 0; i<l.Count; i++)
            { 
                var v = l[i];
                var b = replace(v, typ,dst);
                if (b) return true;
            }
            return false;

        }

    }



}

