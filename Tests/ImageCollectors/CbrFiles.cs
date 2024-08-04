using Xunit;
using System.IO;
using ComicConverter;
using System.Collections.Generic;

namespace Test.ImageCollector
{
    public class CbrFiles
    {

        [Fact]
        public void Unrar()
        {
            string outputDir = "CbrTest1";
            int extractedFiles;

            ImageExporter.UnRar(Samples.Cbrpath, outputDir);

            extractedFiles = Directory.GetFiles(outputDir).Length;
            Directory.Delete(outputDir, true);
            Assert.Equal(3, extractedFiles);
        }

        [Fact]
        public void FileIsNotRar()
        {
            string outputDir = "CbrTest2";
            Assert.Throws<System.FormatException>(() => ImageExporter.UnRar(Samples.Testpath, outputDir));
        }

        [Fact]
        public void FileNotFound()
        {
            string outputDir = "CbrTest3";
            Assert.Throws<FileNotFoundException>(() => ImageExporter.UnRar(Samples.Fakefile, outputDir));
        }

        [Fact]
        public void EmptyAttributeDirectory()
        {
            string outputDir = "CbrTest4";
            ImageExporter.UnRar(Samples.Cbrpath, outputDir);
            ImageExporter.UnRar(Samples.Cbrpath);

            List<string> filesExtracted = [.. Directory.GetFiles(".")];

            foreach (var file in Directory.GetFiles(outputDir))
            {
                if (!filesExtracted.Contains(file.Replace(outputDir, ".")))
                    Assert.True(false);
                else
                    File.Delete(file.Replace(outputDir, "."));
            }

            Directory.Delete(outputDir, true);
        }

        [Fact]
        public void EmptyStringDirectory()
        {
            Assert.Throws<System.FormatException>(() => ImageExporter.UnRar(Samples.Cbrpath, ""));
        }
    }
}
