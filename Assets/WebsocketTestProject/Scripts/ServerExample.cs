using UnityEngine;
using System.Collections;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

public class ServerExample : MonoBehaviour
{

    WebSocketServer server;

    void Start()
    {
        server = new WebSocketServer(3000);
        server.AddWebSocketService<Echo>("/");
        server.AddWebSocketService<Test>("/test");
        server.Start();
    }

    void OnDestroy()
    {
        server.Stop();
        server = null;
    }

}

public class Echo : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        Sessions.Broadcast(e.Data);
    }
}

public class Test : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        Debug.Log("test");
    }
}