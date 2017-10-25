using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class IconBtn : MonoBehaviour
    {
        [SerializeField] private Image m_Icon;
        [SerializeField] private Button m_Btn;

        public void Enable(bool enable, Color c)
        {
            m_Btn.interactable = enable;
            m_Icon.color = c;
        }

        public void Enable(bool enable)
        {
            m_Btn.interactable = enable;
        }

        public void SetColor(Color c)
        {
            m_Icon.color = c;
        }
    }
}
