# TxtToSql

Create SQL CREATE scripts based on text files. Uses all sql files in current directory by default. Run TxtToSql.exe --help for options.

Requires .Net Framework (Windows) or Mono (Mac/Linux). Tested under Mono 2.8 and .Net Framework 4.0, but any reasonably recent version should be fine.

Pre-compiled binaries in the binaries/ folder should work fine on Mac/Linux/Windows.

Or compile your own using the Visual Studio .sln file (Visual Studio Express is free if you don't have a copy, but works only on Windows) or use MonoDevelop, or csc command-line compiler if you are hard-core.

## TODO

This is just the start. It works, but there's much to do. 

- Option to pick which files to process
- Option to save an output file for each input file, optionally in a different directory
- Web interface to allow uploading files to process and showing output in a web page
- Winforms and GTK GUIs

Patches welcome. Send me a pull request. Please make sure you test under Mono and .Net Framework.

