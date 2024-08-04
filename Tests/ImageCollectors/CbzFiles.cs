using Test;
using Xunit;
using System.IO;
using ComicConverter;
using System.Collections.Generic;

namespace Tests.ImageCollectors
{
    public class CbzFiles
    {
        [Fact]
        public void UnZip()
        {
            const string path = "ZipExportDir";

            ImageExporter.UnZip(Samples.Cbzpath, path); // Descompress File.

            Assert.True(Directory.Exists(path)); // Makes sure the directory is created.

            int files = Directory.GetFiles(path).Length;
            Directory.Delete(path, true);

            Assert.Equal(3, files);
        }

        [Fact]
        public void FileIsNotZip()
        {
            Assert.Throws<System.FormatException>(() => ImageExporter.UnZip(Samples.Testpath));
        }

        [Fact]
        public void FileNotFound()
        {
            Assert.Throws<FileNotFoundException>(() => ImageExporter.UnZip("FakeFile.txt"));
        }

        [Fact]
        public void EmptyAttributeDirectory()
        {
            const string folderToCompare = "TestCbzFolderToCompare";

            // Extract the same file in the current directory and in the output directoty
            ImageExporter.UnZip(Samples.Cbzpath);
            List<string> filesExtracted = [.. Directory.GetFiles(".")];
            ImageExporter.UnZip(Samples.Cbzpath, folderToCompare);

            // Get a list of all files in current directory

            foreach (var file in Directory.GetFiles(folderToCompare))
            {
                // Verifies if the files in output directory are in the current and delete them.
                Assert.Contains(file.Replace(folderToCompare, "."), filesExtracted);
                File.Delete(file.Replace(folderToCompare, "."));
            }

            Directory.Delete(folderToCompare, true);
        }

        [Fact]
        public void EmptyStringDirectory()
        {
            Assert.Throws<System.FormatException>(() => ImageExporter.UnZip(Samples.Cbzpath, ""));
        }
    }
}