// See https://aka.ms/new-console-template for more information
using Reddit.AuthTokenRetriever;
using System.Diagnostics;
using RedditMegaThread;

Console.WriteLine("Hello, World!");

Console.WriteLine(Options.ProjectInstallationPath);
Console.WriteLine(Options.Logfile);

RedditMegaThread.Logic.Run();

/*

ToDo:
    - Add DryRun output
    - Comment
    - Add Logging (Logger exists just needs to be called)
    - Fix path with / and \ for linux/windows stuff...
    - Write README.md and readme.txt
    - Get proper config.ini and body.txt
    - Make cron job to run at some special day / time
    - Test configs and commandlines (especially other dates)

*/
