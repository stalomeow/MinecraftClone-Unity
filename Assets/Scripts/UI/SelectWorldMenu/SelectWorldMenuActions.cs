using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ToaruUnity.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Minecraft.UI
{
    internal sealed class SelectWorldMenuActions : ActionCenter
    {
        protected override IActionState CreateState() => new SelectWorldMenuActionState();

        protected override void ResetState(ref IActionState state) => (state as SelectWorldMenuActionState).Worlds = null;

        [Action]
        public bool Close()
        {
            Manager.CloseActiveView();
            return false;
        }

        [Action("New World")]
        public bool OpenNewWorldWindow(AssetReference reference)
        {
            Manager.SwitchView(reference, SwitchViewMode.NavigateOrOpenNew);
            return false;
        }

        [Action("Load Worlds")]
        public IEnumerator<bool> LoadWorlds()
        {
            string folder = Application.persistentDataPath + "/Worlds";

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                yield break;
            }

            Task<string[]> task = Task<string[]>.Factory.StartNew(() =>
            {
                string[] ws = Directory.GetDirectories(folder);
                Array.Sort(ws, (w1, w2) => File.GetLastAccessTime(w1) > File.GetLastAccessTime(w2) ? -1 : 1);
                return ws;
            });

            while (!task.IsCompleted)
            {
                yield return false;
            }

            string[] worlds = task.Result;
            SelectWorldMenuActionState.WorldMeta[] metas = new SelectWorldMenuActionState.WorldMeta[worlds.Length];

            for (int i = 0; i < worlds.Length; i++)
            {
                string uri = $"file://{worlds[i]}/Thumbnail.png";

                using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(uri, false))
                {
                    request.SendWebRequest();

                    while (!request.isDone)
                    {
                        yield return false;
                    }

                    string name = Path.GetFileNameWithoutExtension(worlds[i]);
                    Texture2D thumbnail = (request.downloadHandler as DownloadHandlerTexture).texture;
                    DateTime date = File.GetLastAccessTime(worlds[i]);

                    metas[i] = new SelectWorldMenuActionState.WorldMeta
                    {
                        Name = name,
                        Thumbnail = thumbnail,
                        Date = date
                    };
                }

                yield return false;
            }

            SelectWorldMenuActionState state = GetState<SelectWorldMenuActionState>();
            state.Worlds = metas;
            yield return true; // 告诉ActionCenter状态被更新
        }

        [Action("Load World Settings")]
        public bool LoadWorldSettings(string name)
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/Worlds/" + name + "/settings.json");
            WorldSettings.Active = JsonUtility.FromJson<WorldSettings>(json);
            LoadingUtility.LoadSceneAsync(1);
            return false;
        }
    }
}