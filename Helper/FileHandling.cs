﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RedditMegaThread.Helper
{
    internal class FileHandling
    {
       
        public static Version GetVersionFromFile(string filePath, bool defaultToHighVersion = false)
        {
            Version rtrn = new Version("0.0.0.1");
            if (defaultToHighVersion)
            {
                rtrn = new Version("99.99.99.99");
            }

            if (FileHandling.doesFileExist(filePath))
            {
                try
                {
                    FileVersionInfo FVI = FileVersionInfo.GetVersionInfo(filePath);
                    rtrn = new Version(FVI.FileVersion);
                }
                catch { }
            }

            return rtrn;
        }



        /// <summary>
        /// Gets all the Files in one Folder (and its Subfolders)
        /// </summary>
        /// <param name="pPath"></param>
        /// <returns></returns>
        public static string[] GetFilesFromFolderAndSubFolder(string pPath)
        {
            try
            {

                if (doesPathExist(pPath))
                {
                    return (Directory.GetFiles(pPath, "*", SearchOption.AllDirectories));
                }
            }
            catch
            {

            }
            return (new string[0]);
        }

        /// <summary>
        /// Gets all the Files in one Folder (NOT Subfolders)
        /// </summary>
        /// <param name="pPath"></param>
        /// <returns></returns>
        public static string[] GetFilesFromFolder(string pPath)
        {
            if (doesPathExist(pPath))
            {
                return (Directory.GetFiles(pPath, "*", SearchOption.TopDirectoryOnly));
            }
            else
            {
                return (new string[0]);
            }
        }

        public static void LogHash(string pFilePath)
        {
            string Size = GetSizeOfFile(pFilePath).ToString();
            string Hash = GetHashFromFile(pFilePath);
            Logger.Log("FilePath: '" + pFilePath + "', Size: '" + Size + "', Has: '" + Hash + "'.");
        }


        /// <summary>
        /// Creates the Path of a FilePath
        /// </summary>
        /// <param name="pFilePath"></param>
        public static void createPathOfFile(string pFilePath)
        {
            string path = pFilePath.Substring(0, pFilePath.LastIndexOf('\\'));
            createPath(path);
        }

        /// <summary>
        /// Reads the content of a File. In one string (not string array for line)
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        public static string ReadContentOfFile(string pFilePath)
        {
            string rtrn = "";

            if (doesFileExist(pFilePath))
            {
                rtrn = File.ReadAllText(pFilePath);
            }

            return rtrn;
        }

        /// <summary>
        /// Gets the Creation Date of one file in "yyyy-MM-ddTHH:mm:ss" Format
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        public static DateTime GetCreationDate(string pFilePath, bool UTC = false)
        {
            DateTime creation = DateTime.MinValue;
            if (doesFileExist(pFilePath))
            {
                try
                {
                    if (UTC)
                    {
                        creation = File.GetCreationTimeUtc(pFilePath);
                    }
                    else
                    {
                        creation = File.GetCreationTime(pFilePath);
                    }
                    return creation;
                }
                catch
                {
                    Logger.Log("Getting Creation Date of File: '" + pFilePath + "' failed.");
                }
            }
            return creation;
        }

        /// <summary>
        /// Gets the GetLastWriteDate Date of one file in "yyyy-MM-ddTHH:mm:ss" Format
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        public static DateTime GetLastWriteDate(string pFilePath, bool UTC = false)
        {
            DateTime creation = DateTime.MinValue;
            //rtrn = creation.ToString("yyyy-MM-ddTHH:mm:ss");
            if (doesFileExist(pFilePath))
            {
                try
                {
                    if (UTC)
                    {
                        creation = File.GetLastWriteTimeUtc(pFilePath);
                    }
                    else
                    {
                        creation = File.GetLastWriteTime(pFilePath);
                    }
                    return creation;
                }
                catch
                {
                    Logger.Log("Getting LastModified Date of File: '" + pFilePath + "' failed.");
                }
            }
            return creation;
        }

        /// <summary>
        /// Moves a File from A to B
        /// </summary>
        /// <param name="pMoveFromFilePath"></param>
        /// <param name="pMoveToFilePath"></param>
        public static void moveFile(string pMoveFromFilePath, string pMoveToFilePath)
        {
            if (doesFileExist(pMoveToFilePath))
            {
                Logger.Log("Moving File: '" + pMoveFromFilePath + "' to '" + pMoveToFilePath + "'. TargetFile exists, so im deleting it. YOLO");
            }
            try
            {
                string[] sth = PathSplitUp(pMoveToFilePath);
                createPath(sth[0]);
                File.Move(pMoveFromFilePath, pMoveToFilePath);
            }
            catch (Exception e)
            {
                Logger.Log("Moving File: '" + pMoveFromFilePath + "' to '" + pMoveToFilePath + "' failed.");
            }
        }

        /// <summary>
        /// Moving a Path. Moving (Cut, Paste a Folder) or Rename a folder
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Dest"></param>
        public static void movePath(string Source, string Dest)
        {
            try
            {
                if (doesPathExist(Source))
                {
                    if (doesPathExist(Dest))
                    {
                        DeleteFolder(Dest);
                    }
                    Directory.Move(Source, Dest);
                }
            }
            catch (Exception e)
            {
               Logger.Log("Moving Path: '" + Source + "' to '" + Dest + "' failed.");
            }
        }

        /// <summary>
        /// Overwriting a string array to a file
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <param name="pContent"></param>
        public static void WriteStringToFileOverwrite(string pFilePath, string[] pContent)
        {

            if (doesFileExist(pFilePath))
            {
                deleteFile(pFilePath);
            }
            File.WriteAllLines(pFilePath, pContent);
        }



        /// <summary>
        /// Uses to generate Random Numbers
        /// </summary>
        static Random random = new Random();

        /// <summary>
        /// Used to indicate which instance we log from.
        /// </summary>
        static int intrandom = random.Next(1000, 9999);



        /// <summary>
        /// Method we use to add one line of text as a new line to a text file
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <param name="pLineContent"></param>
        public static void AddToLog(string pFilePath, string pLineContent)
        {
            // Should be quicker than checking if File exists, and also checks for more erros
            try
            {
                StreamWriter sw;
                sw = File.AppendText(pFilePath);
                sw.Write(pLineContent + Environment.NewLine);
                sw.Close();
            }
            catch (Exception e)
            {

             }
        }

        // A lot of self written functions for File stuff below.
        // Lots of overloaded stuff. Nothing checks for errors, like file permissions or sth.

        /// <summary>
        /// ReadFileEachLine
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        public static string[] ReadFileEachLine(string pFilePath)
        {
            if (doesFileExist(pFilePath))
            {
                return File.ReadAllLines(pFilePath);
            }
            else
            {
                return new string[0];
            }
        }

        /// <summary>
        /// GetXMLTagContent
        /// </summary>
        /// <param name="pXML"></param>
        /// <param name="pTag"></param>
        /// <returns></returns>
        public static string GetXMLTagContent(string pXML, string pTag)
        {
            pXML = pXML.Replace("\n", "").Replace("\r", "");

            string rtrn = "";

            Regex regex = new Regex(@"<" + pTag + ">.+</" + pTag + ">");
            Match match = regex.Match(pXML);

            if (match.Success)
            {
                string tmp = match.Value;
                rtrn = tmp.Substring(tmp.IndexOf('>') + 1, tmp.LastIndexOf('<') - 2 - pTag.Length);
            }

            return rtrn;
        }

        /// <summary>
        /// Does remote URL exist. Timeout in MS
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool URLExists(string url, int TimeOutMS = 500)
        {
            bool result = true;

            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Timeout = TimeOutMS;
                webRequest.Method = "GET";

                webRequest.GetResponse();
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Checks if 2 Files are equal.
        /// </summary>
        /// <param name="pFilePathA"></param>
        /// <param name="pFilePathB"></param>
        /// <param name="SlowButStable"></param>
        /// <returns></returns>
        public static bool AreFilesEqual(string pFilePathA, string pFilePathB, bool SlowButStable)
        {
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            bool Sth = AreFilesEqualReal(pFilePathA, pFilePathB, SlowButStable);
            //sw.Stop();
            //Logger.Log("AAAA - It took '" + sw.ElapsedMilliseconds + "' ms to compare: '" + pFilePathA + "' and '" + pFilePathB + "'. Result is: " + Sth.ToString());
            return Sth;
        }



        private static bool StreamsContentsAreEqual(Stream stream1, Stream stream2)
        {
            const int bufferSize = 1024 * sizeof(Int64);
            var buffer1 = new byte[bufferSize];
            var buffer2 = new byte[bufferSize];

            while (true)
            {
                int count1 = stream1.Read(buffer1, 0, bufferSize);
                int count2 = stream2.Read(buffer2, 0, bufferSize);

                if (count1 != count2)
                {
                    return false;
                }

                if (count1 == 0)
                {
                    return true;
                }

                int iterations = (int)Math.Ceiling((double)count1 / sizeof(Int64));
                for (int i = 0; i < iterations; i++)
                {
                    if (BitConverter.ToInt64(buffer1, i * sizeof(Int64)) != BitConverter.ToInt64(buffer2, i * sizeof(Int64)))
                    {
                        return false;
                    }
                }
            }
        }


        /// <summary>
        /// Actual File Comparison Logic
        /// </summary>
        /// <param name="pFilePathA"></param>
        /// <param name="pFilePathB"></param>
        /// <param name="SlowButStable"></param>
        /// <returns></returns>
        public static bool AreFilesEqualReal(string pFilePathA, string pFilePathB, bool SlowButStable)
        {
            FileInfo fileInfo1 = new FileInfo(pFilePathA);
            FileInfo fileInfo2 = new FileInfo(pFilePathB);

            if (!fileInfo1.Exists || !fileInfo2.Exists)
            {
                return false;
            }
            if (fileInfo1.Length != fileInfo2.Length)
            {
                return false;
            }
            else if (SlowButStable)
            {
                using (var file1 = fileInfo1.OpenRead())
                {
                    using (var file2 = fileInfo2.OpenRead())
                    {
                        return StreamsContentsAreEqual(file1, file2);
                    }
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the MD5 Hash of one file
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        public static string GetHashFromFile(string pFilePath)
        {
            string rtrn = "";
            if (doesFileExist(pFilePath))
            {
                try
                {
                    using (var md5 = MD5.Create())
                    {
                        // hash contents
                        byte[] contentBytes = File.ReadAllBytes(pFilePath);
                        md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
                        md5.TransformFinalBlock(new byte[0], 0, 0);
                        return BitConverter.ToString(md5.Hash).Replace("-", "").ToLower();
                    }
                }
                catch (Exception e)
                {
                    Logger.Log("Hashing of a single file failed." + e.ToString());
                }


            }
            return rtrn;
        }


        /// <summary>
        /// Method to get Hash from a Folder
        /// </summary>
        /// <param name="srcPath"></param>
        /// <returns></returns>
        public static string CreateDirectoryMd5(string srcPath)
        {
            if (!doesPathExist(srcPath))
            {
                Logger.Log("ZIP Extraction Path is not a valid Filepath...wut");
                Environment.Exit(5);
            }

            var myFiles = Directory.GetFiles(srcPath, "*", SearchOption.AllDirectories).OrderBy(p => p).ToArray();

            using (var md5 = MD5.Create())
            {
                foreach (var myFile in myFiles)
                {
                    // hash path
                    byte[] pathBytes = Encoding.UTF8.GetBytes(myFile);
                    md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

                    // hash contents
                    byte[] contentBytes = File.ReadAllBytes(myFile);

                    md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
                }

                //Handles empty filePaths case
                md5.TransformFinalBlock(new byte[0], 0, 0);

                return BitConverter.ToString(md5.Hash).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Shoutout to Dr490n. Think I stole this off of him.
        /// </summary>
        /// <returns></returns>
        public static string MostLikelyProfileFolder()
        {
            string profilesDir = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            profilesDir = System.IO.Path.Combine(profilesDir, @"Rockstar Games\GTA V\Profiles");
            var di = new System.IO.DirectoryInfo(profilesDir);
            DateTime lastHigh = new DateTime(1900, 1, 1);
            string highDir = "";
            foreach (var profile in di.GetDirectories())
            {
                if (Regex.IsMatch(profile.Name, "[0-9A-F]{8}"))
                {
                    DateTime created = profile.LastWriteTime;

                    if (created > lastHigh)
                    {
                        highDir = profile.FullName;
                        lastHigh = created;
                    }
                }
            }
            return highDir;
        }

        public static string GetParentFolder(string Path)
        {
            return Path.Substring(0, Path.TrimEnd('\\').LastIndexOf('\\'));
        }

        /// <summary>
        /// Gets String from URL
        /// </summary>
        /// <param name="pURL"></param>
        /// <returns></returns>
        public static string GetStringFromURL(string pURL, bool surpressPopup = false)
        {
            string rtrn = "";
            try
            {
                rtrn = new System.Net.Http.HttpClient().GetStringAsync(pURL).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Logger.Log("GetStringFromURL failed. Probably Network related. URL = '" + pURL + "'");
                Logger.Log("e.ToString():\n" + e.ToString());
                Logger.Log("e.Message.ToString():\n" + e.Message.ToString());
            }

            return rtrn;
        }


        /// <summary>
        /// Get all SubFolders from a Path
        /// </summary>
        /// <param name="pPath"></param>
        /// <returns></returns>
        public static string[] GetSubFolders(string pPath)
        {
            if (doesPathExist(pPath))
            {
                try
                {
                    return Directory.GetDirectories(pPath);
                }
                catch (Exception e)
                {
                    Logger.Log("Get Sub Folders failed. " + e.ToString());
                }
            }
            return new string[0];
        }


        /// <summary>
        /// Makes "C:\Some\Path(\)" and "Somefile.txt" into "C:\Some\Path\Somefile.txt". Doesnt check for errors.
        /// </summary>
        /// <param name="pPath"></param>
        /// <param name="pFile"></param>
        /// <returns></returns>
        public static string PathCombine(string pPath, string pFile)
        {
            return pPath.TrimEnd('\\') + @"\" + pFile;
        }

        /// <summary>
        /// Makes "C:\Some\Path\Somefile.txt" into "C:\Some\Path" and "Somefile.txt". The last "\" disappears. Doesnt check for errors.
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        public static string[] PathSplitUp(string pFilePath)
        {
            string[] rtrn = new string[2];
            rtrn[0] = pFilePath.Substring(0, pFilePath.LastIndexOf('\\'));
            rtrn[1] = pFilePath.Substring(pFilePath.LastIndexOf('\\') + 1);
            return rtrn;
        }

        /// <summary>
        /// Renaming a file. Second param is FileName not FilePath
        /// </summary>
        /// <param name="pFilePathSource"></param>
        /// <param name="pFileNameDest"></param>
        public static void RenameFile(string pFilePathSource, string pFileNameDest)
        {
            if (doesFileExist(pFilePathSource))
            {
                string pFilePathDest = PathSplitUp(pFilePathSource)[0].TrimEnd('\\') + @"\" + pFileNameDest;
                try
                {
                    //FileHandling.AddToDebug("Renaming: '" + pFilePathSource + "' to '" + pFilePathDest + "'.");
                    System.IO.File.Move(pFilePathSource, pFilePathDest);
                }
                catch (Exception e)
                {
                     Logger.Log("Renaming File failed ('" + pFilePathSource + "' to '" + pFilePathDest + "').");

                }
            }
        }

        /// <summary>
        /// Gets the Size of one File in Bytes as Long
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        public static long GetSizeOfFile(string pFilePath)
        {
            long mySize = 0;
            if (doesFileExist(pFilePath))
            {
                FileInfo myFileInfo = new FileInfo(pFilePath);
                mySize = myFileInfo.Length;
            }
            return mySize;
        }

        /// <summary>
        /// read File
        /// </summary>
        /// <param name="pPath"></param>
        /// <param name="pFile"></param>
        /// <returns></returns>
        public static List<string> ReadFile(string pPath, string pFile)
        {
            return ReadFile(PathCombine(pPath, pFile));
        }

        /// <summary>
        /// readFile
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        public static List<string> ReadFile(string pFilePath)
        {
            List<string> rtrnList = new List<string>();
            StreamReader sr;
            string currentLine;
            bool doesLineExist = true;
            if (doesFileExist(pFilePath))
            {
                sr = new StreamReader(pFilePath);
                while (doesLineExist)
                {
                    doesLineExist = false;
                    currentLine = sr.ReadLine();
                    if (currentLine != null)
                    {
                        rtrnList.Add(currentLine);
                        doesLineExist = true;
                    }
                }
                sr.Close();
            }
            return rtrnList;
        }

        /// <summary>
        /// deleteFile
        /// </summary>
        /// <param name="pPath"></param>
        /// <param name="pFile"></param>
        public static void deleteFile(string pPath, string pFile)
        {
            deleteFile(PathCombine(pPath, pFile));
        }

        /// <summary>
        /// deleteFile
        /// </summary>
        /// <param name="pFilePath"></param>
        public static void deleteFile(string pFilePath)
        {
            try
            {
                if (doesFileExist(pFilePath))
                {
                    File.SetAttributes(pFilePath, FileAttributes.Normal);
                    File.Delete(pFilePath);
                }
            }
            catch (Exception e)
            {
                    Logger.Log("Deleting File failed ('" + pFilePath + "').");
            }
        }


        /// <summary>
        /// Copy File A to file B. DOES overwrite
        /// </summary>
        /// <param name="pSource"></param>
        /// <param name="pDestination"></param>
        public static void copyFile(string pSource, string pDestination)
        {
            if (!File.Exists(pSource))
            {
                Logger.Log("Copying File ['" + pSource + "' to '" + pDestination + "'] failed since SourceFile ('" + pSource + "') does NOT exist.");
                return;
            }
            if (File.Exists(pDestination))
            {
                FileHandling.deleteFile(pDestination);
            }
            try
            {
                FileHandling.createPathOfFile(pDestination);
                File.Copy(pSource, pDestination);
            }
            catch (Exception e)
            {
                Logger.Log("Copying File ['" + pSource + "' to '" + pDestination + "'] failed since trycatch failed." + e.Message.ToString());
            }
        }





        /// <summary>
        /// Creates File (and Folder(s). Overrides existing file.
        /// </summary>
        /// <param name="pPath"></param>
        /// <param name="pFile"></param>
        public static void createFile(string pPath, string pFile)
        {
            if (!doesPathExist(pPath))
            {
                createPath(pPath);
            }
            else if (doesFileExist(pPath, pFile))
            {
                deleteFile(pPath, pFile);
            }

            try
            {
                File.CreateText(PathCombine(pPath, pFile)).Close();
            }
            catch (Exception e)
            {
                   Logger.Log("Create File failed ('" + PathCombine(pPath, pFile) + "').");
            }
        }

        /// <summary>
        /// Creates File (and Folder(s)). Overrides existing file.
        /// </summary>
        /// <param name="pFilePath"></param>
        public static void createFile(string pFilePath)
        {
            string[] paths = PathSplitUp(pFilePath);
            createFile(paths[0], paths[1]);
        }

        /// <summary>
        /// Checks if a File exists.
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        public static bool doesFileExist(string pFilePath)
        {
            return File.Exists(pFilePath);
        }

        /// <summary>
        /// Checks if a File exists.
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        public static bool doesFileExist(string pPath, string pFile)
        {
            return doesFileExist(PathCombine(pPath, pFile));
        }

        /// <summary>
        /// Checks if a path exists.
        /// </summary>
        /// <param name="pFolderPath"></param>
        /// <returns></returns>
        public static bool doesPathExist(string pFolderPath)
        {
            return Directory.Exists(pFolderPath);
        }

        /// <summary>
        /// Deletes a Folder
        /// </summary>
        /// <param name="pPath"></param>
        public static void DeleteFolder(string pPath, bool recursive = true)
        {
            try
            {
                if (doesPathExist(pPath))
                {
                    Directory.Delete(pPath, recursive);
                }
            }
            catch (Exception e)
            {
                Logger.Log("Failed to delete Folder for some reason. " + e.ToString());
            }
        }

        /// <summary>
        /// Creates a Path. Works for SubSubPaths.
        /// </summary>
        /// <param name="pFolderPath"></param>
        public static void createPath(string pFolderPath)
        {
            try
            {
                if (!doesPathExist(pFolderPath))
                {
                    Directory.CreateDirectory(pFolderPath);
                }
            }
            catch (Exception e)
            {
                Logger.Log("The code looked good to me. #SadFace. Crashing while creating Path ('" + pFolderPath + "'): " + e.ToString());
            }
        }

        /// <summary>
        /// Deletes target dir...
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="targetDirectory"></param>
        public static void CopyPath(string sourceDirectory, string targetDirectory)
        {
            if (doesPathExist(sourceDirectory))
            {
                if (doesPathExist(targetDirectory))
                {
                    DeleteFolder(targetDirectory);
                }
                var diTarget = new DirectoryInfo(targetDirectory);
                var diSource = new DirectoryInfo(sourceDirectory);

                CopyAll(diSource, diTarget);
            }


        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }


        // ALL SORTS OF RANDOM METHODS

        /// <summary>
        /// String.TrimEnd() now works with a string as well with chars
        /// </summary>
        /// <param name="input"></param>
        /// <param name="suffixToRemove"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static string TrimEnd(string input, string suffixToRemove, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            if (suffixToRemove != null && input.EndsWith(suffixToRemove, comparisonType))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }

            return input;
        }

    } // End of Class
} // End of NameSpace

