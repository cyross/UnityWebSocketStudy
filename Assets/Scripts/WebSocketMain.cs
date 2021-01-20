using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;

public class WebSocketMain : MonoBehaviour
{
    const string WebSocketURL = "ws://localhost:5001";

    public GameObject Character = null;

    private WebSocket ws = null;
    private Vector3 chr_pos;
    private bool update_chr_pos = false;

    // Start is called before the first frame update
    void Start()
    {
        ws = new WebSocket(WebSocketURL);

        // 各種コールバックをデリゲートとして登録

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Websocket opened");
        };

        ws.OnMessage += (sender, e) =>
        {
            chr_pos = JsonUtility.FromJson<Vector3>(e.Data);
            update_chr_pos = true;
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket closed");
        };

        ws.OnError += (sender, e) =>
        {
            Debug.LogError(string.Format("GotError[{0}]", e.Message));
        };

        // 接続
        ws.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        // ボタンを押すごとにGameObjectがひとつ増える
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send("Hello");
        }
        if(update_chr_pos)
        {
            CreateGameObject(chr_pos);
            update_chr_pos = false;
        }
    }

    private void OnDestroy()
    {
        ws.Close();
        ws = null;
    }

    private void CreateGameObject(Vector3 pos)
    {
        Instantiate(Character, pos, Quaternion.identity);
    }
}
