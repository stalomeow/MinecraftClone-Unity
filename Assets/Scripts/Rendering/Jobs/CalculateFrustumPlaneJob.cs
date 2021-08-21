using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace Minecraft.Rendering.Jobs
{
    [BurstCompile]
    public struct CalculateFrustumPlaneJob : IJob
    {
        public float Near;
        public float Far;
        public float FOV;
        public float Aspect;
        public float3x4 Camera;
        [WriteOnly] public NativeArray<float4> Planes;

        public void Execute()
        {
            float halfHeight = Far * tan(radians(FOV * 0.5f));
            float3 up = Camera.c1 * halfHeight;
            float3 right = Camera.c0 * halfHeight * Aspect;
            float3 nearCenter = Camera.c3 + Near * Camera.c2;
            float3 farCenter = Camera.c3 + Far * Camera.c2;
            float3x4 corners = float3x4(
                farCenter - up - right,
                farCenter - up + right,
                farCenter + up - right,
                farCenter + up + right
            );

            Planes[0] = CalculatePlane(Camera.c3, corners.c2, corners.c0); // left
            Planes[1] = CalculatePlane(Camera.c3, corners.c1, corners.c3); // right
            Planes[2] = CalculatePlane(Camera.c3, corners.c3, corners.c2); // top
            Planes[3] = CalculatePlane(Camera.c3, corners.c0, corners.c1); // down
            Planes[4] = CalculatePlane(Camera.c2, nearCenter); // near
            Planes[5] = CalculatePlane(-Camera.c2, farCenter); // far
        }

        private float4 CalculatePlane(in float3 a, in float3 b, in float3 c)
        {
            float3 normal = normalize(cross(b - a, c - a));
            return CalculatePlane(normal, a);
        }

        private float4 CalculatePlane(in float3 normal, in float3 a)
        {
            return float4(normal, -dot(normal, a));
        }
    }
}
