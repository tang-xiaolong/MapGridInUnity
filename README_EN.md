English | [简体中文](./README.md)

<p align="center">
  <a href="https://github.com/tang-xiaolong/MapGridInUnity/blob/main/LICENSE"><img src="https://img.shields.io/npm/l/vue.svg" alt="License"></a>
</p>

<h1 align="center">MapGridInUnity</h1>
<p align="center"><b>A universal game map solution</b></p>

## Main module

### Minimum heap maximum heap module

This module mainly supports subsequent pathfinding algorithms. It uses the list as the actual storage container, and the list elements need to implement the `IComparable` interface. The algorithm adjusts the sorting of heap elements through the `CompareTo `method.

### Universal map module

The module does this by completely separating the map data from the map display. A series of map-related interfaces are separated from the module, such as `IMapGrid `, `IMapShow`, `INode`, `IPathNode`, `INodeEntity`, etc. Each interface is responsible for defining what the implementation class should do. This module uses the interface to realize the universality of the map module. Each class in the module only depends on the method in the interface, and the type is set through the form of dependency injection.

* `IMapGrid`:  The common properties and necessary methods of the map data class are defined.

* `IMapShow`:  Defines the map display related content, such as setting the map node `GameObject`, binding the map data, getting display object and location, etc.

* `INode`:  Defines what a simple node should contain.

* `IPathNode`:  This interface implements all contents of `INode`, and on this basis adds some necessary contents of pathfinding, such as generation value, parent node, whether passable and so on.

* `INodeEntity`:  Defines display interfaces for nodes, such as highlighting, restoring, and so on.

​        

### Pathfinding module

This module contains the A star pathfinding algorithm and optimized minimum heap of A star algorithm in the optimization algorithm, the introduction of the minimum heap to quickly remove the minimum cost of the node from the Open list, introducing the SessionId to speed the list if the node in the Open and Close the list of judgment.



## Gif example

### Create map generate configuration

![创建地图配置](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/CreateGenerateConfig.gif?raw=true)



### Create Map

![创建地图](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/GenerateMap.gif?raw=true)



### Whether the map starting point needs to be offset

![地图是否需要偏移](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/MapOffset.gif?raw=true)



### Pathfinding

![寻路](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/PathFinding.gif?raw=true)



### Map highlighting

![地图高亮](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/MapHighLight.gif?raw=true)





## Example figure

### The grid in the highlighted range of a hexagon map

![六边形地图高亮范围内的格子](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/HexMapRangeHighLight.png?raw=true)



### The hexagon map highlights the grid in the specified direction

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/HexMapLineHighLight.png?raw=true)



### The Hexagonal map highlights the grid within the sector

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/HexMapSectorHighLight.png?raw=true)



### The grid in the highlighted range of the grid map

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/NormalMapRangeHighLight.png?raw=true)



### The grid map highlights the grid in the specified direction

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/NormalMapLineHighLight.png?raw=true)



### Grids in the highlighted sector of the grid map

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/NormalMapSectorHighLight.png?raw=true)



### Hexagonal map pathfinding

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/HexMapPathFinding.png?raw=true)



### Four direction grid map pathfinding

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/NormalMapFourDirPathFinding.png?raw=true)



### Eight direction grid map pathfinding

![](https://github.com/tang-xiaolong/MapGridInUnity/blob/main/Screenshot/NormalMapEightDirPathFinding.png?raw=true)



## Road map

- [x] Added structure class diagram for map module.
- [ ] Take apart the entire module via blog or video.
- [ ] Added pathfinding algorithm test scenario.
- [ ] Added a TileMap example.
- [ ] Map editor based on Unity UI Elements.







