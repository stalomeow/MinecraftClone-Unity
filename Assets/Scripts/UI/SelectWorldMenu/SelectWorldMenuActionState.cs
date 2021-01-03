using System;
using ToaruUnity.UI;
using UnityEngine;

namespace Minecraft.UI
{
    internal sealed class SelectWorldMenuActionState : IActionState
    {
        public struct WorldMeta
        {
            public string Name;
            public Texture2D Thumbnail;
            public DateTime Date;
        }

        public WorldMeta[] Worlds;
    }
}