using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SimpleTexClient : MonoBehaviour
{

    public Texture2D tex2d;
    public string url = "ws://localhost:3000/";
    WebSocket ws;

    // Use this for initialization
    void Start()
    {
        ws = new WebSocket(url);
        ws.OnOpen += (sender, e) =>
        {
            print("OnOpen.sender: "+ sender);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var texData = new TextureData()
            {
                name = tex2d.name,
                data = tex2d.EncodeToPNG(),
            };
            var json = JsonUtility.ToJson(texData);
            if (!ws.IsConnected)
                ws.Connect();
            ws.Send(json);
        }
    }
}
