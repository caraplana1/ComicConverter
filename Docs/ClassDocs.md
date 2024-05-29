# Class Documentation

List of class.

## Index

* [Comic](#comic-class)
* [ComicBuilder](#comicbuilder-class)
* [ImageExporter](#imageexporter-class)
* [ComicFormat](#comicformat-enum)

## Comic (class)

### Attributes

* `Path` <br/>
  Comic File direction.

* `Format` <br/>
  Comic's format enum.

### Constructors

* `Comic(string comicPath)`

### Methods

* `Convert(string outputPath, ComicFormat format)`
  Method to convert the comic to any supported format.

## ComicBuilder (class)

### Methods

* `CreateCBZ(string[] imagesPaths, string fileName)` <br/>
  Creates zip file but with cbz extension

* `CreateCBT(string[] imagesPaths, string fileName)` <br/>
  Creates Tar file but with cbt extension

* `CreatePdf(string[] imagesPaths, string fileName)` <br/>
  Creates Pdf Files with a image per page.

## ImageExporter (class)

### Methods

* `UnRar(string filePath, string outputDir = ".")` <br/>
Extract rar or cbr file in given directory.

* `UnZip(string filePath, string outputDir = ".")` <br/>
Extract zip or cbz file in given directory.

* `UnTar(string filePath, string outputDir = ".")` <br/>
Extract tar or cbt file in given directory.

* `ExtractPdfImages(string filePath, string outputDir = ".")` <br>
Extract Jpeg from pdf file to a directory.


## ComicFormat (enum)

Enum for all comic format available.

`CBR`\
`CBZ`\
`CBT`\
`PDF`
