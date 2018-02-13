using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Utility
{

    [System.Serializable]
    public class IndexFile
    {
        public string Title;
        public string URL;
        public string Data;
        public FileRequestManager.EDATATYPE DataType;
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
        public enum EDATATYPE { GRAMMAR, VOCABULARY };

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
        private string m_ServerUrl = "http://beatrizcv.com/Data/FileData.json";

        [SerializeField]
        private string m_VocabularyIndexFileURL = "http://beatrizcv.com/Data/FileData.json";

        [SerializeField]
        private FileData m_VocabularyIndexData;
        public FileData VocabularyIndexData
        {
            get
            {
                return m_VocabularyIndexData;
            }
        }

        private float m_PercentProgress;
        private string m_ProgressText;

        public string ProgressText
        {
            get { return m_ProgressText; }
        }

        private void Start()
        {
            m_VocabularyIndexData = new FileData();
        }

        private IEnumerator RequestIndexFile(string fileName,Action<FileData> callbackIndexRequest)
        {
            FileData tempFileData = new FileData();

            if (string.IsNullOrEmpty(fileName))
            {
                Debug.Log("<color=blue>" + "[FileRequestManager.RequestIndexFile] file name is empty or null" + "</color>");
                callbackIndexRequest(tempFileData);
                yield return null;
            }

            string url = System.IO.Path.Combine(m_ServerUrl, fileName);

            Debug.Log("<color=blue>" + "[FileRequestManager.RequestIndexFile] Requesting file from: " + url + "</color>");

            WWW wwwFile = new WWW(url);

            yield return wwwFile;

            string jsonData = wwwFile.text;
            if (string.IsNullOrEmpty(jsonData))
            {
                callbackIndexRequest(tempFileData);
                Debug.Log("<color=blue>" + "[FileRequestManager.RequestIndexFile] JSON Data is null empty: " + jsonData + "</color>");
            }

            try
            {

                tempFileData = JsonUtility.FromJson<FileData>(jsonData);

            }
            catch (Exception e)
            {
                callbackIndexRequest(tempFileData);
            }

            // Convert each data in tempFileData
            Debug.Log("<color=blue>" + "[FileRequestManager.RequestIndexFile] Requesting... " + tempFileData.Data.Count + " Files " + "</color>");
            for (int i = 0; i < tempFileData.Data.Count; i++)
            {
                string urlFile = Path.Combine(m_ServerUrl, tempFileData.Data[i].URL);

                if (string.IsNullOrEmpty(urlFile))
                {
                    continue;
                }

                Debug.Log("<color=blue>" + "[FileRequestManager] Requesting: " + urlFile + ": " + (i + 1) + "/" + tempFileData.Data.Count + " : " + tempFileData.Data[i].Title + "</color>");

                WWW www = new WWW(urlFile);

                yield return www;

                tempFileData.Data[i].Data = www.text;
            }

            // Call callback
            callbackIndexRequest(tempFileData);
        }

        public IEnumerator RequestDataFiles()
        {
            m_VocabularyIndexData = new FileData();
            m_PercentProgress = 0.0f;
            m_ProgressText = m_PercentProgress.ToString() + " % ";

            if (string.IsNullOrEmpty(m_ServerUrl))
            {
                Debug.Log("<color=blue>" + "[FileRequestManager.RequestVocabularyData] No server url found" + "</color>");
                yield return null;
            }

            // Request vocabulary
            yield return RequestIndexFile(
                m_VocabularyIndexFileURL,
                (result) => m_VocabularyIndexData = result); // Store the result in a lambda expresion







            /*string url = System.IO.Path.Combine(m_ServerUrl, m_VocabularyIndexFileURL);

            Debug.Log("<color=blue>" + "[FileRequestManager.RequestVocabularyData] Requesting file from: " + url + "</color>");

            WWW wwwFile = new WWW(url);
            yield return wwwFile;
            string jsonData = wwwFile.text;
            if (!string.IsNullOrEmpty(jsonData))
            {
                
                m_VocabularyIndexData = JsonUtility.FromJson<FileData>(jsonData);

                Debug.Log("<color=blue>" + "[FileRequestManager] Requesting... " + m_VocabularyIndexData.Data.Count + " Files " + "</color>");

                //JapaneseApp.AppController.Instance.DebugUI.Log0 = "[FileRequestManager] Requesting... " + m_VocabularyIndexData.Data.Count;
                for (int i = 0; i < m_VocabularyIndexData.Data.Count; i++)
                {
                    string urlFile = System.IO.Path.Combine(m_ServerUrl, m_VocabularyIndexData.Data[i].URL);

                    Debug.Log("<color=blue>" + "[FileRequestManager] Requesting... " + urlFile + " Files " + "</color>");

                    if (string.IsNullOrEmpty(urlFile))
                    {
                        continue;
                    }

                    // Request
                    Debug.Log("<color=blue>" + "[FileRequestManager] Requesting: " + (i + 1) + "/" + m_VocabularyIndexData.Data.Count + " : " + m_VocabularyIndexData.Data[i].Title + "</color>");
                    // JapaneseApp.AppController.Instance.DebugUI.Log0 = "[FileRequestManager] Requesting: " + (i + 1) + "/" + m_VocabularyIndexData.Data.Count + " : " + m_VocabularyIndexData.Data[i].Title;

                    WWW www = new WWW(urlFile);
                    while (!www.isDone)
                    {
                        m_PercentProgress = www.progress * 100.0f;
                        m_ProgressText = m_PercentProgress.ToString() + " % ";
                        yield return null;
                    }

                    m_PercentProgress = www.progress * 100.0f;
                    m_ProgressText = m_PercentProgress.ToString() + " % ";

                    m_VocabularyIndexData.Data[i].Data = www.text;


                    Debug.Log("<color=blue>" + "[FileRequestManager] Data Retrieved: " + m_VocabularyIndexData.Data[i].Data +  "</color>");
                }
                
            }else
            {
                Debug.LogWarning("<color=blue>" + "[FileRequestManager] File Data Json is null or empty" + "</color>");
            } */
        }


        public enum EMediaType {NONE, PICTURE, AUDIO };



        public IEnumerator RequestPicture(string folderName, string fileNname, Action<Texture2D> callbackRequest)
        {

            string localDirectory = Path.Combine(Application.persistentDataPath, folderName);

            if (!Directory.Exists(localDirectory))
            {
                Directory.CreateDirectory(localDirectory);
            }

            string localPath = Path.Combine(localDirectory, fileNname);

            Debug.Log("<color=blue>" + "[FileRequestManager.RequestMedia] Local path :" + localPath + "</color>");

            if (File.Exists(localPath))
            {
                Debug.Log("<color=blue>" + "[FileRequestManager.RequestMedia] File exits at :" + localPath + "</color>");

                byte[] bytes = File.ReadAllBytes(localPath);

                yield return new WaitForEndOfFrame();


                 Texture2D texture = new Texture2D(2, 2);

                 texture.LoadImage(bytes);

                 yield return new WaitForEndOfFrame();

                callbackRequest(texture);

            }
            else
            {
                string directory = Path.Combine(m_ServerUrl, folderName);
                string serverFileURL = Path.Combine(directory, fileNname);

                Debug.Log("<color=blue>" + "[FileRequestManager.RequestPicture] Requesting file :" + serverFileURL + "</color>");

                WWW wwwFile = new WWW(serverFileURL);

                yield return wwwFile;

                if (wwwFile.texture != null)
                {
                    Texture2D texture = new Texture2D(wwwFile.texture.width, wwwFile.texture.height, TextureFormat.DXT1, false);
                    wwwFile.LoadImageIntoTexture(texture);

                    callbackRequest(texture);

                    yield return new WaitForEndOfFrame();

                    // Save bytes
                    byte[] bytes = wwwFile.bytes;
                    File.WriteAllBytes(localPath, bytes);

                    Debug.Log("<color=blue>" + "[FileRequestManager.RequestPicture] Writing: " + wwwFile.bytes.Length + "  At: " + localPath + "</color>");

                    yield return new WaitForEndOfFrame();
                }else
                {
                    Debug.Log("<color=blue>" + "[FileRequestManager.RequestMedia] Pciture data is null: " + serverFileURL + "</color>");

                    callbackRequest(null);
                }
                wwwFile.Dispose();
                wwwFile = null;
            }
        }
    }
}
