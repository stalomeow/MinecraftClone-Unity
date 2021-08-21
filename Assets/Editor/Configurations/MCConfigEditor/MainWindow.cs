using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Minecraft.Configurations;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MinecraftEditor.Configurations.MCConfigEditor
{
    public class MainWindow : EditorWindow
    {
        [MenuItem("Minecraft-Unity/MC Config Editor")]
        private static void OpenWindow()
        {
            GetWindow<MainWindow>();
        }


        public static readonly float HorizontalSplitterDragWidth = 4;
        public static readonly float LeftViewMinWidth = 150;
        public static readonly float RightViewMinWidth = 350;
        public static readonly float WindowMinHeight = 150;
        public static readonly float WindowMinWidth = LeftViewMinWidth + RightViewMinWidth;
        public static readonly string[] ConfigFiles = new string[]
        {
            "block_table.json",
            "block_mesh_table.json",
            "block_texture_table.json",
            "block_material_table.json",
            "item_table.json",
            "biome_table.json",
            "[editor]block_mesh_table.json",
            "[editor]block_texture_table.json",
            "[editor]block_material_table.json",
        };


        #region Serialized Data
        [SerializeField] private string m_ConfigFolder;
        [SerializeField] private WindowGUIMode m_GUIMode;
        [SerializeField] private SplittedViewRects m_SplittedRects;
        [SerializeField] private TreeViewState m_TreeViewState;
        #endregion


        #region Window Data
        private GUIContent m_DefaultTitleContent;
        private GUIContent m_ChangedTitleContent;
        private bool m_Changed;
        #endregion


        #region Controls
        private WindowToolbar m_Toolbar;
        private WindowTreeView m_TreeView;
        private WindowInspector[] m_Inspectors;
        #endregion


        #region Config Data
        public List<BlockData> Blocks;
        public List<ItemData> Items;
        public List<BiomeData> Biomes;
        public AssetPtrList<BlockMesh> BlockMeshes;
        public AssetPtrList<Texture2D> BlockTextures;
        public AssetPtrList<Material> BlockMaterials;
        #endregion


        public string ConfigFolder
        {
            get => m_ConfigFolder;
            set
            {
                if (Directory.Exists(value) && value != m_ConfigFolder)
                {
                    Save(true);
                    m_ConfigFolder = value;
                    ResetWindowTitleTooltip();
                    InitConfigData();
                }
            }
        }

        public WindowGUIMode GUIMode
        {
            get => m_GUIMode;
            set
            {
                if (value != m_GUIMode)
                {
                    m_Inspectors[(int)m_GUIMode].OnDisable();
                    m_Inspectors[(int)value].OnEnable();
                    m_GUIMode = value;

                    m_TreeView.Reload();
                    m_TreeView.SetFocusAndEnsureSelectedItem();
                }
            }
        }

        public bool Changed
        {
            get => m_Changed;
            private set
            {
                if (value != m_Changed)
                {
                    m_Changed = value;
                    titleContent = value ? m_ChangedTitleContent : m_DefaultTitleContent;
                }
            }
        }


        private void OnEnable()
        {
            InitSerializedData();
            InitWindow();
            InitControls();
            RegisterControlEvents();

            InitConfigData();
        }

        private void OnDisable()
        {
            Save(true);
        }

        private void InitSerializedData()
        {
            if (!Directory.Exists(m_ConfigFolder))
            {
                m_ConfigFolder = Application.dataPath;
            }

            if (m_SplittedRects.Equals(default))
            {
                m_SplittedRects = new SplittedViewRects(LeftViewMinWidth);
            }

            m_TreeViewState ??= new TreeViewState();
        }

        private void InitWindow()
        {
            m_DefaultTitleContent = new GUIContent("MC Config Editor", EditorGUIUtility.IconContent("Project").image);
            m_ChangedTitleContent = new GUIContent("MC Config Editor*", EditorGUIUtility.IconContent("Project").image);
            ResetWindowTitleTooltip();
            m_Changed = false;

            titleContent = m_DefaultTitleContent;
            minSize = new Vector2(WindowMinWidth, WindowMinHeight);
        }

        private void ResetWindowTitleTooltip()
        {
            m_DefaultTitleContent.tooltip = $"MC Config Editor - {ConfigFolder}";
            m_ChangedTitleContent.tooltip = $"MC Config Editor - {ConfigFolder} - Changed";
        }

        private void InitControls()
        {
            m_Toolbar = new WindowToolbar(this, m_TreeViewState.searchString);
            m_TreeView = new WindowTreeView(this, m_TreeViewState);
            m_Inspectors = new WindowInspector[]
            {
                new BlockInspector(this),
                new ItemInspector(this),
                new BiomeInspector(this)
            };
        }

        private void RegisterControlEvents()
        {
            m_Toolbar.OnAddButtonClicked += () =>
            {
                int index = -1;
                switch (m_GUIMode)
                {
                    case WindowGUIMode.Blocks:
                        index = Blocks.Count;
                        Blocks.Add(new BlockData { ID = index, InternalName = $"block_{index}" });
                        break;
                    case WindowGUIMode.Items:
                        index = Items.Count;
                        Items.Add(new ItemData { ID = index, InternalName = $"item_{index}" });
                        break;
                    case WindowGUIMode.Biomes:
                        index = -1;
                        Debug.LogWarning("You can not add a custom biome.");
                        break;
                }

                if (index != -1)
                {
                    m_TreeView.Reload();
                    m_TreeView.SetSelection(new int[] { index }, TreeViewSelectionOptions.FireSelectionChanged | TreeViewSelectionOptions.RevealAndFrame);
                    Changed = true;
                }
            };

            m_Toolbar.OnRemoveButtonClicked += () =>
            {
                if (m_GUIMode == WindowGUIMode.Biomes)
                {
                    Debug.LogWarning("You can not remove any biome in the table.");
                    return;
                }

                IList configs = m_GUIMode switch
                {
                    WindowGUIMode.Blocks => Blocks,
                    WindowGUIMode.Items => Items,
                    _ => null
                };

                int selection = m_TreeView.GetSelection()[0];

                if (selection == 0)
                {
                    EditorUtility.DisplayDialog("Invalid Operation", "You can not remove the block/item which has a zero-ID.", "Yes");
                    return;
                }

                configs.RemoveAt(selection);

                for (int i = selection; i < configs.Count; i++)
                {
                    (configs[i] as IOrderedConfigData).ID = i;
                }

                m_TreeView.Reload();
                m_TreeView.SetFocusAndEnsureSelectedItem();
                Changed = true;
            };

            m_Toolbar.OnSaveButtonClicked += () => Save(false);
        }

        private void InitConfigData()
        {
            m_Inspectors[(int)m_GUIMode].OnDisable();
            Changed = false;

            if (!LoadSavedConfigs())
            {
                NewConfigs();
            }

            m_TreeView.Reload();
            m_TreeView.SetFocusAndEnsureSelectedItem();
            m_Inspectors[(int)m_GUIMode].OnEnable();

            BlockMeshes.OnElementRemoved += removedIndex =>
            {
                foreach (BlockData block in Blocks)
                {
                    int index = block.Mesh.GetValueOrDefault(-1);

                    if (index == removedIndex)
                    {
                        block.Mesh = null;
                    }
                    else if (index > removedIndex)
                    {
                        block.Mesh--;
                    }
                }
            };

            BlockTextures.OnElementRemoved += removedIndex =>
            {
                foreach (BlockData block in Blocks)
                {
                    if (block.Textures == null)
                    {
                        continue;
                    }

                    for (int i = 0; i < block.Textures.Length; i++)
                    {
                        int?[] texture = block.Textures[i];

                        if (texture == null)
                        {
                            continue;
                        }

                        for (int j = 0; j < texture.Length; j++)
                        {
                            int index = texture[j].GetValueOrDefault(-1);

                            if (index == removedIndex)
                            {
                                texture[j] = null;
                            }
                            else if (index > removedIndex)
                            {
                                texture[j]--;
                            }
                        }
                    }
                }
            };

            BlockMaterials.OnElementRemoved += removedIndex =>
            {
                foreach (BlockData block in Blocks)
                {
                    int index = block.Material.GetValueOrDefault(-1);

                    if (index == removedIndex)
                    {
                        block.Material = null;
                    }
                    else if (index > removedIndex)
                    {
                        block.Material--;
                    }
                }
            };
        }

        private void NewConfigs()
        {
            Blocks = new List<BlockData>()
            {
                new BlockData { ID = 0, InternalName = "air" } // default
            };

            Items = new List<ItemData>()
            {
                new ItemData { ID = 0, InternalName = "empty" } // default
            };
            Biomes = NewBiomeList();
            BlockMeshes = new AssetPtrList<BlockMesh>();
            BlockTextures = new AssetPtrList<Texture2D>();
            BlockMaterials = new AssetPtrList<Material>();
            Changed = true;
        }

        private List<BiomeData> NewBiomeList()
        {
            BiomeId[] biomes = Enum.GetValues(typeof(BiomeId)) as BiomeId[];
            List<BiomeData> result = new List<BiomeData>(biomes.Length);

            foreach (BiomeId id in biomes)
            {
                result.Add(new BiomeData { ID = (int)id, InternalName = id.ToString() });
            }

            return result;
        }

        private bool LoadSavedConfigs()
        {
            if (!Directory.Exists(ConfigFolder))
            {
                return false;
            }

            try
            {
                ReadFromJsonFile(Path.Combine(ConfigFolder, ConfigFiles[0]), out Blocks);
                ReadFromJsonFile(Path.Combine(ConfigFolder, ConfigFiles[4]), out Items);
                ReadFromJsonFile(Path.Combine(ConfigFolder, ConfigFiles[5]), out Biomes);
                ReadFromJsonFile(Path.Combine(ConfigFolder, ConfigFiles[6]), out BlockMeshes);
                ReadFromJsonFile(Path.Combine(ConfigFolder, ConfigFiles[7]), out BlockTextures);
                ReadFromJsonFile(Path.Combine(ConfigFolder, ConfigFiles[8]), out BlockMaterials);
                return true;
            }
            catch (Exception e)
            {
                if (!(e is FileNotFoundException || e is SerializationException))
                {
                    Debug.LogException(e);
                }
                return false;
            }
        }

        private void OnGUI()
        {
            m_Toolbar.OnGUI();

            Rect splittedViewRect = new Rect(0, WindowToolbar.Height, position.width, position.height - WindowToolbar.Height);
            m_SplittedRects.GetRects(splittedViewRect, HorizontalSplitterDragWidth, LeftViewMinWidth, RightViewMinWidth, out Rect left, out Rect right);

            left.y += 1;
            left.height -= 1; // otherwise it will hide the border of the toolbar
            m_TreeView.searchString = m_Toolbar.SearchString;
            m_TreeView.OnGUI(left);

            if (m_TreeView.HasSelection())
            {
                int selectedIndex = m_TreeView.GetSelection()[0];
                Changed |= m_Inspectors[(int)m_GUIMode].OnGUI(right, selectedIndex);
            }
        }

        public void Save(bool ask)
        {
            if (!Changed)
            {
                return;
            }

            if (ask && !EditorUtility.DisplayDialog("Unsaved configs", $"Unsaved configs at '{ConfigFolder}'.", "Save", "Cancel"))
            {
                return;
            }

            SaveToJsonFile(Path.Combine(ConfigFolder, ConfigFiles[0]), Blocks);
            SaveToJsonFile(Path.Combine(ConfigFolder, ConfigFiles[1]), BlockMeshes.ToArray());
            SaveToJsonFile(Path.Combine(ConfigFolder, ConfigFiles[2]), BlockTextures.ToArray());
            SaveToJsonFile(Path.Combine(ConfigFolder, ConfigFiles[3]), BlockMaterials.ToArray());
            SaveToJsonFile(Path.Combine(ConfigFolder, ConfigFiles[4]), Items);
            SaveToJsonFile(Path.Combine(ConfigFolder, ConfigFiles[5]), Biomes);
            SaveToJsonFile(Path.Combine(ConfigFolder, ConfigFiles[6]), BlockMeshes);
            SaveToJsonFile(Path.Combine(ConfigFolder, ConfigFiles[7]), BlockTextures);
            SaveToJsonFile(Path.Combine(ConfigFolder, ConfigFiles[8]), BlockMaterials);
            Debug.Log($"Configs saved at '{ConfigFolder}'!");

            Changed = false;
            AssetDatabase.Refresh();
        }

        private static void SaveToJsonFile(string path, object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(path, json);
        }

        private static void ReadFromJsonFile<T>(string path, out T obj) where T : class
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            string json = File.ReadAllText(path);
            obj = JsonConvert.DeserializeObject<T>(json);

            if (obj == null)
            {
                throw new SerializationException();
            }
        }
    }
}
