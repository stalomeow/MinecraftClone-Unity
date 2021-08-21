using Minecraft.InspectorExtensions;
using UnityEngine;

namespace Minecraft.Assets
{
    [DisallowMultipleComponent]
    public class AssetManagerUpdater : MonoBehaviour
    {
        private enum AssetCatalogPathType
        {
            DataPath,
            PersistentDataPath,
            StreamingAssetsPath
        }

        [SerializeField] private AssetCatalogPathType m_AssetCatalogPath;
        [SerializeField] private bool m_EnableLog;
        [SerializeField] [ConditionalDisplay("m_EnableLog")] private bool m_LogAssetCatalog;


        private void Awake()
        {
            AssetManager.InitializeIfNeeded(m_AssetCatalogPath switch
            {
                AssetCatalogPathType.DataPath => Application.dataPath,
                AssetCatalogPathType.PersistentDataPath => Application.persistentDataPath,
                AssetCatalogPathType.StreamingAssetsPath => Application.streamingAssetsPath,
                _ => null
            });

            AssetManager.Instance.EnableLog = m_EnableLog;

            if (m_EnableLog && m_LogAssetCatalog)
            {
                AssetManager.Instance.LogAssetCatalog();
            }
        }

        private void Update()
        {
            AssetManager.Instance.Update();
        }
    }
}
