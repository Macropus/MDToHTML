using CommandLine;
using CommandLine.Text;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDToHTML
{
    class Program
    {
        #region command line args

        class Options
        {
            [Option('i', "input-file", Required = false, HelpText = "The markdown file(s) to parse in a comma seperated string")]
            public string InputFile { get; set; }

            [Option('d', "input-directory", Required = false, HelpText = "The directory to enumerate files from")]
            public string InputDirectory 
            {
                get
                {
                    if (string.IsNullOrEmpty(_inputDirectory))
                        return Directory.GetCurrentDirectory();
                    return _inputDirectory;
                }
                set
                {
                    _inputDirectory = value;
                }
            }
            private string _inputDirectory;

            [Option('m', "input-file-mask", DefaultValue = "*.md", Required = false, HelpText = "The file mask to apply to input-directory")]
            public string FileMask { get; set; }

            [Option('o', "output-directory", Required = false, HelpText = "The directory to place the output in")]
            public string OutputDirectory
            {
                get
                {
                    if (string.IsNullOrEmpty(_outputDirectory))
                        return Path.Combine(Directory.GetCurrentDirectory(), "Output");
                    return _outputDirectory;
                }
                set
                {
                    _outputDirectory = value;
                }
            }
            private string _outputDirectory;

            [Option('t', "template", Required = false, HelpText = "The name of the file to use as a template (Replace '{{CONTENT}}' with parsed output)")]
            public string Template { get; set; }

            [ParserState]
            public IParserState LastParserState { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }

        #endregion

        static Options _options = new Options();
        static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                // Get args
                if (!CommandLine.Parser.Default.ParseArguments(args, _options))
                {
                    return;
                }
                
                // Get the files to parse
                string[] filesToParse;
                if (!string.IsNullOrEmpty(_options.InputFile))
                {
                    filesToParse = _options.InputFile.Split(',');
                }
                else
                {
                    // Get all files in the mask string
                    filesToParse = Directory.EnumerateFiles(_options.InputDirectory, _options.FileMask).ToArray<string>();
                }

                foreach (var fileName in filesToParse)
                {
                    try
                    {
                        // Check the file exists
                        if (!File.Exists(fileName))
                        {
                            Console.WriteLine("File {0} does not exist", fileName);
                            continue;
                        }

                        // Load the input file
                        var inputFileString = File.ReadAllText(fileName);

                        // Do the processing
                        var md = new CustomMarkdown();
                        var outputFileString = md.Transform(inputFileString);

                        // Work out the output filename
                        string outputFileName = Path.Combine(Directory.GetCurrentDirectory(), _options.OutputDirectory, string.Format("{0}.html", Path.GetFileNameWithoutExtension(fileName)));

                        // Create a directory, if required
                        if (!string.IsNullOrEmpty(_options.OutputDirectory))
                        {
                            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), _options.OutputDirectory));
                        }

                        // Template
                        // This searches in the temnplate file for the "{{CONTENT}}" token and replaces it with the parsed output
                        if (_options.Template != string.Empty && File.Exists(_options.Template))
                        {
                            var templateString = File.OpenText(_options.Template).ReadToEnd();
                            outputFileString = templateString.Replace("{{CONTENT}}", outputFileString);
                        }

                        // Write the file
                        File.WriteAllText(outputFileName, outputFileString);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred whilst processing this file: {0}", fileName);
                        _logger.ErrorException(string.Format("An error occurred whilst processing the file '{0}'", fileName), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unknown error occurred: {0}", ex.ToString());
                _logger.ErrorException(string.Format("An unknown error occurred"), ex);
            }
        }
    }
}
