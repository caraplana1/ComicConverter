# Class Documentation

List of class Components.

## Index

* [Comic](#comic-class)
* [ComicBuilder](#comicbuilder-class)
* [ImageExtractors](#imageextractors-class)
* [ComicFormat](#comicformat-enum)

## Comic (class)

### Attributes

* `Path` - String <br/>
  Comic File direction.

* `Format` - ComicFormat <br/>
  Comic's format enum.

### Constructors

* `Comic(string comicPath)`

### Methods

* `Convert(string outputPath, ComicFormat format)` - void <br/>
  Method to convert the comic to any supported format into any direction.

* `FindComicFormat()` - ComicFormat <br/>
  Method to find the comic format. If the comic is not a file that is supported will throw an exception.

* `IsValidOutputFormat(ComicFormat format)` - bool <br/>
  Verify is the output format to convert is suported.

* `FindExtractorImageAction(ComicFormat format)` - Action<string, string> <br/>
  Find the correct method to extract images from the supported file given an format.

* `FindComicBuilderAction(ComicFormat format)` - Action<string[], string> <br/>
  Find the correct Method to created a comic in the given supproted format.

## ComicBuilder (class)

### Methods

* `CreateCBZ(string[] imagesPaths, string fileName)` - void <br/>
  Creates zip file but with cbz extension

* `CreateCBT(string[] imagesPaths, string fileName)` - void <br/>
  Creates Tar file but with cbt extension

* `CreatePdf(string[] imagesPaths, string fileName)` - void <br/>
  Creates Pdf Files with a image per page.

## ImageExtractors (class)

### Methods

* `UnRar(string filePath, string outputDir = ".")` - void <br/>
Extract rar or cbr file in given directory.

* `UnZip(string filePath, string outputDir = ".")` - void <br/>
Extract zip or cbz file in given directory.

* `UnTar(string filePath, string outputDir = ".")` - void <br/>
Extract tar or cbt file in given directory.

* `UnSevenZip(string filePath, string outputDir = ".")` - void <br/>
Extract 7z or cb7 file in given directory.

## ComicFormat (enum)

Enum to identify formats of comics.
