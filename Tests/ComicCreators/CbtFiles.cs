using Xunit;
using System.IO;
using ComicConverter;

namespace Test.ComicCreators
{
    public class CbtFiles
    {
        private const string cbtPath = "CreatedCbtFileTest";

        [Fact]
        public void CreateCbt()
        {
            string[] files = Directory.GetFiles(Samples.IMAGESDIR);

            ComicBuilder.CreateCBT(files, cbtPath);

            Assert.True(File.Exists($"{cbtPath}.cbt"));

            File.Delete($"{cbtPath}.cbt");
        }

        [Fact]
        public void ImagesNotFound()
        {
            string[] fakeFiles = ["fakefile1.jpg", "fakefile2.jpg", "fakefile3.jpg"];

            Assert.Throws<IOException>(() => ComicBuilder.CreateCBT(fakeFiles, cbtPath));
        }
    }
}