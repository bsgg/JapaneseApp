using System.Collections;
using UnityEngine;

namespace JapaneseApp
{
    public abstract class Base: MonoBehaviour
    {

        private bool m_Visible;
        public bool Visible
        {
            get { return m_Visible; }
        }

        
        public virtual IEnumerator Initialize()
        {
            yield break;
        }

        public virtual void Finish() {}

        public virtual void Back() {}

        public virtual void Show()
        {
            m_Visible = true;
        }

        public virtual void Hide()
        {
            m_Visible = false;
        }
    }
}
