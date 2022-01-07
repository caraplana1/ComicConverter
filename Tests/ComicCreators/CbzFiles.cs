using Xunit;
using System.IO;
using ComicConverter;

namespace Test.ComicCreators
{
	public class CbzFiles
	{
		#region Paths Region
		internal const string imagesDir = "../../../Samples/Images";
		private const string cbzPath = "Test";
		#endregion

		[Fact]
		public void CreateCbzFile()
		{
			string[] images = Directory.GetFiles(imagesDir);

			ComicBuilder.CreateCBZ(images, cbzPath);

			Assert.True(File.Exists($"{cbzPath}.cbz"));

			File.Delete($"{cbzPath}.cbz");
		}

		[Fact]
		public void ImagesNotFound()
		{
			string[] files = {"Fakename", "fakename2", "fakename3"};

			Assert.Throws<IOException>(() => ComicBuilder.CreateCBZ(files, cbzPath));
		}
	}
}