using System;
using System.IO;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.SevenZip;

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
        public string Path { get; }

        /// <summary>
        /// The given comic's format enum.
        /// </summary>
        /// <value>Enum</value>
        public ComicFormat Format { get; }

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
        /// Constructor
        /// </summary>
        /// <param name="comicPath">Direction of the comic file.</param>
        /// <param name="format">Specify the format</param>
        /// <exception cref="FileNotFoundException"></exception>
        public Comic(string comicPath, ComicFormat format)
        {
            if (!File.Exists(comicPath))
                throw new FileNotFoundException();
            else
                Path = comicPath;

            Format = format;
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

            DirectoryInfo dir = Directory.CreateDirectory(".ConvertedImagesHiddenDir");
            dir.Attributes = FileAttributes.Hidden | FileAttributes.Directory;

            var ExtractImages = FindExtractorImageAction(Format);
            var BuildComic = FindComicBuilderAction(format);

            ExtractImages(Path, dir.Name);

            BuildComic(Directory.GetFiles(dir.Name), outputPath);

            dir.Delete(true);
        }

        /// <summary>
        /// Method to find the comic format. If the comic is not a file that is supported will throw an exception.
        /// </summary>
        /// <returns>The format enum</returns>
        private ComicFormat FindComicFormat()
        {
            if (RarArchive.IsRarFile(Path))
                return ComicFormat.CBR;
            if (ZipArchive.IsZipFile(Path))
                return ComicFormat.CBZ;
            if (TarArchive.IsTarFile(Path))
                return ComicFormat.CBT;
            if (SevenZipArchive.IsSevenZipFile(Path))
                return ComicFormat.CB7;
            if (IsValidPDF())
                return ComicFormat.PDF;

            throw new FormatException("File is not in proper format");
        }

        /// <summary>
        /// Verify if the comic file PDF encrypted
        /// </summary>
        /// <returns>True if it is a valid pdf, false otherwise</returns>
        private bool IsValidPDF()
        {
            StreamReader file = new(Path);
            string firstLine = file.ReadLine().Substring(0, 7);
            file.Close();

            if (firstLine == "%PDF-1.")
                return true;
            else
                return false;
        }

        /// <summary>
        /// Verify is the output format to convert is suported.
        /// </summary>
        /// <param name="format">Format enum to compare</param>
        /// <returns>True if it's valid false otherwise.</returns>
        private bool IsValidOutputFormat(ComicFormat format)
        {
            if (format == ComicFormat.CBZ)
                return true;
            if (format == ComicFormat.CBT)
                return true;
            if (format == ComicFormat.PDF)
                return true;

            return false;
        }

        /// <summary>
        /// Find the correct method to extract images from the supported file given an format.
        /// </summary>
        /// <param name="format">The format of the file to extract images.</param>
        /// <returns>The correct method to extract image to the correct format file.</returns>
        private Action<string, string> FindExtractorImageAction(ComicFormat format)
        {
            return format switch
            {
                ComicFormat.CBR => ImageExtractors.UnRar,
                ComicFormat.CBZ => ImageExtractors.UnZip,
                ComicFormat.CBT => ImageExtractors.UnTar,
                ComicFormat.CB7 => ImageExtractors.UnSevenZip,
                ComicFormat.PDF => ImageExtractors.ExtractPdfImages,
                _ => throw new FormatException(),
            };
        }

        /// <summary>
        /// Find the correct Method to created a comic in the given supproted format.
        /// </summary>
        /// <param name="format">The end format of the file to create</param>
        /// <returns>The Method to transform the images to a supported comic file.</returns>
        private Action<string[], string> FindComicBuilderAction(ComicFormat format)
        {
            return format switch
            {
                ComicFormat.CBZ => ComicBuilder.CreateCBZ,
                ComicFormat.CBT => ComicBuilder.CreateCBT,
                ComicFormat.PDF => ComicBuilder.CreatePdf,
                _ => throw new FormatException(),
            };
        }
    }
}