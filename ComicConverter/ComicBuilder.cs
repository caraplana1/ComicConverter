using System;
using System.IO;
using System.Linq;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using SharpCompress.Common;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.Tar;

namespace ComicConverter
{
    public static class ComicBuilder
    {
        /// <summary>
        /// Creates zip file but with cbz extension
        /// </summary>
        /// <param name="imagesPaths">Arrays of paths to  the images to include in the file</param>
        /// <param name="fileName">Name of the final document. Don't need to add the extension name</param>
        public static void CreateCbz(string[] imagesPaths, string fileName)
        {
            DirectoryInfo dir = CreateHiddenDir(imagesPaths, fileName.Replace('\\', '/').Split('/').Last() + "CBZ");

            using (var archive = ZipArchive.Create())
            {
                archive.AddAllFromDirectory(dir.Name);
                archive.SaveTo($"{fileName}.cbz", CompressionType.Deflate);
            }

            dir.Delete(true);
        }

        /// <summary>
        /// Creates tar file but with cbt extension
        /// </summary>
        /// <param name="imagesPaths">Arrays of paths to  the images to include in the file</param>
        /// <param name="fileName">Name of the final document. Don't need to add the extension name</param>
        public static void CreateCbt(string[] imagesPaths, string fileName)
        {
            DirectoryInfo dir = CreateHiddenDir(imagesPaths, fileName.Replace('\\', '/').Split('/').Last() + "CBT");

            using (var archive = TarArchive.Create())
            {
                archive.AddAllFromDirectory(dir.Name);
                archive.SaveTo($"{fileName}.cbt", CompressionType.None);
            }

            dir.Delete(true);
        }

        /// <summary>
        /// Creates Pdf Files with an image per page.
        /// </summary>
        /// <param name="imagesPaths">Arrays of paths to  the images to include in the file</param>
        /// <param name="fileName">Name of the final document. Don't need to add the extension name</param>
        public static void CreatePdf(string[] imagesPaths, string fileName)
        {
            imagesPaths = imagesPaths.Where(File.Exists).ToArray();
            imagesPaths = imagesPaths.Where(f => f.EndsWith(".PNG", StringComparison.OrdinalIgnoreCase)
                                            || f.EndsWith(".JPEG", StringComparison.OrdinalIgnoreCase)
                                            || f.EndsWith(".JPG", StringComparison.OrdinalIgnoreCase)).ToArray();

            if (imagesPaths.Length is 0)
                throw new IOException();
            
            PdfDocument document = new();
            document.Info.Title = fileName.Replace('\\', '/').Split('/').Last();

            foreach (var image in imagesPaths)
            {
                var imageFile = XImage.FromFile(image);

                var page = document.AddPage();
                page.MediaBox = new PdfRectangle(new XPoint(0, 0), (XPoint)imageFile.Size);

                var graphics = XGraphics.FromPdfPage(page);
                graphics.DrawImage(imageFile, 0, 0);
            }

            document.Save($"{fileName}.pdf");
        }

        /// <summary>
        /// Create a hidden directory and copy all files specified.
        /// </summary>
        /// <param name="filesPaths">Files wanted to copy on the directory</param>
        /// <param name="dirName">Name of the directory to create.</param>
        /// <returns>Directory info class</returns>
        private static DirectoryInfo CreateHiddenDir(string[] filesPaths, string dirName)
        {
            DirectoryInfo dir = Directory.CreateDirectory(dirName);
            dir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

            if (filesPaths.All(f => !File.Exists(f)))
                throw new IOException("There is no file to add");

            filesPaths = filesPaths.Where(File.Exists).ToArray();

            foreach (var image in filesPaths)
            {
                FileInfo fileInfo = new(image);
                File.Copy(fileInfo.FullName, $"{dirName}/{fileInfo.Name}", true);
            }

            return dir;
        }
    }
}