using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RemoveStudios
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourceFolder = @"E:\Repos\062_Urbanbot\01_CAD\MODULES\ApartmentCombinations\Studios";
            var sourceFiles = Directory.GetFiles(sourceFolder);
            var targetFolder = Path.Combine(sourceFolder, @"..\noStudios");
            foreach (var file in sourceFiles)
            {
                //создать новый файл
                var targetPath = Path.Combine(targetFolder, Path.GetFileName(file));
                if (File.Exists(targetPath))
                    File.Delete(targetPath);
                File.Create(targetPath).Close();

                //записать новый файл
                using (StreamReader sr = new StreamReader(file))
                {
                    var lines = new List<string>();
                    while (sr.Peek() >= 0)
                    {
                        var code = sr.ReadLine().Trim();
                        if (string.IsNullOrEmpty(code) || code.Contains("_0_1") || code.Contains("_1_0"))
                            continue;
                        
                        lines.Add(code);
                        if (lines.Count >= 10000)
                        {
                            File.AppendAllLines(targetPath, lines);
                            lines.Clear();
                        }
                    }

                    //дописать оставшиеся строки
                    if (lines.Any())
                        File.AppendAllLines(targetPath, lines);
                }
            }
        }
    }
}
