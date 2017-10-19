using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class ButtonText : MonoBehaviour
    {
        public delegate void OnButtonPress(int index);        

        [SerializeField]  private Text       m_TextButton;
        public string TextButton
        {
            get { return m_TextButton.text; }
            set { m_TextButton.text = value; }
        }

        private int m_ID;
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        [SerializeField] private Button  m_ButtonComponent;
        public Button ButtonComponent
        {
            get { return m_ButtonComponent; }
            set { m_ButtonComponent = value; }
        }

        public void Initialize(string text, int id, OnButtonPress callback)
        {
            m_TextButton.text = text;
            m_ID = id;
            m_ButtonComponent.onClick.AddListener(() => { callback(m_ID); });
        }

        public void Initialize(int id, OnButtonPress callback)
        {
            m_ID = id;
            m_ButtonComponent.onClick.AddListener(() => { callback(m_ID); });
        }

    }
}
