using System;

namespace Minecraft
{
    [Flags]
    public enum BlockFlags
    {
        None = 0,
        /// <summary>
        /// 忽略碰撞
        /// </summary>
        IgnoreCollisions = 1 << 0,
        /// <summary>
        /// 忽略玩家检测放置方块位置时发出的射线
        /// </summary>
        IgnorePlaceBlockRaycast = 1 << 1,
        /// <summary>
        /// 忽略玩家检测摧毁方块位置时发出的射线
        /// </summary>
        IgnoreDestroyBlockRaycast = 1 << 2,
        AffectedByGravity = 1 << 3,
        NeedsRandomTick = 1 << 4,
        Liquid = 1 << 5,
        FlowersAndPlants = 1 << 6,
        IgnoreExplosions = 1 << 7
    }
}