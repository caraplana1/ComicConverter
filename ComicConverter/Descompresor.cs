using System.IO;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives;
using SharpCompress.Common;
using System.Linq;

namespace ComicConverter
{
    public static class Descompresor
    {
        /// <summary>
        /// Extracts all files of a rar file.
        /// </summary>
        /// <param name="filePath">File to extract</param>
        /// <param name="outputDir">Directory for all files extractect from filePath</param>
        public static void UnRar(string filePath, string outputDir = ".output")
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The file doesn't exists");

            else if (!RarArchive.IsRarFile(filePath))
                throw new System.FormatException("The file is not a rar file");

           Directory.CreateDirectory(outputDir);

           using RarArchive archive = RarArchive.Open(filePath);
           
           foreach (var entry in archive.Entries.Where(a => !a.IsDirectory))
           {
                entry.WriteToDirectory(outputDir, new ExtractionOptions()
                {
                    ExtractFullPath = false,
                    Overwrite = true
                });
           }
        }
    }
}
