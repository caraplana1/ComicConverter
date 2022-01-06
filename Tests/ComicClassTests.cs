using Xunit;
using System;
using System.IO;
using ComicConverter;

namespace Tests
{
	public class ComicClassTests
	{
		private const string cbrPath = "../../../Samples/CROSSED Wish you were here.cbr";
		private const string cbzPath = "../../../Samples/Crossed 3D.cbz";
		private const string testPath = "../../../Samples/File.test";

		[Fact]
		public void TransformComicFromCbr2Cbz()
		{
			Comic comic = new(cbrPath);

			comic.Convert("Test", ComicFormat.CBZ);

			Assert.True(File.Exists("Test.cbz"));
		}

		[Fact]
		public void TransformComicFromCbz2Cbt()
		{
			Comic comic = new(cbzPath);

			comic.Convert("Test", ComicFormat.CBT);

			Assert.True(File.Exists("Test.cbt"));
		}

		[Fact]
		public void ConvertToInvalidFormat()
		{
			Comic comic = new(cbzPath);

			Assert.Throws<FormatException>(() => comic.Convert("Test", ComicFormat.CBR));
		}

		[Fact]
		public void InputComicInvalidFormat()
		{
			Comic comic;

			Assert.Throws<FormatException>(() => comic = new(testPath));
		}
	}
}