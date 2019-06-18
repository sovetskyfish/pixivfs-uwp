# Pixiv UWP 2

[<img src='https://assets.windowsphone.com/85864462-9c82-451e-9355-a3d5f874397a/English_get-it-from-MS_InvariantCulture_Default.png' alt='English badge' width=284 height=104/>](https://www.microsoft.com/store/apps/9PM8K64J71PL?cid=storebadge&ocid=badge)

[![编译状态](https://dev.azure.com/tobiichiamane/pixivfs-uwp/_apis/build/status/tobiichiamane.pixivfs-uwp?branchName=master)](https://dev.azure.com/tobiichiamane/pixivfs-uwp/_build?definitionId=1)

已发布至商店。所有的依赖都是Nuget包，所以除了VS2019和Windows SDK 17763以外你不需要任何特殊的环境来编译代码。你也可以从Azure Pipelines获取Artifact，或者从Release页面获取包。

Almost useable. All dependencies are available on Nuget, so you don't need any special environment to compile except Visual Studio 2019 and Windows SDK 17763. Instead, you can get the compiled artifact from Azure Pipelines, or get it from the releases page.

由于.NET Native[暂不完整支持F#](https://github.com/dotnet/corert/issues/6055)，本应用暂时使用[pixivcs](https://github.com/tobiichiamane/pixivcs)而不是[pixivfs](https://github.com/tobiichiamane/pixivfs)编写。

Because .NET Native [hasn't fully supported F# yet](https://github.com/dotnet/corert/issues/6055), this project is based on [pixivcs](https://github.com/tobiichiamane/pixivcs) rather than [pixivfs](https://github.com/tobiichiamane/pixivfs). 

# 翻译/Translation

本应用急需翻译人员。使用[英文资源文件](https://github.com/tobiichiamane/pixivfs-uwp/blob/master/PixivFSUWP/Strings/en/Resources.resw)为蓝本，翻译出其它的语言。请按照[微软的规范](https://docs.microsoft.com/en-us/windows/uwp/publish/supported-languages)给资源目录命名。

Any help to translate this app will be appreciated. Please use [Resources.resw](https://github.com/tobiichiamane/pixivfs-uwp/blob/master/PixivFSUWP/Strings/en/Resources.resw) as your blueprint.

# Buy me a coffee? 

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/P5P8WD5C)
