using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ToaruUnity.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minecraft.UI
{
    internal sealed class NewWorldMenuActions : ActionCenter
    {
        protected override IActionState CreateState()
        {
            return new NewWorldMenuActionState();
        }

        protected override void ResetState(ref IActionState state)
        {
            NewWorldMenuActionState s = state as NewWorldMenuActionState;
            s.ResPackNames.Value = default;
            s.ErrorText.Value = default;
        }


        [Action]
        public bool Close()
        {
            Manager.CloseActiveView(out _, default);
            return false;
        }

        [Action("Create World")]
        public bool CreateWorld(string name, string s, string type, string resPackName)
        {
            NewWorldMenuActionState state = GetState<NewWorldMenuActionState>();

            if (string.IsNullOrEmpty(name))
            {
                state.ErrorText.Value = "invalid world name";
                return true;
            }

            string folder = Application.dataPath + "/Worlds/" + name;

            if (Directory.Exists(folder))
            {
                state.ErrorText.Value = "world has already existed";
                return true;
            }

            if (!int.TryParse(s, out int seed))
            {
                seed = string.IsNullOrEmpty(s) ? (Process.GetCurrentProcess().Id + DateTime.Now.GetHashCode()) : s.GetHashCode();
            }

            WorldType worldType;

            switch (type)
            {
                case "Endless":
                    worldType = WorldType.Normal;
                    break;
                case "Plain":
                    worldType = WorldType.Plain;
                    break;
                case "Old":
                    worldType = WorldType.Fixed;
                    break;
                default:
                    state.ErrorText.Value = "invalid world type";
                    return true;
            }

            if (string.IsNullOrEmpty(resPackName))
            {
                resPackName = WorldConsts.DefaultResourcePackageName;
            }

            WorldSettings.Active = new WorldSettings
            {
                Name = name,
                Type = worldType,
                Mode = PlayMode.Creative,
                Seed = seed,
                Position = Vector3.down,
                BodyRotation = Quaternion.identity,
                CameraRotation = Quaternion.identity,
                ResourcePackageName = resPackName
            };

            LoadingUtility.LoadSceneAsync(1);
            return false;
        }

        [Action("Load ResPack")]
        public IEnumerator<bool> LoadResPack()
        {
            string path = Path.Combine(Application.streamingAssetsPath, WorldConsts.ResourcePackagesFolderName);
            string[] packs = Directory.GetDirectories(path);
            yield return false;

            string[] names = new string[packs.Length];

            for (int i = 0; i < packs.Length; i++)
            {
                names[i] = Path.GetFileNameWithoutExtension(packs[i]);
            }

            GetState<NewWorldMenuActionState>().ResPackNames.Value = names;
            yield return true;
        }
    }
}