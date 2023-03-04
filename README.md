[English](./README_EN.md) | 简体中文

<p align="center">
  <a href="https://github.com/tang-xiaolong/MapGridInUnity/blob/main/LICENSE"><img src="https://img.shields.io/npm/l/vue.svg" alt="License"></a>
</p>
<h1 align="center">MapGridInUnity</h1>
<p align="center"><b>一个通用的游戏地图解决方案</b></p>

## 主要模块

### 最小堆最大堆模块

该模块主要是为后续寻路算法做支持的，其内部使用了列表作为实际存储容器，列表元素需要实现`IComparable`接口，算法通过接口内的`CompareTo`方法对堆内元素做排序调整。

### 通用的地图模块

该模块做到了将地图数据与地图显示完全抽离，模块中抽离了一系列地图相关的接口，比如`IMapGrid`，`IMapShow`, `INode`, `IPathNode`,`INodeEntity`等等，每个接口负责定义实现类应该要有什么功能。本模块借助接口来实现地图模块的通用性，模块中各个类只依赖接口内方法，类型则通过依赖注入的形式来设置。

* `IMapGrid`:  定义了地图数据类的常用的属性和必要的方法。

* `IMapShow`: 定义了地图显示相关的内容，比如设置地图节点`GameObject`，绑定地图数据，获取显示对象与位置等等。

* `INode`: 定义了一个最简单的节点应该要包含什么内容。

* `IPathNode`: 定义了可寻路的节点应该包含什么内容，该接口实现`INode`的全部内容，并在该基础上增加了寻路的一些必要内容，比如代价值，父节点，是否可通行等等。

* `INodeEntity`: 定义了节点的显示接口，比如高亮，还原等等。

​        

### 寻路模块

该模块中包含了普通的A星寻路算法和经过最小堆优化的A星算法，在优化算法中，引入了最小堆来快速从Open列表中取出代价最小的节点，引入SessionId来加快节点是否在Open列表与Close列表的判断。



## Gif示例

### 创建生成地图配置

![创建地图配置](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/CreateGenerateConfig.gif?raw=true)



### 创建地图

![创建地图](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/GenerateMap.gif?raw=true)



### 地图起点是否需要偏移

![地图是否需要偏移](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/MapOffset.gif?raw=true)



### 寻路

![寻路](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/PathFinding.gif?raw=true)



### 地图高亮

![地图高亮](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/MapHighLight.gif?raw=true)





## 示例图

### 六边形地图高亮范围内的格子

![六边形地图高亮范围内的格子](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/HexMapRangeHighLight.png?raw=true)



### 六边形地图高亮指定方向上的格子

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/HexMapLineHighLight.png?raw=true)



### 六边形地图高亮扇形范围内的格子

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/HexMapSectorHighLight.png?raw=true)



### 网格地图高亮范围内的格子

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/NormalMapRangeHighLight.png?raw=true)



### 网格地图高亮指定方向上的格子

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/NormalMapLineHighLight.png?raw=true)



### 网格地图高亮扇形范围内的格子

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/NormalMapSectorHighLight.png?raw=true)



### 六边形地图寻路

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/HexMapPathFinding.png?raw=true)



### 四方向网格地图寻路

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/NormalMapFourDirPathFinding.png?raw=true)



### 八方向网格地图寻路

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/NormalMapEightDirPathFinding.png?raw=true)



## 系统类图

![MapGridInUnity类图.drawio](https://img-bucket11.oss-cn-hangzhou.aliyuncs.com/BlogImage/202303041648233.svg)

## 路线图

- [x] 增加地图模块的结构类图。
- [ ] 图文或者视频对整个模块做拆解。
- [ ] 增加寻路算法测试场景。
- [ ] 增加TileMap示例。
- [ ] 基于Unity UI Elements的地图编辑器。







