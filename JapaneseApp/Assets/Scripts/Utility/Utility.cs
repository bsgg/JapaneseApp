using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JapaneseApp
{
    public static class Utility
    {
        public static List<T> Shuffle<T>(List<T> source)
        {
            T[] elements = source.ToArray();
            List<T> copy = new List<T>();
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                int swapIndex = Random.Range(0, i + 1);
                copy.Add(elements[swapIndex]);
                elements[swapIndex] = elements[i];
            }
            return copy;
        }

        public static int Compare(string a, string b)
        {
            if (a.Length > b.Length)
            {
                return -1;
            }
            else if (a.Length < b.Length)
            {
                return 1;
            }

            return 0;
        }


        /// <summary>
        /// Method to load a JSON with a given path
        /// </summary>
        /// <returns>The JSON resource.</returns>
        /// <param name="pathFile">Path file.</param>
        public static string LoadJSONResource(string pathFile)
        {
            TextAsset text_asset = (TextAsset)Resources.Load(pathFile, typeof(TextAsset));
            if (text_asset == null)
            {
                Debug.Log("ERROR: Could not find file: Assets/Resources/" + pathFile);
                return "";
            }
            string json_string = text_asset.ToString();
            return json_string;
        }
    }
}
