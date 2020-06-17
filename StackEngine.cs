using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using SymbolTable;

namespace StackEngine
{
    public partial class Engine
    {
        public static SymTab SYMTAB;
        public static Stack<Symbol> DATA = new Stack<Symbol>();
        public static Stack<Symbol> RETURN = new Stack<Symbol>();
        public static Dictionary<string, object> opcodeDict;

        public Engine()
        {
            opcodeDict = new Dictionary<string, object>()
            {
                {"DROP",(Action)DROP},
                {"NIP", (Action)NIP},
                {"DUP", (Action)DUPE},
                {"OVER",(Action)OVER},
                {"TUCK",(Action)TUCK},
                {"SWAP",(Action)SWAP},
                {"PICK",(Action)PICK}, //not implemented

                {"ROT", (Action)ROTE},
                {"-ROT",(Action)NOT_ROTE},
                {"?DUP",(Action)QUESTION_DUPE },
                {"ROLL",(Action)ROLL},
                {"2DROP",(Action)TWO_DROP},
                {"2NIP",(Action)TWO_NIP},
                {"2DUP",(Action)TWO_DUPE},
                {"2OVER",(Action)TWO_OVER},
                {"2TUCK",(Action)TWO_TUCK},
                {"2SWAP",(Action)TWO_SWAP},
                {"2ROT",(Action)TWO_ROT},

                {"!", (Action)BANGSIGN},
                {"@", (Action)ATSIGN},
                {".", (Action)DOT},
                {"DEF", (Action)DEF},
                {"CHR", (Action)CHR},
                {"END", (Action)END },


                {"ASNUM", (Action)ASNUM },
                {"ASTRING",(Action)ASSTRING},
                {"@DICT",(Action)ATDICT },
                {"ARITHOP", (Action)ARITHOP }

            };
        }

        public Engine Parse(string script)
        {
            var tokens = Regex.Matches(script, @"[\""].+?[\""]|['].+?[']|\S+", RegexOptions.Singleline).Cast<Match>().Select(m => m.Value).ToList();
            foreach (var token in tokens)
            {

                //if (int.TryParse(token, out int intVal))
                //{
                //    DATA.Push(SYMTAB.CreateSymbol(SymbolTable.SymbolType.INTEGER, intVal));
                //    continue;
                //}
                //
                //if (double.TryParse(token, out double dblVal))
                //{
                //    DATA.Push(SYMTAB.CreateSymbol(SymbolTable.SymbolType.DOUBLE, dblVal));
                //    continue;
                //}
                //
                //if (bool.TryParse(token, out bool boolVal))
                //{
                //    DATA.Push(SYMTAB.CreateSymbol(SymbolTable.SymbolType.BOOLEAN, boolVal));
                //    continue;
                //}

                if (opcodeDict.ContainsKey(token))
                {
                    ((Action)opcodeDict[token])();
                }
                else if (SYMTAB.ExistsEntry(token))
                {
                    SymtabEvaluate(token);
                }
                else
                {
                    var tempToken = token;
                    if (tempToken.Substring(0, 1) == "\"" && tempToken.Substring(tempToken.Length - 1) == "\"")
                    {
                        tempToken = token.Substring(1, tempToken.Length - 2);
                    }
                    DATA.Push(SYMTAB.CreateSymbol(SymbolTable.SymbolType.STRING, tempToken));

                }

            }
            return this;
        }

        public void SetSymTab(ref SymTab st)
        {
            SYMTAB = st;
        }

        public Engine SymtabEvaluate(string token)
        {
            if (!SYMTAB.ExistsEntry(token))
            {
                throw new Exception($"{token}??");
            }
            else
            {
                if (SYMTAB.RetrieveEntry(token).Type == SymbolType.ACTION)
                    ((Action)SYMTAB.RetrieveEntry(token).Value)();
                else
                { // if it's a value, just recover it. But how do you tell?
                    Parse(SYMTAB.RetrieveEntry(token).Value.ToString());
                }
            }
            return this;
        }

        public void Needing(SymbolType[] types)
        {
            if (types.Length < DATA.Count)
            {
                throw new Exception($"Operation requires {types.Length} parameters but only {DATA.Count} provided!");
            }
            for (var e = 0; e < DATA.Count; e++)
            {
                if (DATA.ElementAt(e).Type != types[e])
                {
                    throw new Exception($"Stack element {e} requires {types[e]}!");
                }
            }
        }

        public Symbol Pop()
        {
            return DATA.Pop();
        }

        public void Push(Symbol s)
        {
            DATA.Push(s);
        }

        public int Count()
        {
            return DATA.Count;
        }

        public Boolean Needs(int n)
        {
            return this.Count() >= n;
        }

        public Engine AddOpcode(string name, Action action)
        {
            opcodeDict.Add(name, action);
            return this;
        }
    }
}
