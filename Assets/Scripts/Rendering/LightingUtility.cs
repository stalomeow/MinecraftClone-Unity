using Minecraft.Configurations;
using UnityEngine;

namespace Minecraft.Rendering
{
    [XLua.LuaCallCSharp]
    public static class LightingUtility
    {
        public const int MaxLight = 15;
        public const int SkyLight = MaxLight - 1; // *避免第一个实际接受到天空光照的方块过亮

        public const int MaxBlockFaceCount = 6;
        public const int MaxBlockFaceCornerCount = 4;
        public const int AmbientLightSampleCount = 4;

        public static readonly Vector3Int[,,] AmbientLightSampleDirections = new Vector3Int[MaxBlockFaceCount, MaxBlockFaceCornerCount, AmbientLightSampleCount]
        {
            // BlockFace.PositiveX
            {
                // BlockFaceCorner.LeftBottom
                {
                    new Vector3Int(1, 0, 0),
                    new Vector3Int(1, -1, 0),
                    new Vector3Int(1, 0, -1),
                    new Vector3Int(1, -1, -1)
                },
                // BlockFaceCorner.RightBottom
                {
                    new Vector3Int(1, 0, 0),
                    new Vector3Int(1, -1, 0),
                    new Vector3Int(1, 0, 1),
                    new Vector3Int(1, -1, 1)
                },
                // BlockFaceCorner.LeftTop
                {
                    new Vector3Int(1, 0, 0),
                    new Vector3Int(1, 1, 0),
                    new Vector3Int(1, 0, -1),
                    new Vector3Int(1, 1, -1)
                },
                // BlockFaceCorner.RightTop
                {
                    new Vector3Int(1, 0, 0),
                    new Vector3Int(1, 1, 0),
                    new Vector3Int(1, 0, 1),
                    new Vector3Int(1, 1, 1)
                }
            },
            // BlockFace.PositiveY
            {
                // BlockFaceCorner.LeftBottom
                {
                    new Vector3Int(0, 1, 0),
                    new Vector3Int(0, 1, -1),
                    new Vector3Int(-1, 1, 0),
                    new Vector3Int(-1, 1, -1)
                },
                // BlockFaceCorner.RightBottom
                {
                    new Vector3Int(0, 1, 0),
                    new Vector3Int(0, 1, -1),
                    new Vector3Int(1, 1, 0),
                    new Vector3Int(1, 1, -1)
                },
                // BlockFaceCorner.LeftTop
                {
                    new Vector3Int(0, 1, 0),
                    new Vector3Int(0, 1, 1),
                    new Vector3Int(-1, 1, 0),
                    new Vector3Int(-1, 1, 1)
                },
                // BlockFaceCorner.RightTop
                {
                    new Vector3Int(0, 1, 0),
                    new Vector3Int(0, 1, 1),
                    new Vector3Int(1, 1, 0),
                    new Vector3Int(1, 1, 1)
                }
            },
            // BlockFace.PositiveZ
            {
                // BlockFaceCorner.LeftBottom
                {
                    new Vector3Int(0, 0, 1),
                    new Vector3Int(0, -1, 1),
                    new Vector3Int(1, 0, 1),
                    new Vector3Int(1, -1, 1)
                },
                // BlockFaceCorner.RightBottom
                {
                    new Vector3Int(0, 0, 1),
                    new Vector3Int(0, -1, 1),
                    new Vector3Int(-1, 0, 1),
                    new Vector3Int(-1, -1, 1)
                },
                // BlockFaceCorner.LeftTop
                {
                    new Vector3Int(0, 0, 1),
                    new Vector3Int(0, 1, 1),
                    new Vector3Int(1, 0, 1),
                    new Vector3Int(1, 1, 1)
                },
                // BlockFaceCorner.RightTop
                {
                    new Vector3Int(0, 0, 1),
                    new Vector3Int(0, 1, 1),
                    new Vector3Int(-1, 0, 1),
                    new Vector3Int(-1, 1, 1)
                }
            },
            // BlockFace.NegativeX
            {
                // BlockFaceCorner.LeftBottom
                {
                    new Vector3Int(-1, 0, 0),
                    new Vector3Int(-1, -1, 0),
                    new Vector3Int(-1, 0, 1),
                    new Vector3Int(-1, -1, 1)
                },
                // BlockFaceCorner.RightBottom
                {
                    new Vector3Int(-1, 0, 0),
                    new Vector3Int(-1, -1, 0),
                    new Vector3Int(-1, 0, -1),
                    new Vector3Int(-1, -1, -1)
                },
                // BlockFaceCorner.LeftTop
                {
                    new Vector3Int(-1, 0, 0),
                    new Vector3Int(-1, 1, 0),
                    new Vector3Int(-1, 0, 1),
                    new Vector3Int(-1, 1, 1)
                },
                // BlockFaceCorner.RightTop
                {
                    new Vector3Int(-1, 0, 0),
                    new Vector3Int(-1, 1, 0),
                    new Vector3Int(-1, 0, -1),
                    new Vector3Int(-1, 1, -1)
                }
            },
            // BlockFace.NegativeY
            {
                // BlockFaceCorner.LeftBottom
                {
                    new Vector3Int(0, -1, 0),
                    new Vector3Int(0, -1, -1),
                    new Vector3Int(1, -1, 0),
                    new Vector3Int(1, -1, -1)
                },
                // BlockFaceCorner.RightBottom
                {
                    new Vector3Int(0, -1, 0),
                    new Vector3Int(0, -1, -1),
                    new Vector3Int(-1, -1, 0),
                    new Vector3Int(-1, -1, -1)
                },
                // BlockFaceCorner.LeftTop
                {
                    new Vector3Int(0, -1, 0),
                    new Vector3Int(0, -1, 1),
                    new Vector3Int(1, -1, 0),
                    new Vector3Int(1, -1, 1)
                },
                // BlockFaceCorner.RightTop
                {
                    new Vector3Int(0, -1, 0),
                    new Vector3Int(0, -1, 1),
                    new Vector3Int(-1, -1, 0),
                    new Vector3Int(-1, -1, 1)
                }
            },
            // BlockFace.NegativeZ
            {
                // BlockFaceCorner.LeftBottom
                {
                    new Vector3Int(0, 0, -1),
                    new Vector3Int(0, -1, -1),
                    new Vector3Int(-1, 0, -1),
                    new Vector3Int(-1, -1, -1)
                },
                // BlockFaceCorner.RightBottom
                {
                    new Vector3Int(0, 0, -1),
                    new Vector3Int(0, -1, -1),
                    new Vector3Int(1, 0, -1),
                    new Vector3Int(1, -1, -1)
                },
                // BlockFaceCorner.LeftTop
                {
                    new Vector3Int(0, 0, -1),
                    new Vector3Int(0, 1, -1),
                    new Vector3Int(-1, 0, -1),
                    new Vector3Int(-1, 1, -1)
                },
                // BlockFaceCorner.RightTop
                {
                    new Vector3Int(0, 0, -1),
                    new Vector3Int(0, 1, -1),
                    new Vector3Int(1, 0, -1),
                    new Vector3Int(1, 1, -1)
                }
            },
        };

        public static bool IsOpaqueBlock(this BlockData block)
        {
            return block.LightOpacity == MaxLight;
        }

        public static float MapLight01(float light)
        {
            return Mathf.Clamp01(light / MaxLight);
        }

        public static Vector2 MapLight01(Vector2 light)
        {
            return new Vector2(MapLight01(light.x), MapLight01(light.y));
        }

        public static float GetEmissionValue(this BlockData block)
        {
            return MapLight01(block.LightValue);
        }

        public static int GetBlockedLight(int light, BlockData block)
        {
            return Mathf.Clamp(light - block.LightOpacity, 0, MaxLight);
        }

        public static Vector2 AmbientOcclusion(int x, int y, int z, BlockFace face, BlockFaceCorner corner, IWorldRAccessor accessor)
        {
            int faceIndex = (int)face;
            int cornerIndex = (int)corner;
            Vector2 lights = Vector2Int.zero;

            for (int i = 0; i < AmbientLightSampleCount; i++)
            {
                Vector3Int dir = AmbientLightSampleDirections[faceIndex, cornerIndex, i];
                lights.x += accessor.GetSkyLight(x + dir.x, y + dir.y, z + dir.z);
                lights.y += accessor.GetAmbientLight(x + dir.x, y + dir.y, z + dir.z);
            }

            return MapLight01(lights / AmbientLightSampleCount);
        }
    }
}