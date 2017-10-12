using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Utility
{
    public class Scroll : MonoBehaviour
    {
        public delegate void ItemAction(int buttonID);
        public ItemAction OnItemPress;

        [Header("Prefab Item")]
        [SerializeField] protected GameObject m_ItemScrollPrefab;

        [Header("Prefab Item")]
        [SerializeField]  protected RectTransform m_ContentRecTransform;

        [Header("Grid content layout")]
        [SerializeField]  protected GridLayoutGroup m_GridContent;

        [SerializeField]
        protected ScrollRect m_ScrollObject;

        /// <summary>
        /// List of objects in content panel
        /// </summary>
        protected List<GameObject> m_ListElements;

        /// <summary>
	    /// Method to init menu
	    /// </summary>
	    /// <param name="data">Data to fill the scroll</param>
        public void InitScroll(List<string> data)
        {
            // Clear list elements
            ClearListElements();
            m_ListElements = new List<GameObject>();

            // Set the final size of the content
            int numberElements = data.Count;
            SetSizeContent(numberElements);

            for (int i = 0; i < numberElements; i++)
            {
                GameObject element = Instantiate(m_ItemScrollPrefab) as GameObject;
                m_ListElements.Add(element);
                element.transform.SetParent(m_ContentRecTransform.transform);

                ButtonText buttonText = element.GetComponent<ButtonText>();
                if (buttonText != null)
                {
                    buttonText.Initialize(data[i], i, OnItemButtonPress);
                }              
            }
        }

        protected void ClearListElements()
        {
            if (m_ListElements != null)
            {
                for (int i = 0; i < m_ListElements.Count; i++)
                {
                    Destroy(m_ListElements[i]);
                }
            }
        }

        protected void SetSizeContent(int numberElements)
        {
            float hContent = (m_GridContent.cellSize.y * numberElements) + (m_GridContent.spacing.y * (numberElements - 1)) + m_GridContent.padding.top + m_GridContent.padding.bottom;
            m_ContentRecTransform.sizeDelta = new Vector2(m_ContentRecTransform.sizeDelta.x, hContent);
        }


        public void ResetPosition()
        {
            m_ScrollObject.verticalNormalizedPosition = 1.0f;
        }
       
        public void OnItemButtonPress(int id)
        {
            if (OnItemPress != null)
            {
                OnItemPress(id);
            }
        }
    }
}
