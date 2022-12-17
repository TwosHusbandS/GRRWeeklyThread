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
            // Get info on another subreddit.
            Subreddit GRR = MyRC.Subreddit(Options.SUBREDDIT).About();


            if (!Options.SKIPCONFIRMATION)
            {
                Console.WriteLine("Do you want to Post?");
                Console.WriteLine("Post is in: '{0}'", GRR.Fullname);
                Console.WriteLine("Title is: '{0}'", Options.REDDIT_POST_TITLE);
                if (Options.DRYRUN)
                {
                    Console.WriteLine("ATTENTION: ITS A DRY RUN AND WILL BE POSTED TO FILE!");
                }
                Console.Write("Post (Y/N, default N): ");
                string input = Console.ReadLine();
                if (String.IsNullOrEmpty(input) || input.ToUpper() != "Y")
                {
                    return;
                }
            }


            if (Options.DRYRUN)
            {

            }   
            else
            {
                // Get the top post from a subreddit.
                SelfPost SP = GRR.SelfPost(Options.REDDIT_POST_TITLE, Options.REDDIT_POST_BODY).Submit();

                if (Options.DISTINGUISH)
                {
                    SP.Distinguish("yes");
                }
                if (Options.STICKY)
                {
                    SP.SetSubredditSticky(-1, false);
                }
            }
        }
    }
}
