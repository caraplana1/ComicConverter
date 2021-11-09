using System.IO;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives;
using SharpCompress.Common;
using System.Linq;

namespace ComicConverter
{
    public static class Decompressor
    {
        /// <summary>
        /// Extracts all files of a rar file.
        /// </summary>
        /// <param name="filePath">File to extract</param>
        /// <param name="outputDir">Directory for all files extractect from filePath</param>
        public static void UnRar(string filePath, string outputDir = ".")
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The file doesn't exists");

            if (!RarArchive.IsRarFile(filePath))
                throw new System.FormatException("The file is not a rar file");

            if (string.IsNullOrEmpty(outputDir))
                throw new System.FormatException("Th directoty cannot be null or empty");

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
    
        public static void UnZip(string filePath, string outputDir =".")
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            if (!ZipArchive.IsZipFile(filePath))
                throw new System.FormatException("The file is not a zip file");

            if (string.IsNullOrEmpty(outputDir))
                throw new System.FormatException("Th directoty cannot be null or empty");

            Directory.CreateDirectory(outputDir);

			using ZipArchive zip = ZipArchive.Open(filePath);

            foreach (var entry in zip.Entries.Where(a => !a.IsDirectory))
                entry.WriteToDirectory(outputDir, new ExtractionOptions(){
                    Overwrite = true,
                    ExtractFullPath = false
                });
		}
    }
}
