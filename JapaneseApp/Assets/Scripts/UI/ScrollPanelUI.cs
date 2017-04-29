using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace JapaneseApp
{
    public class ScrollPanelUI : MonoBehaviour
    {
        public delegate void ActionButtonPress(int index);
        public ActionButtonPress OnButtonPress;

        [Header("Prefab Item")]
        [SerializeField]
        private GameObject m_ItemMenuPrefab;

        [Header("Prefab Item")]
        [SerializeField]
        private RectTransform m_ContentRecTransform;

        [Header("Grid content layout")]
        [SerializeField]
        private GridLayoutGroup m_GridContent;

        /// <summary>
        /// List of objects in content panel
        /// </summary>
        private List<GameObject> m_ListElements;
        /// <summary>
        /// Method to init menu
        /// </summary>
        /// <param name="data">Data to fill the scroll</param>
        public void InitScroll(List<string> data)
        {
            if (m_ListElements != null)
            {
                for (int i = 0; i < m_ListElements.Count; i++)
                {
                    Destroy(m_ListElements[i]);
                }
            }

            m_ListElements = new List<GameObject>();
            int numberElements = data.Count;

            float hContent = (m_GridContent.cellSize.y * numberElements) + (m_GridContent.spacing.y * (numberElements - 1)) + m_GridContent.padding.top + m_GridContent.padding.bottom;
            m_ContentRecTransform.sizeDelta = new Vector2(m_ContentRecTransform.sizeDelta.x, hContent);

            for (int i = 0; i < numberElements; i++)
            {
                GameObject element = Instantiate(m_ItemMenuPrefab) as GameObject;
                m_ListElements.Add(element);
                element.transform.SetParent(m_ContentRecTransform.transform);

                RectTransform cellRectTransform = element.GetComponent<RectTransform>();
                if (cellRectTransform != null)
                {
                    cellRectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }

                MenuButton menuB = element.GetComponent<MenuButton>();
                menuB.SetupMenuButton(data[i], i, DelegateButtonPress);
            }
        }

        public void DelegateButtonPress(int id)
        {
            if (OnButtonPress != null)
            {
                OnButtonPress(id);
            }
        }

    }
}
