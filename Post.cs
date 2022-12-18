using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reddit.Controllers;

namespace RedditMegaThread
{
    internal class Post
    {
        public static void PostPost(Reddit.RedditClient MyRC)
        {
            // Get Subreddit of the one we are trying to post in:
            Subreddit SR = MyRC.Subreddit(Options.SUBREDDIT).About();

            // If we skip conformation
            if (Options.SKIPCONFIRMATION)
            {
                Helper.Logger.Log("---- SkipConfirmation, not asking user.");
            }
            // If we do NOT skip conformation
            else
            {
                Helper.Logger.Log("---- NO SkipConfirmation, I am asking user.");
                Console.WriteLine("Do you want to Post?");
                Console.WriteLine("Post is in: '{0}'", SR.Name);
                Console.WriteLine("Title is: '{0}'", Options.REDDIT_POST_TITLE);
                if (Options.DRYRUN)
                {
                    Console.WriteLine("ATTENTION: ITS A DRY RUN AND WILL BE POSTED TO FILE!");
                }
                Console.Write("Post (Y/N, default N): ");
                string input = Console.ReadLine();
                Helper.Logger.Log("---- NO SkipConfirmation, users answer was: '" + input + "'");
                if (String.IsNullOrEmpty(input) || input.ToUpper() != "Y")
                {
                    Helper.Logger.Log("---- NO SkipConfirmation, users answer was either null, empty, or not 'Y'");
                    return;
                }
            }

            // If its a dryrun and we write to file
            if (Options.DRYRUN)
            {
                Helper.Logger.Log("---- DryRun. Gathering info and writing to file");

                // Add all infos to a string list
                List<string> output = new List<string>();
                output.Add("Subreddit: '" + Options.SUBREDDIT + "'");
                output.Add("Distinguised: '" + Options.DISTINGUISH + "'");
                output.Add("Sticky: '" + Options.STICKY + "'");
                output.Add("Title: '" + Options.REDDIT_POST_TITLE + "'");
                output.Add("Body starting in new Line:");
                output.Add(Options.REDDIT_POST_BODY);

                // Generate Filename
                string Filename = Path.Combine(Options.ProjectInstallationPath.TrimEnd(Path.DirectorySeparatorChar), @"DryRun_" + Options.DATE + ".txt");

                // Write to File
                Helper.FileHandling.WriteStringToFileOverwrite(Filename, output.ToArray());
            }   
            else
            {
                Helper.Logger.Log("---- Not a dryrun. Actually posting...");

                // Submitting the actual post.
                SelfPost SP = SR.SelfPost(Options.REDDIT_POST_TITLE, Options.REDDIT_POST_BODY).Submit();
            
                if (Options.DISTINGUISH)
                {
                    Helper.Logger.Log("---- We want to distinguish...");
                    SP.Distinguish("yes");
                }
                if (Options.STICKY)
                {
                    Helper.Logger.Log("---- We want to sticky...");
                    SP.SetSubredditSticky(-1, false);
                }
            }
        }
    }
}
