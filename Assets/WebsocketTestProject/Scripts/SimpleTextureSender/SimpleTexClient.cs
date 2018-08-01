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

    private void OnDestroy()
    {
        if (ws != null)
            ws.Close();
        ws = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var data = tex2d.GetRawTextureData();
            if (!ws.IsConnected)
                ws.Connect();
            ws.Send(data);
        }
    }
}
