using System.Collections;
using System.Collections.Generic;
using Minecraft.Configurations;
using UnityEditor;
using UnityEngine;

namespace MinecraftEditor.Configurations.MCConfigEditor
{
    public class BiomeInspector : WindowInspector
    {
        private static GUIContent s_IDContent = new GUIContent("ID");
        private static GUIContent s_InternalNameContent = new GUIContent("Internal Name");

        private static GUIContent s_BaseHeightContent = new GUIContent("Base Height");
        private static GUIContent s_HeightVariationContent = new GUIContent("Height Variation");
        private static GUIContent s_TemperatureContent = new GUIContent("Temperature");
        private static GUIContent s_RainfallContent = new GUIContent("Rainfall");
        private static GUIContent s_EnableSnowContent = new GUIContent("Enable Snow");
        private static GUIContent s_EnableRainContent = new GUIContent("Enable Rain");
        private static GUIContent s_TopBlockContent = new GUIContent("Top Block");
        private static GUIContent s_FillerBlockContent = new GUIContent("Filler Block");
        private static GUIContent s_TreesPerChunkContent = new GUIContent("Trees Per Chunk");
        private static GUIContent s_ExtraTreeChanceContent = new GUIContent("Extra Tree Chance");
        private static GUIContent s_GrassPerChunkContent = new GUIContent("Grass Per Chunk");
        private static GUIContent s_FlowersPerChunkContent = new GUIContent("Flowers Per Chunk");
        private static GUIContent s_MushroomsPerChunkContent = new GUIContent("Mushrooms Per Chunk");
        private static GUIContent s_DeadBushPerChunkContent = new GUIContent("Dead Bush Per Chunk");
        private static GUIContent s_ReedsPerChunkContent = new GUIContent("Reeds Per Chunk");
        private static GUIContent s_CactiPerChunkContent = new GUIContent("Cacti Per Chunk");
        private static GUIContent s_ClayPerChunkContent = new GUIContent("Clay Per Chunk");
        private static GUIContent s_WaterlilyPerChunkContent = new GUIContent("Waterlily Per Chunk");
        private static GUIContent s_SandPatchesPerChunkContent = new GUIContent("Sand Patches Per Chunk");
        private static GUIContent s_GravelPatchesPerChunkContent = new GUIContent("Gravel Patches Per Chunk");


        public BiomeInspector(MainWindow mainWindow) : base(mainWindow) { }

        protected override void OnScrollableGUI(ref VerticalGUIRect rect, int index)
        {
            BiomeData biome = MainWnd.Biomes[index];

            rect.Space(10);

            using (new EditorGUI.DisabledGroupScope(true))
            {
                EditorGUI.IntField(rect.Next, s_IDContent, biome.ID);
                EditorGUI.TextField(rect.Next, s_InternalNameContent, biome.InternalName);
            }

            biome.BaseHeight = EditorGUI.FloatField(rect.Next, s_BaseHeightContent, biome.BaseHeight);
            biome.HeightVariation = EditorGUI.FloatField(rect.Next, s_HeightVariationContent, biome.HeightVariation);
            biome.Temperature = EditorGUI.FloatField(rect.Next, s_TemperatureContent, biome.Temperature);
            biome.Rainfall = EditorGUI.FloatField(rect.Next, s_RainfallContent, biome.Rainfall);
            biome.EnableSnow = EditorGUI.Toggle(rect.Next, s_EnableSnowContent, biome.EnableSnow);
            biome.EnableRain = EditorGUI.Toggle(rect.Next, s_EnableRainContent, biome.EnableRain);
            biome.TopBlock = EditorGUI.TextField(rect.Next, s_TopBlockContent, biome.TopBlock);
            biome.FillerBlock = EditorGUI.TextField(rect.Next, s_FillerBlockContent, biome.FillerBlock);

            biome.TreesPerChunk = EditorGUI.IntField(rect.Next, s_TreesPerChunkContent, biome.TreesPerChunk);
            biome.ExtraTreeChance = EditorGUI.FloatField(rect.Next, s_ExtraTreeChanceContent, biome.ExtraTreeChance);
            biome.GrassPerChunk = EditorGUI.IntField(rect.Next, s_GrassPerChunkContent, biome.GrassPerChunk);
            biome.FlowersPerChunk = EditorGUI.IntField(rect.Next, s_FlowersPerChunkContent, biome.FlowersPerChunk);
            biome.MushroomsPerChunk = EditorGUI.IntField(rect.Next, s_MushroomsPerChunkContent, biome.MushroomsPerChunk);

            biome.DeadBushPerChunk = EditorGUI.IntField(rect.Next, s_DeadBushPerChunkContent, biome.DeadBushPerChunk);
            biome.ReedsPerChunk = EditorGUI.IntField(rect.Next, s_ReedsPerChunkContent, biome.ReedsPerChunk);
            biome.CactiPerChunk = EditorGUI.IntField(rect.Next, s_CactiPerChunkContent, biome.CactiPerChunk);

            biome.ClayPerChunk = EditorGUI.IntField(rect.Next, s_ClayPerChunkContent, biome.ClayPerChunk);
            biome.WaterlilyPerChunk = EditorGUI.IntField(rect.Next, s_WaterlilyPerChunkContent, biome.WaterlilyPerChunk);
            biome.SandPatchesPerChunk = EditorGUI.IntField(rect.Next, s_SandPatchesPerChunkContent, biome.SandPatchesPerChunk);
            biome.GravelPatchesPerChunk = EditorGUI.IntField(rect.Next, s_GravelPatchesPerChunkContent, biome.GravelPatchesPerChunk);
        }
    }
}
