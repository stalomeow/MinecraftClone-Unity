using Minecraft.ItemsData;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XLua;

#pragma warning disable CS0649

namespace Minecraft
{
    [LuaCallCSharp]
    [DisallowMultipleComponent]
    public sealed class InventoryManager : MonoBehaviour
    {
        private readonly WaitForSeconds m_Wait = new WaitForSeconds(3);

        [SerializeField] private Transform m_Selected;
        [SerializeField] private TextMeshProUGUI m_ItemText;
        [SerializeField] private Image[] m_Items;
        private ItemType[] m_ItemTypes;
        private int m_CurrentIndex;

        public ItemType CurrentItem => m_ItemTypes[m_CurrentIndex];

        private void OnEnable()
        {
            m_ItemText.text = string.Empty;
            m_ItemTypes = new ItemType[m_Items.Length];
            m_CurrentIndex = 0;
        }

        private void Update()
        {
            float scroll = -Input.GetAxis("Mouse ScrollWheel");

            if (scroll == 0)
                return;

            m_CurrentIndex += scroll > 0 ? 1 : -1;

            if (m_CurrentIndex < 0)
            {
                m_CurrentIndex = m_Items.Length - 1;
            }
            else if (m_CurrentIndex >= m_Items.Length)
            {
                m_CurrentIndex = 0;
            }

            m_Selected.position = m_Items[m_CurrentIndex].transform.position;

            StopAllCoroutines();
            StartCoroutine(ShowItemText());
        }

        private IEnumerator ShowItemText()
        {
            m_ItemText.text = WorldManager.Active.GetCurrentItem().ItemName;

            yield return m_Wait;

            m_ItemText.text = string.Empty;
        }

        public void SetItem(int index, ItemType type)
        {
            if (m_ItemTypes[index] == type)
                return;

            m_ItemTypes[index] = type;
            Item item = WorldManager.Active.DataManager.GetItemByType(type);
            m_Items[index].sprite = item.Icon;
        }
    }
}