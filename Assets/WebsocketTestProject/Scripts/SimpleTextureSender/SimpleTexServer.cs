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
        if (FetchData.data != null)
        {
            if (tex2d == null)
                tex2d = new Texture2D(512, 512, TextureFormat.RGB24, false);
            //try
            {
                Debug.Log(System.BitConverter.ToString(FetchData.data));
                tex2d.LoadRawTextureData(FetchData.data);
                tex2d.Apply();
                FetchData.data = null;
            }
            //catch 
            //{
            //    FetchData.data = null;
            //}
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label(string.Format("Server.Port: {0}", server.Port));
        if (tex2d != null)
            GUILayout.Label(tex2d);
        GUILayout.EndVertical();
    }
}

public class FetchData : WebSocketBehavior
{
    public static byte[] data;

    protected override void OnMessage(MessageEventArgs e)
    {
        data = e.RawData;
    }
}