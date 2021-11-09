using System.IO;
using System.Linq;
using SharpCompress.Common;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.SevenZip;

namespace ComicConverter
{
    public static class Decompressor
    {
        /// <summary>
        /// Extract rar file in given directory.
        /// </summary>
        /// <param name="filePath">File to extract</param>
        /// <param name="outputDir">Directory to store extracted files.</param>
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
    
        /// <summary>
        /// Extract zip file in given directory.
        /// </summary>
        /// <param name="filePath">File to extract.</param>
        /// <param name="outputDir">Directory to store extracted files.</param>
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

        public static void UnTar(string filepath, string outputDir = ".") => throw new System.NotImplementedException();

        public static void UnSevenZip(string filePath, string outputDir = ".") => throw new System.NotImplementedException();
    }
}