using Xunit;
using System;
using System.IO;
using ComicConverter;

namespace Test
{
	public class ComicConverter
	{
		[Fact]
		public void ConvertCbr2Cbz()
		{
			Comic comic = new(Samples.CBRPATH);

			comic.Convert("Test", ComicFormat.CBZ);

			Assert.True(File.Exists("Test.cbz"));

			File.Delete("Test.cbz");
		}

		[Fact]
		public void ConvertCbz2Cbt()
		{
			Comic comic = new(Samples.CBZPATH);

			comic.Convert("Test", ComicFormat.CBT);

			Assert.True(File.Exists("Test.cbt"));

			File.Delete("Test.cbt");
		}

		[Fact]
		public void OutInvalidFormat()
		{
			Comic comic = new(Samples.CBZPATH);

			Assert.Throws<FormatException>(() => comic.Convert("Test", ComicFormat.CBR));
		}

		[Fact]
		public void InInvalidFormat()
		{
			Comic comic;

			Assert.Throws<FormatException>(() => comic = new(Samples.TESTPATH));
		}
	}
}