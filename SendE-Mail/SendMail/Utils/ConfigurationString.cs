using System;
using System.Collections.Generic;
using System.Text;

namespace SendMail.Utils
{
    class ConfigurationString
    {
        //change the input string substitute a substring with another string
        public static string changeString(string initial, string from = "{nome}", string change="")
        {
            string result = "";
            for (int i = 0; i < initial.Length; i++)
            {
                if ((initial[i] == from[0]) && (i + from.Length < initial.Length))
                    for (int x = i, y = 0; y < from.Length; x++, y++)
                    {
                        if (initial[x] != from[y]) break;
                        if (y == from.Length - 1)
                        {
                            result += change;
                            i = i + from.Length;
                        }
                    }
                result += initial[i];
            }
            return result;
        }

        //change the input string substitute a substring with another string
        public static string changeString2(string initial, string from = "{nome}", string change = "")
        {
            return initial.Replace(from, change);
        }
    }
}
