using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using MsgPack;

public enum DataType { json, msgPack, };
public abstract class WebSocketDataBehaviour<T> : WebSocketDataBehaviour
{
    public static WebSocketDataBehaviour<T> Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType<WebSocketDataBehaviour<T>>();
            return _Instance;
        }
    }
    static WebSocketDataBehaviour<T> _Instance;

    public string remoteAddress = "localhost";
    public int remotePort = 3000;
    public string path = "/";
    public DataType dataType;

    WebSocket ws;
    bool sending;
    protected Queue<T> receivedData { get { return WebsocketDataServer.JsonGetter<T>.recievedData; } }
    ObjectPacker packer;

    private void Start()
    {
        if (Instance != this)
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (ws == null) return;
        ws.Close();
        ws = null;
    }

    void ConnectToServer()
    {
        if (ws == null)
        {
            var url = string.Format("ws://{0}:{1}{2}", remoteAddress, remotePort, path);
            Debug.Log(url);
            ws = new WebSocket(url);
        }
        ws.Connect();
    }

    public override void AddService()
    {
        WebsocketDataServer.Instance.AddService<T>(path);
    }

    #region onClient
    public void SendData(T data)
    {
        if (ws == null || !ws.IsConnected)
            ConnectToServer();
        sending = true;
        switch (dataType)
        {
            case DataType.json:
                var json = JsonUtility.ToJson(data);
                ws.SendAsync(json, OnSendCompleted);
                break;
            case DataType.msgPack:
                if (packer == null)
                    packer = new ObjectPacker();
                var msg = packer.Pack(data);
                ws.SendAsync(msg, OnSendCompleted);
                break;
        }
    }
    void OnSendCompleted(bool completed)
    {
        if (completed)
            sending = false;
        else
            print("send error");
    }
    #endregion
}

public abstract class WebSocketDataBehaviour : MonoBehaviour
{
    public abstract void AddService();
}