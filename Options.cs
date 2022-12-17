using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RedditMegaThread
{
    internal class Options
    {
        public static string REDDIT_CLIENT_ID = "";
        public static string REDDIT_CLIENT_SECRET = "";
        public static string REDDIT_ACCESS_TOKEN = "";
        public static string REDDIT_REFRESH_TOKEN = "";
        public static bool DISTINGUISH = true;
        public static bool STICKY = false;
        public static string SUBREDDIT = "";
        public static string REDDIT_POST_TITLE = "";
        public static string REDDIT_POST_BODY = "";

        public static bool DRYRUN = false;
        public static bool SKIPCONFIRMATION = false;
        public static string DATE = "";

        public static string Logfile { get; private set; } = ProjectInstallationPath.TrimEnd('/') + @"/logfile.log";
        public static string Configfile { get; private set; } = ProjectInstallationPath.TrimEnd('/') + @"/config.ini";
        public static string Bodyfile { get; private set; } = ProjectInstallationPath.TrimEnd('/') + @"/body.txt";

        public static string ProjectInstallationPath
        {
            get
            {
                return System.AppContext.BaseDirectory;
                //return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //return (Directory.GetParent(ProjectInstallationPathBinary).ToString());
            }
        }
    }
}
