using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RedditMegaThread
{
    internal class CommandLineArgs
    {
        public static void Interpret()
        {
            string[] CommandLineArgs = Environment.GetCommandLineArgs();

            for (int i = 1; i <= CommandLineArgs.Length - 1; i++)
            {
                if (CommandLineArgs[i].TrimStart('-').TrimStart('/').ToUpper() == "DRYRUN")
                {
                    Options.DRYRUN = true;
                }
                if (CommandLineArgs[i].TrimStart('-').TrimStart('/').ToUpper() == "DRYRUN")
                {
                    Options.SKIPCONFIRMATION = true;
                }
                if (CommandLineArgs[i].TrimStart('-').TrimStart('/').ToUpper() == "DATE")
                {
                    if (i < CommandLineArgs.Length - 1)
                    {
                        string Input = CommandLineArgs[i + 1];
                        string RegexFormat = "^[0-9]{2}.[0-9]{2}.20[0-9]{2}$"; // DD.MM.YYYY
                        if (Regex.IsMatch(Input, RegexFormat))
                        {
                            Options.DATE = Input;
                        }
                    }
                }
            }
        }
    }
}
