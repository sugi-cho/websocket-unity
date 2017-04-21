using UnityEngine;
using System.Collections;
using WebSocketSharp;
using WebSocketSharp.Net;

public class ClientExample : MonoBehaviour
{

    WebSocket ws;
    WebSocket ws2;

    void Start()
    {
        ws = new WebSocket("ws://localhost:3000/");
        

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket Open");
        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("WebSocket Message Type: " + e.GetType() + ", Data: " + e.Data);
        };

        ws.OnError += (sender, e) =>
        {
            Debug.Log("WebSocket Error Message: " + e.Message);
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket Close");
        };

        ws.Connect();

        ws2 = new WebSocket("ws://localhost:3000/test");
        ws2.Connect();

    }

    void Update()
    {

        if (Input.GetKeyUp("s"))
        {
            ws.Send("Test Message");
        }
        if (Input.GetKeyDown(KeyCode.T))
            ws2.Send("call");

    }

    void OnDestroy()
    {
        ws.Close();
        ws = null;
    }
}