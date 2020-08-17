using System;
using System.Collections.Generic;
using System.Reflection;

namespace lextool
{
    public partial class YDEF
    {
        public static string get_name(object[] o)
        {
            if (o.Length > 0) return (string)o[0];
            return null;
        }
        public static string get_name(int type)
        {
            if (Enum.IsDefined(typeof(TOKEN), type))
            {
                var n = (TOKEN)type;
                return n.ToString();
            }
            var list = get_syntax_order();
            var find = list.Find(i => get_type(i) == type);
            if (find != null) return get_name(find);
            return null;
        }
        public static int get_type(object[] o)
        {
            if (o.Length > 1) return (int)o[1];
            return -1;
        }
        public static int get_type(string syntax)
        {
            var list = get_syntax_order();
            var find = list.Find(i => get_name(i) == syntax);
            if (find != null) return get_type(find);
            return YDEF.ERROR;
        }
        public class TreeSet
        {
            public object[] syntax_tree; //分解元            ※利用時に便宜
            public string name;          //文法名            ※　　〃
            public int type;             //トークンタイプ　　※　　〃

            public List<object> list;        //トークンタイプ、または、文字列
            public Func<int, VALUE[], int[], VALUE> make_func;
            public List<int> make_index;

            public override string ToString()
            {
                return name;
            }
        }
        public static List<TreeSet> get_syntax_set(object[] syntax_tree)
        {
            List<TreeSet> list = new List<TreeSet>();
            TreeSet ts = null;
            bool bMakeOrTree = false;

            string name = (string)syntax_tree[0];
            int type = (int)syntax_tree[1];

            for (int i = 2; i < syntax_tree.Length; i++)
            {
                var o = syntax_tree[i];
                if (o.GetType() == typeof(int) && (int)o == YDEF.__OR__)
                {
                    list.Add(ts);
                    ts = null;
                    bMakeOrTree = false;
                    continue;
                }

                if (o.GetType() == typeof(int) && (int)o == YDEF.__MAKE__)
                {
                    bMakeOrTree = true;
                    ts.make_index = new List<int>();
                    continue;
                }

                if (bMakeOrTree)
                {
                    if (ts.make_func == null)
                    {
                        ts.make_func = (Func<int, VALUE[], int[], VALUE>)o;
                        continue;
                    }
                    else
                    {
                        ts.make_index.Add((int)o);
                        continue;
                    }
                }
                else
                {
                    if (ts == null)
                    {
                        ts = new TreeSet();
                        ts.syntax_tree = syntax_tree;
                        ts.name = name;
                        ts.type = type;
                        ts.list = new List<object>();
                    }
                    var newo = o;
                    if (o.GetType() == typeof(string))
                    {
                        int temp = get_type((string)o);
                        if (temp >= 0) newo = (object)temp;
                    }
                    ts.list.Add(newo);
                    continue;
                }
            }
            if (ts != null)
            {
                list.Add(ts);
            }
            return list;
        }

        private static List<FieldInfo> __sx_members;
        public static List<FieldInfo> get_syntax_list()
        {
            if (__sx_members != null) return __sx_members;

            var type = typeof(YDEF);

            __sx_members = new List<FieldInfo>();
            foreach (var i in type.GetFields())
            {
                if (i.Name.StartsWith("sx_")) __sx_members.Add(i);
            }

            return __sx_members;
        }

        private static List<object[]> __syntax_order;
        public static List<object[]> get_syntax_order()
        {
            if (__syntax_order != null) return __syntax_order;
            var infos = get_syntax_list();
            __syntax_order = new List<object[]>();

            foreach (var i in infos)
            {
                var o = (object[])i.GetValue(null);
                __syntax_order.Add(o);
            }

            __syntax_order.Sort((a, b) => get_type(b) - get_type(a));

            return __syntax_order;
        }

        public static void insert_rest_children_if_use(ref List<VALUE> list)// リスト中にRESTがある場合、RESTの子供を上位に挿入し、RESTを消す
        {
            bool bDone = false;

            Action<List<VALUE>> work = null;
            work = (l) =>
            {
                if (bDone) return;
                for (int i = 0; i < l.Count; i++)
                {
                    var v = l[i];
                    if (v.type == YDEF.REST)
                    {
                        l.RemoveAt(i);
                        l.InsertRange(i, v.list);
                        bDone = true;
                        return;
                    }
                }
            };

            work(list);
        }

    }

    public class VALUE
    {
        public double n;
        public string s;
        public object o;

        public int type;
        public List<VALUE> list;
        public bool IsType(int itype)
        {
            if (itype == type) return true;
            if (list != null && list.Count == 1)//保持要素が１つの場合は、そのタイプもチェック対象
            {
                return list[0].IsType(itype);
            }
            return false;
        }
        public bool IsType(string s)
        {
            int tp = YDEF.get_type(s);
            return IsType(tp);
        }
        public string GetString()
        {
            if (s != null) return s;
            if (list != null && list.Count == 1)
            {
                return list[0].GetString();
            }
            return null;
        }
        public string GetTerminal()
        {
            if (s!=null) return s;
            if (o!=null && o.GetType()==typeof(double)) return o.ToString();
            return null;
        }
        public object GetTerminalObject()
        {
            if (o != null)
            {
                var t = o.GetType();
                if (t == typeof(string) || t==typeof(double))
                {
                    return o;
                }
            }
            return null;
        }
        public object GetTerminalObject_ascent()//遡って取得
        {
            var o = GetTerminalObject();
            if (o!=null) return o;
            if (list != null && list.Count == 1)
            {
                o = list[0].GetTerminalObject_ascent();
            }
            return o;
        }

        //デバッグ
        public int dbg_line;
        public int dbg_col;

        public override string ToString()
        {
            string s = null;

            s += type.ToString() + ":" + YDEF.get_name(type);

            return s + ":" + (o != null ? o.ToString() : "null");
        }
        public string get_type_name()
        {
            return YDEF.get_name(type);
        }
        public string get_ascent_types() //タイプを遡って纏めて文字列化。listの先頭のみが対象
        {
            string s = null;
            Action<VALUE> printtype = null;
            printtype = (v) =>
            {
                if (v==null) return;
                if (s!=null) s+="-";
                s+= YDEF.get_name(v.type);
                if (v.list != null && v.list.Count > 0)
                {
                    printtype(v.list[0]);
                }
            };

            printtype(this);

            return s;
        }
        public string get_all_terminals() //全終端記号を出力
        {
            string s = null;
            Action<VALUE> print_terminals = null;
            print_terminals = (v) => {
                var n = v.GetTerminal();
                if (n != null)
                {
                    if (s != null) s += ",";
                    s += n;
                }
                else
                {
                    if (v.list != null) foreach (var v2 in v.list)
                    {
                        print_terminals(v2);
                    }
                }
            };
            print_terminals(this);
            return s;
        }
        public int get_dbg_line()
        {
            int line = -1;
            Travarse(v => {
                if (Enum.IsDefined(typeof(TOKEN),v.type))
                {
                    line = v.dbg_line;
                    return true;
                }
                return false;
            });
            return line;
        }

        //実行時用

        public VALUE FindValueByTravarse(int itype) //指定タイプをトラバースして検索　(listを辿りながら)
        {
            if (itype == type) return this;
            if (list==null) return null;

            for(int i = 0; i<list.Count;i++)//１．中のタイプのみを確認
            {
                if (list[i].type == itype) return list[i];
            }
            for(int i = 0; i<list.Count;i++)//２．一つずつ中を検索
            {
                var v = list[i].FindValueByTravarse(itype);
                if (v!=null) return v;
            }

            return null;
        }

        public bool ReplaceValueByTravarse(int itype, VALUE dst)//トラバースして、最初に見つけたのを入れ替える。
        {
            if (itype==type)//自身の入れ替えはＮＧ
            {
                sys.logline("Unexpected. Cannot replace self");
                return false;
            }
            if (list == null) return false;

            for (int i = 0; i<list.Count; i++)
            {
                if (list[i].type == itype)
                {
                    list[i] = dst;
                    return true;
                }
            }
            for(int i = 0; i<list.Count;i++)
            {
                if (list[i].ReplaceValueByTravarse(itype,dst))
                {
                    return true;
                }
            }
            return false;
        }

        public VALUE get_child(int index)
        {
            if (index >= list.Count) { sys.logline("get_child index exceeded"); return null; }
            var v = list[index];
            return v;
        }

        public void Travarse(Func<VALUE,bool> func)//汎用トラバース  funcの戻り値がtrue時は、以降の確認をしない。
        {
            bool bDone = false;
            Action<VALUE> work = null;
            work = (v) => {
                if (!bDone)
                {
                    bDone = func(v);
                    if (bDone) return;
                }
                if (v.list != null) {
                    for (int i = 0; i < v.list.Count; i++)
                    {
                        bDone = func(v.list[i]);
                        if (bDone) return;
                    }
                    for (int i = 0; i < v.list.Count; i++)
                    {
                        work(v.list[i]);
                        if (bDone) return;
                    }
                }
            };
            work(this);
        }

    }
}
