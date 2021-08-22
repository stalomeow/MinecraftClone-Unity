using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace MinecraftEditor.Assets
{
    public static class MenuUtils
    {
        public const string LoadFromAssetBundleFilesDefine = "LOAD_ASSET_BUNDLE_FROM_FILE";

        public const string LoadFromAssetBundleFilesMenu = "Minecraft-Unity/Assets/Load Mode/From AssetBundle Files";

        public const string LoadFromAssetDatabaseMenu = "Minecraft-Unity/Assets/Load Mode/From AssetDatabase";


        [MenuItem(LoadFromAssetBundleFilesMenu, false)]
        public static void LoadFromAssetBundleFilesMenuItem()
        {
            BuildTargetGroup group = EditorUserBuildSettings.selectedBuildTargetGroup;
            PlayerSettings.GetScriptingDefineSymbolsForGroup(group, out string[] defines);

            if (!defines.Contains(LoadFromAssetBundleFilesDefine))
            {
                defines = defines.Append(LoadFromAssetBundleFilesDefine).ToArray();
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, defines);
            }
        }

        [MenuItem(LoadFromAssetBundleFilesMenu, true)]
        public static bool LoadFromAssetBundleFilesMenuItemValidate()
        {
            BuildTargetGroup group = EditorUserBuildSettings.selectedBuildTargetGroup;
            PlayerSettings.GetScriptingDefineSymbolsForGroup(group, out string[] defines);
            Menu.SetChecked(LoadFromAssetBundleFilesMenu, defines.Contains(LoadFromAssetBundleFilesDefine));
            return true;
        }


        [MenuItem(LoadFromAssetDatabaseMenu, false)]
        public static void LoadFromAssetDatabaseMenuItem()
        {
            BuildTargetGroup group = EditorUserBuildSettings.selectedBuildTargetGroup;
            PlayerSettings.GetScriptingDefineSymbolsForGroup(group, out string[] defines);
            List<string> defineList = defines.ToList();

            if (defineList.Remove(LoadFromAssetBundleFilesDefine))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, defineList.ToArray());
            }
        }

        [MenuItem(LoadFromAssetDatabaseMenu, true)]
        public static bool LoadFromAssetDatabaseMenuItemValidate()
        {
            BuildTargetGroup group = EditorUserBuildSettings.selectedBuildTargetGroup;
            PlayerSettings.GetScriptingDefineSymbolsForGroup(group, out string[] defines);
            Menu.SetChecked(LoadFromAssetDatabaseMenu, !defines.Contains(LoadFromAssetBundleFilesDefine));
            return true;
        }
    }
}
