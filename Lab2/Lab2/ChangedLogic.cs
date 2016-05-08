using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Windows.Forms;

namespace Lab2 
{
    public class program 
    {
        private int index;
        private string code;
        private List<string> identifiersTable;
        private List<string> constantsTable;
        private string tokens;
        private string[] keywords = {"for", "if", "while", "foreach"};
        private bool isInValidVariable = false;
        private bool isInvalidConstant = false;

        public int Index
        {
            set { index = value; }
            get { return index; }
        }

        public List<string> IdentifiersTable
        {
            set { identifiersTable = value; }
            get { return identifiersTable; }
        }

        public List<string> ConstantsTable
        {
            set { constantsTable = value; }
            get { return constantsTable; }
        } 

        public string Tokens
        {
            set { tokens = value; }
            get { return tokens; }
        }

        private void ComparisonsMachine()
        {
            if (code.Length == Index + 1)
            {
                if (code[Index] == '>') tokens += "(4,1)";
                else if (code[Index] == '<') tokens += "(4,2)";
                Index += 1;
            } 
            if (code[Index] == '>' && code[Index+1] != '=') 
            {
                tokens += "(4,1)";
                Index += 1;
            }
            if (code[Index] == '<' && code[Index+1] != '=') 
            {
                tokens += "(4,2)";
                Index += 1;
            }
            if (code[Index] == '>' && code[Index+1] == '=') 
            {
                tokens += "(4,3)";
                Index += 2;
            }
            if (code[Index] == '<' && code[Index+1] == '=') 
            {
                tokens += "(4,4)";
                Index += 2;
            }
            if (code[Index] == '!' && code[Index+1] == '=') 
            {
                tokens += "(4,5)";
                Index += 2;
            } 
            if (code[Index] == '=' && code[Index+1] == '=') 
            {
                tokens += "(4,6)";
                Index += 2;
            }
        }

        private void OperationsMachine()
        {
            if ("+-*/".Contains(code[Index].ToString()))
            {
                tokens += "(3," + code[Index].ToString() + ")";
                Index += 1; 
            }
            else if (code.Length == Index + 1 && code[Index] == '=')
            {
                tokens += "(3,=)";
                Index += 1;
            }
            else if (code[Index] == '=' && code[Index+1] != '=')
            {
                tokens += "(3,=)";
                Index += 1;
            } 
        }

        private void BracketsMachine()
        {
            if ("{}[]()".Contains(code[Index].ToString())) 
                tokens += "(5," + code[Index++].ToString() + ")";
        }

        private void EndOfStatementMachine()
        {
            if (code[Index].ToString() == ";")
            {
                tokens += "(6)";
                Index++;
            }
        }
        
        private void IdentifiersMachine()
        {
            string varuable = "";
            int i = Index;
            while (Char.IsLetter(code, i) || Char.IsDigit(code, i) || code[i] == '_')
            {
                if (Char.IsDigit(code, i) && varuable[varuable.Length - 1] == '_' &&
                    varuable.Any(x => !char.IsLetter(x)))
                {
                    isInValidVariable = true;
                    return;
                }
                 varuable += code[i++]; 
            }
            bool hasLetter = false;
            for (int j = 0; j < code.Length; j++)
            {
                if (Char.IsLetter(varuable, j))
                {
                    hasLetter = true; break;
                }
            }
            if (!hasLetter) return;
            if (Array.IndexOf(keywords, varuable) != -1)
            {
                int key_index = Array.IndexOf(keywords, varuable);
                Tokens += "(" + (key_index + 7) + ")";
                Index += keywords[key_index].Length;
                return;
            }    
            if (IdentifiersTable.IndexOf(varuable) == -1)
                IdentifiersTable.Add(varuable);
            Index = i;
            tokens += "(1," + IdentifiersTable.IndexOf(varuable) + ")";     
        }

        private void ConstantsMachine()
        {
            string constant = "";
	        int i = Index;
            while (Char.IsDigit(code, i))
            {
                constant += code[i++].ToString();
            }
            if (i < code.Length && code[i] == '.') 
            {
                constant += code[i++].ToString();
                while (i < code.Length && Char.IsDigit(code, i))
                {
                    constant += code[i++].ToString();
                }
                if (i < code.Length - 1 && code[i] == 'e' && (Char.IsDigit(code, i+1) || code[i+1] == '+' || code[i+1]=='-'))
                {
                    constant += code[i++].ToString();
                    constant += code[i++].ToString();
                    while (i < code.Length && Char.IsDigit(code, i))
                    {
                        constant += code[i++].ToString();
                    }
                }
            }

            if (ConstantsTable.IndexOf(constant) == -1)
            {
                ConstantsTable.Add(constant);
            }
            Index = i;
            Tokens += "(2," + ConstantsTable.IndexOf(constant) + ")";
        }

        public string LexicalAnalyzer(string s)
        {
            
            code = s + " ";
            Tokens = "";
            Index = 0;
            IdentifiersTable = new List<string>();
            ConstantsTable = new List<string>();
            while (Index < code.Length)
            {
                if (code[Index] == ';')
                {
                    EndOfStatementMachine();
                }
                if ("=*/+-".Contains(code[Index]))
                {
                    OperationsMachine();
                }
                if ("><!=".Contains(code[Index]))
                {
                    ComparisonsMachine();
                }
                else if ("{}[]()".Contains(code[Index]))
                {
                    BracketsMachine();
                }
                else if (Char.IsLetter(code, Index) || code[Index] == '_')
                {
                    IdentifiersMachine();
                    if (isInValidVariable)
                    {
                        MessageBox.Show("ERROR:::invalid syntax at position " + Index);
                        IdentifiersTable.Clear();
                        ConstantsTable.Clear();
                        Tokens = "";
                        break;
                    }
                }
                else if (Char.IsDigit(code, Index))
                {
                    ConstantsMachine();
                }
                else if ("\r\n\t ".Contains(code[Index]))
                {
                    Index++;
                }
                else
                {
                    MessageBox.Show("ERROR:::invalid syntax at position " + Index);
                    IdentifiersTable.Clear();
                    ConstantsTable.Clear();
                    Tokens = "";
                    break;
                }
            }
            return Tokens;
        }

        public static void tMain(string[] args)
        {
            program p = new program();
            string s = "for (i = 1; i < 10; i = i + 1) {__break_fast89 =  14.32e+12+ 0012.14 >=   14342;  }";
            System.Console.WriteLine(p.LexicalAnalyzer(args[0]));
            System.Console.WriteLine();
            for (int i = 0; i < p.IdentifiersTable.Count; i++)
                System.Console.WriteLine(p.IdentifiersTable[i]);
            System.Console.WriteLine();
            for (int i = 0; i < p.ConstantsTable.Count; i++)
                System.Console.WriteLine(p.ConstantsTable[i]);
        }
    } 
}
