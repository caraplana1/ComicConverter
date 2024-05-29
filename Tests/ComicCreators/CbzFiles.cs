using Xunit;
using System.IO;
using ComicConverter;

namespace Test.ComicCreators
{
    public class CbzFiles
    {
        private const string cbzPath = "CreatedCbzFileTest";

        [Fact]
        public void CreateCbz()
        {
            string[] images = Directory.GetFiles(Samples.IMAGESDIR);

            ComicBuilder.CreateCBZ(images, cbzPath);

            Assert.True(File.Exists($"{cbzPath}.cbz"));

            File.Delete($"{cbzPath}.cbz");
        }

        [Fact]
        public void ImagesNotFound()
        {
            string[] files = ["Fakename", "fakename2", "fakename3"];

            Assert.Throws<IOException>(() => ComicBuilder.CreateCBZ(files, cbzPath));
        }
    }
}