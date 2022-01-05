using Xunit;
using System.IO;
using System.Linq;
using ComicConverter;
using System.Collections.Generic;

namespace Tests
{
	public class ComicClassTests
	{
		private const string cbrPath = "../../../Samples/CROSSED Wish you were here.cbr";
		private const string cbzPath = "../../../Samples/Crossed 3D.cbz";

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
			Comic comic = new(cbzPath, ComicFormat.CBZ);

			comic.Convert("Test", ComicFormat.CBT);

			Assert.True(File.Exists("Test.cbt"));
		}
	}
}