# PixivFSUWP

[![编译状态](https://dev.azure.com/tobiichiamane/pixivfs-uwp/_apis/build/status/tobiichiamane.pixivfs-uwp?branchName=master)](https://dev.azure.com/tobiichiamane/pixivfs-uwp/_build?definitionId=1)

基本上可以用了。所有的依赖都是Nuget包，所以除了VS2019和Windows SDK 17763以外你不需要任何特殊的环境来编译代码。你也可以从Azure Pipelines获取Artifact，或者从Release页面获取包。

Almost useable. All dependencies are available on Nuget, so you don't need any special environment to compile except Visual Studio 2019 and Windows SDK 17763. Instead, you can get the compiled artifact from Azure Pipelines, or get it from the releases page.

由于.NET Native[暂不完整支持F#](https://github.com/dotnet/corert/issues/6055)，本应用暂时使用[pixivcs](https://github.com/tobiichiamane/pixivcs)而不是[pixivfs](https://github.com/tobiichiamane/pixivfs)编写。

Because .NET Native [hasn't fully supported F# yet](https://github.com/dotnet/corert/issues/6055), this project is based on [pixivcs](https://github.com/tobiichiamane/pixivcs) rather than [pixivfs](https://github.com/tobiichiamane/pixivfs). 

多语言支持还未起步，此应用目前仅以简体中文呈现。

Multi-language support is yet to be developed, thus this application only supports Simplified Chinese.

Buy me a coffee? 
[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/P5P8WD5C)