## Demonstration
![EasySave](https://user-images.githubusercontent.com/93043965/146806310-aeddb03e-bd93-4c68-b83c-fbde28ecd6d9.gif)

## Project description

EasySave software is a Console application using .Net Core.

The software allows you to create an unlimited number of backup jobs

A backup job is defined by:

* An appellation

* A source directory

* A target directory

* A type (Complete, differential)

   
The user can request the execution of one of the backup jobs or the sequential execution of all the jobs.

The directories (sources and targets) can be on:

* Local disks

* External disks

* Network readers

All items in the source directory are affected by the backup.

Daily Log File:

EasySave writes in real time in a daily log file the history of the actions of the backup jobs. The minimum information expected is:

* Timestamp

* Name of the backup job

* Full address of the Source file (UNC format)

* Full address of the destination file (UNC format)

* File size

* File transfer time in ms (negative if error)

The software record in real time, in a single file, the progress of the backup jobs. The information to record for each backup job is:

* Name of the backup job

* Timestamp

The files (Daily Log and Status) are in JSON format. 
    

## Tech Stack

* Visual Studio 2019
* .NET Core 
* Git 


## Features

* Create Backup Job
* Execute a specific Backup 
* Execute All Backups job
* Show created Backups
* Delete a specific Backup 

## Screenshots

![App Screenshot](https://via.placeholder.com/468x300?text=App+Screenshot+Here)

