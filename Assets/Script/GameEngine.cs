using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    public Camera MainCamera;
    public PlayerController Player;

    public GameObject Human_NJ;
    public static GameEngine Instance { get; private set; }

    WsClient _client;
    List<Human_NJ> human_nj_List = new List<Human_NJ>();
    List<UserData> _users = new List<UserData>();
    int _frameCount;
    public bool GameStarted { get; private set; } = false;
    Stack<Message> _message = new Stack<Message>();
    private void Awake()
    {
        Human_NJ.SetActive(false);
        Instance = this;
    }
    void Start()
    {
        Init();
    }
    public void Init()
    {
        //_client = new WsClient("ws://192.168.8.85:3000");
        string hostname = Dns.GetHostName();
        IPAddress[] adrList = Dns.GetHostAddresses(hostname);
        _client = new WsClient("ws://"+ adrList[1].ToString() + ":3000");
        _client.OnMessage = onMessage;
    }
    void onMessage(Message msg)
    {
        _message.Push(msg);
    }
    private void OnApplicationQuit()
    {
        _client.Dispose();
    }
    void Update()
    {
        if (_message.Count > 0)
        {
            var msg = _message.Pop();
            updateMessageForGameStarted(msg);
        }
    }
    void updateMessageForGameStarted(Message msg)
    {
        //Debug.Log(msg.Type);
        switch (msg.Type)
        {
            case Message.Join:
                _client.SendMessage(Message.GameStart, "Human_NJ");
                break;
            case Message.GameStart:
                {
                    var data = JsonUtility.FromJson<GameStartMessage>(msg.Data);
                    if (GameStarted)
                    {//誰もいます
                        var human_nj = FindHumanNJ(data.Player.ID);
                        if (human_nj == null)
                        {
                            _users.Add(data.Player);
                            human_nj_List.Add(createPlayer(data.Player));
                        }
                    }
                    else
                    {//誰もいません
                        _users = data.Users;
                        foreach (var user in _users)
                        {
                            var obj = createPlayer(user);
                            human_nj_List.Add(obj);
                            if (user.ID == data.Player.ID)
                            {
                                MainCamera.transform.SetParent(obj.transform, false);
                                MainCamera.transform.localPosition = new Vector3(0, (float)0.0163, (float)-0.004899);
                                
                                Player.Init(obj);
                            }
                        }
                    }
                    GameStarted = true;
                }
                break;
            case Message.ExitUser:
                {
                    var data = JsonUtility.FromJson<ExitUserMessage>(msg.Data);
                    var user = _users.First(u => u.WsName == data.WsName);
                    var human_nj = human_nj_List.First(v => v.UserID == user.ID);
                    human_nj_List.Remove(human_nj);
                    Destroy(human_nj.gameObject);
                }
                break;
            case Message.ActionWalk:
                {
                    var data = JsonUtility.FromJson<ActionWalk__Massage>(msg.Data);
                    if(data.UserID == Player.Human_NJ.UserID)
                    {
                        break;
                    }
                    var human_nj = FindHumanNJ(data.UserID);
                    human_nj.X = data.UserPos.x;
                    human_nj.Y = data.UserPos.y;
                    human_nj.Z = data.UserPos.z;
                }
                break;
            case Message.ActionRotation:
                {
                    var data = JsonUtility.FromJson<ActionRotation__Massage>(msg.Data);
                    if (data.UserID == Player.Human_NJ.UserID)
                    {
                        break;
                    }
                    var human_nj = FindHumanNJ(data.UserID);
                    human_nj.Rotation = data.UserRotation;
                }
                break;
            case Message.ActionViewRotation:
                {
                    var data = JsonUtility.FromJson<ActionViewRotation__Massage>(msg.Data);
                    if (data.UserID == Player.Human_NJ.UserID)
                    {
                        break;
                    }
                    var human_nj = FindHumanNJ(data.UserID);
                    human_nj.PlayerCamera = human_nj.getCamera();
                    human_nj.PlayerCamera.transform.localEulerAngles = new Vector3(-data.UserViewRotation, 0, 0);
                }
                break;
            case Message.ActionSneak:
                {
                    var data = JsonUtility.FromJson<ActionSneak__Message>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);

                }
                break;
            case Message.ActionJump:
                {
                    var data = JsonUtility.FromJson<Jump__Message>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);
                }
                break;
            //case Message.ActionSneak:
            //    {
            //        var data = JsonUtility.FromJson<Sneak__Message>(msg.Data);
            //        var human_nj = FindHumanNJ(data.UserID);
            //        if (!human_nj.IsSneak)
            //        {
            //            human_nj.SetSneak();
            //            human_nj.IsSneak = human_nj.GetSneakStatic();
            //            human_nj.SneakName = human_nj.GetSneakName();
            //        }
            //    }
            //    break;
            case Message.ActionShot:
                {
                    var data = JsonUtility.FromJson<ActionShotMessage>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);
                    human_nj.Shot();
                    human_nj.IsSneak = human_nj.GetSneakStatic();
                }
                break;
            case Message.ActionDamge:
                {
                    var data = JsonUtility.FromJson<ActionDmgMessage>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);
                    human_nj.Damage(data.Dmg);

                    if (human_nj.IsDead)
                    {
                        human_nj_List.Remove(human_nj);
                        if (human_nj.UserID == Player.Human_NJ.UserID)
                        {
                            _client.Dispose();
                            GameStarted = false;
                            MainCamera.transform.SetParent(null, false);
                            foreach(var n in human_nj_List)
                            {
                                Destroy(n.gameObject);
                            }
                            human_nj_List.Clear();
                        }
                        StartCoroutine(human_nj.Dead());
                    }
                }
                break;
        }
    }
    public void Send(string type,object data)
    {
        _client.SendMessage(type, data);
    }
    public UserData FindUser(int id)
    {
        return _users.First(u => u.ID == id);
    }
    public Human_NJ FindHumanNJ(int id)
    {
        return human_nj_List.FirstOrDefault(v => v.UserID == id);
    }
    Human_NJ createPlayer(UserData u)
    {
        var obj = Instantiate(Human_NJ);
        var pos = new Vector3(u.X, u.Y, u.Z);
        obj.transform.position = pos;
        obj.SetActive(true);
        var human_nj = obj.GetComponent<Human_NJ>();
        human_nj.X = u.X;
        human_nj.Y = u.Y;
        human_nj.Z = u.Z;
        human_nj.UserID = u.ID;
        human_nj.Hp = u.Hp;
        human_nj.Dmg = u.Dmg;
        return human_nj;
    }
}

[Serializable]
public struct UserData
{
    public int ID;
    public string WsName;
    public string Name;
    public float Hp;
    public float Dmg;
    public float X;
    public float Y;
    public float Z;
    public bool IsSneak;
    public bool IsJump;
    public string SneakName;
    public Quaternion Rotation;
    public Camera PlayerCamera;
    public GameObject SneakObj;
}
[Serializable]
class GameStartMessage
{
    public List<UserData> Users;
    public UserData Player;
}
[Serializable]
class ExitUserMessage
{
    public string WsName;
}
[Serializable]
public struct ActionShotMessage
{
    public int UserID;
}
[Serializable]
public struct ActionDmgMessage
{
    public int UserID;
    public float Dmg;
}
[Serializable]
public struct ActionSneakMessage
{
    public int UserID;
}
[Serializable]
public struct ActionJumpMessage
{
    public int UserID;
}
[Serializable]
public struct Jump__Message
{
    public int UserID;
}
[Serializable]
public struct Sneak__Message
{
    public int UserID;
    public bool IsSneak;
}
[Serializable]
public struct ActionWalk__Massage
{
    public int UserID;
    public Vector3 UserPos;
}
[Serializable]
public struct ActionRotation__Massage
{
    public int UserID;
    public Quaternion UserRotation;
}
[Serializable]
public struct ActionViewRotation__Massage
{
    public int UserID;
    public float UserViewRotation;
}
public struct ActionSneak__Message
{
    public int UserID;
    public GameObject SneakObj;
}
public partial struct Message
{
    public const string GameStart = "gameStart";
    public const string ExitUser = "exitUser";
    public const string Join = "join";
    public const string UpdateUser = "updateUser";

    public const string ActionShot = "actionShot";
    public const string ActionDamge = "actionDamage";

    public const string ActionWalk_F = "actionWalkF";
    public const string ActionWalk_B = "actionWalkB";
    public const string ActionWalk_L = "actionWalkL";
    public const string ActionWalk_R = "actionWalkR";

    public const string ActionWalk_FL = "actionWalkFL";
    public const string ActionWalk_BL = "actionWalkBL";
    public const string ActionWalk_FR = "actionWalkFR";
    public const string ActionWalk_BR = "actionWalkBR";

    public const string ActionView_XL = "actionViewXL";
    public const string ActionView_XR = "actionViewXR";

    public const string ActionSneak = "actionSneak";
    public const string ActionJump = "actionJump";

    public const string ActionWalk = "actionWalk";

    public const string ActionRotation = "actionRotation";
    public const string ActionViewRotation = "actionViewRotation";
}