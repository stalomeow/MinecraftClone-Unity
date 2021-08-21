using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace Minecraft.Rendering.Jobs
{
    [BurstCompile]
    public struct FrustumCullingJob : IJobParallelForFilter
    {
        [ReadOnly] public NativeArray<float4> Planes;
        [ReadOnly] public NativeArray<int3> Sections;
        [ReadOnly] public int3 SectionOffset;
        [ReadOnly] public int3 SectionSize;

        public bool Execute(int index)
        {
            float3 aabbMin = Sections[index] + SectionOffset;
            float3 aabbMax = aabbMin + SectionSize;

            for (int i = 0; i < 6; i++)
            {
                float4 plane = Planes[i];
                float4 min = float4(aabbMin, 1);
                float4 max = float4(aabbMax, 1);

                if (plane.x > 0)
                {
                    min.x = aabbMax.x;
                    max.x = aabbMin.x;
                }

                if (plane.y > 0)
                {
                    min.y = aabbMax.y;
                    max.y = aabbMin.y;
                }

                if (plane.z > 0)
                {
                    min.z = aabbMax.z;
                    max.z = aabbMin.z;
                }

                if (dot(plane, min) <= 0 && dot(plane, max) <= 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
