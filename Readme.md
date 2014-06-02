Simple console app which converts Markdown files to HTML using [MarkdownDeep](http://www.toptensoftware.com/markdowndeep)

# Usage
## Arguments

* `i`, `input-file` - The name of the file to load.
* `d`, `input-directory` - The location of the directory to enumerate. Defaults to current directory if empty string specified.
* `m`, `input-file-mask` - The file mask to use when enumerating the directory. Defaults to `*.md`.
* `o`, `output-directory` - The output directory for converted files. Defaults to Output.
* `t`, `template` - The name of the file to use as a template (Replaces the token '{{CONTENT}}' with parsed HTML output).

## Single file conversion
`MDToHTML.exe -i Readme.md`

## Directory conversion in current directory
`MDToHTML.exe -d ""`

## Directory conversion in custom directory
`MDToHTML.exe -d "C:\Markdown"`

## Directory conversion in custom directory with file mask
`MDToHTML.exe -d "C:\Markdown" -m "*.markdown"`

## Template usage
`MDToHTML.exe -i Readme.md -t Template.html`

Template.html:

	<!DOCTYPE html>

	<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
	<head>
	    <meta charset="utf-8" />
	    <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/2.3.2/css/bootstrap.css">
	    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/2.3.2/js/bootstrap.js"></script>
	    <title></title>
	</head>
	<body>
	    <div class="container">
	        {{CONTENT}}
	    </div>
	</body>
	</html>
	