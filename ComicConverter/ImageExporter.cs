using System;
using SkiaSharp;
using System.IO;
using System.Linq;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using SharpCompress.Common;
using SharpCompress.Archives;
using PdfSharpCore.Pdf.Advanced;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.Tar;
using System.Runtime.InteropServices;

namespace ComicConverter
{
    public static class ImageExporter
    {
        /// <summary>
        /// Export rar or cbr file in given directory.
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
                throw new FormatException("The directory cannot be null or empty");

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
        /// Export zip or cbz file in given directory.
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
                throw new FormatException("The directory cannot be null or empty");

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
        /// Export tar or cbt file in given directory.
        /// </summary>
        /// <param name="filePath">File to extract</param>
        /// <param name="outputDir">Directory to store extracted files.</param>
        public static void UnTar(string filePath, string outputDir = ".")
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            if (string.IsNullOrEmpty(outputDir))
                throw new FormatException("The directory cannot be null or empty");

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
        /// Export images from pdf document.
        /// </summary>
        /// <param name="filePath">Pdf file</param>
        /// <param name="outputDir">Output directory</param>
        public static void ExportPdfImages(string filePath, string outputDir = ".")
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            if (!IsValidPDF(filePath) || outputDir?.Length == 0)
                throw new FormatException();

            DirectoryInfo dir = Directory.CreateDirectory(outputDir);
            FileInfo file = new(filePath);
            var counter = 0;

            var document = PdfReader.Open(file.FullName);

            foreach (var page in document.Pages)
            {
                PdfDictionary resources = page.Elements.GetDictionary("/Resources");
                PdfDictionary xObjects = resources?.Elements.GetDictionary("/XObject");

                if (xObjects == null) continue;
                var items = xObjects.Elements.Values;

                foreach (var item in items)
                {
                    PdfReference reference = item as PdfReference;
                    if (reference?.Value is PdfDictionary xObject && xObject.Elements.GetString("/Subtype") == "/Image")
                        ExportImage(xObject, $"{dir.FullName}/{file.Name.Split('.').First()}{++counter}");
                }
            }
        }

        /// <summary>
        /// Verify if the comic file PDF encrypted
        /// </summary>
        /// <returns>True if it is a valid pdf, false otherwise</returns>
        private static bool IsValidPDF(string path)
        {
            StreamReader file = new(path);
            var firstLine = file.ReadLine()?.Substring(0, 8);
            file.Close();

            return firstLine is "%PDF-1.4" or "%PDF-1.5";
        }

        /// <summary>
        /// Detects the image encoded and export the image.
        /// </summary>
        /// <param name="image">Bytes that represent an image.</param>
        /// <param name="name">Name of the stream to save the image.</param>
        private static void ExportImage(PdfDictionary image, string name)
        {
            string filter = image.Elements.GetName("/Filter");

            switch (filter)
            {
                case "/DCTDecode":
                    ExportJpegFromPdf(image, name);
                    break;
                case "/FlateDecode":
                    ExportPngImage(image, name);
                    break;
            }
        }

        /// <summary>
        /// Save a file with jpeg codification.
        /// </summary>
        /// <param name="image">Bytes that represent an image.</param>
        /// <param name="name">Name of the stream to save the image.</param>
        private static void ExportJpegFromPdf(PdfDictionary image, string name)
        {
            var stream = image.Stream.Value;
            FileStream fs = new($"{name}.jpeg", FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new(fs);
            bw.Write(stream);
            bw.Close();
        }

        /// <summary>
        /// Save a file with Png codification.
        /// </summary>
        /// <param name="image">Bytes that represent an image.</param>
        /// <param name="name">Name of the stream to save the image.</param>
        private static void ExportPngImage(PdfDictionary image, string name)
        {
            int width = image.Elements.GetInteger(PdfImage.Keys.Width);
            int height = image.Elements.GetInteger(PdfImage.Keys.Height);

            var canUnFilter = image.Stream.TryUnfilter();
            byte[] decodedBytes;

            if (canUnFilter)
                decodedBytes = image.Stream.Value;
            else
            {
                PdfSharpCore.Pdf.Filters.FlateDecode flate = new PdfSharpCore.Pdf.Filters.FlateDecode();
                decodedBytes = flate.Decode(image.Stream.Value, image);
            } 
            
            int bitsPerComponent = decodedBytes.Length * 8 / (width * height);
                

            SKColorType colorType = bitsPerComponent switch
            {
                1 => SKColorType.Alpha8,
                8 => SKColorType.Gray8,
                16 => SKColorType.Rgb565,
                24 => SKColorType.Rgb888x,
                32 => SKColorType.Rgba8888,
                _ => throw new Exception("Unknown Pixel format " + bitsPerComponent),
            };

            SKBitmap bitmap = new SKBitmap(width, height, colorType, bitsPerComponent <= 24 ? SKAlphaType.Opaque : SKAlphaType.Premul);
            var pixels = bitmap.GetPixels();

            // ReSharper disable once PossibleLossOfFraction
            // the loss of fraction is not possible due to bitsPerComponent is multiple of 8.
            int length = (int)Math.Ceiling((decimal)width * (bitsPerComponent / 8));

            for (int i = 0; i < height; i++)
            {
                var offset = i * length;
                Int64 scanOffset = i * bitmap.RowBytes;
                Marshal.Copy(decodedBytes, offset, new IntPtr(bitmap.GetAddress(0, 0).ToInt64() + scanOffset), length);
            }

            using var imagePng = SKImage.FromBitmap(bitmap);
            using var data = imagePng.Encode(SKEncodedImageFormat.Png, 100);
            using var filestream = File.OpenWrite($"{name}.png");
            data.SaveTo(filestream);
        }
    }
}
