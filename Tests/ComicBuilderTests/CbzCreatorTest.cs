using Xunit;
using System;
using System.IO;
using System.Linq;
using ComicConverter;
using System.Collections.Generic;

namespace Tests.ComicBuilderTests
{
	public class CbzCreatorTest
	{
		#region Paths Region
		internal const string imagesDir = "../../../Samples/Images";
		private const string cbzPath = "Test";
		#endregion

		[Fact]
		public void CreateCbzFile()
		{
			Assert.True(Directory.Exists(imagesDir));

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