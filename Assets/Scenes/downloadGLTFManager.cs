using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

namespace APIManager
{
    public class downloadGLTFManager : MonoBehaviour
    {
        [SerializeField] private string urlBucket;
        [SerializeField] private string urlAPI;
        private JSONNode node;
        private List<assetData> _filteredData = new List<assetData>();
        private string _dir;

        void Start()
        {
            StartCoroutine(GetRequest(urlAPI));
        }
        IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;
                
                string result = webRequest.downloadHandler.text;
                Debug.Log(result);
                JSONNode node = JSON.Parse(result); //Parsing raw JSON
                Debug.Log(node);
                JSONArray arrayRawData;
                arrayRawData = node["data"].AsArray;

                foreach(JSONNode entryNode in arrayRawData){
                    assetData filteredNode;
                    filteredNode.wearableName = entryNode["wearableName"].Value;
                    filteredNode.fileMeta = entryNode["fileMeta"];
                    _filteredData.Add(filteredNode);

                    //debug for fileMeta raw JSON
                    //Debug.Log(entryNode);
                    //Debug.Log(filteredNode.wearableName+ "," + entryNode["fileMeta"]);
                }
                node = PullData("test-orang-lari v0.0.1");
                //Debug.Log(node);
                _dir = node["assetBundleUrl"].Value;

                //Testing append link

                //downloadedlink = urlBucket + _dir;
                //Debug.Log(downloadedlink); 
                // var gltf = gameObject.AddComponent<GLTFast.GltfAsset>();
                // gltf.Url = downloadedlink;
            }
        }
        
        public string getURL()
        {
            if (_dir != null)
            {
                //Debug.Log(_dir);
                return urlBucket + _dir; //updated append link
            }
            return null;
        }

        public JSONNode PullData(string wearableIdKey)
        {
            assetData searchResult = 
                _filteredData.Find(entryNode=> entryNode.wearableName == wearableIdKey);
            return searchResult.fileMeta;        
        }
    }
}
