using Xunit;
using System.IO;
using ComicConverter;

namespace Test.ComicCreators
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
            string[] fakeFiles = ["fakefile1.jpg", "fakefile2.jpg", "fakefile3.jpg"];

            Assert.Throws<IOException>(() => ComicBuilder.CreateCbt(fakeFiles, CbtPath));
        }
    }
}