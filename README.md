# gmic-sharp-pdn-example

An example [Paint.NET](https://www.getpaint.net) Effect plugin that uses [gmic-sharp-pdn](https://github.com/0xC0000054/gmic-sharp-pdn).   
This example adds the G'MIC [water](https://gmic.eu/reference.shtml#water) command to Paint.NET.

**Menu location:** Distort > GmicSharpPdn Example

**Compatibility:** 4.2.11+

## Dependencies

This repository depends on libraries from the following repositories:

[gmic-sharp-pdn](https://github.com/0xC0000054/gmic-sharp-pdn), extends gmic-sharp for use with Paint.NET Effect plugins.   
[gmic-sharp](https://github.com/0xC0000054/gmic-sharp), provides the .NET G'MIC wrapper that gmic-sharp-pdn uses.   
[gmic-sharp-native](https://github.com/0xC0000054/gmic-sharp-native), provides the native interface between gmic-sharp and [libgmic](https://github.com/dtschump/gmic).

## License

This project is licensed under the terms of the MIT License.   
See [License.txt](License.txt) for more information.

### Native libraries

The gmic-sharp native libraries (GmicSharpNative*) are dual-licensed under the terms of the either the [CeCILL v2.1](https://cecill.info/licences/Licence_CeCILL_V2.1-en.html) (GPL-compatible) or [CeCILL-C v1](https://cecill.info/licences/Licence_CeCILL-C_V1-en.html) (similar to the LGPL).  
Pick the one you want to use.

This was done to match the licenses used by [libgmic](https://github.com/dtschump/gmic).

# Source code

## Prerequisites

* Visual Studio 2019
* Paint.NET 4.2.11+
* [gmic-sharp-pdn](https://github.com/0xC0000054/gmic-sharp-pdn)

## Building the plugin

* Open the solution
* Change the PaintDotNet references in the GmicSharpPdnExample project to match your Paint.NET install location
* Change the GmicSharp and GmicSharpPdn references in the GmicSharpPdnExample project to match the location of those files
* Update the post build events to copy the build output to the Paint.NET Effects folder
* Build the solution

## Build notes

This example project uses [ILMerge.Fody](https://github.com/tom-englert/ILMerge.Fody) to merge `GmicSharp.dll` and `GmicSharpPdn.dll` into
the main plugin file (`GmicSharpExample.dll`) after the project is built.   
This allows the final plugin to only require the `GmicSharpNative*` DLLs as external dependencies in the Paint.NET Effects folder.

ILMerge.Fody is configured in [FodyWeavers.xml](src/FodyWeavers.xml) to only include the DLL files whose filenames start with `GmicSharp`,
this stops ILMerge from including any Paint.NET DLLs that may be present in the output folder when merging.
