using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{

    [System.Serializable]
    public class IndexFile
    {
        public string FileName;
        public string URL;
        public string Data;
    }

    [System.Serializable]
    public class FileData
    {
        public List<IndexFile> Data;
        public FileData()
        {
            Data = new List<IndexFile>();
        }
    }

    public class FileRequestManager : MonoBehaviour
    {
        #region Instance
        private static FileRequestManager m_Instance;
        public static FileRequestManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = (FileRequestManager)FindObjectOfType(typeof(FileRequestManager));

                    if (m_Instance == null)
                    {
                        Debug.LogError("An instance of " + typeof(FileRequestManager) + " is needed in the scene, but there is none.");
                    }
                }
                return m_Instance;
            }
        }
        #endregion Instance

        [SerializeField]
        private string m_FileDataUrl = "http://beatrizcv.com/Data/FileData.json";

        [SerializeField]
        private FileData m_FileData;
        public FileData FileData
        {
            get
            {
                return m_FileData;
            }
        }

        private float m_PercentProgress;
        private string m_ProgressText;

        public string ProgressText
        {
            get { return m_ProgressText; }
        }
        
        public IEnumerator RequestFiles()
        {
            m_FileData = new FileData();
            m_PercentProgress = 0.0f;
            m_ProgressText = m_PercentProgress.ToString() + " % ";

            if (string.IsNullOrEmpty(m_FileDataUrl))
            {
                Debug.LogWarning("<color=yellow>" + "[FileRequestManager] File Data Url is null or empty" + "</color>");
                yield return null;
            }

            WWW wwwFile = new WWW(m_FileDataUrl);
            yield return wwwFile;
            string jsonData = wwwFile.text;
            if (!string.IsNullOrEmpty(jsonData))
            {
                if (!string.IsNullOrEmpty(jsonData))
                {
                    m_FileData = JsonMapper.ToObject<FileData>(jsonData);

                    Debug.LogWarning("<color=yellow>" + "[FileRequestManager] Requesting... " + m_FileData.Data.Count + " Files " + "</color>");

                    JapaneseApp.AppController.Instance.DebugUI.Log0 = "[FileRequestManager] Requesting... " + m_FileData.Data.Count;
                    for (int i = 0; i < m_FileData.Data.Count; i++)
                    {
                        if (string.IsNullOrEmpty(m_FileData.Data[i].URL))
                        {
                            continue;
                        }

                        // Request
                        Debug.LogWarning("<color=yellow>" + "[FileRequestManager] Requesting: " + (i + 1) + "/" + m_FileData.Data.Count + " : " + m_FileData.Data[i].FileName + "</color>");
                        JapaneseApp.AppController.Instance.DebugUI.Log0 = "[FileRequestManager] Requesting: " + (i + 1) + "/" + m_FileData.Data.Count + " : " + m_FileData.Data[i].FileName;

                        WWW www = new WWW(m_FileData.Data[i].URL);
                        while (!www.isDone)
                        {
                            m_PercentProgress = www.progress * 100.0f;
                            m_ProgressText = m_PercentProgress.ToString() + " % ";
                            yield return null;
                        }

                        m_PercentProgress = www.progress * 100.0f;
                        m_ProgressText = m_PercentProgress.ToString() + " % ";

                        m_FileData.Data[i].Data = www.text;


                        Debug.LogWarning("<color=yellow>" + "[FileRequestManager] Data Retrieved: " + m_FileData.Data[i].Data +  "</color>");
                    }
                }
            }else
            {
                Debug.LogWarning("<color=yellow>" + "[FileRequestManager] File Data Json is null or empty" + "</color>");
            }            
        }

    }
}
