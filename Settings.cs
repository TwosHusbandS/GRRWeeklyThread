using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditMegaThread
{
    internal class Settings
    {
        public static void ReadSettings()
        {
            string configFileContent = Helper.FileHandling.ReadContentOfFile(Options.Configfile);
            string bodyFileContent = Helper.FileHandling.ReadContentOfFile(Options.Bodyfile);

            Options.REDDIT_CLIENT_ID = Helper.FileHandling.GetXMLTagContent(configFileContent, "REDDIT_CLIENT_ID");
            Options.REDDIT_CLIENT_SECRET = Helper.FileHandling.GetXMLTagContent(configFileContent, "REDDIT_CLIENT_SECRET");
            Options.REDDIT_ACCESS_TOKEN = Helper.FileHandling.GetXMLTagContent(configFileContent, "REDDIT_ACCESS_TOKEN");
            Options.REDDIT_REFRESH_TOKEN = Helper.FileHandling.GetXMLTagContent(configFileContent, "REDDIT_REFRESH_TOKEN");
            Options.SUBREDDIT = Helper.FileHandling.GetXMLTagContent(configFileContent, "SUBREDDIT");
            Options.REDDIT_POST_TITLE = Helper.FileHandling.GetXMLTagContent(configFileContent, "TITLE");
            Options.REDDIT_POST_BODY = bodyFileContent;
            if (Helper.FileHandling.GetXMLTagContent(configFileContent, "DISTINGUISH").ToUpper() == "FALSE")
            {
                Options.DISTINGUISH = false;
            }
            else
            {
                Options.DISTINGUISH = true;
            }
            if (Helper.FileHandling.GetXMLTagContent(configFileContent, "STICKY").ToUpper() == "TRUE")
            {
                Options.STICKY = true;
            }
            else
            {
                Options.STICKY = false;
            }
        }
    }
}
