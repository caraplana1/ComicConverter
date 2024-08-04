using System;
using System.IO;
using System.Linq;
using ComicConverter;
using Test;
using Xunit;

namespace Tests.ImageCollectors
{
    public class CbtFiles
    {
        [Fact]
        public void ExtractImages()
        {
            string path = "CbtExport1";

            ImageExporter.UnTar(Samples.Cbtpath, path);
            int files = Directory.GetFiles(path).Length;
            Directory.Delete(path, true);
            
            Assert.Equal(3, files);

        }

        [Fact]
        public void FilesIsNotTar()
        {
            Assert.Throws<FormatException>(() => ImageExporter.UnTar(Samples.Cbrpath));
        }

        [Fact]
        public void FileNotFound()
        {
            Assert.Throws<FileNotFoundException>(() => ImageExporter.UnTar(Samples.Fakefile));
        }

        [Fact]
        public void EmptyAttributeDirectory()
        {
            string path = "CbtExport2";

            ImageExporter.UnTar(Samples.Cbtpath, path);
            ImageExporter.UnTar(Samples.Cbtpath);

            var files = Directory.GetFiles(path).ToList();
            var filesCurrentDir = Directory.GetFiles(".").ToList();

            foreach (var file in files)
            {
                Assert.Contains(file.Replace(path, "."), filesCurrentDir);
                File.Delete(file.Replace(path, "."));
            }

            Directory.Delete(path, true);
        }

        [Fact]
        public void EmptyStringDirectory()
        {
            Assert.Throws<FormatException>(() => ImageExporter.UnTar(Samples.Cbtpath, ""));
        }
    }
}