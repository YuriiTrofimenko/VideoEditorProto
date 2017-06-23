using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace VideoLib
{
    public class VideoProcessor
    {
        /*static VideoProcessor()
        {
                
        }*/

        public static String processResource(String _fileName, String _resourcesPath, String _outputPath)
        {
            //String resultString = "Ok\n";
            String resultString = "";
            String extString = _fileName.Substring(_fileName.Length - 3);
            //Если файл - видео
            if (extString.Equals("mp4") || extString.Equals("avi"))
            {
                if (!File.Exists(_outputPath + "audio\\" + _fileName.Remove(_fileName.Length - 3) + "mp3"))
                {
                    resultString = extractAudio(_fileName, _resourcesPath, _outputPath);
                }
                if (!File.Exists(_outputPath + "video\\" + _fileName))
                {
                    resultString += createPreviewVideo(_fileName, _resourcesPath, _outputPath);
                }
                
            }
            return resultString;
        }

        //
        private static String extractAudio(String _fileName, String _resourcesPath, String _outputPath)
        {
            String resultString = "";
            //Создаем объект процесса для запуска внешнего исполняемого файла
            Process proc = new Process();
            //Получаем путь к внешнему исполняемому файлу (утилите обработки видео)
            String ffmpegPath =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg\\ffmpeg.exe")
                    .Replace("VideoEditorProto\\ffmpeg", "VideoLib\\ffmpeg");
            //Устанавливаем имя файла для запуска
            proc.StartInfo.FileName = ffmpegPath;
            //Устанавливаем аргументы командной строки
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
            //Выводим отладочную информацию - ответ утилиты
            StreamReader reader = proc.StandardError;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                //Console.WriteLine(line);
                if (line == "")
                {
                    break;
                }
                resultString += line + "\n";
            }

            reader.Close();
            if (proc != null && proc.HasExited == false) proc.Kill();
            //proc.Close();
            return resultString;
        }

        private static String createPreviewVideo(String _fileName, String _resourcesPath, String _outputPath)
        {
            String resultString = "";
            //Создаем объект процесса для запуска внешнего исполняемого файла
            Process proc = new Process();
            //Получаем путь к внешнему исполняемому файлу (утилите обработки видео)
            String ffmpegPath =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg\\ffmpeg.exe")
                    .Replace("VideoEditorProto\\ffmpeg", "VideoLib\\ffmpeg");
            //Устанавливаем имя файла для запуска
            proc.StartInfo.FileName = ffmpegPath;
            //Устанавливаем аргументы командной строки
            //-i "_resourcesPath\video\_fileName" -s 320x240 "_outputPath\video\_fileName"
            proc.StartInfo.Arguments =
                "-i "
                + "\"" + _resourcesPath + "video\\" + _fileName + "\""
                + " -s 320x240 "
                + "\"" + _outputPath + "video\\" + _fileName + "\"";
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
            //Выводим отладочную информацию - ответ утилиты
            StreamReader reader = proc.StandardError;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                //Console.WriteLine(line);
                if (line == "")
                {
                    break;
                }
                resultString += line + "\n";
            }

            reader.Close();
            if (proc != null && proc.HasExited == false) proc.Kill();
            //proc.Close();

            //Eager preparing mpg file in the new thread
            //Task.Run(() =>
            //{
            //    createTmpVideo(_fileName, _outputPath);
            //});

            return resultString;
        }

        //

        public static String processLayoutChange(List<ResourceModel> _resModelList, int _begin, int _end, String _resourcesPath, String _outputPath, String _previewFileName, Color rgb)
        {
            String resultString = "";
            //Создаем объект процесса для запуска внешнего исполняемого файла
            Process proc = new Process();
            //Получаем путь к внешнему исполняемому файлу (утилите обработки видео)
            String ffmpegPath =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg\\ffmpeg.exe")
                    .Replace("VideoEditorProto\\ffmpeg", "VideoLib\\ffmpeg");
            //Устанавливаем имя файла для запуска
            proc.StartInfo.FileName = ffmpegPath;

            String filesString = "concat:\"";
            foreach (ResourceModel rm in _resModelList)
            {
                if (!File.Exists(_outputPath + "video\\" + rm.fileName.Remove(rm.fileName.Length - 3) + "mpg"))
                {
                    createTmpVideo(rm.fileName, _outputPath);
                }

                filesString +=
                    _outputPath + "video\\" + rm.fileName.Remove(rm.fileName.Length - 3) + "mpg" + "|";
            }
            filesString = filesString.Remove(filesString.Length - 1) + "\"";

            //ffmpeg - i 1.mp4 -f lavfi - i "color=red:s=320x240" - filter_complex "[0:v]setsar=sar=1/1[s]; [s][1:v]blen
            //d = shortest = 1:all_mode = screen:all_opacity = 0.7[out]" -map [out] -map 0:a output.mp4
            //сверху наложение красного цвета на видео

            //Устанавливаем аргументы командной строки
            //-i "_resourcesPath\video\_fileName" -s 320x240 "_outputPath\video\_fileName"
            proc.StartInfo.Arguments =
                "-i "
                + filesString;
            /*if (!(rgb.R == 1 && rgb.G == 1 && rgb.B == 1))
                proc.StartInfo.Arguments += " -f lavfi - i \"color=0x" + ColorTranslator.ToHtml(rgb).Remove(0, 1) + ":s = 320x240\" - filter_complex \"[0:v]setsar = sar = 1 / 1[s]; [s] [1:v] blen "
                + "/d = shortest = 1:all_mode = screen:all_opacity = 0.7[out]\" - map[out] - map 0:a";*/

            proc.StartInfo.Arguments += " -c copy "
                + "\"" + _outputPath + "video\\" + _previewFileName + ".mpg" + "\"";
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
            //Выводим отладочную информацию - ответ утилиты
            StreamReader reader = proc.StandardError;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                //Console.WriteLine(line);
                if (line == "")
                {
                    break;
                }
                resultString += line + "\n";
            }
            reader.Close();
            if (proc != null && proc.HasExited == false) proc.Kill();

            //
            mpgToMp4(_previewFileName + ".mpg", _outputPath);

            return null;
        }

        //
        private static String createTmpVideo(String _fileName, String _outputPath)
        {
            String resultString = "";
            Process proc = new Process();
            String ffmpegPath =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg\\ffmpeg.exe")
                    .Replace("VideoEditorProto\\ffmpeg", "VideoLib\\ffmpeg");
            proc.StartInfo.FileName = ffmpegPath;
            //аргументы командной строки
            //-i "_outputPath\video\_fileName" -qscale:v 1 "_outputPath\video\_fileName"
            proc.StartInfo.Arguments =
                "-i "
                + "\"" + _outputPath + "video\\" + _fileName + "\""
                + " -qscale:v 1 "
                + "\"" + _outputPath + "video\\" + _fileName.Remove(_fileName.Length - 3) + "mpg" + "\"";
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            if (!proc.Start())
            {
                resultString = "Error:\n";
            } else{/*proc.WaitForExit();*/}
            StreamReader reader = proc.StandardError;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line == "")
                {
                    break;
                }
                resultString += line + "\n";
            }
            reader.Close();
            if (proc != null && proc.HasExited == false) proc.Kill();
            //proc.Close();

            //if (!proc.HasExited)
            //{
            //    proc.WaitForExit(1000);
            //    if (!proc.HasExited)
            //    {
            //        proc.Kill();
            //    }
            //}

            //proc.Kill();

            return resultString;
        }

        //
        private static String mpgToMp4(String _fileName, String _outputPath)
        {
            String resultString = "";
            Process proc = new Process();
            String ffmpegPath =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg\\ffmpeg.exe")
                    .Replace("VideoEditorProto\\ffmpeg", "VideoLib\\ffmpeg");
            proc.StartInfo.FileName = ffmpegPath;
            //аргументы командной строки
            //-i "E:\Projects\CS\ASP.NET\ASP.NET MVC\VideoEditorProto\VideoResources\intermediate_all.mpg" -qscale:v 2 "E:\Projects\CS\ASP.NET\ASP.NET MVC\VideoEditorProto\VideoResources\output.mp4"
            proc.StartInfo.Arguments =
                "-i "
                + "\"" + _outputPath + "video\\" + _fileName + "\""
                + " -qscale:v 2 "
                + "\"" + _outputPath + "video\\" + _fileName.Remove(_fileName.Length - 3) + "mp4" + "\"";
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            if (!proc.Start())
            {
                resultString = "Error:\n";
            }
            else {/*proc.WaitForExit();*/}
            StreamReader reader = proc.StandardError;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line == "")
                {
                    break;
                }
                resultString += line + "\n";
            }
            reader.Close();
            if (proc != null && proc.HasExited == false) proc.Kill();

            return resultString;
        }
    }
}
