# 更新 - 2020-08-19

* 移除`BlockEvents`、`BlockRegistry`、`ItemRegistry`
* 引入`XLua`，并将部分逻辑移至`Lua`
* 所有方块事件全部改用`Lua`编写
* 编写了一套简单的资源管理系统。材质球、方块数据、物品数据、`Lua`脚本全部使用该系统从`AssetBundle`加载
* 将`WorldManager`的部分逻辑移动至`DataManager`

* 修复了游戏在打包后运行，无法关闭的问题
* 修复了部分情况下光照无法更新的问题