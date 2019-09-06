# 解决登录问题的一般步骤

## 检查您的应用版本

任何早于[v2.2.2](https://github.com/tobiichiamane/pixivfs-uwp/tree/v2.2.2)的版本都有可能出现无法使用用户名和密码登录的情况，这是Pixiv升级了其API所造成的。请更新应用至v2.2.2及以后版本来获得正常体验。

## 中国大陆用户特别注意

Pixiv及其相关服务在中国大陆处于被封锁状态，若希望使用PixivUWP，则必须使用稳定的代理工具；对于部分代理工具（如Shadowsocks等）则需要解除UWP应用的本地回环限制。

**!实验性功能!** 在[版本2.2.1](https://github.com/tobiichiamane/pixivfs-uwp/tree/v2.2.1)之后，你可以在登录时勾选“直连”功能以期绕过中国大陆的封锁。对于某些地区，此功能未必有效；对于此功能有效的地区，连接速度可能会不理想。

### 解除UWP应用本地回环限制

**使用命令提示符**

- 若您的应用来自于GitHub或Azure DevOps：
```cmd
checknetisolation loopbackexempt -a -n=31db69de-89d1-4fa9-9878-fcd942f090ca_5v6yvh15ag700
```

- 若您的应用来自于商店：
```cmd
checknetisolation loopbackexempt -a -n=18416PixeezPlusProject.PixivUWP_fsr1r9g7nfjfw
```

**使用[Fiddler](https://www.telerik.com/fiddler)**

- 下载并安装Fiddler
- 启动Fiddler，并点击工具栏上第一个按钮“WinConfig”
- 勾选PixivUWP并保存更改

### 确保代理工具正在代理PixivUWP正在使用的域名

PixivUWP使用下列域名完成应用功能：

- app-api.pixiv.net/*
- public-api.secure.pixiv.net/*
- oauth.secure.pixiv.net/*
- public-api.pixiv.net/*

Pixiv的图片由以下域名提供：

- i.pximg.net/*

请确保将这些域名加入你的代理列表中。

## 确保你有正常的网络连接

能正常访问互联网不代表着您能正常访问Pixiv及其相关服务。请确保您的网络能正常连接至Pixiv。

## 检查你的用户名和密码

错误的用户名和密码将不能登入PixivUWP。PixivUWP目前不提供未登录状态的任何功能。
