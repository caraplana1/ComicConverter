using Xunit;
using System.IO;
using ComicConverter;

namespace Test.ComicCreators
{
    public class CbzFiles
    {
        private const string CbzPath = "CreatedCbzFileTest";

        [Fact]
        public void CreateCbz()
        {
            string[] images = Directory.GetFiles(Samples.Imagesdir);

            ComicBuilder.CreateCbz(images, CbzPath);

            Assert.True(File.Exists($"{CbzPath}.cbz"));

            File.Delete($"{CbzPath}.cbz");
        }

        [Fact]
        public void ImagesNotFound()
        {
            string[] files = ["Fakename", "fakename2", "fakename3"];

            Assert.Throws<IOException>(() => ComicBuilder.CreateCbz(files, CbzPath));
        }
    }
}