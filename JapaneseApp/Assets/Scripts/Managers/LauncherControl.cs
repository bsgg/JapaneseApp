using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JapaneseApp
{
    [Serializable]
    public class IndexFile
    {
        public string Title;
        public string URL;
        public string Data;
        public LauncherControl.EDATATYPE DataType;
    }
    

    [Serializable] 
    public class FileData
    {
        public List<IndexFile> Data;
        public FileData()
        {
            Data = new List<IndexFile>();
        }
    }

    public class LauncherControl : Base
    {
        public enum EDATATYPE { GRAMMAR, VOCABULARY, DIALOG };

        [SerializeField]
        private LauncherUI m_UI;
        public LauncherUI UI
        {
            get
            {
                return m_UI;
            }
        }


        [SerializeField]
        private string m_ServerUrl = "http://beatrizcv.com/Data/Japanese";

        [SerializeField]
        private string m_VocabularyIndexFileName = "VocabularyIndexData.json";

        [SerializeField]
        private string m_DialogIndexFileName = "DialogIndexData.json";

        [SerializeField]
        private FileData m_VocabularyIndexData;
        public FileData VocabularyIndexData
        {
            get
            {
                return m_VocabularyIndexData;
            }
        }

        [SerializeField]
        private FileData m_DialogIndexData;
        public FileData DialogIndexData
        {
            get
            {
                return m_DialogIndexData;
            }
        }

        private float m_FileIndexTotalPercent = 80.0f;

        private void Start()
        {
            m_VocabularyIndexData = new FileData();
        }

        public override void Show()
        {
            base.Show();
            
            m_UI.Show();
        }
        

        public override void Hide()
        {
            base.Hide();
            m_UI.Hide();
        }

        public void UpdateProgress(string text, float value)
        {
            m_UI.ContentText = text;
            m_UI.ProgressValue = value;
        }

        public override IEnumerator Initialize()
        {
            m_UI.ProgressValue = 0.0f;
            m_UI.Show();

            m_UI.ActiveButtons(false);

            m_VocabularyIndexData = new FileData();
            m_UI.ContentText = "Connecting to the server to download the data...";

            if (string.IsNullOrEmpty(m_ServerUrl))
            {
                Debug.Log("<color=blue>" + "[FileRequestManager.RequestVocabularyData] No server url found" + "</color>");
                yield return null;
            }

            // Request vocabulary
            yield return RequestIndexFile(
                m_VocabularyIndexFileName,
                (result) => m_VocabularyIndexData = result); // Store the result in a lambda expresion

            yield return RequestIndexFile(
                m_DialogIndexFileName,
                (result) => m_DialogIndexData = result); // Store the result in a lambda expresion


            m_UI.ProgressValue = m_FileIndexTotalPercent / 100.0f;
            //m_UI.ContentText = " Completed! ";

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

            string url = Path.Combine(m_ServerUrl, fileName);

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
                       

            float percent = 0.0f;
            m_UI.ProgressValue = percent;

            float amount = m_FileIndexTotalPercent / tempFileData.Data.Count;
            //m_UI.ContentText = "Loading: " + percent + "%";

            for (int i = 0; i < tempFileData.Data.Count; i++)
            {
                m_UI.ContentText = "Downloading File " + (i+1) + "/" + (tempFileData.Data.Count) + ": " + tempFileData.Data[i].Title;

                string urlFile = Path.Combine(m_ServerUrl, tempFileData.Data[i].URL);

                if (string.IsNullOrEmpty(urlFile))
                {
                    continue;
                }

                Debug.Log("<color=blue>" + "[FileRequestManager] Requesting: " + urlFile + ": " + (i + 1) + "/" + tempFileData.Data.Count + " : " + tempFileData.Data[i].Title + "</color>");

                WWW www = new WWW(urlFile);

                yield return www;

                tempFileData.Data[i].Data = www.text;

                percent += amount;
                m_UI.ProgressValue = percent/100.0f;

                //m_UI.ContentText = "Loading: " + percent + "%";
            }

            // Call callback
            callbackIndexRequest(tempFileData);
        }

        public IEnumerator LoadAudio(string folderName, string fileName, string ext,Action<AudioClip> callbackRequest)
        {
            string localDirectory = Path.Combine(Application.persistentDataPath, folderName);

            if (!Directory.Exists(localDirectory))
            {
                Directory.CreateDirectory(localDirectory);
            }

            string fileNameExt = fileName + ext;
            string localPath = Path.Combine(localDirectory, fileNameExt);

            m_UI.ContentText = "Downloading Audio File: " + fileName;

            Debug.Log("<color=blue>" + "[FileRequestManager.RequestMedia] Local path :" + localPath + "</color>");

            if (File.Exists(localPath))
            {
                Debug.Log("<color=blue>" + "[FileRequestManager.RequestMedia] File exits at :" + localPath + "</color>");

                byte[] bytes = File.ReadAllBytes(localPath);

                yield return new WaitForEndOfFrame();

                if (bytes.Length > 0)
                {
                    AudioClip audio = AudioClip.Create(fileName, bytes.Length, 1, 44100, false);
                    yield return new WaitForEndOfFrame();

                    callbackRequest(audio);
                }else
                {
                    Debug.Log("<color=blue>" + "[FileRequestManager.RequestMedia] Unable to load audio :" + localPath + " the lenght is 0</color>");
                    callbackRequest(null);
                }

            }
            else
            {
                string directory = Path.Combine(m_ServerUrl, folderName);
                string serverFileURL = Path.Combine(directory, fileNameExt);

                Debug.Log("<color=blue>" + "[FileRequestManager.RequestPicture] Requesting file :" + serverFileURL + "</color>");

                WWW wwwFile = new WWW(serverFileURL);

                yield return wwwFile;

                AudioClip audio = wwwFile.GetAudioClip(false,true);
                audio.name = fileName;

                yield return new WaitForEndOfFrame();

                

                if (audio != null)
                {

                    callbackRequest(audio);

                    yield return new WaitForEndOfFrame();

                    // Save bytes
                    byte[] bytes = wwwFile.bytes;
                    File.WriteAllBytes(localPath, bytes);

                    Debug.Log("<color=blue>" + "[FileRequestManager.RequestPicture] Writing: " + wwwFile.bytes.Length + "  At: " + localPath + "</color>");

                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    Debug.Log("<color=blue>" + "[FileRequestManager.RequestMedia] Pciture data is null: " + serverFileURL + "</color>");

                    callbackRequest(null);
                }

                wwwFile.Dispose();
                wwwFile = null;
            }

        }

        public IEnumerator LoadPicture(string folderName, string fileName, Action<Texture2D> callbackRequest)
        {

            string localDirectory = Path.Combine(Application.persistentDataPath, folderName);

            if (!Directory.Exists(localDirectory))
            {
                Directory.CreateDirectory(localDirectory);
            }

            string localPath = Path.Combine(localDirectory, fileName);

            m_UI.ContentText = "Loading " + fileName + " File";

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
                string serverFileURL = Path.Combine(directory, fileName);

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
