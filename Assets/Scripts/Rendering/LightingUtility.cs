using System.Reflection.Emit;
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

        public static Vector2 AmbientOcclusion(Vector3Int pos, BlockFace face, BlockFaceCorner corner, IWorldRAccessor accessor, bool fastMode)
        {
            if (fastMode)
            {
                float skyLight = MapLight01(accessor.GetSkyLight(pos.x, pos.y, pos.z));
                float ambient = MapLight01(accessor.GetAmbientLight(pos.x, pos.y, pos.z));
                return new Vector2(skyLight, ambient);
            }

            int faceIndex = (int)face;
            int cornerIndex = (int)corner;
            Vector2 lights = Vector2Int.zero;

            for (int i = 0; i < AmbientLightSampleCount; i++)
            {
                Vector3Int p = pos + AmbientLightSampleDirections[faceIndex, cornerIndex, i];
                lights.x += accessor.GetSkyLight(p.x, p.y, p.z);
                lights.y += accessor.GetAmbientLight(p.x, p.y, p.z);
            }

            return MapLight01(lights / AmbientLightSampleCount);
        }
    }
}