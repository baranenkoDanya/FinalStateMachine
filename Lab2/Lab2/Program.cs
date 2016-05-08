using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            program p = new program();
            String s = @"for (i = 1; i < 10; i = i + 1) 
{ 
_break_fast98 = 14.332e+123 / 134; 
AcceptButton[i + 1] = 123.5*i;
}";     
            String tokens = p.LexicalAnalyzer(s);
            bool isValid = p.SyntaxAnalyzer(tokens);
            System.Console.WriteLine(isValid);
            System.Console.Read();
        }
    }
    
 
}
