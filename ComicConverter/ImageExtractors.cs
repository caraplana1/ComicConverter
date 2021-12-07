using System;
using System.IO;
using System.Linq;
using System.Drawing;
using GroupDocs.Parser;
using SharpCompress.Common;
using System.Drawing.Imaging;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.SevenZip;

namespace ComicConverter
{
    public static class ImageExtractors
    {
		public static void ExtractImages(string filePath, string outputDir = ".")
		{
			if (!File.Exists(filePath))
				throw new FileNotFoundException();

			if (filePath.Substring(filePath.Length - 3) != "pdf" )
				throw new FormatException("The file is not pdf file");

			if (String.IsNullOrEmpty(outputDir))
                throw new System.FormatException("The directoty cannot be null or empty");

			Directory.CreateDirectory(outputDir);

			using Parser parser = new (filePath);

			if (!parser.Features.Images)
				return;

			var fileImages = parser.GetImages();
			int counter = 0;

			foreach (var image in fileImages)
				Image.FromStream(image.GetImageStream()).Save($"{outputDir}/{counter++}.png", ImageFormat.Png);
		}

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
           
           foreach (var entry in archive.Entries.Where(x => !x.IsDirectory))
                entry.WriteToDirectory(outputDir, new ExtractionOptions()
                {
                    ExtractFullPath = false,
                    Overwrite = true
                });
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

            foreach (var entry in zip.Entries.Where(x => !x.IsDirectory))
                entry.WriteToDirectory(outputDir, new ExtractionOptions()
                {
                    Overwrite = true,
                    ExtractFullPath = false
                });
		}

        /// <summary>
        /// Extract tar file in given directory.
        /// </summary>
        /// <param name="filePath">File to extract</param>
        /// <param name="outputDir">Directory to store extracted files.</param>
        public static void UnTar(string filePath, string outputDir = ".") 
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            if (string.IsNullOrEmpty(outputDir))
                throw new System.FormatException("Th directoty cannot be null or empty");

            if(!TarArchive.IsTarFile(filePath))
                throw new System.FormatException("The file is not a tar file");

            Directory.CreateDirectory(outputDir);

            using TarArchive tar = TarArchive.Open(filePath);

            foreach (var entry in tar.Entries.Where(x => !x.IsDirectory))
                entry.WriteToDirectory(outputDir, new ExtractionOptions()
                {
                    Overwrite = true,
                    ExtractFullPath = false
                });
        }


        /// <summary>
        /// Extract 7z file in given directory.
        /// </summary>
        /// <param name="filePath">File to extract</param>
        /// <param name="outputDir">Directory to store extracted files.</param>
        public static void UnSevenZip(string filePath, string outputDir = ".")
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            if (string.IsNullOrEmpty(outputDir))
                throw new System.FormatException("Th directoty cannot be null or empty");

            if(!SevenZipArchive.IsSevenZipFile(filePath))
                throw new System.FormatException("The file is not a 7z file");

            Directory.CreateDirectory(outputDir);

            using SevenZipArchive sevenZip = SevenZipArchive.Open(filePath);

            foreach (var entry in sevenZip.Entries.Where(x => !x.IsDirectory))
                entry.WriteToDirectory(outputDir, new ExtractionOptions()
                {
                    Overwrite = true,
                    ExtractFullPath = false
                });
        }
    }
}