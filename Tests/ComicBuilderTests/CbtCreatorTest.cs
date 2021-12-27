using Xunit;
using System.IO;
using ComicConverter;

namespace Tests.ComicBuilderTests
{
	public class CbtCreatorTest
	{
		[Fact]
		public void CreateCbtTest()
		{
			string[] files = Directory.GetFiles(CbzCreatorTest.imagesDir);

			ComicBuilder.CreateCBT(files, "CbtTest");

			Assert.True(File.Exists("CbtTest.cbt"));

			File.Delete("CbtTest.cbt");
		}

		[Fact]
		public void ImageNotFound()
		{
			string[] fakeFiles = {"fakefile1.jpg", "fakefile2.jpg", "fakefile3.jpg"};

			Assert.Throws<IOException>(() => ComicBuilder.CreateCBT(fakeFiles, "cbtTest"));
		}
	}
}