using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NET_Release_Tool
{
    static class FileOperations
    {
        public static void AttemptAction(int pIORetries, Action pAction, string pActionDescriptionForErrorMessage, List<string> pErrorOut, StreamWriter pLogger)
        {
            bool success = false;
            for (int i = 0; i < pIORetries; i++)
            {
                try
                {
                    pAction();
                    success = true;
                    break;
                }
                catch (Exception ex)
                {
                    pLogger.WriteLine("==ERROR==" + ex.InnermostExceptionMessage());
                }
            }
            if (!success) { pErrorOut.Add(string.Format("Failed ({0} attempts) at {1}", pIORetries, pActionDescriptionForErrorMessage)); }
        }


        public static IReadOnlyCollection<string> ClearFolder(string pFolder, Action<double, string> pCallbackStatus, StreamWriter pLogger, int pIORetries)
        {
            pFolder = StringControls.NormalizeFolderPath(pFolder);
            pCallbackStatus(0, "Calculating...");

            var files = Directory.GetFiles(pFolder, "*", SearchOption.AllDirectories);
            var folders = Directory.GetDirectories(pFolder, "*", SearchOption.AllDirectories);

            int itemCount = files.Length + folders.Length;

            List<string> errors = new List<string>();
            int iItem = 0;

            foreach (var file in files)
            {
                pCallbackStatus(
                    ++iItem / (double)itemCount,
                    string.Format("Deleting {0} : {1}",
                    Path.GetDirectoryName(file),
                    Path.GetFileName(file)));

                AttemptAction(pIORetries, () => File.Delete(file), "deleting file: " + file, errors, pLogger);
            }

            foreach (var folder in folders)
            {
                pCallbackStatus(++iItem / (double)itemCount, "Deleting " + folder);

                if (Directory.Exists(folder))
                {
                    AttemptAction(pIORetries, () => Directory.Delete(folder, true), "deleting folder: " + folder, errors, pLogger);
                }
            }

            return errors;
        }

        public static IReadOnlyCollection<string> CopyFolder(string pSourceRoot, string pDestinationRoot, Action<double, string> pCallbackStatus, StreamWriter pLogger, int pIORetries)
        {
            pSourceRoot = StringControls.NormalizeFolderPath(pSourceRoot);
            pDestinationRoot = StringControls.NormalizeFolderPath(pDestinationRoot);
            pCallbackStatus(0, "Calculating...");

            var files = Directory.GetFiles(pSourceRoot, "*", SearchOption.AllDirectories);
            var folders = Directory.GetDirectories(pSourceRoot, "*", SearchOption.AllDirectories);
            int itemCount = files.Length + folders.Length;
            List<string> errors = new List<string>();
            int iItem = 0;

            foreach (var folder in folders)
            {
                pCallbackStatus(++iItem / (double)itemCount, "Creating " + folder);

                var relFolder = folder.Substring(pSourceRoot.Length);
                var dstFolder = pDestinationRoot + relFolder;
                AttemptAction(pIORetries, () => Directory.CreateDirectory(dstFolder), "creating folder: " + relFolder, errors, pLogger);
            }

            foreach (var file in files)
            {
                pCallbackStatus(
                    ++iItem / (double)itemCount,
                    string.Format("Copying {0} : {1}",
                    Path.GetDirectoryName(file),
                    Path.GetFileName(file)));

                var relFile = file.Substring(pSourceRoot.Length);
                var dstFile = pDestinationRoot + relFile;
                AttemptAction(pIORetries, () => File.Copy(file, dstFile, true), "copying file: " + relFile, errors, pLogger);
            }

            return errors;
        }

        public static IReadOnlyCollection<string> ClearExtraFiles(string pSourceRoot, string pDestinationRoot, Action<double, string> pCallbackStatus, StreamWriter pLogger, int pIORetries)
        {
            pSourceRoot = StringControls.NormalizeFolderPath(pSourceRoot);
            pDestinationRoot = StringControls.NormalizeFolderPath(pDestinationRoot);
            pCallbackStatus(0, "Calculating...");

            var filesSrc = Directory.GetFiles(pSourceRoot, "*", SearchOption.AllDirectories);
            var foldersSrc = Directory.GetDirectories(pSourceRoot, "*", SearchOption.AllDirectories);

            var filesDst = Directory.GetFiles(pDestinationRoot, "*", SearchOption.AllDirectories);
            var foldersDst = Directory.GetDirectories(pDestinationRoot, "*", SearchOption.AllDirectories);

            int itemCount = filesDst.Length + foldersDst.Length;
            List<string> errors = new List<string>();
            int iItem = 0;

            foreach (var file in filesDst)
            {
                pCallbackStatus(
                    ++iItem / (double)itemCount,
                    string.Format("Checking for leftovers - {0} : {1}",
                    Path.GetDirectoryName(file),
                    Path.GetFileName(file)));

                var relFile = file.Substring(pDestinationRoot.Length);
                var dstFile = pDestinationRoot + relFile;
                var srcFile = pSourceRoot + relFile;
                if (!File.Exists(srcFile))
                {
                    AttemptAction(pIORetries, () => File.Delete(dstFile), "deleting leftover file: " + relFile, errors, pLogger);
                }
            }

            foreach (var folder in foldersDst)
            {
                pCallbackStatus(++iItem / (double)itemCount, "Checking for leftovers: " + folder);

                var relFolder = folder.Substring(pDestinationRoot.Length);
                var dstFolder = pDestinationRoot + relFolder;
                var srcFolder = pSourceRoot + relFolder;
                if (!Directory.Exists(srcFolder))
                {
                    AttemptAction(pIORetries, () => Directory.Delete(dstFolder, true), "deleting leftover folder: " + relFolder, errors, pLogger);
                }
            }

            return errors;
        }

        public static IReadOnlyCollection<string> BackupFolder(string pSourceRoot, string pDestinationRoot, Action<double, string> pCallbackStatus, StreamWriter pLogger, int pIORetries)
        {
            pDestinationRoot = StringControls.NormalizeFolderPath(pDestinationRoot);
            string rel = DateTime.Now.ToString("yyyyMMdd HH.mm.ss tt");
            return CopyFolder(pSourceRoot, pDestinationRoot + rel, pCallbackStatus, pLogger, pIORetries);
        }

        public static IReadOnlyCollection<string> CopyWebConfig(string pSourceFile, string pDestinationFolder, Action<double, string> pCallbackStatus, StreamWriter pLogger, int pIORetries)
        {
            string msg = string.Format("Copying config file from {0} to {1}", Path.GetDirectoryName(pSourceFile), pDestinationFolder);

            pCallbackStatus(0.25, msg);
            List<string> errors = new List<string>();
            AttemptAction(pIORetries, () => File.Copy(pSourceFile, pDestinationFolder + "web.config", true), msg, errors, pLogger);
            pCallbackStatus(1.0, "Complete");

            return errors;
        }
    }
}
