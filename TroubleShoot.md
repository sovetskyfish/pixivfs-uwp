# 解决登录问题的一般步骤
## 中国大陆用户特别注意

Pixiv及其相关服务在中国大陆处于被封锁状态，若希望使用PixivUWP，则必须使用稳定的代理工具；对于部分代理工具（如Shadowsocks等）则需要解除UWP应用的本地回环限制。

### 解除UWP应用本地回环限制

这里使用免费实用工具[Fiddler](https://www.telerik.com/fiddler)的一个子功能来达成目的：

- 下载并安装Fiddler
- 启动Fiddler，并点击工具栏上第一个按钮“WinConfig”
- 勾选PixivUWP并保存更改

### 确保代理工具正在代理PixivUWP正在使用的域名

PixivUWP使用下列域名完成应用功能：

- app-api.pixiv.net/*
- public-api.secure.pixiv.net/*
- oauth.secure.pixiv.net/*
- public-api.pixiv.net/*

请确保将这些域名加入你的代理列表中。

## 确保你有正常的网络连接

能正常访问互联网不代表着您能正常访问Pixiv及其相关服务。请确保您的网络能正常连接至Pixiv。

## 检查你的用户名和密码

错误的用户名和密码将不能登入PixivUWP。PixivUWP目前不提供未登录状态的任何功能。
