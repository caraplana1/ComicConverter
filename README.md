# ComicConverter

A multi-platform .NET library written in Net standard 2.0 for comic formating.

## Supported Formats

These are the comic formats that we currently support.

| Format |   Extract Images   |    Create Comic    |
|:------:|:------------------:|:------------------:|
|  CBR   |         ✔️         |         ❌          |
|  CBZ   |         ✔️         |         ✔️         |
|  CBT   |         ✔️         |         ✔️         |
|  PDF   | :heavy_check_mark: | :heavy_check_mark: |

We can't convert to CBR files due to RAR compression algorithm license.

## Samples

For start using the library just paste `using ComicConverter;` in your C# file and follow the next examples depending on what you want.\
- **Note**:  
**The final file result will append the file extension to the given name**

- **Convert CBR to PDF.**

```C#
string comicPath = "./MyComic.cbr";

Comic comic = new Comic(comicPath);
comic.Convert("new Name", ComicFormat.PDF);
```

- **Convert PDF to CBZ**

```C#
string pdfPath = "Comic.pdf";

Comic pdf = new Comic(pdfPath);
pdf.Convert("MyCbzComic", ComicFormat.CBZ);
```

- **Create a CBZ comic from images paths.**

```C#
string[] imagesPaths = {"Image1.png", "Folder/Image2.png"};

ComicBuilder.CreateCbz(imagesPaths, "comicName");
```

- **Extract Cbr content to a folder.**

```C#
string folder = "MyFolder";
string comic = "MyComic.cbr";

ImageExporter.UnRar(comic, folder);
```

## Documentation

For class documentation go to [Documentation](Docs/ClassDocs.md).
