using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace e621PoolDownloader {
    public static class Utils {
        struct RegexThing {
            public Regex exp;
            public bool lit;
            public string var;
            public RegexThing (string expStr, string varName, bool notVar = false) {
                exp = new Regex (expStr, RegexOptions.ECMAScript);
                lit = notVar;
                var = varName;
            }
        }
        class VariableHolder {
            private object obj;
            private Type type;
            public string name;

            public dynamic ToVar () { return Convert.ChangeType (obj, type); }

            public VariableHolder (object newObj, Type newType, string newName) {
                this.obj = newObj; this.type = newType; this.name = newName;
            }
            public static VariableHolder MakeVariableHolder<T> (T variable, string name) {
                return new VariableHolder ((object) variable, typeof (T), name);
            }
            public static List<VariableHolder> MakeHolderList (params Tuple<Type, object, string> [] vars) {
                List<VariableHolder> ret = new List<VariableHolder> ();
                foreach (var top in vars) {
                    var variable = Convert.ChangeType (top.Item2, top.Item1);
                    ret.Add (MakeVariableHolder<object> (variable, top.Item3));
                }
                return ret;
            }
        }
        static RegexThing [] regexes = {
            new RegexThing ("(?<!\\$)\\$f", "filename"),
            new RegexThing ("(?<!\\$)\\$e", "extension"),
            new RegexThing ("(?<!\\$)\\$i", "index"),
            new RegexThing ("(?<!\\$)\\$[^fei\\$]", " ", true),
            new RegexThing ("(?<!\\$)\\$\\$", "$", true),
        };
        public static string ParseFilenameMask (string mask, string filename, string extension, int index = -1) {
            List<VariableHolder> vars = new List<VariableHolder> {
                VariableHolder.MakeVariableHolder<string> (filename,  "filename"),
                VariableHolder.MakeVariableHolder<string> (extension, "extension"),
                VariableHolder.MakeVariableHolder<int>    (index,     "index"),
            };

            if (index == -1)
                index = 0;

            string ret = mask;
            foreach (RegexThing regexp in regexes) {
                var variable = regexp.lit ? regexp.var : vars.First (v => String.Compare (v.name, regexp.var) == 0).ToVar ();

                if ((variable.GetType () == typeof (string) && String.IsNullOrEmpty (variable)) || variable == null)
                    variable = "";
                var exp = regexp.exp;
                ret = exp.Replace (ret, variable.ToString ());
            }

            return ret;
        }
    }
}
