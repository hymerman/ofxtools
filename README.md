Readme last updated 5th August 2015


Overview
========

This is a little suite of tools for working with OFX files and files downloaded from some banks I use.

Since OFX is a horrible file format, everything is done using my little intermediate file format written for an app I'm making called FineAnts. It's just a simple JSON format with a ".statementjson" extension. There are utilities for converting between OFX and FineAnts.

Most of these programs are simple command line programs, which take a list of files or directories as arguments and output FineAnts .statementjson files. Any directories you pass in will be scanned for files recursively.

If OFX is what you're after (most likely), you'll want to use FineAntsToOFX to convert the result to OFX.

Clearly using the programs from the command line is a bit fiddly, so you can also use them by dragging the input files or directories onto the executables, which is what I do.


Included utilities
==================

*	**FineAntsToOFX**: Converts FineAnts .statementjson files to OFX files.

*	**FirstDirectCSVToFineAnts**: Converts CSV files from the First Direct website (called "Microsoft Excel" files on their website) to FineAnts .statementjson files. Tested on CSV files downloaded for a current account and regular saver.

*	**HalifaxCreditCardCSVToFineAnts**: Converts CSV files from a credit card on the Halifax website to FineAnts .statementjson files. The downloads Halifax provides format credit card and current account data differently, hence the two different tools. Tested on a clarity credit card.

*	**HalifaxCSVToFineAnts**: Converts CSV files from a current account on the Halifax website to FineAnts .statementjson files. The downloads Halifax provides format credit card and current account data differently, hence the two different tools. Tested on a rewards current account.

*	**OfxEditor**: Simple editor for FineAnts .statementjson files. It doesn't allow you to edit things like the account type and number, or transaction types, since they're almost completely useless and ignored by most tools. It'll strip out anything not stored in my .statementjson format, so don't use it to save over .ofx files directly if you're worried about losing that extra information. It's extremely flaky and works just well enough for me to do exactly the editing I need to do. Don't expect much...

*	**OfxMerger**: Takes multiple FineAnts .statementjson files and merges them into one. It will warn you about overlaps and gaps in date ranges of statements, and also about discrepencies between closing balances and sums of transactions. Useful both for merging files and for checking that you don't have missing data.

*	**OFXToFineAnts**: Converts OFX files to FineAnts .statementjson files.

*	**SantanderTextToFineAnts**: Converts statements downloaded from Santander current accounts in "text" format to FineAnts .statementjson files.


Building
========

If binaries aren't available (they aren't at the time of writing, but I plan to upload some soon), this is how you go about getting them:

You'll need Visual Studio 2015 with C# (Microsoft provide a free 'Community' edition which is fine). Download the source to ofxtools from https://github.com/hymerman/ofxtools - either through git or using the 'download as zip' button. Open OfxTools.sln, then from Visual Studio, choose "build -> batch build ...", then 'select all', and then 'build'. It'll churn away for a few seconds building all the tools. Once it's done, there'll be a bunch of executables inside each sub-project's directory, and you're good to go.


Older converters
================

There are some converters still included which may or may not work any more; I don't have these accounts any more and don't have access to recent data from their websites. They're provided 'just in case' someone needs them, but don't hold out too much hope that they'll work! I've included the date I last knew they were working. They'll be removed at some point, once I decide there's no way they're of any use to anyone.

*	**AmexOFXCleaner**: The American Express website makes OFX downloads available, but they're formatted really weirdly and I found some tools that read OFX files didn't read them properly. Run them through AmexOFXCleaner to clean them. This is one of the few tools that deals only with OFX files and not .statementjson files. Last tested in May 2011.

*	**HSBCToFineAnts**: Converts 'previous statement' pages saved as HTML to FineAnts .statementjson files. I do this by clicking 'previous statements' on the sidebar to get the list of previous statements, then right-clicking each one and choosing 'save as' to save the file. Last tested in July 2012.

*	**PostOfficeCSVToFineAnts**: Converts CSV files from the Post Office Savings website to FineAnts .statementjson files. Last tested in April 2012.

This project used to also support statements from Egg credit cards (which no longer exist) and ING direct (which I haven't used for years and thus can't maintain). If you need to convert statements from either of them, you may need to get your hands dirty and look back through the history of this project a bit!
