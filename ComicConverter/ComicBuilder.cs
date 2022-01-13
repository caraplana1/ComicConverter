using System.IO;
using System.Linq;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using SharpCompress.Common;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.Tar;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

namespace ComicConverter
{
	public static class ComicBuilder
	{
		/// <summary>
		/// Creates zip file but with cbz extension
		/// </summary>
		/// <param name="imagesPath">Arrays of paths to  the images to include in the file</param>
		/// <param name="fileName">Name of the final document. Dont need to add the extension name</param>
		public static void CreateCBZ(string[] imagesPaths, string fileName)
		{
			DirectoryInfo dir = CreateHiddenDir(imagesPaths, $"{fileName.Split('/').Last()}CBZ");

			using (var archive = ZipArchive.Create())
			{
				archive.AddAllFromDirectory(dir.Name);
				archive.SaveTo($"{fileName}.cbz", CompressionType.Deflate);
			}

			dir.Delete(true);
		}

		/// <summary>
		/// Creates tar file but with cbt extension
		/// </summary>
		/// <param name="imagesPaths">Arrays of paths to  the images to include in the file</param>
		/// <param name="fileName">Name of the final document. Dont need to add the extension name</param>
		public static void CreateCBT(string[] imagesPaths, string fileName)
		{
			DirectoryInfo dir = CreateHiddenDir(imagesPaths, $"{fileName.Split('/').Last()}CBT");

			using (var archive = TarArchive.Create())
			{
				archive.AddAllFromDirectory(dir.Name);
				archive.SaveTo($"{fileName}.cbt", CompressionType.None);
			}

			dir.Delete(true);
		}

		public static void CreatePdf(string[] imagesPaths, string fileName)
		{
			imagesPaths = imagesPaths.Where(f => File.Exists(f)).ToArray();
			imagesPaths = imagesPaths.Where(f => f.ToUpper().EndsWith(".PNG") || f.ToUpper().EndsWith(".JPEG") || f.ToUpper().EndsWith(".JPG")).ToArray();

			PdfDocument document = new();
			document.Info.Title = fileName;
			PdfPage page;
			XGraphics graphics;
			XImage imageFile;
<<<<<<< HEAD
=======
			double x;
>>>>>>> fa9bb62 (Pdf File Created, need to modify scale)

			foreach (var image in imagesPaths)
			{
				page = document.AddPage();
<<<<<<< HEAD
				imageFile = XImage.FromFile(image);

				// TODO: Calculate a PageSize for an image size
				// // page.Size = CalculatePageSize(imageFile.Size);
				graphics = XGraphics.FromPdfPage(page);

				graphics.DrawImage(imageFile, 0, 0);
=======
				graphics = XGraphics.FromPdfPage(page);

				imageFile = XImage.FromFile(image);
				x = (250 - imageFile.PixelWidth * 72 / imageFile.HorizontalResolution) / 2;
				graphics.DrawImage(imageFile, x, 0);
>>>>>>> fa9bb62 (Pdf File Created, need to modify scale)
			}

			document.Save($"{fileName}.pdf");
		}

		/// <summary>
		/// Simple funtion to create a hidden directory and copy all files specificataed.
		/// </summary>
		/// <param name="filesPaths">Files wanted to copy on the directory</param>
		/// <returns>Directory info</returns>
		private static DirectoryInfo CreateHiddenDir(string[] filesPaths, string dirName)
		{
			DirectoryInfo dir = Directory.CreateDirectory(dirName);
			dir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

			if (filesPaths.All(f => !File.Exists(f))) 
				throw new IOException("There is no file to add");
			
			filesPaths = filesPaths.Where(f => File.Exists(f)).ToArray();
			FileInfo fileInfo;

			foreach (var image in filesPaths)
			{
				fileInfo = new(image);
				File.Copy(fileInfo.FullName, $"{dirName}/{fileInfo.Name}", true);
			}

			return dir;
		}

		private static PageSize CalculatePageSize(XSize imageSize)
		{
			// TODO: Implement Size Calculation https://www.prepressure.com/library/paper-size
			throw new System.NotImplementedException();
		}
	}
}