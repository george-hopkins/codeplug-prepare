codeplug-prepare
================

[![Build Status](https://ci.appveyor.com/api/projects/status/ct47mmu2bdn9noho/branch/master?svg=true)](https://ci.appveyor.com/project/george-hopkins/codeplug-prepare/branch/master)
[![Download](https://img.shields.io/badge/download-master-blue.svg)](https://ci.appveyor.com/api/projects/george-hopkins/codeplug-prepare/artifacts/codeplug-prepare.zip?branch=master)

A simple helper utility to extract the keys for [codeplug][0]. This tool requires that you own a copy of MOTOTRBO CPS.


Usage
-----

 * Download the pre-built binaries (link above) or build the project with MSBuild
 * Download [OpenSSL](https://www.openssl.org/) (e.g. from [indy.fulgan.com](https://indy.fulgan.com/SSL/)) and extract it into the project folder
 * Open a terminal (press <kbd>Win</kbd>+<kbd>R</kbd> and enter `cmd`)
 * Navigate to the project folder (e.g. `cd C:\Users\Example\Downloads\codeplug-prepare`)
 * Start the tool by entering `CodeplugPrepare`
 * Execute the command involving `openssl`
 * All done! You can now use the file `yourkey.pem` and the values of `CTB_KEY` and `CTB_IV` to [read and write codeplugs][0].


[0]: https://github.com/george-hopkins/codeplug
