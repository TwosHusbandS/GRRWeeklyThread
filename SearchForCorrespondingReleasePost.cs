using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reddit;
using Reddit.Controllers;

namespace RedditMegaThread
{
    internal class SearchForCorrespondingReleasePost
    {
        public static string GetLink(Reddit.RedditClient MyRC)
        {
            // Always get GRR Subreddit here, since we always want to search for the release post in the GRR Subreddit
            Subreddit GRR = MyRC.Subreddit("GermanRap").About();

            // Building our search string
            string SearchString = "Die Releases am " + Options.DATE;
            Helper.Logger.Log("---- searchstring: '" + SearchString + "'");

            // Building our return string
            string CorrespondingReleasePostLink = "";

            // If subreddit is not null for some reason...
            if (GRR != null)
            {
                // Search GRR with searchstring by relevance, save in List of Posts
                List<Reddit.Controllers.Post> ListOfPosts = GRR.Search(q: SearchString, sort: "relevance");

                if (ListOfPosts.Count == 0)
                {
                    Helper.Logger.Log("---- search returned nothing.");
                }

                // Loop through List of Posts
                foreach (Reddit.Controllers.Post MyPost in ListOfPosts)
                {
                    // If Post title atually contains our full search string
                    if (MyPost.Title.ToUpper().Contains(SearchString.ToUpper())) 
                    {
                        // Set it as Post, break out of loop
                        CorrespondingReleasePostLink = MyPost.Permalink;
                        break;
                    }
                }

                // Make sure our return ist null or empty
                if (!String.IsNullOrEmpty(CorrespondingReleasePostLink))
                {
                    // return correct link
                    return CorrespondingReleasePostLink;
                }
                else
                {
                    Helper.Logger.Log("---- search returned no posts manually matching our searchstring.");
                    // just log, we return at the very bottom
                }
            }

            // If we dont find a good link, for whatever reason, return rickroll
            Helper.Logger.Log("---- return rickroll.");
            return @"https://www.youtube.com/watch?v=dQw4w9WgXcQ";
        }
    }
}
