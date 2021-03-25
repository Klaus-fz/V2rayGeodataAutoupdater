# V2rayGeodataAutoupdater
An auto-updater for V2Ray's Geo/IP data

一个用于自动更新V2ray的Geodata，并自动唤起V2ray客户端的工具。
基于.Net5，因此此工具理论上可以支持多平台，但只在windows平台上测试过。
**运行需要.Net5运行库！**

GeoData的来源可以参照 @Loyalsoldier 维护的项目： https://github.com/Loyalsoldier/v2ray-rules-dat/

使用前需修改运行目录下的updater.config.json文件进行配置。

**updater.config.json：**

```javascript
{
    // 是否在唤起客户端后继续显示console
    "IsShowConsole": true,
    // 是否更新GeoIP
    "IsUpdateGeoIp": true,
    // GeoIp来源
    "GeoIpSource": "https://github.com/Loyalsoldier/v2ray-rules-dat/releases/latest/download/geoip.dat",
    // 备用GeoIp来源
    "GeoIpSourceReserve": "https://cdn.jsdelivr.net/gh/Loyalsoldier/v2ray-rules-dat@release/geoip.dat",
    // 是否更新GeoSite
    "IsUpdateGeoSite": true,
    // GeoSite来源
    "GeoSiteSource": "https://github.com/Loyalsoldier/v2ray-rules-dat/releases/latest/download/geosite.dat",
    // 备用GeoSite来源
    "GeoSiteSourceReserve": "https://cdn.jsdelivr.net/gh/Loyalsoldier/v2ray-rules-dat@release/geosite.dat",
    // 是否在更新后直接启动V2ray客户端
    "IsLanchClient": true,
    // 客户端路径（这里使用的是V2rayN）
    "ClientPath": "E:\\v2rayN-Core\\v2rayN.exe",
    // 客户端参数（可选）
    "ClientParameter": "",
    // 是否保存GeoData到其他路径，false时会保存到和客户端相同的路径，否则保存到自定义的路径
    "IsDataSaveElsewhere": false,
    // GeoData自定义保存路径
    "DataSavePath": ""
}
```

注：由于没有运行V2ray前，Github的地址存在可能不能访问的问题，所以不建议只写Github的更新源。本程序的策略为在来源和备用来源中任何一个可更新即可（在有两个源的情况下，先下载完成者为准）。


