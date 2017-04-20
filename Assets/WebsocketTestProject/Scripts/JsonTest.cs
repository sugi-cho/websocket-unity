using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTest : MonoBehaviour
{

    public Texture2D tex;
    public JsonData data;
    public string jsonText;

    // Use this for initialization
    void Start()
    {
        data.data = tex.EncodeToPNG();
        data.name = tex.name;
        jsonText = JsonUtility.ToJson(data);
    }

    [System.Serializable]
    public class JsonData
    {
        public byte[] data;
        public string name;
    }
}
