using Xunit;
using System.IO;
using System.Linq;
using ComicConverter;
using System.Collections.Generic;

namespace ExtractorsTests
{
	public class PdfExtractorTests
	{
		#region Fields region

		internal readonly string pdfPath = "../../../Samples/Injustice Year #01.pdf"; 
		internal readonly string testPath = "../../../Samples/file.test";
		private readonly string pdfOutputDir = "PDfDirTest";

		#endregion

		[Fact]
		public void ExtractPdfImagesTest()
		{
			ImageExtractor.ExtractPdfImages(pdfPath, pdfOutputDir);

			Assert.True(Directory.Exists(pdfOutputDir));
			Assert.Equal(23, Directory.GetFiles(pdfOutputDir).Length);
			Directory.Delete(pdfOutputDir, true);
		}

		[Fact]
		public void IsNotPdfFileTest()
		{
			Assert.Throws<System.FormatException>(() => ImageExtractor.ExtractPdfImages(testPath));
		}

		[Fact]
		public void FileNotFound()
		{
			Assert.Throws<FileNotFoundException>(() => ImageExtractor.ExtractPdfImages("Nowhere.txt"));
		}

		[Fact]
		public void OutputDirEmptyString()
		{
			Assert.Throws<System.FormatException>(() => ImageExtractor.ExtractPdfImages(pdfPath, ""));
		}

		[Fact]
		public void GiveOnlyFilePath()
		{
			string testDir = "pdfExtracted";

			ImageExtractor.ExtractPdfImages(pdfPath);
			ImageExtractor.ExtractPdfImages(pdfPath, testDir);

			string[] extractedFiles = Directory.GetFiles(".");

			foreach (var file in Directory.GetFiles(testDir))
			{
				Assert.Contains(file.Replace(testDir, "."), extractedFiles);
				File.Delete(file.Replace(testDir, "."));
			}

			Directory.Delete(testDir, true);
		}
	}
}