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
            Settings.ReadSettings();
            CommandLineArgs.Interpret();

            if (String.IsNullOrEmpty(Options.DATE))
            {
                if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
                {
                    Options.DATE = DateTime.Today.ToString("dd.MM.yyyy");
                }
                else
                {
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

            // We have all Options, everything, lets go

            Reddit.RedditClient MyRC = new Reddit.RedditClient(appId: Options.REDDIT_CLIENT_ID, appSecret: Options.REDDIT_CLIENT_SECRET, refreshToken: Options.REDDIT_REFRESH_TOKEN);

            string CorrespondingReleasePostLink = SearchForCorrespondingReleasePost.GetLink(MyRC);

            Options.REDDIT_POST_TITLE = Options.REDDIT_POST_TITLE.Replace("%CURRENT_DATE%", Options.DATE);
            Options.REDDIT_POST_TITLE = Options.REDDIT_POST_TITLE.Replace("%CURRENT_DATE%", Options.DATE);
            Options.REDDIT_POST_BODY = Options.REDDIT_POST_BODY.Replace("%RELEASE_LISTE_LINK%", CorrespondingReleasePostLink);
            Options.REDDIT_POST_BODY = Options.REDDIT_POST_BODY.Replace("%RELEASE_LISTE_LINK%", CorrespondingReleasePostLink);

            Post.PostPost(MyRC);
        }
    }
}
