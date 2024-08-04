using Test;
using Xunit;
using System;
using System.IO;
using ComicConverter;

namespace Tests.ComicCreators
{
    public class CbtFiles
    {
        private const string CbtPath = "CreatedCbtFileTest";
        
        [Fact]
        public void CreateCbt()
        {
            string[] files = Directory.GetFiles(Samples.Imagesdir);

            ComicBuilder.CreateCbt(files, CbtPath);

            Assert.True(File.Exists($"{CbtPath}.cbt"));

            File.Delete($"{CbtPath}.cbt");
        }

        [Fact]
        public void ImagesNotFound()
        {
            string[] files = ["fakefile.jpg", "fakefile1.jpg", "fakefile2.jpg"];

            Assert.Throws<IOException>(() => ComicBuilder.CreateCbt(files, CbtPath));
        }
    }
}