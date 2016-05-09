using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
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
        private string[] keywords = {"for", "while", "if"};
        private bool isInValidVariable = false;
        private bool isInvalidConstant = false;

<<<<<<< HEAD
        public string StackState;
        public string TokenState;

=======
>>>>>>> 963657a4fc2ddb92699bb478f7d48243d86f9d2f
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
                        MessageBox.Show("ERROR ::: lexical error at position " + Index);
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
                    MessageBox.Show("ERROR ::: lexical error at position " + Index);
                    IdentifiersTable.Clear();
                    ConstantsTable.Clear();
                    Tokens = "";
                    break;
                }
            }
            return Tokens;
        }

        public bool SyntaxAnalyzer(String tokensString)
        {
            if (tokensString == "")
                return false;

            List<String> stack = new List<String>() { "CODE", "$" }; // $ - a symbol of the end of the file
            String[] buffer = tokensString.Split(new String[] { ")(" }, StringSplitOptions.None);
            for (int i = 1; i < buffer.Length; i++)
            {
                buffer[i] = "(" + buffer[i];
            }
            List<String> tokens = new List<String>(buffer);
            tokens.Add("$");

            List<String> nonterminals = new List<String>() {
                "CODE", "STATEMENT", "BARE", "BARE1", "OBJECT", "COMPLEX", "IDENTIFIER", "INDEX", "FOR_STATEMENT", "BOOL_EXP",
                "WHILE_STATEMENT", "FOREACH_STATEMENT", "IF_STATEMENT"
            };

            List<String> terminals = new List<String>() {
                "(1", "(2", "(3,=", "(3", "(4", "(5,(", "(5,)", "(5,[", "(5,]", "(5,{", "(5,}", "(6", "(7", "(8", "(9", "$"
            };

            List<String>[ , ] table = new List<String>[nonterminals.Count, terminals.Count];

            /* CODE   =>   STATEMENT   CODE    |   E
               STATEMENT   =>   BARE   |   COMPLEX
               BARE   =>   IDENTIFIER    =    OBJECT   BARE’   ; 
               BARE’    =>   operation  OBJECT   BARE’   |   E
               OBJECT   =>   constant   |   IDENTIFIER     
<<<<<<< HEAD
               COMPLEX   =>   FOR_STATEMENT   |   WHILE_STATEMENT   |   IF_STATEMENT
               IDENTIFIER   =>   varuable   INDEX
               INDEX   =>   [   OBJECT   BARE’  |   E   ] 
=======
               COMPLEX   =>   FOR_STATEMENT   |   WHILE_STATEMENT   |   FOREACH_STATEMENT   |   IF_STATEMENT
               IDENTIFIER   =>   varuable   INDEX
               INDEX   =>   [   OBJECT   BARE’   ] 
>>>>>>> 963657a4fc2ddb92699bb478f7d48243d86f9d2f
               FOR_STATEMENT   =>   for   (   BARE ?   ;   BOOL_EXP ?   ;   BARE ?   )   {   CODE   }
               WHILE_STATEMENT   =>   while   (   BOOL_EXP ?   )   {   CODE   }
               IF_STATEMENT   =>   if   (   BOOL_EXP ?   )   {   CODE   }
               BOOL_EXP   =>   OBJECT   BARE’   comparison   OBJECT   BARE’          */

            table[0, 0] = new List<String>() { "STATEMENT", "CODE" };
            table[0, 12] = table[0, 0];
            table[0, 13] = table[0, 0];
            table[0, 14] = table[0, 0];
            table[1, 0] = new List<String>() { "BARE", "(6" };
            table[1, 12] = new List<String>() { "COMPLEX" };
            table[1, 13] = table[1, 12];
            table[1, 14] = table[1, 12];
            table[2, 0] = new List<String>() { "IDENTIFIER", "(3,=", "OBJECT", "BARE1" };
            table[3, 3] = new List<String>() { "(3", "OBJECT", "BARE1" };
            table[4, 0] = new List<String>() { "IDENTIFIER" };
            table[4, 1] = new List<String>() { "(2" };
            table[5, 12] = new List<String>() { "FOR_STATEMENT" };
            table[5, 13] = new List<String>() { "WHILE_STATEMENT" };
            table[5, 14] = new List<String>() { "IF_STATEMENT" };
            table[6, 0] = new List<String>() { "(1", "INDEX" };
            table[7, 7] = new List<String>() { "(5,[", "OBJECT", "BARE1", "(5,]" };
            table[8, 12] = new List<String>() { "(7", "(5,(", "BARE", "(6", "BOOL_EXP", "(6", "BARE", "(5,)", "(5,{", "CODE", "(5,}" };
            table[9, 0] = new List<String>() { "OBJECT", "BARE1", "(4", "OBJECT", "BARE1" };
            table[10, 13] = new List<String>() { "(8", "(5,(", "BOOL_EXP", "(5,)", "(5,{", "CODE", "(5,}" };
            table[11, 14] = new List<String>() { "(9", "(5,(", "BOOL_EXP", "(5,)", "(5,{", "CODE", "(5,}" };

            List<String> emptyStrings = new List<String>() { "CODE", "BARE1", "INDEX" };

            while (!tokens[0].StartsWith(stack[0]) || stack.Count != 1 || tokens.Count != 1)
            {
                if (tokens[0].StartsWith(stack[0]) || tokens[0] == stack[0])
                {
                    tokens.RemoveAt(0);
                    stack.RemoveAt(0);
                    continue;
                }
                else if (nonterminals.IndexOf(stack[0]) == -1)
                {
                    return false;
                }
                String nonterminal = stack[0];
                stack.RemoveAt(0);
                int terminalIndex = -1;
                for (int i = 0; i < terminals.Count; i++)
                {
                    if (tokens[0].StartsWith(terminals[i]))
                    {
                        terminalIndex = i;
                        break;
                    }
                }
                List<String> follow = table[nonterminals.IndexOf(nonterminal), terminalIndex];
                if (follow == null)
                {
                    if (!emptyStrings.Contains(nonterminal))
                    {
                        return false;
                    }
                }
                else
                {
                    follow = follow.ToList();
                    follow.AddRange(stack);
                    stack = follow.ToList();
                }

                //display the stack and tokens after each step
<<<<<<< HEAD
                foreach (String s in stack)
                {
                    this.StackState += s + ", ";
                }
                this.StackState += "\n\n\n";
                foreach (String s in tokens)
                {
                    this.Tokens += s + ", ";
                }
                this.Tokens += "\n\n\n";
=======
                System.Console.WriteLine("\nStack:");
                foreach (String s in stack)
                {
                    System.Console.Write(s + ", ");
                }
                System.Console.WriteLine("\n\nTokens:");
                foreach (String s in tokens)
                {
                    System.Console.Write(s + ", ");
                }
                System.Console.WriteLine();
>>>>>>> 963657a4fc2ddb92699bb478f7d48243d86f9d2f
            }
            return true;
        }

<<<<<<< HEAD
        public string Launch(string input)
        {
            var tempText = this.LexicalAnalyzer(input);
            if (tempText == "" || tempText == null || tempText == " ")
            {
                return "Token string is NOT VALID";
            }
            if (this.SyntaxAnalyzer(tempText))
            {
                return "Token string is VALID";
            }
            return "Token string is NOT VALID";
        }

=======
>>>>>>> 963657a4fc2ddb92699bb478f7d48243d86f9d2f
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
