using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;

namespace Lab2
{
    public class FinalStateMachine
    {
        public string ResultLexeme { get; set; }

        public List<string> IdentificateTable { get; set; }

        public List<string> ConstantTable { get; set; }

        private void IfMachine(string input)
        {
            if (input == "if")
            {
                ResultLexeme += "(7)";
            }
        }

        private void ForMachine(string input)
        {
            if (input == "for")
            {
                ResultLexeme += "(6)";
            }
        }

        private void WhileMachine(string input)
        {
            if (input == "while")
            {
                ResultLexeme += "(8)";
            }
        }
        
        private void OperationsMachine(string input)
        {
            if (input.Length == 1 & "=+-*/".Contains(input))
            {
                ResultLexeme += "(3," + input + ")";
            }
        }

        private void ComparisonsMachine(string input)
        {
            switch (input)
            {
                case ">":
                    ResultLexeme += "(4,1)";
                    break;
                case "<":
                    ResultLexeme += "(4,2)";
                    break;
                case ">=":
                    ResultLexeme += "(4,3)";
                    break;
                case "<=":
                    ResultLexeme += "(4,4)";
                    break;
                case "!=":
                    ResultLexeme += "(4,5)";
                    break;
                case "==":
                    ResultLexeme += "(4,6)";
                    break;
            }
        }

        private void BracesMachine(string input)
        {
            if (input.Length == 1 & "(){}[]".Contains(input))
            {
                ResultLexeme += "(5," + input + ")";
            }
        }

        private void ConstantMachine(string input)
        {
            if (input == "for" || input == "if" || input == "while" || input == "foreach" || "()[]{}".Contains(input)) { return; }
            if ("0123456789".Contains(input[0].ToString()))
            {
                if (!ConstantTable.Contains(input))
                {
                    ConstantTable.Add(input);
                }
                ResultLexeme += "(2," + ConstantTable.IndexOf(input) + ")";
            }
        }

        private void VariableMachine(string input)
        {
            if (input == "for" || input == "if" || input == "while" || input == "foreach") { return; }
            bool check = input.Any(x => !char.IsLetter(x));
            if (input[0].ToString() == "_") { check = true; }
            if (!check)
            {
                if (!IdentificateTable.Contains(input))
                {
                    IdentificateTable.Add(input);
                }
                ResultLexeme += "(1," + IdentificateTable.IndexOf(input) + ")";
            }
        }

        public string ShowIdentificateTable()
        {
            string res = "";
            for (int i = 0; i < IdentificateTable.Count; i++)
            {
                res += "№: " + i + ", Identifier: " + IdentificateTable[i] + "\n";
            }
            return res;
        }

        public string ShowConstantTable()
        {
            string res = "";
            for (int i = 0; i < ConstantTable.Count; i++)
            {
                res += "№: " + i + ", Constant: " + ConstantTable[i] + "\n";
            }
            foreach (var VARIABLE in res)
            {
                Console.Write(VARIABLE.ToString());
            }
            return res;
        }

        public string ToLexeme(string input)
        {
            string[] temp = InputHandler.GetSplitedString(input);
            ResultLexeme = "";
            ConstantTable = new List<string>();
            IdentificateTable = new List<string>();
            for (int i = 0; i < temp.Length; i++)
            {
                BracesMachine(temp[i]);
                ComparisonsMachine(temp[i]);
                ConstantMachine(temp[i]);
                ForMachine(temp[i]);
                IfMachine(temp[i]);
                OperationsMachine(temp[i]);
                VariableMachine(temp[i]);
                WhileMachine(temp[i]);
            }
            return ResultLexeme;
        }
    }
}
