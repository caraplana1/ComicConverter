using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
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
using System.Runtime.InteropServices;

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
                                    ExportImage(xObject, $"{dir.FullName}/{file.Name.Split('.').First()}{++counter}");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Verify if the comic file PDF encrypted
        /// </summary>
        /// <returns>True if it is a valid pdf, false otherwise</returns>
        static private bool IsValidPDF(string path)
        {
            StreamReader file = new(path);
            string firstLine = file.ReadLine().Substring(0, 7);
            file.Close();

            if (firstLine == "%PDF-1.")
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="name"></param>
        static private void ExportImage(PdfDictionary image, string name)
        {
            string filter = image.Elements.GetName("/Filter");

            if (filter == "/DCTDecode")
                ExportJpegFromPdf(image, name);
            else if (filter == "/FlateDecode")
                ExportPngImage(image, name);
        }

        /// <summary>
        /// Write to a file the jpeg bytes.
        /// </summary>
        /// <param name="image">Bytes that represent an image.</param>
        /// <param name="name">Name of the stream to save the image.</param>
        private static void ExportJpegFromPdf(PdfDictionary image, string name)
        {
            byte[] stream = image.Stream.Value;
            FileStream fs = new($"{name}.jpeg", FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new(fs);
            bw.Write(stream);
            bw.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
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

            int bitsPerComponent = 0;
            while (decodedBytes.Length - ((width * height) * bitsPerComponent / 8) != 0)
                bitsPerComponent++;
            var pixelFormat = bitsPerComponent switch
            {
                1 => PixelFormat.Format1bppIndexed,
                8 => PixelFormat.Format8bppIndexed,
                16 => PixelFormat.Format16bppArgb1555,
                24 => PixelFormat.Format24bppRgb,
                32 => PixelFormat.Format32bppArgb,
                64 => PixelFormat.Format64bppArgb,
                _ => throw new Exception("Unkwon Pixel format " + bitsPerComponent),
            };

            Bitmap bmp = new(width, height, pixelFormat);

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);

            int length = (int)Math.Ceiling((double)width * (bitsPerComponent / 8));

            for (int i = 0; i < height; i++)
            {
                int offset = i * length;
                int scanOffset = i * bmpData.Stride;
                Marshal.Copy(decodedBytes, offset, new IntPtr(bmpData.Scan0.ToInt64() + scanOffset), length);
            }

            bmp.UnlockBits(bmpData);
            bmp.Save(name + ".png", ImageFormat.Png);
        }
    }
}
