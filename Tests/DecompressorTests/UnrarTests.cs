using Xunit;
using System.IO;
using System.Linq;
using ComicConverter;
using System.Collections.Generic;

namespace DecompressorsTests
{
    public class UnrarTests
    {
        /// <summary>
        ///  Test the unrar method with normal parameters
        /// </summary>
        [Fact]
        public void UnrarTest()
        {
            string comicPath = "../../../Samples/CROSSED Wish you were here.cbr";
            string outputPath = "../../../Samples/Folder";
            string[] extractedFiles;

            Decompressor.UnRar(comicPath, outputPath);

            if (!Directory.Exists(outputPath))
                Assert.True(false);

            extractedFiles = Directory.GetFiles(outputPath);
            Directory.Delete(outputPath, true);
            Assert.True(extractedFiles.Length > 0);
        }

        /// <summary>
        /// Test if the unrar method can handle a file that doesn't exists
        /// </summary>
        [Fact]
        public void RarFileNotExists()
        {
            string filePath = "file.txt";
            string output = "Folder";

            Assert.Throws<FileNotFoundException>(() => Decompressor.UnRar(filePath, output));
        }

        /// <summary>
        /// Test if the unrar method can handle a non rar file.
        /// </summary>
        [Fact]
        public void FileIsNotRar()
        {
            string filePath = "../../../Samples/File.test";

            Assert.Throws<System.FormatException>(() => Decompressor.UnRar(filePath, "Folder"));
        }

        /// <summary>
        /// Invoke the method but with just one parameter
        /// </summary>
        [Fact]
        public void EmptyOutputFolder()
        {
            string filePath = "../../../Samples/CROSSED Wish you were here.cbr";

            Decompressor.UnRar(filePath, "Folder");
            Decompressor.UnRar(filePath);

            List<string> filesExtracted = Directory.GetFiles(".").ToList<string>();

            foreach (var file in Directory.GetFiles("Folder"))
            {
                if (!filesExtracted.Contains(file.Replace("Folder", ".")))
                    Assert.True(false);
                else
                    File.Delete(file.Replace("Folder", "."));
            }

            Directory.Delete("Folder", true);
        }
    }
}
