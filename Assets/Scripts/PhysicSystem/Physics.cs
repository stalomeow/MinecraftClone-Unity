using System;
using Minecraft.Configurations;
using UnityEngine;
using static Minecraft.WorldConsts;

namespace Minecraft.PhysicSystem
{
    [XLua.LuaCallCSharp]
    public static class Physics
    {
        public static Vector3 Gravity => UnityEngine.Physics.gravity;

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
        public static bool RaycastBlock(Ray ray, float maxDistance, IWorld world, Func<BlockData, bool> selectBlock, out BlockRaycastHit hit)
        {
            return RaycastBlock(ray.origin, ray.origin + ray.direction * maxDistance, world, selectBlock, out hit);
        }

        public static bool RaycastBlock(Vector3 origin, Vector3 direction, float maxDistance, IWorld world, Func<BlockData, bool> selectBlock, out BlockRaycastHit hit)
        {
            return RaycastBlock(origin, direction * maxDistance, world, selectBlock, out hit);
        }

        public static bool RaycastBlock(Vector3 start, Vector3 end, IWorld world, Func<BlockData, bool> selectBlock, out BlockRaycastHit hit)
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
                BlockFace direction;

                // 选出候选方块中离起点(当前)最近的，更新起点、要检测的方块坐标
                if (xt < yt && xt < zt)
                {
                    start.x = newX;
                    start.y += dy * xt;
                    start.z += dz * xt;

                    direction = endX > startX ? BlockFace.PositiveX : BlockFace.NegativeX;
                }
                else if (yt < zt)
                {
                    start.x += dx * yt;
                    start.y = newY;
                    start.z += dz * yt;

                    direction = endY > startY ? BlockFace.PositiveY : BlockFace.NegativeY;
                }
                else
                {
                    start.x += dx * zt;
                    start.y += dy * zt;
                    start.z = newZ;

                    direction = endZ > startZ ? BlockFace.PositiveZ : BlockFace.NegativeZ;
                }

                startX = Mathf.FloorToInt(start.x);
                startY = Mathf.FloorToInt(start.y);
                startZ = Mathf.FloorToInt(start.z);

                switch (direction)
                {
                    case BlockFace.NegativeX:
                        startX--; // 以方块内各轴最小坐标为方块坐标，这里得到的是X上最大坐标所以要-1
                        break;
                    case BlockFace.NegativeY:
                        startY--;
                        break;
                    case BlockFace.NegativeZ:
                        startZ--;
                        break;
                }

                // 检测新起点方块
                Vector3Int pos = new Vector3Int(startX, startY, startZ);
                BlockData block = world.RWAccessor.GetBlock(startX, startY, startZ);
                // AABB? boundingBox = block.GetBoundingBox(pos, world);

                if (!selectBlock(block))
                {
                    continue;
                }

                Vector3 normal = direction switch
                {
                    BlockFace.PositiveX => Vector3.left,
                    BlockFace.NegativeX => Vector3.right,
                    BlockFace.PositiveY => Vector3.down,
                    BlockFace.NegativeY => Vector3.up,
                    BlockFace.PositiveZ => Vector3.back,
                    BlockFace.NegativeZ => Vector3.forward,
                    _ => default
                };

                hit = new BlockRaycastHit(pos, normal, world, block);
                return true;
            }

            hit = default;
            return false;
        }

        public static bool CheckGroundedAABB(Vector3 position, AABB aabb, IWorld world, out BlockData groundBlock)
        {
            aabb += position;
            int y = Mathf.FloorToInt(aabb.Min.y - 0.5f);

            if (y >= 0 && y < ChunkHeight)
            {
                int startX = Mathf.FloorToInt(aabb.Min.x);
                int endX = Mathf.FloorToInt(aabb.Max.x - 0.05f); // 稍微做一个偏移，避免误判
                int startZ = Mathf.FloorToInt(aabb.Min.z);
                int endZ = Mathf.FloorToInt(aabb.Max.z - 0.05f); // 稍微做一个偏移，避免误判

                for (int x = startX; x <= endX; x++)
                {
                    for (int z = startZ; z <= endZ; z++)
                    {
                        groundBlock = world.RWAccessor.GetBlock(x, y, z);
                        AABB? blockAABB = groundBlock.GetBoundingBox(x, y, z, world, true);

                        if (blockAABB != null && aabb.Intersects(blockAABB.Value))
                        {
                            return true;
                        }
                    }
                }
            }

            groundBlock = null;
            return false;
        }

        public static CollisionFlags CollideWithTerrainAABB(Vector3 position, AABB aabb, IWorld world, PhysicMaterial material, float time, ref Vector3 velocity, out Vector3 movement)
        {
            aabb += position;

            for (int i = 0; i < 3; i++)
            {
                if (Mathf.Abs(velocity[i]) <= Mathf.Epsilon)
                {
                    velocity[i] = 0;
                }
            }

            CollisionFlags flags = CollisionFlags.None;
            bool negativeY = velocity.y < 0;
            movement = default;

            if (CollideWithTerrainAABB(1, aabb, world, material, time, ref velocity.y, out movement.y))
            {
                flags |= negativeY ? CollisionFlags.Below : CollisionFlags.Above;
            }

            aabb += new Vector3(0, movement.y, 0);

            if (CollideWithTerrainAABB(0, aabb, world, material, time, ref velocity.x, out movement.x))
            {
                flags |= CollisionFlags.Sides;
            }

            aabb += new Vector3(movement.x, 0, 0);

            if (CollideWithTerrainAABB(2, aabb, world, material, time, ref velocity.z, out movement.z))
            {
                flags |= CollisionFlags.Sides;
            }

            return flags;
        }

        private static void CalculateVelocityAfterCollisionWithTerrain(PhysicMaterial material, BlockData blockCollided, ref float velocity)
        {
            // 碰撞公式：
            // v1' = ((m1 - e * m2) * v1 + (1 + e) * m2 * v2) / (m1 + m2)
            //
            // 其中：
            // v1' 为碰撞体碰撞后的速度
            // m1 为碰撞体的质量
            // v1 为碰撞体初速度
            // m2 为地形质量
            // v2 为地形初速度
            // e 为恢复系数
            //
            // 在这里显然有：
            // m1 << m2
            // v2 = 0
            //
            // 于是公式简化为：
            // v1' = -e * v1

            float e = blockCollided.PhysicMaterial.CoefficientOfRestitution;
            e = PhysicMaterial.CombineValue(e, material.CoefficientOfRestitution);
            velocity *= -e;

            if (Mathf.Abs(velocity) <= Mathf.Epsilon)
            {
                velocity = 0;
            }
        }

        private static bool CollideWithTerrainAABB(int axis, AABB aabb, IWorld world, PhysicMaterial material, float time, ref float velocity, out float movement)
        {
            movement = velocity * time;

            if (movement == 0)
            {
                return false;
            }

            Vector3Int start = default;
            Vector3Int end = default;
            Vector3Int step = default;

            if (movement > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (axis == i)
                    {
                        start[i] = Mathf.FloorToInt(aabb.Max[i]);
                        end[i] = Mathf.FloorToInt(aabb.Max[i] + movement);
                    }
                    else
                    {
                        start[i] = Mathf.FloorToInt(aabb.Min[i]);
                        end[i] = Mathf.FloorToInt(aabb.Max[i]);
                    }

                    step[i] = 1;
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (axis == i)
                    {
                        end[i] = Mathf.FloorToInt(aabb.Min[i] + movement);
                        step[i] = -1;
                    }
                    else
                    {
                        end[i] = Mathf.FloorToInt(aabb.Max[i]);
                        step[i] = 1;
                    }

                    start[i] = Mathf.FloorToInt(aabb.Min[i]);
                }
            }

            end += step;

            for (int x = start.x; x != end.x; x += step.x)
            {
                for (int z = start.z; z != end.z; z += step.z)
                {
                    for (int y = start.y; y != end.y; y += step.y)
                    {
                        BlockData block = world.RWAccessor.GetBlock(x, y, z);
                        AABB? blockAABB = block.GetBoundingBox(x, y, z, world, true);

                        if (blockAABB == null)
                        {
                            continue;
                        }

                        AABB blockBB = blockAABB.Value;
                        bool flag = true;

                        for (int i = 0; i < 3; i++)
                        {
                            if (i != axis)
                            {
                                flag &= blockBB.Max[i] > aabb.Min[i] && blockBB.Min[i] < aabb.Max[i];
                            }
                        }

                        if (!flag)
                        {
                            continue;
                        }

                        // 下面两个 if 中的注释是多余条件。注释掉可以解决部分情况下穿墙的问题
                        if (movement > 0 /* && aabb.Max[axis] <= blockBB.Min[axis] */)
                        {
                            float maxMovement = blockBB.Min[axis] - aabb.Max[axis];

                            if (movement > maxMovement)
                            {
                                CalculateVelocityAfterCollisionWithTerrain(material, block, ref velocity);
                                movement = maxMovement;
                                return true;
                            }
                        }
                        else if (movement < 0 /* && aabb.Min[axis] >= blockBB.Max[axis] */)
                        {
                            float maxMovement = blockBB.Max[axis] - aabb.Min[axis];

                            if (movement < maxMovement)
                            {
                                CalculateVelocityAfterCollisionWithTerrain(material, block, ref velocity);
                                movement = maxMovement;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}