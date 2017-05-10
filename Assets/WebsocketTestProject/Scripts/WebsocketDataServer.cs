using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using MsgPack;

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

    private void OnDestroy()
    {
        if (server != null)
            server.Stop();
        _server = null;
    }

    public void AddService<T>(string path)
    {
        server.AddWebSocketService<JsonGetter<T>>(path);
    }

    public class JsonGetter<T> : WebSocketBehavior
    {
        ObjectPacker packer;
        public static Queue<T> recievedData = new Queue<T>();
        protected override void OnMessage(MessageEventArgs e)
        {
            T data = default(T);
            if (e.IsText)
                data = JsonUtility.FromJson<T>(e.Data);
            else if (e.IsBinary)
                data = PackMsg(e.RawData);
            if (!e.IsPing)
                lock (recievedData)
                    recievedData.Enqueue(data);
        }

        T PackMsg(byte[] data)
        {
            if (packer == null)
                packer = new ObjectPacker();
            return packer.Unpack<T>(data);
        }
    }
}