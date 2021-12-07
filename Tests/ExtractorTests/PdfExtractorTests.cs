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
			ImageExtractors.ExtractImages(pdfPath, pdfOutputDir);

			Assert.True(Directory.Exists(pdfOutputDir));
			Assert.Equal(23, Directory.GetFiles(pdfOutputDir).Length);
			Directory.Delete(pdfOutputDir, true);
		}

		[Fact]
		public void IsNotPdfFileTest()
		{
			Assert.Throws<System.FormatException>(() => ImageExtractors.ExtractImages(testPath));
		}

		[Fact]
		public void FileNotFound()
		{
			Assert.Throws<FileNotFoundException>(() => ImageExtractors.ExtractImages("Nowhere.txt"));
		}

		[Fact]
		public void OutputDirEmptyString()
		{
			Assert.Throws<System.FormatException>(() => ImageExtractors.ExtractImages(pdfPath, ""));
		}

		[Fact]
		public void GiveOnlyFilePath()
		{
			string testDir = "pdfExtracted";

			ImageExtractors.ExtractImages(pdfPath);
			ImageExtractors.ExtractImages(pdfPath, testDir);

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