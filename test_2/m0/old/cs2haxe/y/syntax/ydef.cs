using System;
using System.Collections.Generic;
using System.Reflection;
namespace cs2haxe
{
    /*
     *   Lex & Yacc の .y定義に当たる部分。 
     *　
     *　 .yに似せた構造となっている。
     *　 
     *　 文法は、ここだけを変更すればよい。
     * 
     */


    //終末トークン
    public enum TOKEN
    {
        UNKNOWN = 0,
        NUM     ,//= 1,
        STR     ,//= 2,
        QSTR    ,//= 3,
        SYM     ,//= 4,
        SP      ,//= 5,
        EOL     ,//= 6,
        EOF     ,//= 7,

        VAR     ,//= 8,
        PRE     ,//= 9,

        REST    ,//= 10,
        CMT     ,//= 11,

        ERROR   = -1
    }


    public partial class YDEF
    {
        //未定
        public static int UNKNOWN = (int)TOKEN.UNKNOWN;   //取り扱いを簡単にするために再定義

        //基本
        public static int NUM  = (int)TOKEN.NUM;          //   　　　　　〃
        public static int STR  = (int)TOKEN.STR;          //   　　　　　〃
        public static int QSTR = (int)TOKEN.QSTR;         //   　　　　　〃
        public static int SYM  = (int)TOKEN.SYM;          //   　　　　　〃
        public static int SP   = (int)TOKEN.SP;           //   　　　　　〃
        public static int EOL  = (int)TOKEN.EOL;          //   　　　　　〃
        public static int EOF  = (int)TOKEN.EOF;          //   　　　　　〃

        //専用                                            //   　　　　　〃
        public static int VAR  = (int)TOKEN.VAR;          //   　　　　　〃
        public static int PRE  = (int)TOKEN.PRE;          //   　　　　　〃

        //特殊                                            //   　　　　　〃
        public static int REST  = (int)TOKEN.REST;        //   　　　　　〃
        public static int CMT   = (int)TOKEN.CMT;         //   　　　　　〃
        public static int ERROR = (int)TOKEN.ERROR;       //   　　　　　〃

        //構文分析用
        public static int __OR__   = 100;
        public static int __MAKE__ = 101;
 
        //文字列
        public static string CMTSTR = "//";
        public static string CMTSTR_S = "/*";
        public static string CMTSTR_E = "*/";
        public static string DQ = "\"";

        //構文ツリー     .yの表現をコードで実装。　第１，２要素はルール名とトークンタイプ。 __OR__は選択支。__MAKE__は処理用のファンクション
        //               
        //     トークンタイプの大きい方が結びつきが強い。
        //     __MAKE__の後は 処理ファンクション、引数の番号(Base 0)
        //

        public static object[] sx_sentence        =  { "sx_sentence",
                                                       1001,
                                                             "sx_screen_sentence",                          __MAKE__, YCODE.DO_NEW, 0,
                                                             __OR__,
                                                             "sx_layer_sentence",                           __MAKE__, YCODE.DO_NEW, 0,
                                                             __OR__,
                                                             "sx_set_sentance",                             __MAKE__, YCODE.DO_NEW, 0,
                                                             __OR__,
                                                             "sx_display_sentence",                         __MAKE__, YCODE.DO_NEW, 0,
                                                             __OR__,
                                                             "sx_pifelse_sentence",                         __MAKE__, YCODE.DO_NEW, 0,
                                                      };

        public static object[] sx_screen_sentence =  { "sx_screen_sentence",
                                                       1003,
                                                             "SCREEN", QSTR, EOL,                           __MAKE__, YCODE.DO_NEW, 0, 1,
                                                             __OR__,
                                                             "SCREEN", QSTR, "sx_option_phrase", EOL,       __MAKE__, YCODE.DO_NEW, 0, 1, 2,
                                                     };
        public static object[] sx_layer_sentence  =  { "sx_layer_sentence",
                                                       1004,
                                                             "LAYER", QSTR, STR, EOL,                       __MAKE__, YCODE.DO_NEW, 0, 1, 2,
                                                             __OR__,
                                                             "LAYER" , QSTR,STR, "sx_option_phrase",EOL,    __MAKE__, YCODE.DO_NEW, 0, 1, 2, 3,
                                                     };

        public static object[] sx_display_sentence=  { "sx_display_sentence",
                                                       1006,
                                                             "DISPLAY", QSTR, "sx_pos", EOL ,                   __MAKE__, YCODE.DO_NEW, 0,1,2,
                                                             __OR__,
                                                             "DISPLAY", QSTR, "sx_pos", "sx_option_phrase", EOL,__MAKE__, YCODE.DO_NEW, 0,1,2,3
                                                     };
        
        public static object[] sx_prepro_setence  =  { "sx_prepro_setence",
                                                       1007,
                                                             "sx_pif_sentence",                 __MAKE__, YCODE.DO_NEW, 0,
                                                             __OR__,
                                                             "sx_pelif_sentence",               __MAKE__, YCODE.DO_NEW, 0,
                                                             __OR__,
                                                             "sx_pelse_sentence",               __MAKE__, YCODE.DO_NEW, 0,
                                                             __OR__,
                                                             "sx_pendif_sentence",              __MAKE__, YCODE.DO_NEW, 0,
                                                             __OR__,
                                                             "sx_pset_sentence",                __MAKE__, YCODE.DO_NEW, 0,
                                                     };

        public static object[] sx_pif_sentence    =  { "sx_pif_sentence",
                                                       1008,
                                                             "#IF",    "sx_function", EOL,      __MAKE__, YCODE.DO_NEW, 0, 1};

        public static object[] sx_pelif_sentence  =  { "sx_pelif_sentence",
                                                       1009,
                                                             "#ELIF",  "sx_function", EOL,      __MAKE__, YCODE.DO_NEW, 0, 1};

        public static object[] sx_pelse_sentence  =  { "sx_pelse_sentence",
                                                       1010,
                                                             "#ELSE"  ,EOL,                     __MAKE__, YCODE.DO_NEW, 0   };
        public static object[] sx_pendif_sentence =  { "sx_pendif_sentence",
                                                       1011,
                                                             "#ENDIF" ,EOL,                     __MAKE__, YCODE.DO_NEW, 0   };

        public static object[] sx_pset_sentence   =  { "sx_pset_sentence",
                                                       1012,
                                                             "#SET", VAR,REST, EOL,             __MAKE__, YCODE.DO_NEW, 0,1,2 };


        public static object[] sx_option_phrase   =  { "sx_option_phrase",
                                                       1013,
                                                           "{","sx_function","}",              __MAKE__,YCODE.DO_NEW, 1,
                                                            __OR__,
                                                            "{","sx_function",";","}",          __MAKE__,YCODE.DO_NEW, 1,
                                                            __OR__,
                                                            "{","sx_function","}",              __MAKE__,YCODE.DO_NEW, 1,
                                                            __OR__,
                                                            "{", "sx_function", ";" , "}" ,     __MAKE__,YCODE.DO_NEW, 1,
                                                            __OR__,
                                                            "{", "sx_function_list", "}",       __MAKE__,YCODE.DO_NEW, 1,
                                                            __OR__,
                                                            "{", "sx_function_list", ";", "}",  __MAKE__,YCODE.DO_NEW, 1,
                                                     };

        public static object[] sx_function_list   =  { "sx_function_list",
                                                       1014, "sx_function", ";", "sx_function",      __MAKE__,YCODE.DO_NEW, 0, 2,
                                                            __OR__,
                                                            "sx_function_list", ";", "sx_function" ,__MAKE__,YCODE.DO_ADD, 0, 2
                                                     };

        public static object[] sx_function        =  { "sx_function",
                                                       1015,
                                                            STR,"(",")",                 __MAKE__,YCODE.DO_NEW, 0,
                                                            __OR__,
                                                            STR,"(","sx_param",")",        __MAKE__,YCODE.DO_NEW, 0, 2,
                                                            __OR__,
                                                            STR,"(","sx_param_list",")",   __MAKE__,YCODE.DO_NEW, 0, 2,
                                                     };

        public static object[] sx_param_list      =  { "sx_param_list",
                                                       1017,
                                                            "sx_param",",", "sx_param",       __MAKE__,YCODE.DO_NEW,0,2,
                                                            __OR__,
                                                            "sx_param_list", ",", "sx_param", __MAKE__,YCODE.DO_NEW,0,2,
                                                     };

        public static object[] sx_param           =  { "sx_param",
                                                       1018,
                                                            QSTR,    __MAKE__,YCODE.DO_NEW,0,
                                                            __OR__,  
                                                            NUM,     __MAKE__,YCODE.DO_NEW,0,
                                                     };
       
        public static object[] sx_pos             =  { "sx_pos",
                                                       1020,
                                                            "(",NUM,",",NUM, ")",                   __MAKE__,YCODE.DO_NEW,1,3,
                                                            __OR__,
                                                            "(",NUM,",",NUM,",",NUM, ")",           __MAKE__,YCODE.DO_NEW,1,3,5,
                                                            __OR__,
                                                            "(",QSTR,",", NUM,",",NUM, ")",         __MAKE__,YCODE.DO_NEW,1,3,5,
                                                            __OR__,
                                                            "(", QSTR,",", NUM,",",NUM,",",NUM,")", __MAKE__,YCODE.DO_NEW,1,3,5,7,
                                                     };
    }
}