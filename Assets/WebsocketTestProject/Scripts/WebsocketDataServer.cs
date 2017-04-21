using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WebsocketDataServer : MonoBehaviour
{
    public static WebsocketDataServer Instance { get { if (_Instance == null) _Instance = FindObjectOfType<WebsocketDataServer>(); return _Instance; } }
    static WebsocketDataServer _Instance;

    WebSocketServer server { get { if (_server == null) _server = new WebSocketServer(listenPort); return _server; } }
    WebSocketServer _server;

    public int listenPort = 3000;

    private void Start()
    {
        if (Instance != this) Destroy(gameObject);
        var dataBehaviours = FindObjectsOfType<WebSocketDataBehaviour>();
        foreach (var db in dataBehaviours)
            db.AddService();
        server.Start();
    }

    public void AddService<T>(string path) where T : Data
    {
        server.AddWebSocketService<DataGetter<T>>(path);
    }

    public abstract class Data { }

    public class DataGetter<T> : WebSocketBehavior where T : Data
    {
        public static Queue<T> recievedData = new Queue<T>();
        protected override void OnMessage(MessageEventArgs e)
        {
            var str = e.Data;
            T data = JsonUtility.FromJson<T>(str);
            lock (recievedData)
                recievedData.Enqueue(data);
        }
    }
}
