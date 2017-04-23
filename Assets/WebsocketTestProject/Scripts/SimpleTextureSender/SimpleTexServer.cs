using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class SimpleTexServer : MonoBehaviour
{
    public int listenPort = 3000;

    WebSocketServer server;
    Texture2D tex2d;

    // Use this for initialization
    void Start()
    {
        server = new WebSocketServer(listenPort);
        server.AddWebSocketService<FetchData>("/");
        server.Start();
    }

    private void OnDestroy()
    {
        if (server != null)
            server.Stop();
        server = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (FetchData.TexData != null)
        {
            var texData = FetchData.TexData;
            if (tex2d == null)
                tex2d = new Texture2D(2, 2);
            tex2d.name = texData.name;
            tex2d.LoadImage(texData.data);
            FetchData.TexData = null;
        }
    }

    private void OnGUI()
    {
        if (tex2d == null)
            return;
        GUILayout.BeginVertical("box");
        GUILayout.Label(tex2d.name);
        GUILayout.Label(tex2d);
        GUILayout.EndVertical();
    }
}

public class TextureData
{
    public string name;
    public byte[] data;
}

public class FetchData : WebSocketBehavior
{
    public static TextureData TexData;

    protected override void OnMessage(MessageEventArgs e)
    {
        var dataStr = e.Data;
        var tData = JsonUtility.FromJson<TextureData>(dataStr);
        TexData = tData;
    }
}