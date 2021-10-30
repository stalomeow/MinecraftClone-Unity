using System.Collections.Generic;
using Minecraft.Assets;
using Minecraft.Configurations;
using Minecraft.PhysicSystem;
using Minecraft.Rendering;
using MinecraftEditor.Assets;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using ArrayUtility = Minecraft.Utils.ArrayUtility;

namespace MinecraftEditor.Configurations.MCConfigEditor
{
    public class BlockInspector : WindowInspector
    {
        private static GUIContent s_IDContent = new GUIContent("ID");
        private static GUIContent s_InternalNameContent = new GUIContent("Internal Name");

        private static GUIContent s_RewardItemContent = new GUIContent("Reward Item");
        private static GUIContent s_FlagsContent = new GUIContent("Flags");
        private static GUIContent s_RotationAxesContent = new GUIContent("Rotation Axes");
        private static GUIContent s_EntityConversionContent = new GUIContent("Entity Conversion");

        private static GUIContent s_HardnessContent = new GUIContent("Hardness");
        private static GUIContent s_LightValueContent = new GUIContent("Light Value");
        private static GUIContent s_LightOpacityContent = new GUIContent("Light Opacity");
        private static GUIContent s_PhysicStateContent = new GUIContent("Physic State");
        private static GUIContent s_PhysicMaterialContent = new GUIContent("Physic Material");
        private static GUIContent s_CoefficientOfRestitutionContent = new GUIContent("Coefficient Of Restitution");
        private static GUIContent s_CoefficientOfStaticFrictionContent = new GUIContent("Coefficient Of Static Friction");
        private static GUIContent s_CoefficientOfDynamicFrictionContent = new GUIContent("Coefficient Of Dynamic Friction");

        private static GUIContent s_MeshContent = new GUIContent("Mesh");
        private static GUIContent s_MaterialContent = new GUIContent("Material");
        private static GUIContent[] s_TextureContents = new GUIContent[]
        {
            new GUIContent("Albedo"),
            new GUIContent("Normal"),
            new GUIContent("MER")
        };

        private static GUIContent s_DigAudioContent = new GUIContent("Dig Audio");
        private static GUIContent s_PlaceAudioContent = new GUIContent("Place Audio");
        private static GUIContent s_StepAudiosContent = new GUIContent("Step Audios");

        private static string[] s_FieldCategoryNames = new string[]
        {
            "Common", "Physics", "Rendering", "Audio"
        };


        private int m_SelectedCategoryIndex;
        private ReorderableList m_StepAudioList;

        public BlockInspector(MainWindow mainWindow) : base(mainWindow)
        {
            m_SelectedCategoryIndex = 0;
        }

        protected override void OnBeforeScrollableGUI(ref VerticalGUIRect rect, int index)
        {
            base.OnBeforeScrollableGUI(ref rect, index);

            rect.Space(10);
            m_SelectedCategoryIndex = GUI.Toolbar(rect.GetNext(22, false, true), m_SelectedCategoryIndex, s_FieldCategoryNames);
            rect.Space(10);
        }

        protected override void OnScrollableGUI(ref VerticalGUIRect rect, int index)
        {
            BlockData block = MainWnd.Blocks[index];

            switch (m_SelectedCategoryIndex)
            {
                case 0: CommonFieldsGUI(ref rect, block); break;
                case 1: PhysicsFieldsGUI(ref rect, block); break;
                case 2: RenderingFieldsGUI(ref rect, block); break;
                case 3: AudioFieldsGUI(ref rect, block); break;
            }
        }

        private void CommonFieldsGUI(ref VerticalGUIRect guiRect, BlockData block)
        {
            using (new EditorGUI.DisabledGroupScope(true))
            {
                EditorGUI.IntField(guiRect.Next, s_IDContent, block.ID);
            }

            block.InternalName = EditorGUI.TextField(guiRect.Next, s_InternalNameContent, block.InternalName);

            block.RewardItem = EditorGUI.TextField(guiRect.Next, s_RewardItemContent, block.RewardItem);
            block.Flags = (BlockFlags)EditorGUI.EnumFlagsField(guiRect.Next, s_FlagsContent, block.Flags);
            block.RotationAxes = (BlockRotationAxes)EditorGUI.EnumFlagsField(guiRect.Next, s_RotationAxesContent, block.RotationAxes);
            block.EntityConversion = (BlockEntityConversion)EditorGUI.EnumPopup(guiRect.Next, s_EntityConversionContent, block.EntityConversion);
        }

        private void PhysicsFieldsGUI(ref VerticalGUIRect guiRect, BlockData block)
        {
            block.Hardness = EditorGUI.IntField(guiRect.Next, s_HardnessContent, block.Hardness);
            block.LightValue = EditorGUI.IntSlider(guiRect.Next, s_LightValueContent, block.LightValue, 0, LightingUtility.MaxLight);
            block.LightOpacity = EditorGUI.IntSlider(guiRect.Next, s_LightOpacityContent, block.LightOpacity, 0, LightingUtility.MaxLight);
            block.PhysicState = (PhysicState)EditorGUI.EnumPopup(guiRect.Next, s_PhysicStateContent, block.PhysicState);
            guiRect.Space(10);

            EditorGUI.LabelField(guiRect.Next, s_PhysicMaterialContent, EditorStyles.boldLabel);
            block.PhysicMaterial.CoefficientOfRestitution = EditorGUI.Slider(guiRect.Next, s_CoefficientOfRestitutionContent, block.PhysicMaterial.CoefficientOfRestitution, 0, 1);
            block.PhysicMaterial.CoefficientOfStaticFriction = EditorGUI.FloatField(guiRect.Next, s_CoefficientOfStaticFrictionContent, block.PhysicMaterial.CoefficientOfStaticFriction);
            block.PhysicMaterial.CoefficientOfDynamicFriction = EditorGUI.FloatField(guiRect.Next, s_CoefficientOfDynamicFrictionContent, block.PhysicMaterial.CoefficientOfDynamicFriction);
        }

        private void RenderingFieldsGUI(ref VerticalGUIRect guiRect, BlockData block)
        {
            MainWnd.BlockMeshes.ElementGUI(guiRect.Next, s_MeshContent, ref block.Mesh);

            AssetPtr meshPtr = MainWnd.BlockMeshes[block.Mesh];
            BlockMesh mesh = EditorAssetUtility.LoadAssetByGUID<BlockMesh>(meshPtr.AssetGUID);

            if (!mesh)
            {
                EditorGUI.HelpBox(guiRect.GetNext(40, true, true), "You should assign an empty mesh if you do not want this block to be rendered.", MessageType.Error);
            }
            else if (mesh.Faces.Length == 0)
            {
                EditorGUI.HelpBox(guiRect.GetNext(40, true, true), "This block will never be rendered.", MessageType.Warning);
            }
            else
            {
                MainWnd.BlockMaterials.ElementGUI(guiRect.Next, s_MaterialContent, ref block.Material);

                BlockMesh.FaceData[] faces = mesh.Faces;
                ArrayUtility.EnsureArrayReferenceAndSize(ref block.Textures, faces.Length, true);

                for (int i = 0; i < faces.Length; i++)
                {
                    BlockFace face = faces[i].Face;
                    EditorGUI.LabelField(guiRect.Next, face.ToString(), EditorStyles.boldLabel);

                    ArrayUtility.EnsureArrayReferenceAndSize(ref block.Textures[i], 3, true);

                    EditorGUI.indentLevel++;
                    for (int j = 0; j < 3; j++)
                    {
                        MainWnd.BlockTextures.ElementGUI(guiRect.Next, s_TextureContents[j], ref block.Textures[i][j]);
                    }
                    EditorGUI.indentLevel--;
                }
            }
        }

        private void AudioFieldsGUI(ref VerticalGUIRect guiRect, BlockData block)
        {
            block.DigAudio = EditorAssetUtility.AssetPtrField(guiRect.Next, s_DigAudioContent, block.DigAudio, typeof(AudioClip));
            block.PlaceAudio = EditorAssetUtility.AssetPtrField(guiRect.Next, s_PlaceAudioContent, block.PlaceAudio, typeof(AudioClip));
            guiRect.Space(10);

            m_StepAudioList ??= new ReorderableList(null, typeof(AssetPtr), true, true, true, true)
            {
                elementHeight = EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight,
                drawHeaderCallback = (Rect pos) =>
                {
                    EditorGUI.LabelField(pos, s_StepAudiosContent);
                },
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    rect.y += EditorGUIUtility.standardVerticalSpacing;
                    rect.height = EditorGUIUtility.singleLineHeight;

                    AssetPtr asset = m_StepAudioList.list[index] as AssetPtr;
                    m_StepAudioList.list[index] = EditorAssetUtility.AssetPtrField(rect, GUIContent.none, asset, typeof(AudioClip));
                }
            };
            m_StepAudioList.list = block.StepAudios ??= new List<AssetPtr>();
            m_StepAudioList.DoList(guiRect.GetNext(m_StepAudioList.GetHeight(), true, true));
        }
    }
}

