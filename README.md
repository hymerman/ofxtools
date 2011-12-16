Readme last updated 16th December 2011

Overview
========

This is a little suite of tools for fiddling with OFX files, and getting them from websites and other downloads of some banks I use.

Since OFX is a horrible file format, everything is done using my little intermediate file format written for an app I'm making called FineAnts. It's just a simple xml format with a ".statement" extension. There are utilities for converting between OFX and FineAnts.

Most of these programs are simple command line programs, which take a list of files or directories as arguments and output either OFX files or a FineAnts .statement files. If OFX is what you're after (most likely), you'll want to use OFXToFineAnts to convert the result to OFX. Any directories you pass in will be scanned for files recursively.

Clearly using the programs from the command line is a bit fiddly, so you can also use them by dragging the input files or directories onto the executables.

Included utilities
==================

*	**AmexOFXCleaner**: The American Express website makes OFX downloads available, but they're formatted really weirdly and I found some tools that read OFX files didn't read them properly. Run them through AmexOFXCleaner to clean them.

*	**FineAntsToOFX**: Converts FineAnts .statement files to OFX files.

*	**FirstDirectCSVToFineAnts**: Converts CSV files from the First Direct website (called "Microsoft Excel" files on their website) to FineAnts .statement files. Tested on CSV files downloaded for a current account and regular saver.

*	**HalifaxCSVToFineAnts**: Converts CSV files from the Halifax website to FineAnts .statement files. Tested just on a rewards current account.

*	**HSBCToFineAnts**: Converts 'previous statement' pages saved as HTML to FineAnts .statement files. I do this by clicking 'previous statements' on the sidebar to get the list of previous statements, then right-clicking each one and choosing 'save as' to save the file.

*	**OfxEditor**: Simple editor for OFX and FineAnts .statement files. It doesn't allow you to edit things like the account type and number, or transaction types, since they're almost completely useless and ignored by most tools. It'll strip out anything not stored in my .statement format, so don't use it to save over .ofx files directly if you're worried about losing that extra information.

*	**OfxMerger**: Takes multiple OFX files and merges them into one. This currently doesn't operate on FineAnts .statement files. It will warn you about overlaps and gaps in date ranges of statements, and also about discrepencies between closing balances and sums of transactions.

*	**OFXToFineAnts**: Converts OFX files to FineAnts .statement files.

*	**PostOfficeCSVToFineAnts**: Converts CSV files from the Post Office Savings website to FineAnts .statement files.

Building
========

If binaries aren't available (they aren't at the time of writing, but I plan to upload some soon), this is how you go about getting them:

You'll need Visual C# (Microsoft provide a free 'Express Edition' which is fine). Download the source to ofxtools from https://github.com/hymerman/ofxtools - either through git or using the 'download as zip' button. Open OfxTools.sln, then from Visual Studio, choose "build -> batch build ...", then 'select all', and then 'build'. It'll churn away for a few seconds building all the tools. Once it's done, there'll be a bunch of executables inside each sub-project's directory, and you're good to go.


Older converters
================

This project used to also support statements from Egg credit cards (which no longer exist) and ING direct (which I haven't used for years and thus can't maintain). If you need to convert statements from either of them, you may need to get your hands dirty and look back through the history of this project a bit!