# TxtToSql

Create SQL CREATE scripts based on text files. Uses all sql files in current directory by default. Run TxtToSql.exe --help for options.

Requires .Net Framework (Windows) or Mono (Mac/Linux). Tested under Mono 2.8 and .Net Framework 4.0, but any reasonably recent version should be fine.

Pre-compiled binaries in the binaries/ folder should work fine on Mac/Linux/Windows.

Or compile your own using the Visual Studio .sln file (Visual Studio Express is free if you don't have a copy, but works only on Windows) or use MonoDevelop, or csc command-line compiler if you are hard-core.

## What is does

What the program does is best shown with an example. Say you have a CSV file that looks a bit like this (truncated to fit):

	Survey..Survey.ID,Survey.Name,Interviewer..Person.ID,Person.ID,District..District.Name,Interview.Latitude,...
	2011060136,NAADS CKW M E Baseline,Person-022015,Person-026628,Kasese,1.46556,29.94113,10/30/11 12:42,SMD-00...
	2011060136,NAADS CKW M E Baseline,Person-022015,Person-026628,Kasese,1.46556,29.94113,10/30/11 12:42,SMD-00...

You may want to put this table into a database ('SQL mode'), or simple understand exactly how the fields are formatted and what they contain ('Summarize mode'). See the following sections for more information and sample output.

### SQL Mode

When TxtToSql is run with no options, it writes to stdout SQL CREATE statements suitable for each .csv file in the current directory. If the only file were the one shown above, you would get output something like this:

	CREATE TABLE Interviewee_Survey (
	Survey_Survey_ID INT NOT NULL,
	Survey_Name TEXT NOT NULL,
	Interviewer_Person_ID TEXT NULL,
	Person_ID TEXT NOT NULL,
	District_District_Name TEXT NULL,
	Interview_Latitude REAL NOT NULL,
	Interview_Longitude REAL NOT NULL,
	Interview_GPS_Timestamp TIMESTAMP NULL,
	Submission_Name TEXT NOT NULL,
	Submission_Answer_Name TEXT NOT NULL,
	Handset_Submit_Time TIMESTAMP NOT NULL,
	Submission_Latitude REAL NOT NULL,
	Submission_Longitude REAL NOT NULL,
	Submission_GPS_Timestamp TIMESTAMP NULL
	);

TxtToSql checks for nulls, and also figures out what data type is most appropriate. It currently creates Postgresql compatible SQL, and handles the following types:

 - INT
 - TEXT
 - REAL
 - TIMESTAMP

### Summarize Mode

When run with the --summarize (-s) option, TxtToSql instead writes to stdout something like this:

	File name: Interviewee_Survey.csv
	99271 recs
	----

	Columns: 
	----

	Survey..Survey.ID; Nulls:No; Distinct:4; Top:2011060136/90949; System.Int32; Max:2011070802; Min:2011060136
	Survey.Name; Nulls:No; Distinct:4; Top:NAADS CKW M E Baseline/90949; System.String
	Interviewer..Person.ID; Nulls:Yes; Distinct:237; Top:Person-025118/3314; System.String
	Person.ID; Nulls:No; Distinct:2390; Top:Person-000972/12843; System.String
	District..District.Name; Nulls:Yes; Distinct:10; Top:Kasese/45881; System.String
	Interview.Latitude; Nulls:No; Distinct:2377; Top:0/6787; System.Double; Max:4.68852; Min:-5.3194
	Interview.Longitude; Nulls:No; Distinct:2379; Top:0/6787; System.Double; Max:106.89533; Min:0
	Interview.GPS.Timestamp; Nulls:Yes; Distinct:2678; Top:<NULL>/6787; System.DateTime
	Submission.Name; Nulls:No; Distinct:3104; Top:SMD-0000011071/43; System.String
	Submission.Answer.Name; Nulls:No; Distinct:1; Top:NA/99271; System.String
	Handset.Submit.Time; Nulls:No; Distinct:2997; Top:7/14/11 22:28/133; System.DateTime
	Submission.Latitude; Nulls:No; Distinct:1960; Top:0/5827; System.Double; Max:4.68553; Min:-5.31823
	Submission.Longitude; Nulls:No; Distinct:1958; Top:0/5827; System.Double; Max:106.89765; Min:0
	Submission.GPS.Timestamp; Nulls:Yes; Distinct:2155; Top:<NULL>/5827; System.DateTime

	Unique values per column: 
	----

	Survey..Survey.ID: 2011060136;2011070802;2011070164;2011070162
	Survey.Name: NAADS CKW M E Baseline;PPI Survey;NAADS CKW M E Mbale;M and E Survey Gulu
	District..District.Name: Kasese;<NULL>;Kapchorwa;Masindi;Kitgum;Amuru;Mbale;Luwero Nakaseke;Oyam;Gulu
	Submission.Answer.Name: NA
	--------

The *Columns* section lists all columns in the CSV file, with the following information:

 - Column name
 - Has nulls?
 - Number of distinct values
 - The most common value (i.e. the mode)
 - The best data type for this column
 - For int columns and double columns, the max and min values

The *Unique values per column* section shows, for each column with less than 50 distinct values, a list of all distinct values.

## TODO

This is just the start. It works, but there's much to do. 

- Option to pick which files to process
- Option to save an output file for each input file, optionally in a different directory
- More SQL dialects for output
- Option to create indexes for possible ID columns (based on column name and data type)
- Web interface to allow uploading files to process and showing output in a web page
- Winforms and GTK GUIs

Patches welcome. Send me a pull request. Please make sure you test under Mono and .Net Framework.

