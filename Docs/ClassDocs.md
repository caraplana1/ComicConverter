# Class Documentation

List of class Components.

## Index

* [Comic](#comic-class)
* [ComicBuilder](#comicbuilder-class)
* [ImageExtractors](imageextractor-class)
* [ComicFormat](#comicformat-enum)

## Comic (class)

### Attributes

* **Path** [String]
  Comic File direction.

* **Format** [ComicFormat]
  Comic's format enum.

### Constructors

* **Comic(string comicPath)**

### Methods

* **Convert(string outputPath, ComicFormat format)** [void]
  Method to convert the comic to any supported format into any direction.

* **FindComicFormat()** [ComicFormat]
  Method to find the comic format. If the comic is not a file that is supported will throw an exception.

* **IsValidOutputFormat(ComicFormat format)** [bool]
  Verify is the output format to convert is suported.

* **FindExtractorImageAction(ComicFormat format)** [Action<string, string>]
  Find the correct method to extract images from the supported file given an format.

* **FindComicBuilderAction(ComicFormat format)** [Action<string[], string>]
  Find the correct Method to created a comic in the given supproted format.

## ComicBuilder (class)

### Methods

* **CreateCBZ(string[] imagesPaths, string fileName)** [void]
  Creates zip file but with cbz extension

* **CreateCBT(string[] imagesPaths, string fileName)** [void]
  Creates Tar file but with cbt extension

## ImageExtractors (class)

### Methods

* **UnRar(string filePath, string outputDir = ".")** [void]
Extract rar or cbr file in given directory.

* **UnZip(string filePath, string outputDir = ".")** [void]
Extract zip or cbz file in given directory.

* **UnTar(string filePath, string outputDir = ".")** [void]
Extract tar or cbt file in given directory.

* **UnSevenZip(string filePath, string outputDir = ".")** [void]
Extract 7z or cb7 file in given directory.

## ComicFormat (enum)

Enum to identify formats of comics.