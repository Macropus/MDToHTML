Simple console app which converts Markdown files to HTML using [MarkdownDeep](http://www.toptensoftware.com/markdowndeep)

# Usage
## Arguments

* `i`, `input-file` - The name of the file to load.
* `d`, `input-directory` - The location of the directory to enumerate. Defaults to current directory if empty string specified.
* `m`, `input-file-mask` - The file mask to use when enumerating the directory. Defaults to `*.md`.
* `o`, `output-directory` - The output directory for converted files. Defaults to Output.

## Single file conversion
`MDToHTML.exe -i Readme.md`

## Directory conversion in current directory
`MDToHTML.exe -d ""`

## Directory conversion in custom directory
`MDToHTML.exe -d "C:\Markdown"`

## Directory conversion in custom directory with file mask
`MDToHTML.exe -d "C:\Markdown" -m "*.markdown"`