using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab2
{
    public static class InputHandler
    {
        public static string[] GetSplitedString(string input)
        {
            string[] result = input.Split(new char[] {' '});
            List<string> temp = new List<string>();
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i].Length > 1 & result[i].Contains("+") || result[i].Contains("-") || result[i].Contains(";"))
                {
                    if (result[i].Contains("++"))
                    {
                        foreach (var VARIABLE in result[i].Split(new char[] {'+'}))
                        {
                            temp.Add(VARIABLE);
                        }
                        temp.Add("+");
                        temp.Add("+");
                    }
                    else if (result[i].Contains("+"))
                    {
                        foreach (var VARIABLE in result[i].Split(new char[] { '+' }))
                        {
                            temp.Add(VARIABLE);
                        }
                        temp.Add("+");
                    }
                    else if (result[i].Contains("-"))
                    {
                        foreach (var VARIABLE in result[i].Split(new char[] {'-'}))
                        {
                            temp.Add(VARIABLE);
                        }
                        temp.Add("-");
                    }
                    else if (result[i].Contains(";"))
                    {
                        foreach (var VARIABLE in result[i].Split(new char[] {';'}))
                        {
                            temp.Add(VARIABLE);
                        }
                        temp.Add(";");
                    }
                }
                else
                {
                    temp.Add(result[i]);
                }
            }
            temp = temp.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            return temp.ToArray();
        }

        public static string ShowStringArray(string[] input)
        {
            string res = "";
            foreach (var VARIABLE in input)
            {
                //Console.Write(VARIABLE + " | ");
                res += VARIABLE + "\n";
            }
            return res;
        }
    }
}
