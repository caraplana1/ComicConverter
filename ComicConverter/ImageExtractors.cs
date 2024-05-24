using System;
using System.IO;
using System.Linq;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.Advanced;
using PdfSharpCore.Pdf.IO;
using SharpCompress.Common;
using SharpCompress.Archives;
using System.Collections.Generic;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.SevenZip;

namespace ComicConverter
{
    public static class ImageExtractors
    {
        /// <summary>
        /// Extract rar or cbr file in given directory.
        /// </summary>
        /// <param name="filePath">File to extract</param>
        /// <param name="outputDir">Directory to store extracted files.</param>
        public static void UnRar(string filePath, string outputDir = ".")
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The file doesn't exists");

            if (!RarArchive.IsRarFile(filePath))
                throw new FormatException("The file is not a rar file");

            if (string.IsNullOrEmpty(outputDir))
                throw new FormatException("The directoty cannot be null or empty");

            Directory.CreateDirectory(outputDir);

            using RarArchive archive = RarArchive.Open(filePath);

            foreach (var entry in archive.Entries.Where(x => !x.IsDirectory))
            {
                entry.WriteToDirectory(outputDir, new ExtractionOptions()
                {
                    ExtractFullPath = false,
                    Overwrite = true
                });
            }
        }

        /// <summary>
        /// Extract zip or cbz file in given directory.
        /// </summary>
        /// <param name="filePath">File to extract.</param>
        /// <param name="outputDir">Directory to store extracted files.</param>
        public static void UnZip(string filePath, string outputDir = ".")
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            if (!ZipArchive.IsZipFile(filePath))
                throw new FormatException("The file is not a zip file");

            if (string.IsNullOrEmpty(outputDir))
                throw new FormatException("The directoty cannot be null or empty");

            Directory.CreateDirectory(outputDir);

            using ZipArchive zip = ZipArchive.Open(filePath);

            foreach (var entry in zip.Entries.Where(x => !x.IsDirectory))
            {
                entry.WriteToDirectory(outputDir, new ExtractionOptions()
                {
                    Overwrite = true,
                    ExtractFullPath = false
                });
            }
        }

        /// <summary>
        /// Extract tar or cbt file in given directory.
        /// </summary>
        /// <param name="filePath">File to extract</param>
        /// <param name="outputDir">Directory to store extracted files.</param>
        public static void UnTar(string filePath, string outputDir = ".")
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            if (string.IsNullOrEmpty(outputDir))
                throw new FormatException("The directoty cannot be null or empty");

            if (!TarArchive.IsTarFile(filePath))
                throw new FormatException("The file is not a tar file");

            Directory.CreateDirectory(outputDir);

            using TarArchive tar = TarArchive.Open(filePath);

            foreach (var entry in tar.Entries.Where(x => !x.IsDirectory))
            {
                entry.WriteToDirectory(outputDir, new ExtractionOptions()
                {
                    Overwrite = true,
                    ExtractFullPath = false
                });
            }
        }

        /// <summary>
        /// Extract 7z or cb7 file in given directory.
        /// </summary>
        /// <param name="filePath">File to extract</param>
        /// <param name="outputDir">Directory to store extracted files.</param>
        public static void UnSevenZip(string filePath, string outputDir = ".")
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            if (string.IsNullOrEmpty(outputDir))
                throw new FormatException("The directoty cannot be null or empty");

            if (!SevenZipArchive.IsSevenZipFile(filePath))
                throw new FormatException("The file is not a 7z file");

            Directory.CreateDirectory(outputDir);

            using SevenZipArchive sevenZip = SevenZipArchive.Open(filePath);

            foreach (var entry in sevenZip.Entries.Where(x => !x.IsDirectory))
            {
                entry.WriteToDirectory(outputDir, new ExtractionOptions()
                {
                    Overwrite = true,
                    ExtractFullPath = false
                });
            }
        }

        /// <summary>
        /// Extract Jpeg from pdf file to a directory.
        /// </summary>
        /// <param name="filePath">Pdf file</param>
        /// <param name="outputDir">Output directory</param>
        public static void ExtractPdfImages(string filePath, string outputDir = ".")
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            if (!IsValidPDF(filePath) || outputDir?.Length == 0)
                throw new FormatException();

            DirectoryInfo dir = Directory.CreateDirectory(outputDir);
            FileInfo file = new(filePath);
            int counter = 0;

            PdfDocument document = PdfReader.Open(file.FullName);

            foreach (var page in document.Pages)
            {
                PdfDictionary resources = page.Elements.GetDictionary("/Resources");
                if (resources is not null)
                {
                    PdfDictionary xObjects = resources.Elements.GetDictionary("/XObject");

                    if (xObjects is not null)
                    {
                        ICollection<PdfItem> items = xObjects.Elements.Values;

                        foreach (var item in items)
                        {
                            PdfReference reference = item as PdfReference;
                            if (reference is not null)
                            {
                                if (reference.Value is PdfDictionary xObject && xObject.Elements.GetString("/Subtype") == "/Image")
                                    ExtractJpegFromPdf(xObject, $"{dir.FullName}/{file.Name.Split('.').First()}{++counter}");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Write to a file the jpeg bytes.
        /// </summary>
        /// <param name="image">Bytes that represent an image.</param>
        /// <param name="name">Name of the stream to save the image.</param>
        private static void ExtractJpegFromPdf(PdfDictionary image, string name)
        {
            if (image.Elements.GetName("/Filter") != "/DCTDecode")
                return;

            byte[] stream = image.Stream.Value;
            FileStream fs = new($"{name}.jpeg", FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new(fs);
            bw.Write(stream);
            bw.Close();
        }

        /// <summary>
        /// Verify if the comic file PDF encrypted
        /// </summary>
        /// <returns>True if it is a valid pdf, false otherwise</returns>
        static private bool IsValidPDF(string path)
        {
            StreamReader file = new(path);
            string firstLine = file.ReadLine().Substring(0, 7);

            if (firstLine == "%PDF-1.")
                return true;
            else
                return false;
        }
    }
}
