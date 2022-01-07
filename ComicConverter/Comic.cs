using System;
using System.IO;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.SevenZip;

namespace ComicConverter
{
	public class Comic
	{
		#region Field Declaration

		public string Path {get;}
		public ComicFormat Format {get;}

		#endregion

		public Comic(string comicPath)
		{
			this.Path = comicPath;
			this.Format = GetComicFormat();

			if (!File.Exists(comicPath))
				throw new FileNotFoundException();
		}

		public void Convert(string outputPath, ComicFormat format)
		{
			if (!IsValidOutputFormat(format))
				throw new FormatException($"Can't convert comic to {format}");

			DirectoryInfo dir = Directory.CreateDirectory(".ConvertedImagesHiddenDir");
			dir.Attributes = FileAttributes.Hidden | FileAttributes.Directory;

			var ExtractImages = GetExtractorImageAction(this.Format);
			var BuildComic = GetComicBuilderAction(format);

			ExtractImages(this.Path, dir.Name);

			BuildComic(Directory.GetFiles(dir.Name), outputPath);

			dir.Delete(true);
		}

		private ComicFormat GetComicFormat()
		{
			if (RarArchive.IsRarFile(Path))
				return ComicFormat.CBR;
			if (ZipArchive.IsZipFile(Path))
				return ComicFormat.CBZ;
			if (TarArchive.IsTarFile(Path))
				return ComicFormat.CBT;
			if (SevenZipArchive.IsSevenZipFile(Path))
				return ComicFormat.CB7;

			throw new FormatException("The file cannot be used beacuse is not in a propper format.");
		}

		private bool IsValidOutputFormat(ComicFormat format)
		{
			if (format == ComicFormat.CBZ)
				return true;
			else if (format == ComicFormat.CBT)
				return true;

			return false;
		}

		private Action<string, string> GetExtractorImageAction(ComicFormat format)
		{
			return format switch
			{
				ComicFormat.CBR => ImageExtractors.UnRar,
				ComicFormat.CBZ => ImageExtractors.UnZip,
				ComicFormat.CBT => ImageExtractors.UnTar,
				ComicFormat.CB7 => ImageExtractors.UnSevenZip,
				_ => throw new FormatException(),
			};
		}

		private Action<string[],string> GetComicBuilderAction(ComicFormat format)
		{
			return format switch
			{
				ComicFormat.CBZ => ComicBuilder.CreateCBZ,
				ComicFormat.CBT => ComicBuilder.CreateCBT,
				_ => throw new FormatException(),
			};
		}
	}
}