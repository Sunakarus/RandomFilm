using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RandomFilm
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string path = @"G:\Movies\";
            string logFileName = "seen.txt";
            string hr = "---------------------------------------";
            string[] files = Directory.GetDirectories(path, "_*");
            Random ran = new Random();

            //log seen films
            string[] seen = Directory.GetDirectories(path);
            seen = seen.Except(files).ToArray();

            List<string> filesToLog = new List<string>();

            if (File.Exists(path + logFileName))
            {
                StreamReader reader = new StreamReader(path + logFileName);
                string line = reader.ReadLine();
                while (line != null)
                {
                    filesToLog.Add(line);
                    line = reader.ReadLine();
                }
                reader.Close();
            }

            bool logging = false;
            int logged = 0;

            for (int i = 0; i < seen.Length; i++)
            {
                seen[i] = seen[i].Substring(path.Length);
                if (!filesToLog.Contains(seen[i]))
                {
                    if (!logging)
                    {
                        Console.WriteLine("Logging seen films.\n{0}", hr);
                        logging = true;
                    }
                    filesToLog.Add(seen[i]);
                    logged++;
                    Console.WriteLine("Logged: {0}", seen[i]);
                }
            }

            if (filesToLog.Contains("") || !filesToLog.SequenceEqual(seen.ToList()))
            {
                if (!logging)
                {
                    logging = true;
                    Console.WriteLine("Managing log file.\n{0}", hr);
                }
                filesToLog.RemoveAll(x => x == "");
            }

            if (logging)
            {
                StreamWriter writer = new StreamWriter(path + logFileName);
                filesToLog.Sort();
                for (int i = 0; i < filesToLog.Count; i++)
                {
                    writer.WriteLine(filesToLog[i]);
                }
                writer.Close();

                Console.WriteLine("{1}\nLogging complete. Logged films: {0}\n", logged, hr);
            }

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Substring(path.Length + 1);
                Console.WriteLine(files[i]);
            }
            Console.WriteLine("{1}\nUnseen movies: {0}\n", files.Length, hr);

            string info = "Randomly chosen movie: ";

            Console.Write(info);
            do
            {
                int cursorPos = files.Length + 3;
                if (logging)
                {
                    cursorPos += 5 + logged;
                }
                Console.SetCursorPosition(info.Length, cursorPos);
                for (int i = info.Length; i < Console.WindowWidth; i++)
                {
                    Console.Write(" ");
                }
                Console.SetCursorPosition(info.Length, cursorPos);

                Console.WriteLine(files[ran.Next(files.Length)]);
                Console.WriteLine("Press R to reroll.");

                int windowHeight = cursorPos + 3;
                windowHeight = windowHeight > 84 ? 84 : windowHeight; //to prevent ArgumentOutOfBounds exception
                Console.SetWindowSize(Console.WindowWidth, windowHeight);
                Console.SetWindowPosition(0, cursorPos + 3 - windowHeight);
            }
            while (Console.ReadKey(true).Key == ConsoleKey.R);
        }
    }
}