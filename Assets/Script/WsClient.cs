using System;
using UnityEngine;
using WebSocketSharp;


public partial struct Message
{
    public string Type;
    public string Data;
}

public class WsClient
{
    WebSocket _ws;
    public Action<Message> OnMessage;

    public WsClient(string url)
    {
        _ws = new WebSocket(url);
        _ws.OnMessage += onMessage;
        _ws.Connect();
    }

    void onMessage(object sender, MessageEventArgs e)
    {
        var msg = JsonUtility.FromJson<Message>(e.Data);
        Debug.Log("Type : " + msg.Type);
        OnMessage(msg);
    }

    public void SendMessage(string type, object data)
    {
        _ws.Send(JsonUtility.ToJson(new Message
        {
            Type = type,
            Data = JsonUtility.ToJson(data)
        }));
    }

    public void SendMessage(string type, string data)
    {
        _ws.Send(JsonUtility.ToJson(new Message
        {
            Type = type,
            Data = data
        }));
    }

    public void Dispose()
    {
        _ws.Close();
    }
}
