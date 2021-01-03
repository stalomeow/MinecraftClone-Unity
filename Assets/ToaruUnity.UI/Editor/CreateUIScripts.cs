using System.IO;
using ToaruUnity.UI;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace ToaruUnityEditor.UI
{
    public class CreateUIScripts
    {
        [MenuItem("Assets/Create/ToaruUnity/UI C# Scripts/Default View")]
        public static void CreatDefaultViewScripts()
        {
            EndNameEditAction action = ScriptableObject.CreateInstance<CreateViewScripts>();
            Texture2D icon = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, action, "NewDefaultView", icon, nameof(AbstractView));
        }

        [MenuItem("Assets/Create/ToaruUnity/UI C# Scripts/Default View (UGUI)")]
        public static void CreatDefaultUGUIViewScripts()
        {
            EndNameEditAction action = ScriptableObject.CreateInstance<CreateViewScripts>();
            Texture2D icon = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, action, "NewDefaultView", icon, nameof(AbstractUGUIView));
        }

        [MenuItem("Assets/Create/ToaruUnity/UI C# Scripts/Tween View (UGUI)")]
        public static void CreatDefaultTweenUGUIViewScripts()
        {
            EndNameEditAction action = ScriptableObject.CreateInstance<CreateViewScripts>();
            Texture2D icon = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, action, "NewTweenView", icon, nameof(TweenUGUIView));
        }
    }

    public class CreateViewScripts : EndNameEditAction
    {
        public const string CLASS_NAME = "#CLASS_NAME#";
        public const string BASE_NAME = "#BASE_NAME#";
        public const string ACTIONS_NAME = "#ACTIONS_NAME#";
        public const string ACTION_STATE_NAME = "#ACTION_STATE_NAME#";

        private const string VIEW_TEMPLATE = @"using System.Collections;
using System.Collections.Generic;
using ToaruUnity.UI;
using UnityEngine;

[InjectActions(typeof(#ACTIONS_NAME#))]
public class #CLASS_NAME# : #BASE_NAME#
{
	
}
";

        private const string ACTION_STATE_TEMPLATE = @"using System.Collections;
using System.Collections.Generic;
using ToaruUnity.UI;
using UnityEngine;

class #CLASS_NAME# : IActionState
{

}
";

        private const string ACTIONS_TEMPLATE = @"using System.Collections;
using System.Collections.Generic;
using ToaruUnity.UI;
using UnityEngine;

class #CLASS_NAME# : ActionCenter
{
    protected override IActionState CreateState()
    {
        return new #ACTION_STATE_NAME#();
    }

    protected override void ResetState(ref IActionState state)
    {
        state = new #ACTION_STATE_NAME#();
    }
}
";

        public override void Action(int instanceId, string pathName, string baseClassName)
        {
            string viewName = Path.GetFileName(pathName).Replace(" ", "");
            string actionStateName = viewName + "ActionState";
            string actionsName = viewName + "Actions";

            pathName = Path.Combine(Path.GetDirectoryName(pathName), viewName);
            Directory.CreateDirectory(pathName);

            string viewScript = Path.Combine(pathName, viewName + ".cs");
            string actionStateScript = Path.Combine(pathName, actionStateName + ".cs");
            string actionsScript = Path.Combine(pathName, actionsName + ".cs");

            using (StreamWriter writer = File.CreateText(viewScript))
            {
                string text = VIEW_TEMPLATE;
                text = text.Replace(CLASS_NAME, viewName);
                text = text.Replace(BASE_NAME, baseClassName);
                text = text.Replace(ACTIONS_NAME, actionsName);

                writer.Write(text);
            }

            using (StreamWriter writer = File.CreateText(actionStateScript))
            {
                string text = ACTION_STATE_TEMPLATE;
                text = text.Replace(CLASS_NAME, actionStateName);

                writer.Write(text);
            }

            using (StreamWriter writer = File.CreateText(actionsScript))
            {
                string text = ACTIONS_TEMPLATE;
                text = text.Replace(CLASS_NAME, actionsName);
                text = text.Replace(ACTION_STATE_NAME, actionStateName);

                writer.Write(text);
            }

            AssetDatabase.Refresh();
        }
    }
}