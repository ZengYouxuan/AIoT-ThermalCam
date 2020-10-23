# AIoT-ThermalCam
# 项目需求

1.当有人员进出办公场所大门时，通过在大门附近安装的红外热成像仪实时测量出人员的体温，并将体温信息和人员照片通过5G网络实时传到Azure IoT并保存；

2.一旦测量出的人员体温超出标准（比如37.8°C），那么将此体温超标告警信息推送到办公场所管理人员的手机App上

3.办公场所管理人员可以实时通过手机App (借助手机App，管理人员可以移动化的实时监控，而不需要固定在进出口大门处) 查看体温超标告警信息，并查看体温超标人员照片；同时进一步对该人员进行体温复核，并将复核的体温结果提交；如确实体温超标，则能够及时采取防控措施

4.使用Power BI Pro获取实时摘要报告，访问进出口的体温数据，警报和响应，并通过大屏展示出来，让办公场所所有人员都能够实时看到当前的最新体温监测情况。



# 架构设计

![avatar](https://zengyx.blob.core.chinacloudapi.cn/aiothermal/A.jpg)



# 主要工作

Ø相关设备与产品采购，并提供使用说明和操作账号

​     红外热成像仪，安卓手机，21V Azure，Power BI Pro

Ø手机App（安卓版）开发与部署

​     供办公场所管理人员移动化的实时监控

Ø红外热成像仪安装部署和联调，保证设备正常运行工作

Ø红外热成像仪数据传递到Azure的接口设计与开发

​     将红外热成像仪监测到的体温信息和照片信息通过5G网络上传到Azure，并通过Azure IoT等资源服务将 实时告警信息推送到手机APP

​     考虑到可能出现断网的情况，因此将红外热成像仪监测的保存在本地网关（本地网关部署在Surface上），等设备恢复联网，将本地存储的数据自动上传到Azure的Blob Storage中，本地存储设置保存时间，过期后会自动删除

Ø使用Power BI Pro实时获取Azure上的监测数据，进行大屏展现，全面直观



# 具体实现方案

Ø告警信息通知与查看

Ø体温超标人员信息查看并提交复核信息

Ø红外热成像仪安装、调试、SDK开发

​      配置红外热成像仪的SDK，用于摄像头功能的开发。

Ø图像数据采集、分析

​      从红外热成像仪读取RGB和红外摄像头拍摄的照片，并得到对应的温度信息，打包成适合Azure IoT Hub的形式。

Ø边缘网关IoT Edge系统配置及部署

​      在Edge Device（Device采用Surface）上安装IoT Edge，部署$edgeAgent与$edgeHub，这两个是默认的。Edge module_blob storage采用微软官方标准的。Edge module_rule_engine单独开发。针对断网情况，Edge端配置、部署一个blob model，将Leaf Device数据保存在本地网关上（即Surface上），等设备恢复联网，将本地存储的图片自动上传到云端的Blob Storage中，本地Blob设置保存时间，过期后会自动删除。为了安全考虑，制作证书，主证书在Gateway中安装，从证书在Leaf Device中安装。

Ø  规则引擎编写及部署

​		开发Edge module_rule_engine，用于设定数据的传输规则。如设定温度阈值（比如37.8摄氏度，超过此温度，才触发上传信息。

Ø  设计、部署Azure资源服务，打通各模块链接

 		配置IoT Hub管理边缘网关设备，数据存储进云上blob。IoT Hub与Event Hub相连，交互数据。通过Azure Function与Event Hub对接，将数据存储到Cosmos DB中，与Power BI Pro连接，实时显示。同时通过Azure Function处理数据，并经过Notification Hub发送通知到手机APP。建立ACR仓库，将项目所需容器上传到云端，需要时下拉配置









