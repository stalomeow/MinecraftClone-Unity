using System;
using UnityEngine;

namespace Minecraft
{
    [Serializable]
    [XLua.LuaCallCSharp]
    public class WorldSetting
    {
        public string Name;
        public int Seed;
        public Vector3 PlayerPosition;
        public Quaternion PlayerRotation;
        public Quaternion CameraRotation;
        public string ResourcePackageName;
    }
}