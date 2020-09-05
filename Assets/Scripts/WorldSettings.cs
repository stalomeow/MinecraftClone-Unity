using System;
using UnityEngine;

namespace Minecraft
{
    [Serializable]
    public class WorldSettings
    {
        public string Name; // 别改

        public WorldType Type; // 别改
        public PlayMode Mode;

        public int Seed; // 别改

        public Vector3 Position;
        public Quaternion BodyRotation;
        public Quaternion CameraRotation;

        public string ResourcePackageName;


        public static WorldSettings Active = null;
    }
}