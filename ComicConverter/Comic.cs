using System;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.SevenZip;
using System.Linq;
using System.Collections.Generic;

namespace ComicConverter
{
	public class Comic
	{
		#region Field Declaration

		private readonly string comicPath;
		private ComicFormat comicFormat;

		#endregion

		public Comic(string comicPath)
		{
			this.comicPath = comicPath;
			this.comicFormat = GetComicFormat();
		}

		public void Convert(string outputPath, ComicFormat format)
		{
			if (!IsValidOutputFormat(format))
				throw new FormatException($"Can't convert comic to {format}");
		}

		private ComicFormat GetComicFormat()
		{
			if (RarArchive.IsRarFile(comicPath))
				return ComicFormat.CBR;
			if (ZipArchive.IsZipFile(comicPath))
				return ComicFormat.CBZ;
			if (TarArchive.IsTarFile(comicPath))
				return ComicFormat.CBT;
			if (SevenZipArchive.IsSevenZipFile(comicPath))
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
			switch (format)
			{
				case ComicFormat.CBR: return ImageExtractors.UnRar;

				case ComicFormat.CBZ : return ImageExtractors.UnZip;

				case ComicFormat.CBT: return ImageExtractors.UnTar;

				case ComicFormat.CB7: return ImageExtractors.UnSevenZip;
			}
		}
	}
}