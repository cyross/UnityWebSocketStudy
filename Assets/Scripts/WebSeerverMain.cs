using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

public class WebSeerverMain : MonoBehaviour
{
    const int Port = 5001;

    private WebSocketServer wserver = null;

    // Start is called before the first frame update
    void Awake()
    {
        wserver = new WebSocketServer(Port);
        wserver.AddWebSocketService<Echo>("/");
        wserver.Start();
    }

    private void OnDestroy()
    {
        wserver.Stop();
        wserver = null;
    }
}

public class Echo : WebSocketBehavior
{
    private float x = 0.0f;
    private float y = 0.0f;

    protected override void OnMessage(MessageEventArgs e)
    {
        // Randomが動作しないため、OnMessageが呼ばれるごとにxの値を更新するようにした
        Vector3 initial_pos = new Vector3(x, y, 0.0f);
        string json = JsonUtility.ToJson(initial_pos);

        Sessions.Broadcast(json);
        Debug.Log(string.Format("Send from Server: {0}", json));

        x = x + 0.1f;
    }

    protected override void OnClose(CloseEventArgs e)
    {
        Debug.Log("Server Close");
    }
}