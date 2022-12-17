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
            Subreddit GRR = MyRC.Subreddit("GermanRap").About();

            string SearchString = "Die Releases am " + Options.DATE;
            Reddit.Controllers.Post CorrestpondingReleasePost = null;

            if (GRR != null)
            {
                List<Reddit.Controllers.Post> ListOfPosts = GRR.Search(q: SearchString, sort: "relevance");

                foreach (Reddit.Controllers.Post MyPost in ListOfPosts)
                {
                    if (MyPost.Title.ToUpper() == SearchString.ToUpper()) 
                    {
                        CorrestpondingReleasePost = MyPost;
                        break;
                    }
                }

                if (CorrestpondingReleasePost != null)
                {
                    return CorrestpondingReleasePost.Permalink;
                }
            }

            return @"https://www.youtube.com/watch?v=dQw4w9WgXcQ";
        }
    }
}
