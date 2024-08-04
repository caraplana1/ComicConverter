using System;
using System.IO;
using ComicConverter.Enums;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.Tar;

namespace ComicConverter
{
    /// <summary>
    /// Class to represent a comic. Only contains the direction info and format.
    /// </summary>
    public class Comic
    {
        #region Field Declaration

        /// <summary>
        ///	Comic File direction.
        /// </summary>
        /// <value>String</value>
        private string Path { get; }

        /// <summary>
        /// Comic's format enum.
        /// </summary>
        /// <value>Enum</value>
        private ComicFormat Format { get; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="comicPath">Direction of the comic file.</param>
        public Comic(string comicPath)
        {
            if (!File.Exists(comicPath))
                throw new FileNotFoundException();
            else
                Path = comicPath;

            Format = FindComicFormat();
        }

        /// <summary>
        /// Convert the file to the given format.
        /// </summary>
        /// <param name="outputPath">File name to be converted.(without extension)</param>
        /// <param name="format">Format to be converted.</param>
        /// <exception cref="FormatException"></exception>
        public void Convert(string outputPath, ComicFormat format)
        {
            if (!IsValidOutputFormat(format))
                throw new FormatException($"Can't convert comic to {format}");

            var dir = Directory.CreateDirectory(".ConvertedImagesHiddenDir");
            dir.Attributes = FileAttributes.Hidden | FileAttributes.Directory;

            var extractImages = FindExtractorImageAction(Format);
            var buildComic = FindComicBuilderAction(format);

            extractImages(Path, dir.Name);

            buildComic(Directory.GetFiles(dir.Name), outputPath);

            dir.Delete(true);
        }

        /// <summary>
        /// Method to find the comic format. If the comic is not a file that is supported will throw an exception.
        /// </summary>
        /// <returns>The format enum</returns>
        private ComicFormat FindComicFormat()
        {
            if (RarArchive.IsRarFile(Path))
                return ComicFormat.Cbr;
            if (ZipArchive.IsZipFile(Path))
                return ComicFormat.Cbz;
            if (TarArchive.IsTarFile(Path))
                return ComicFormat.Cbt;
            if (IsValidPdf())
                return ComicFormat.Pdf;

            throw new FormatException("File is not in proper format");
        }

        /// <summary>
        /// Verify if the comic file PDF encrypted
        /// </summary>
        /// <returns>True if it is a valid pdf, false otherwise</returns>
        private bool IsValidPdf()
        {
            StreamReader file = new(Path);
            var firstLine = file.ReadLine()?.Substring(0, 8);
            file.Close();

            return firstLine is "%PDF-1.4" or "%PDF-1.5";
        }

        /// <summary>
        /// Verify is the output format to convert is supported.
        /// </summary>
        /// <param name="format">Format enum to compare</param>
        /// <returns>True if it's valid false otherwise.</returns>
        private static bool IsValidOutputFormat(ComicFormat format)
            => format is ComicFormat.Cbz or ComicFormat.Cbt or ComicFormat.Pdf;
        

        /// <summary>
        /// Find the correct method to extract images from the supported file given a format.
        /// </summary>
        /// <param name="format">The format of the file to extract images.</param>
        /// <returns>The correct method to extract image to the correct format file.</returns>
        private Action<string, string> FindExtractorImageAction(ComicFormat format)
        {
            return format switch
            {
                ComicFormat.Cbr => ImageExporter.UnRar,
                ComicFormat.Cbz => ImageExporter.UnZip,
                ComicFormat.Cbt => ImageExporter.UnTar,
                ComicFormat.Pdf => ImageExporter.ExportPdfImages,
                _ => throw new FormatException(),
            };
        }

        /// <summary>
        /// Find the correct Method to create a comic in the given supported format.
        /// </summary>
        /// <param name="format">The end format of the file to create</param>
        /// <returns>The Method to transform the images to a supported comic file.</returns>
        private Action<string[], string> FindComicBuilderAction(ComicFormat format)
        {
            return format switch
            {
                ComicFormat.Cbz => ComicBuilder.CreateCbz,
                ComicFormat.Cbt => ComicBuilder.CreateCbt,
                ComicFormat.Pdf => ComicBuilder.CreatePdf,
                _ => throw new FormatException(),
            };
        }
    }
}