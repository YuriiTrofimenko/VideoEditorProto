using System;
using System.Diagnostics;
using System.IO;

namespace VideoLib
{
    public class VideoProcessor
    {
        /*static VideoProcessor()
        {
                
        }*/

        public static String processResource(String _fileName, String _resourcesPath, String _outputPath)
        {
            String resultString = "Ok\n";
            String extString = _fileName.Substring(_fileName.Length - 3);
            if (extString.Equals("mp4") || extString.Equals("avi"))
            {
                Process proc = new Process();
                String ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg\\ffmpeg.exe").Replace("VideoEditorProto\\ffmpeg", "VideoLib\\ffmpeg");

                proc.StartInfo.FileName = ffmpegPath;
                proc.StartInfo.Arguments =
                    "-i "
                    + "\"" + _resourcesPath + "video\\" + _fileName + "\""
                    + " -vn -ar 44100 -ac 2 -ab 192K -f mp3 "
                    + "\"" + _outputPath + "audio\\" + _fileName.Remove(_fileName.Length - 3) + "mp3\"";
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                if (!proc.Start())
                {
                    //Console.WriteLine("Error starting");
                    //resultString = "Error";
                    resultString = "Error:\n";
                    //return resultString;
                }
                else
                {
                    //proc.WaitForExit();
                }
                StreamReader reader = proc.StandardError;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    //Console.WriteLine(line);
                    resultString += line + "\n";
                }
                proc.Close();
            }
            return resultString;
        }
    }
}
