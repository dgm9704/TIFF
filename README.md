# TIFF
A .NET library for handling Tagged Image File Format + related tests and tools

Main goals are to 

a) read and show as much info as possible from any correct TIF 

b) read, modify and write baseline compliant files correctly


## Caveat
Not production safe for output when the file contains non-baseline tags.

For example SubIFDS, Exif etc will most likely result in a broken file.


## Environment
I use [Visual Studio Code](https://code.visualstudio.com/) 

on [Ubuntu 17.10](https://www.ubuntu.com/desktop/1710)

Code is written in [C#](https://docs.microsoft.com/en-us/dotnet/csharp/index) 7.0, targeting 

[.NET Standard 2.0](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md) / 

[.NET Core 2.0](https://docs.microsoft.com/en-us/dotnet/core/)

Test framework is [xUnit.net](https://xunit.github.io/)

## TIFF info

These are excellent resources :

[LibTIFF homepage](http://www.libtiff.org/) 

[LibTIFF test images](http://download.osgeo.org/libtiff/)

[AWare Systems](https://www.awaresystems.be/imaging/tiff.html)

## Diwen.Tiff
netstandard 2.0 library for handling TIFF

Started out in VB 6.0 as tool for tag dumping and page counting, 

then ported to C# / .NET 2.0 as a learning/sanity project.

### License
GNU Lesser General Public License v3.0

[http://www.gnu.org/licenses/gpl.txt](http://www.gnu.org/licenses/gpl.txt)

[http://www.gnu.org/licenses/lgpl.txt](http://www.gnu.org/licenses/lgpl.txt)

## Diwen.Tiff.Test
netcore 2.0 project for testing the library (and acting as documentation)

### License
[Free Public License 1.0.0](https://opensource.org/licenses/FPL-1.0.0)

## TiffDump
A commandline application that uses Diwen.Tiff for dumping TIFF info 

### License
[Free Public License 1.0.0](https://opensource.org/licenses/FPL-1.0.0)

## images for testing 

I do not claim any license or ownership for these.
If there is a problem, please inform me.

### testfiles
Various images that do not represent any kind of test suite but were 
included along the way to test some specific features

### libtiffpic
Files from libtiff testsuite