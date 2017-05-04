using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                resultString = extractAudio(_fileName, _resourcesPath, _outputPath);
                resultString += createPreviewVideo(_fileName, _resourcesPath, _outputPath);
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
                resultString += line + "\n";
            }
            proc.Close();
            return resultString;
        }

        //
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
                resultString += line + "\n";
            }
            proc.Close();
            //Eager preparing mpg file in the new thread
            //Task.Run(() =>
            //{
            //    createTmpVideo(_fileName, _outputPath);
            //});

            return resultString;
        }

        //
        public static String processLayoutChange(List<ResourceModel> _resModelList, int _begin, int _end, String _resourcesPath, String _outputPath, String _previewFileName)
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
                createTmpVideo(rm.fileName, _outputPath);

                filesString +=
                    _outputPath + "video\\" + rm.fileName.Remove(rm.fileName.Length - 3) + "mpg" + "\"";
            }
            filesString = filesString.Remove(filesString.Length - 1) + "\"";

            //Устанавливаем аргументы командной строки
            //-i "_resourcesPath\video\_fileName" -s 320x240 "_outputPath\video\_fileName"
            proc.StartInfo.Arguments =
                "-i "
                + filesString
                + " -c copy "
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
                resultString += line + "\n";
            }
            proc.Close();
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
                resultString += line + "\n";
            }
            proc.Close();
            return resultString;
        }
    }
}
