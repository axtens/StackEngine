using SymbolTable;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackEngine
{
    public partial class Engine
    {
        public void DUPE()
        {
            DataNeeds(1);
            DATA.Push(DATA.Peek());
            return;
        }
        public void DROP()
        {
            DataNeeds(1);
            DATA.Pop();
            return;
        }
        public void SWAP()
        {
            DataNeeds(2);
            var swap1 = DATA.Pop();
            var swap2 = DATA.Pop();
            DATA.Push(swap1);
            DATA.Push(swap2);
            return;
        }
        public void OVER()
        {
            DataNeeds(2);
            DATA.Push(DATA.ElementAt(1));
            return;
        }
        public void ROTE()
        {
            DataNeeds(3);
            var rot1 = DATA.Pop();
            var rot2 = DATA.Pop();
            var rot3 = DATA.Pop();
            DATA.Push(rot2);
            DATA.Push(rot1);
            DATA.Push(rot3);
            return;
        }
        public void NOT_ROTE()
        {
            DataNeeds(3);
            var rot1 = DATA.Pop();
            var rot2 = DATA.Pop();
            var rot3 = DATA.Pop();
            DATA.Push(rot1);
            DATA.Push(rot3);
            DATA.Push(rot2);
            return;
        }
        public void NIP()
        {
            SWAP();
            DROP();
            return;
        }
        public void TUCK()
        {
            SWAP();
            OVER();
            return;
        }

        private void TWO_ROT()
        {
            throw new NotImplementedException();
        }

        private void TWO_SWAP()
        {
            throw new NotImplementedException();
        }

        private void TWO_TUCK()
        {
            throw new NotImplementedException();
        }

        private void TWO_OVER()
        {
            throw new NotImplementedException();
        }

        private void TWO_DUPE()
        {
            throw new NotImplementedException();
        }

        private void TWO_NIP()
        {
            throw new NotImplementedException();
        }

        private void TWO_DROP()
        {
            throw new NotImplementedException();
        }

        private void ROLL()
        {
            throw new NotImplementedException();
        }

        private void QUESTION_DUPE()
        {
            throw new NotImplementedException();
        }

        private void PICK()
        {
            throw new NotImplementedException();
        }

        private void ARITHOP()
        {
            Needing(new SymbolType[] { SymbolType.STRING, SymbolType.DOUBLE, SymbolType.DOUBLE });
            var op = DATA.Pop();
            var lh = DATA.Pop();
            var rh = DATA.Pop();
            switch (op.Value.ToString())
            {
                case "+":
                    DATA.Push(SYMTAB.CreateSymbol(SymbolType.DOUBLE, (double)lh.Value + (double)rh.Value));
                    break;
                case "-":
                    DATA.Push(SYMTAB.CreateSymbol(SymbolType.DOUBLE, (double)lh.Value - (double)rh.Value));
                    break;
                case "*":
                    DATA.Push(SYMTAB.CreateSymbol(SymbolType.DOUBLE, (double)lh.Value * (double)rh.Value));
                    break;
                case "/":
                    DATA.Push(SYMTAB.CreateSymbol(SymbolType.DOUBLE, (double)lh.Value / (double)rh.Value));
                    break;
            }
        }

        private void ATDICT()
        {
            throw new NotImplementedException();
        }

        private void ASSTRING()
        {
            throw new NotImplementedException();
        }

        private void ASNUM()
        {
            DataNeeds(1);
            var val = DATA.Pop();
            DATA.Push(SYMTAB.CreateSymbol(SymbolType.DOUBLE, double.Parse(val.Value.ToString())));
        }

        private void END()
        {
            DataNeeds(1);
            var chr = DATA.Pop();
        }

        private void CHR()
        {
            DataNeeds(1);
            Check(0, SymbolType.DOUBLE);
            var chr = DATA.Pop();
            DATA.Push(SYMTAB.CreateSymbol(SymbolType.STRING, (char)((int)chr.Value)));
        }

        private void Check(int v, SymbolType t)
        {
            var elt = DATA.ElementAt(v);
            if (elt.Type != t)
            {
                throw new Exception($"Type error element {v}: needs {t}");
            }
        }

        private void DOT()
        {
            DataNeeds(1);
            Console.Write($"{DATA.Pop().Value}");
        }

        private void BANGSIGN()
        {
            DataNeeds(2);
            Check(0, SymbolType.STRING);
            SYMTAB.UpdateEntry(DATA.Pop().Value.ToString(), DATA.Pop());
        }

        private void ATSIGN()
        {
            DataNeeds(1);
            Check(0, SymbolType.STRING);
            var varname = DATA.Pop().Value.ToString();
            if (SYMTAB.ExistsEntry(varname))
            {
                var ent = SYMTAB.RetrieveEntry(varname);
                DATA.Push(ent);
            }
            else
            {
                throw new Exception($"No entry in symbol table called {varname}");
            }

        }

        public void DEF()
        {
            var defname = DATA.Pop().Value
                               .ToString();
            var defvalue = DATA.Pop().Value;
            SYMTAB.UpdateEntry(defname, SymbolType.STRING, defvalue);
        }


    }
}
