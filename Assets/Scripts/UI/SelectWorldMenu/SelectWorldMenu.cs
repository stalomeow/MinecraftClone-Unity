using System.Collections;
using System.Collections.Generic;
using TMPro;
using ToaruUnity.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace Minecraft.UI
{
    [InjectActions(typeof(SelectWorldMenuActions))]
    public sealed class SelectWorldMenu : TweenUGUIView
    {
        [SerializeField] private Transform m_Content;
        [SerializeField] private GameObject m_LoadingText;
        [SerializeField] private Button m_BackButton;
        [SerializeField] private Button m_NewWorldButton;
        [SerializeField] private GameObject m_WorldTemplate;

        [SerializeField] private AssetReference m_NewWorldMenu;

        private List<GameObject> m_WorldLabels;
        private Coroutine m_RefreshCoroutine;


        protected override void OnCreate()
        {
            base.OnCreate();

            //m_BackButton.onClick.AddListener(() => Actions.Execute("Close"));
            //m_NewWorldButton.onClick.AddListener(() => Actions.Execute("New World", m_NewWorldMenu));

            m_WorldLabels = new List<GameObject>();
            m_RefreshCoroutine = null;
        }

        protected override IEnumerator OnOpen(object param)
        {
            foreach (GameObject label in m_WorldLabels)
            {
                Destroy(label);
            }
            m_WorldLabels.Clear();
            m_LoadingText.SetActive(true);
            Actions.Execute("Load Worlds");

            return base.OnOpen(param);
        }

        protected override void OnRefreshView(IActionState s)
        {
            SelectWorldMenuActionState state = s as SelectWorldMenuActionState;

            if (m_RefreshCoroutine != null)
            {
                StopCoroutine(m_RefreshCoroutine);
            }
            m_RefreshCoroutine = StartCoroutine(Refresh(state.Worlds));
        }

        private IEnumerator Refresh(SelectWorldMenuActionState.WorldMeta[] worlds)
        {
            m_LoadingText.SetActive(false);
            yield return null;

            for (int i = 0; i < worlds.Length; i++)
            {
                SelectWorldMenuActionState.WorldMeta world = worlds[i];
                GameObject go = Instantiate(m_WorldTemplate, m_Content, false);
                Transform transform = go.transform;
                string name = world.Name;

                transform.Find("Thumbnail/Panel/Name").GetComponent<TextMeshProUGUI>().text = world.Name;
                transform.Find("Thumbnail").GetComponentInChildren<RawImage>().texture = world.Thumbnail;
                transform.Find("Thumbnail/Panel/Meta").GetComponent<TextMeshProUGUI>().text = world.Date.ToShortDateString();
                transform.GetComponent<Button>().onClick.AddListener(() => Actions.Execute("Load World Settings", name));

                m_WorldLabels.Add(go);
                go.SetActive(true);
                yield return null;
            }
        }
    }
}