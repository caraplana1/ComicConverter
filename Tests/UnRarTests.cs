using System.IO;
using Xunit;
using ComicConverter;
using System.Collections.Generic;
using System.Linq;

namespace DecompressorsTests
{
    public class UnrarTests
    {
        [Fact]
        public void UnrarTest()
        {
            string comicPath = "../../../Samples/CROSSED Wish you were here.cbr";
            string outputPath = "../../../Samples/Folder";
            string[] extractedFiles;

            try
            {
                Decompressor.UnRar(comicPath, outputPath);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            if (!Directory.Exists(outputPath))
                Assert.True(false);

            extractedFiles = Directory.GetFiles(outputPath);
            Directory.Delete(outputPath, true);
            Assert.True(extractedFiles.Length > 0);
        }

        [Fact]
        public void RarFileNotExists()
        {
            string filePath = "file.txt";
            string output = "Folder";

            Assert.Throws<FileNotFoundException>(() => Decompressor.UnRar(filePath, output));
        }

        [Fact]
        public void FileIsNotRar()
        {
            string filePath = "../../../Samples/File.test";

            Assert.Throws<System.FormatException>(() => Decompressor.UnRar(filePath, "Folder"));
        }

        [Fact]
        public void EmptyOutputFolder()
        {
            string filePath = "../../../Samples/CROSSED Wish you were here.cbr";
            List<string> filesExtracted;

            Decompressor.UnRar(filePath, "Folder");
            Decompressor.UnRar(filePath);

            filesExtracted = Directory.GetFiles(".").ToList<string>();

            for (int i = 0; i < filesExtracted.Count; i++)
                filesExtracted[i] = filesExtracted[i][2..];

            foreach (var file in Directory.GetFiles("Folder").ToList<string>())
            {
                if (!filesExtracted.Contains(file[7..]))
                    Assert.True(false);
                else
                    File.Delete(@".\\" + file[7..]);
            }

            Directory.Delete("Folder", true);
        }
    }
}
