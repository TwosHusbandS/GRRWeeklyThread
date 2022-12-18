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
            // Get all Command Line Args
            string[] CommandLineArgs = Environment.GetCommandLineArgs();

            string cmdline = String.Join(" ", CommandLineArgs);
            Helper.Logger.Log("---- CommandLineArgs: '" + cmdline + "'");

            // Loop through them, skip the first one since its just the name of the file itself
            for (int i = 1; i <= CommandLineArgs.Length - 1; i++)
            {
                // strip all of them from "/" and "-"

                // If dryrun, set Options.DRYRUN to true (default false)
                if (CommandLineArgs[i].TrimStart('-').TrimStart('/').ToUpper() == "DRYRUN")
                {
                    Helper.Logger.Log("---- Dryrun detected, setting Options.DRYRUN = true");
                    Options.DRYRUN = true;
                }

                // If skipconfirmation, set Options.SKIPCONFIRMATION to true (default false)
                if (CommandLineArgs[i].TrimStart('-').TrimStart('/').ToUpper() == "SKIPCONFIRMATION")
                {
                    Helper.Logger.Log("---- skipconformation detected, setting Options.SKIPCONFIRMATION = true");
                    Options.SKIPCONFIRMATION = true;
                }

                // If command lines containes DATE
                if (CommandLineArgs[i].TrimStart('-').TrimStart('/').ToUpper() == "DATE")
                {
                    Helper.Logger.Log("---- custom date detected, lets see");

                    // Check if we have an argument after it (the actual date)
                    if (i < CommandLineArgs.Length - 1)
                    {
                        Helper.Logger.Log("---- argument after custom date exist, its: '" + CommandLineArgs[i + 1] + "'");

                        // Read the argument after, make sure it matches regex
                        string Input = CommandLineArgs[i + 1];
                        string RegexFormat = "^[0-9]{2}.[0-9]{2}.20[0-9]{2}$"; // DD.MM.YYYY
                        if (Regex.IsMatch(Input, RegexFormat))
                        {
                            // Set custom date
                            Helper.Logger.Log("---- argument after custom date exist, and matches regex. Setting custom date.");
                            Options.DATE = Input;
                        }
                        else
                        {
                            Helper.Logger.Log("---- argument after custom date exist, but DOES NOT regex. NOT setting custom date.");
                        }
                    }
                    else
                    {
                        Helper.Logger.Log("---- argument after custom date doesnt exist, exiting");
                    }
                }
            }
        }
    }
}
