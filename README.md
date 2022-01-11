# ComicConverter

A multi-plataform .NET library written in Net standard 2.0 for comic file formating.

## Suported Formats

These are the format that we current support. More in the way!

| Format | Extract Images |   Create Comic   |
| :----: | :------------: | :--------------: |
|  CBR  |      ✔️      |        ❌        |
|  CBZ  |      ✔️      |       ✔️       |
|  CBT  |      ✔️      |       ✔️       |
|  CB7  |      ✔️      |        ❌        |
|  PDF  |       ❌       | ❌ (Coming soon) |

## Documentation

For documentation go to [Documentation](Docs/Usage.md).

## Samples

To convert to another format just include `using ComicConverter;` and follow the next examples depending of what you want.

- Convert CBR to CBZ.

```C#
string comicPath = "./MyComic.cbr";

Comic comic = new Comic(comicPath);
comic.Convert("new Name", ComicFormat.CBZ);
```

- Create a CBZ comic from images paths.

```C#
string[] imagesPaths = {"Image1.png", "Folder/Image2.png"};

ComicBuilder.CreateCBZ(imagesPaths, "comicName");
```

- Extract CBR's content to a folder.

```C#
string folder = "MyFolder";
string comic = "MyComic.cbr";

ImageExtractors.UnRar(comic, folder);
```
