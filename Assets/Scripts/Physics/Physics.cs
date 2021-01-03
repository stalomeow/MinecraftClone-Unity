using System;
using UnityEngine;

namespace Minecraft.XPhysics
{
    public static class Physics
    {
        /// <summary>
        /// 方块射线检测 (start, end]
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="maxDistance"></param>
        /// <param name="world"></param>
        /// <param name="selectBlock"></param>
        /// <param name="hit"></param>
        /// <see cref="https://blog.csdn.net/xfgryujk/article/details/52948543"/>
        /// <returns></returns>
        public static bool RaycastBlock(Ray ray, float maxDistance, World world, Func<Block, bool> selectBlock, out BlockRaycastHit hit)
        {
            return RaycastBlock(ray.origin, ray.origin + ray.direction * maxDistance, world, selectBlock, out hit);
        }

        public static bool RaycastBlock(Vector3 origin, Vector3 direction, float maxDistance, World world, Func<Block, bool> selectBlock, out BlockRaycastHit hit)
        {
            return RaycastBlock(origin, direction * maxDistance, world, selectBlock, out hit);
        }

        public static bool RaycastBlock(Vector3 start, Vector3 end, World world, Func<Block, bool> selectBlock, out BlockRaycastHit hit)
        {
            int startX = Mathf.FloorToInt(start.x);
            int startY = Mathf.FloorToInt(start.y);
            int startZ = Mathf.FloorToInt(start.z);

            int endX = Mathf.FloorToInt(end.x);
            int endY = Mathf.FloorToInt(end.y);
            int endZ = Mathf.FloorToInt(end.z);

            int count = 200; // 上限200个方块，防止死循环

            while ((startX != endX || startY != endY || startZ != endZ) && (count-- > 0))
            {
                float newX = startX;
                float newY = startY;
                float newZ = startZ;

                if (endX > startX)
                {
                    newX += 1;
                }

                if (endY > startY)
                {
                    newY += 1;
                }

                if (endZ > startZ)
                {
                    newZ += 1;
                }

                float xt = float.PositiveInfinity;
                float yt = float.PositiveInfinity;
                float zt = float.PositiveInfinity;

                float dx = end.x - start.x;
                float dy = end.y - start.y;
                float dz = end.z - start.z;

                // 向X方向选了候选方块
                if (endX != startX)
                {
                    xt = (newX - start.x) / dx;
                }

                if (endY != startY)
                {
                    yt = (newY - start.y) / dy;
                }

                if (endZ != startZ)
                {
                    zt = (newZ - start.z) / dz;
                }

                // 最终选了哪个方向的候选方块
                BlockDirection direction;

                // 选出候选方块中离起点(当前)最近的，更新起点、要检测的方块坐标
                if (xt < yt && xt < zt)
                {
                    start.x = newX;
                    start.y += dy * xt;
                    start.z += dz * xt;

                    direction = endX > startX ? BlockDirection.PositiveX : BlockDirection.NegativeX;
                }
                else if (yt < zt)
                {
                    start.x += dx * yt;
                    start.y = newY;
                    start.z += dz * yt;

                    direction = endY > startY ? BlockDirection.PositiveY : BlockDirection.NegativeY;
                }
                else
                {
                    start.x += dx * zt;
                    start.y += dy * zt;
                    start.z = newZ;

                    direction = endZ > startZ ? BlockDirection.PositiveZ : BlockDirection.NegativeZ;
                }
                
                startX = Mathf.FloorToInt(start.x);
                startY = Mathf.FloorToInt(start.y);
                startZ = Mathf.FloorToInt(start.z);

                if (direction == BlockDirection.NegativeX)
                {
                    startX--; // 以方块内各轴最小坐标为方块坐标，这里得到的是X上最大坐标所以要-1
                }

                if (direction == BlockDirection.NegativeY)
                {
                    startY--;
                }

                if (direction == BlockDirection.NegativeZ)
                {
                    startZ--;
                }

                // 检测新起点方块
                Vector3Int pos = new Vector3Int(startX, startY, startZ);
                Block block = world.GetBlock(startX, startY, startZ);
                AABB boundingBox = block.GetBoundingBox(pos);

                if (block.HasAnyFlag(BlockFlags.IgnoreCollisions) || !selectBlock(block))
                {
                    continue;
                }

                Vector3 normal;

                switch (direction)
                {
                    case BlockDirection.PositiveX:
                        normal = Vector3.left;
                        break;
                    case BlockDirection.NegativeX:
                        normal = Vector3.right;
                        break;
                    case BlockDirection.PositiveY:
                        normal = Vector3.down;
                        break;
                    case BlockDirection.NegativeY:
                        normal = Vector3.up;
                        break;
                    case BlockDirection.PositiveZ:
                        normal = Vector3.back;
                        break;
                    case BlockDirection.NegativeZ:
                        normal = Vector3.forward;
                        break;
                    default:
                        normal = default;
                        break;
                }

                hit = new BlockRaycastHit(pos, normal, world, block);
                return true;
            }

            hit = default;
            return false;
        }
    }
}