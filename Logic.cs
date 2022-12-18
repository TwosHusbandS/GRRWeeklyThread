using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditMegaThread
{
    internal class Logic
    {
        public static void Run()
        {
            // Initiate Logger
            Helper.Logger.Init();

            // Read Settings
            Helper.Logger.Log("Reading Settings...");
            Settings.ReadSettings();
            Helper.Logger.Log("Reading Settings...DONE");

            // Interpret our Command Line Arguments
            Helper.Logger.Log("Interpreting Command Line Args...");
            CommandLineArgs.Interpret();
            Helper.Logger.Log("Interpreting Command Line Args...DONE");

            // If OPTIONS.DATE is not set via Command Line Args
            // - if today is friday
            // - - take today
            // - if not
            // - - take last friday
            Helper.Logger.Log("Working some magic regarding Options.DATE...");
            if (String.IsNullOrEmpty(Options.DATE))
            {
                if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
                {
                    Options.DATE = DateTime.Today.ToString("dd.MM.yyyy");
                }
                else
                {
                    // Retract 1 day from "today" until it reaches a friday
                    DateTime LastFriday = DateTime.Today;
                    while (true)
                    {
                        if (LastFriday.DayOfWeek == DayOfWeek.Friday)
                        {
                            break;
                        }
                        LastFriday = LastFriday.AddDays(-1);
                    }
                    Options.DATE = LastFriday.ToString("dd.MM.yyyy");
                }
            }
            Helper.Logger.Log("Working some magic regarding Options.DATE...DONE");

            // We have all Options, Command Lines etc...lets init the reddit client
            Helper.Logger.Log("Initiating the Reddit Client itself...");
            Reddit.RedditClient MyRC = new Reddit.RedditClient(appId: Options.REDDIT_CLIENT_ID, appSecret: Options.REDDIT_CLIENT_SECRET, refreshToken: Options.REDDIT_REFRESH_TOKEN);
            Helper.Logger.Log("Initiating the Reddit Client itself...DONE");

            // Grabing the correct ReleasePost Link
            Helper.Logger.Log("Searching for the correstping ReleasePost Link...");
            string CorrespondingReleasePostLink = SearchForCorrespondingReleasePost.GetLink(MyRC);
            Helper.Logger.Log("Searching for the correstping ReleasePost Link...DONE");

            // Replacing our %PLACEHOLDER% for Date and ReleasePostLink in Title and Body
            Helper.Logger.Log("Replacing our %PLACEHOLDER% for Date and ReleasePostLink in Title and Body...");
            Options.REDDIT_POST_TITLE = Options.REDDIT_POST_TITLE.Replace("%CURRENT_DATE%", Options.DATE);
            Options.REDDIT_POST_TITLE = Options.REDDIT_POST_TITLE.Replace("%CURRENT_DATE%", Options.DATE);
            Options.REDDIT_POST_BODY = Options.REDDIT_POST_BODY.Replace("%RELEASE_LISTE_LINK%", CorrespondingReleasePostLink);
            Options.REDDIT_POST_BODY = Options.REDDIT_POST_BODY.Replace("%RELEASE_LISTE_LINK%", CorrespondingReleasePostLink);
            Helper.Logger.Log("Replacing our %PLACEHOLDER% for Date and ReleasePostLink in Title and Body...DONE");

            // Actually Posting our Post
            Helper.Logger.Log("Posting...");
            Post.PostPost(MyRC);
            Helper.Logger.Log("Posting...DONE");
        }
    }
}
