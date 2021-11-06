using System.IO;
using Xunit;
using ComicConverter;

namespace Tests
{
    public class DescompresorsTests
    {
        [Fact]
        public void UnrarTest()
        {
            string comicPath = "../../../Samples/CROSSED Wish you were here.cbr";
            string outputPath = "../../../Samples/Folder";
            string[] extractedFiles;

            try
            {
                Descompresor.UnRar(comicPath, outputPath);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            if (!Directory.Exists(outputPath))
                Assert.True(false);

            extractedFiles = Directory.GetFiles(outputPath);
            Assert.True(extractedFiles.Length > 0);
        }
    }
}
