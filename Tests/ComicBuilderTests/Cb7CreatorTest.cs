using Xunit;
using System.IO;
using ComicConverter;

namespace Tests.ComicBuilderTests
{
	public class Cb7CreatorTest
	{
		[Fact]
		public void CreateCb7Test()
		{
			string[] images = Directory.GetFiles(CbzCreatorTest.imagesDir);

			ComicBuilder.CreateCB7(images, "Cb7Test");

			Assert.True(File.Exists("Cb7Test.cb7"));

			File.Delete("Cb7Test.cb7");
		}

		[Fact]
		public void ImageNotFound()
		{
			string[] falseImages = {"fake1.jpg", "fake2.jpg", "fake3.jpg"};

			Assert.Throws<IOException>(() => ComicBuilder.CreateCB7(falseImages, "Test"));
		}
	}
}