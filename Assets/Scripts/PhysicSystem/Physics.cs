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
                AABB boundingBox = block.GetBoundingBox(pos);

                if (block.HasFlag(BlockFlags.IgnoreCollisions) || !selectBlock(block))
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
            int y = Mathf.FloorToInt(aabb.Min.y) - 1;

            if (y >= 0 && y < ChunkHeight)
            {
                int startX = Mathf.FloorToInt(aabb.Min.x);
                int endX = Mathf.FloorToInt(aabb.Max.x - 0.01f);
                int startZ = Mathf.FloorToInt(aabb.Min.z);
                int endZ = Mathf.FloorToInt(aabb.Max.z - 0.01f);

                for (int x = startX; x <= endX; x++)
                {
                    for (int z = startZ; z <= endZ; z++)
                    {
                        groundBlock = world.RWAccessor.GetBlock(x, y, z);

                        if (groundBlock == null || groundBlock.HasFlag(BlockFlags.IgnoreCollisions))
                        {
                            continue;
                        }

                        AABB blockAABB = groundBlock.GetBoundingBox(x, y, z);

                        if (aabb.Intersects(blockAABB))
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

            movement = velocity * time;
            CollisionFlags flags = CollisionFlags.None;

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

            static void CalculateVelocity(BlockData block, PhysicMaterial material, ref float velocity)
            {
                float e = block.PhysicMaterial.CoefficientOfRestitution;
                e = PhysicMaterial.CombineValue(e, material.CoefficientOfRestitution);
                velocity *= -e;

                if (Mathf.Abs(velocity) <= Mathf.Epsilon)
                {
                    velocity = 0;
                }
            }

            if (movement.y > 0)
            {
                int startX = Mathf.FloorToInt(aabb.Min.x);
                int endX = Mathf.FloorToInt(aabb.Max.x - 0.01f);
                int startY = Mathf.FloorToInt(aabb.Max.y);
                int endY = Mathf.FloorToInt(aabb.Max.y + movement.y);
                int startZ = Mathf.FloorToInt(aabb.Min.z);
                int endZ = Mathf.FloorToInt(aabb.Max.z - 0.01f);

                for (int x = startX; x <= endX; x++)
                {
                    for (int z = startZ; z <= endZ; z++)
                    {
                        for (int y = startY; y <= endY; y++)
                        {
                            BlockData block = world.RWAccessor.GetBlock(x, y, z);

                            if (block != null && !block.HasFlag(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = block.GetBoundingBox(x, y, z);

                                if (aabb.Intersects(blockAABB))
                                {
                                    flags |= CollisionFlags.Above;
                                    movement.y = 0;
                                    velocity.y = 0;
                                }
                                else
                                {
                                    float maxMovement = blockAABB.Min.y - aabb.Max.y;

                                    if (movement.y > maxMovement)
                                    {
                                        flags |= CollisionFlags.Above;
                                        movement.y = maxMovement;
                                        CalculateVelocity(block, material, ref velocity.y);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
            else if (movement.y < 0)
            {
                int startX = Mathf.FloorToInt(aabb.Min.x);
                int endX = Mathf.FloorToInt(aabb.Max.x - 0.01f);
                int startY = Mathf.FloorToInt(aabb.Min.y);
                int endY = Mathf.FloorToInt(aabb.Min.y + movement.y);
                int startZ = Mathf.FloorToInt(aabb.Min.z);
                int endZ = Mathf.FloorToInt(aabb.Max.z - 0.01f);

                for (int x = startX; x <= endX; x++)
                {
                    for (int z = startZ; z <= endZ; z++)
                    {
                        for (int y = startY; y >= endY; y--)
                        {
                            BlockData block = world.RWAccessor.GetBlock(x, y, z);

                            if (block != null && !block.HasFlag(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = block.GetBoundingBox(x, y, z);

                                if (aabb.Intersects(blockAABB))
                                {
                                    flags |= CollisionFlags.Below;
                                    movement.y = 0;
                                    velocity.y = 0;
                                }
                                else
                                {
                                    float maxMovement = blockAABB.Max.y - aabb.Min.y; // negative

                                    if (movement.y < maxMovement)
                                    {
                                        flags |= CollisionFlags.Below;
                                        movement.y = maxMovement;
                                        CalculateVelocity(block, material, ref velocity.y);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

            aabb += new Vector3(0, movement.y, 0);

            if (movement.x > 0)
            {
                int startX = Mathf.FloorToInt(aabb.Max.x);
                int endX = Mathf.FloorToInt(aabb.Max.x + movement.x);
                int startY = Mathf.FloorToInt(aabb.Min.y);
                int endY = Mathf.FloorToInt(aabb.Max.y - 0.01f);
                int startZ = Mathf.FloorToInt(aabb.Min.z);
                int endZ = Mathf.FloorToInt(aabb.Max.z - 0.01f);

                for (int z = startZ; z <= endZ; z++)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        for (int x = startX; x <= endX; x++)
                        {
                            BlockData block = world.RWAccessor.GetBlock(x, y, z);

                            if (block != null && !block.HasFlag(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = block.GetBoundingBox(x, y, z);

                                if (aabb.Intersects(blockAABB))
                                {
                                    flags |= CollisionFlags.Sides;
                                    movement.x = 0;
                                    velocity.x = 0;
                                }
                                else
                                {
                                    float maxMovement = blockAABB.Min.x - aabb.Max.x;

                                    if (movement.x > maxMovement)
                                    {
                                        flags |= CollisionFlags.Sides;
                                        movement.x = maxMovement;
                                        CalculateVelocity(block, material, ref velocity.x);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
            else if (movement.x < 0)
            {
                int startX = Mathf.FloorToInt(aabb.Min.x);
                int endX = Mathf.FloorToInt(aabb.Min.x + movement.x);
                int startY = Mathf.FloorToInt(aabb.Min.y);
                int endY = Mathf.FloorToInt(aabb.Max.y - 0.01f);
                int startZ = Mathf.FloorToInt(aabb.Min.z);
                int endZ = Mathf.FloorToInt(aabb.Max.z - 0.01f);

                for (int z = startZ; z <= endZ; z++)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        for (int x = startX; x >= endX; x--)
                        {
                            BlockData block = world.RWAccessor.GetBlock(x, y, z);

                            if (block != null && !block.HasFlag(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = block.GetBoundingBox(x, y, z);

                                if (aabb.Intersects(blockAABB))
                                {
                                    flags |= CollisionFlags.Sides;
                                    movement.x = 0;
                                    velocity.x = 0;
                                }
                                else
                                {
                                    float maxMovement = blockAABB.Max.x - aabb.Min.x; // negative

                                    if (movement.x < maxMovement)
                                    {
                                        flags |= CollisionFlags.Sides;
                                        movement.x = maxMovement;
                                        CalculateVelocity(block, material, ref velocity.x);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

            aabb += new Vector3(movement.x, 0, 0);

            if (movement.z > 0)
            {
                int startX = Mathf.FloorToInt(aabb.Min.x);
                int endX = Mathf.FloorToInt(aabb.Max.x - 0.01f);
                int startY = Mathf.FloorToInt(aabb.Min.y);
                int endY = Mathf.FloorToInt(aabb.Max.y - 0.01f);
                int startZ = Mathf.FloorToInt(aabb.Max.z);
                int endZ = Mathf.FloorToInt(aabb.Max.z + movement.z);

                for (int x = startX; x <= endX; x++)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        for (int z = startZ; z <= endZ; z++)
                        {
                            BlockData block = world.RWAccessor.GetBlock(x, y, z);

                            if (block != null && !block.HasFlag(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = block.GetBoundingBox(x, y, z);

                                if (aabb.Intersects(blockAABB))
                                {
                                    flags |= CollisionFlags.Sides;
                                    movement.z = 0;
                                    velocity.z = 0;
                                }
                                else
                                {
                                    float maxMovement = blockAABB.Min.z - aabb.Max.z;

                                    if (movement.z > maxMovement)
                                    {
                                        flags |= CollisionFlags.Sides;
                                        movement.z = maxMovement;
                                        CalculateVelocity(block, material, ref velocity.z);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
            else if (movement.z < 0)
            {
                int startX = Mathf.FloorToInt(aabb.Min.x);
                int endX = Mathf.FloorToInt(aabb.Max.x - 0.01f);
                int startY = Mathf.FloorToInt(aabb.Min.y);
                int endY = Mathf.FloorToInt(aabb.Max.y - 0.01f);
                int startZ = Mathf.FloorToInt(aabb.Min.z);
                int endZ = Mathf.FloorToInt(aabb.Min.z + movement.z);

                for (int x = startX; x <= endX; x++)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        for (int z = startZ; z >= endZ; z--)
                        {
                            BlockData block = world.RWAccessor.GetBlock(x, y, z);

                            if (block != null && !block.HasFlag(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = block.GetBoundingBox(x, y, z);

                                if (aabb.Intersects(blockAABB))
                                {
                                    flags |= CollisionFlags.Sides;
                                    movement.z = 0;
                                    velocity.z = 0;
                                }
                                else
                                {
                                    float maxMovement = blockAABB.Max.z - aabb.Min.z; // negative

                                    if (movement.z < maxMovement)
                                    {
                                        flags |= CollisionFlags.Sides;
                                        movement.z = maxMovement;
                                        CalculateVelocity(block, material, ref velocity.z);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

            return flags;
        }
    }
}