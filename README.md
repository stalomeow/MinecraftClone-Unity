# A Minecraft clone made with Unity

[这里是中文的README（Here is the README in Chinese）](README_CN.md)

![Screenshot](Screenshots/0.png)

![Screenshot](Screenshots/1.png)

![Screenshot](Screenshots/2.png)



## Features

* Infinite chunk generation
* Caves and Ores
* Lighting And Physics
* TNT Explosion
* Flowing water
* Sand affected by gravity
* Saves world to files
* Audios and Particle effects
* Some bugs (〃'▽'〃)



## Block Editor and Item Editor in Unity

![Screenshot](Screenshots/3.png)

![Screenshot](Screenshots/4.png)

![Screenshot](Screenshots/5.png)

![Screenshot](Screenshots/6.png)

You can create a new block or item without writing any code! Some complex block logic only needs to be written in the corresponding event.



## Modify the settings of a world

Enter the path `C:\Users\${your username}\AppData\LocalLow\JinYuhan\MinecraftClone\Worlds`, to open the folder named with the name of the world to be modified, and find `settings.json` , double click to open editing.

| 字段                    | 类型                  | 用途                                                         |
| ----------------------- | --------------------- | ------------------------------------------------------------ |
| `Name`                  | `System.String`       | The name of the world                                        |
| `Type`                  | `System.Int32`        | The type of world (temporarily reserved)                     |
| `Mode`                  | `System.Byte`         | The play mode (temporarily reserved)                         |
| `Seed`                  | `System.Int32`        | The seed of the world                                        |
| `RenderChunkRadius`     | `System.Int32`        | Radius of the render chunks (chunks within the radius will be rendered) |
| `HorizontalFOVInDEG`    | `System.Single`       | Horizontal viewing angle (degree)                            |
| `MaxChunkCountInMemory` | `System.Int32`        | The maximum number of chunks reserved in memory. After this number is exceeded, some chunks will be unloaded |
| `EnableDestroyEffect`   | `System.Boolean`      | Whether to show the effects of destroying blocks             |
| `Position`              | `UnityEngine.Vector3` | The player's position at the end of last game                |


## References

**In no particular order**

1. [TrueCraft](https://github.com/ddevault/TrueCraft)
2. [MineClone-Unity](https://github.com/bodhid/MineClone-Unity)
3. [MinecraftClone](https://github.com/Shedelbower/MinecraftClone)
4. [Making a Minecraft Clone](https://www.shedelbower.dev/projects/minecraft_clone/)
5. [Minecraft_Wiki](https://minecraft-zh.gamepedia.com/Minecraft_Wiki)
6. [炒鸡嗨客协管徐的CSDN博客](https://blog.csdn.net/xfgryujk)



## Changes

[2020-08-19](CHANGE_LOG-2020-08-19.md)