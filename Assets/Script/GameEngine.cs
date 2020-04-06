using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour
{
    public GameObject ExitBtn;
    public GameObject Retry;
    public int GameEngineID;
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
        ExitBtn.SetActive(false);
        Human_NJ.SetActive(false);
        Instance = this;
    }
    void Start()
    {
        Init();
    }
    public void Init()
    {
        //string hostname = Dns.GetHostName();
        //IPAddress[] adrList = Dns.GetHostAddresses(hostname);
        _client = new WsClient("ws://192.168.8.85:3000");
        //_client = new WsClient("ws://"+ adrList[1].ToString() + ":3000");
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
    private void FixedUpdate()
    {
        //while(_message.Count > 0)
        //{
        //    var msg = _message.Pop();
        //    updateMessageForGameStarted(msg);
        //}
    }
    void Update()
    {
        while (_message.Count > 0)
        {
            var msg = _message.Pop();
            updateMessageForGameStarted(msg);
        }
    }
    void updateMessageForGameStarted(Message msg)
    {
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
                                GameEngineID = obj.UserID;
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
            case Message.UpdatePlayerStatic:
                {
                    var data = JsonUtility.FromJson<UpdatePlayerStatic__Massage>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);
                    if (data.UserID != Player.Human_NJ.UserID)
                    {
                        human_nj.X = data.UserPos.x;//Global
                        human_nj.Y = data.UserPos.y;//Global
                        human_nj.Z = data.UserPos.z;//Global
                        human_nj.MoveV = data.MoveV;
                        human_nj.MoveH = data.MoveH;
                        human_nj.IsWalkStop = data.IsWalkStop;

                        human_nj.Rotation = data.UserRotation;//Global

                        human_nj.IsSneak = data.IsSneak;
                        human_nj.SneakName = data.SneakName;
                        
                        human_nj.UserViewRotation = data.UserViewRotation;
                        human_nj.UpdateOtherPlayerStatic(human_nj.MoveV, human_nj.MoveH, human_nj.IsWalkStop);//Local

                        human_nj.tokuten = data.tokuten;
                    }
                    string tokutenbannText = "得点板\n";
                    for (int i = 0; i < human_nj_List.Count; i++)
                    {
                        tokutenbannText += human_nj_List[i].UserID + " : " + human_nj_List[i].tokuten + "点\n";
                    }
                    human_nj.UpdateTokutenbann(tokutenbannText);
                }
                break;
            case Message.ActionShot:
                {
                    var data = JsonUtility.FromJson<ActionShotMessage>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);

                    human_nj.Shot();
                }
                break;
            case Message.ActionSword:
                {
                    var data = JsonUtility.FromJson<ActionSwordMessage>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);
                    
                    human_nj.Giri();
                }
                break;
            case Message.ActionSkill:
                {
                    var data = JsonUtility.FromJson<ActionSkillMessage>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);
                    human_nj.ActionSkill();
                }
                break;
            case Message.ActionSkill2:
                {
                    var data = JsonUtility.FromJson<ActionSkill2Message>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);
                    human_nj.ActionSkill2();
                }
                break;
            case Message.ActionDamge:
                {
                    var data = JsonUtility.FromJson<ActionDmgMessage>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);
                    if(human_nj.Hp > 0)
                    {
                        human_nj.Damage(data.Dmg);
                    }
                    human_nj.tokuten = human_nj.GetTokuten();
                    var killer = FindHumanNJ(data.killID);

                    if (human_nj.IsDead)
                    {
                        killer.tokuten = killer.tokuten + human_nj.tokuten + 1;
                        killer.SetTokuten(killer.tokuten);
                        if (killer.tokuten >= 100)
                        {
                            Send(Message.GameEnd, new GameEndMessage { UserID = killer.UserID });
                        }

                        StartCoroutine(human_nj.Dead());

                        if (human_nj.UserID == Player.Human_NJ.UserID)
                        {
                            _client.Dispose();
                            GameStarted = false;
                            MainCamera.transform.SetParent(null, false);
                            foreach (var n in human_nj_List)
                            {
                                Destroy(n.gameObject);
                            }
                            human_nj_List.Clear();
                        }

                        human_nj_List.Remove(human_nj);
                    }
                    killer.MainUserAction();
                    //killer.UpdateOtherPlayerStatic(killer.MoveV, killer.MoveH, killer.IsWalkStop);//Local
                }
                break;
            case Message.GameEnd:
                {
                    var data = JsonUtility.FromJson<GameEndMessage>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);
                    _client.Dispose();
                    GameStarted = false;
                    MainCamera.transform.SetParent(null, false);
                    foreach (var n in human_nj_List)
                    {
                        Destroy(n.gameObject);
                    }
                    human_nj_List.Clear();

                    ExitBtn.SetActive(true);
                    Retry.SetActive(false);
                    Cursor.lockState = CursorLockMode.None;
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
    public void OnClickRetry()
    {
        Application.Quit();
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
    public float MoveH;
    public float MoveV;
    public bool IsWalkStop;
    public Quaternion UserViewRotation;
    public Quaternion Rotation;
    public Camera PlayerCamera;
    public bool IsSneak;
    public string SneakName;

    public int tokuten;
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
public struct ActionSwordMessage
{
    public int UserID;
}
[Serializable]
public struct ActionSkillMessage
{
    public int UserID;
}
[Serializable]
public struct ActionSkill2Message
{
    public int UserID;
}
[Serializable]
public struct ActionDmgMessage
{
    public int UserID;
    public float Dmg;
    public int killID;
}
[Serializable]
public struct UpdatePlayerStatic__Massage
{
    public int UserID;
    public Vector3 UserPos;
    public float MoveH;
    public float MoveV;
    public bool IsWalkStop;
    public Quaternion UserRotation;
    public Quaternion UserViewRotation;
    public bool IsSneak;
    public string SneakName;
    public int tokuten;
}
[Serializable]
public struct GameEndMessage
{
    public int UserID;
}
public partial struct Message
{
    public const string GameStart = "gameStart";
    public const string ExitUser = "exitUser";
    public const string Join = "join";

    public const string ActionSkill = "actionSkill";
    public const string ActionSkill2 = "actionSkill2";
    public const string ActionSword = "actionSword";
    public const string ActionShot = "actionShot";
    public const string ActionDamge = "actionDamage";

    public const string UpdatePlayerStatic = "updatePlayerStatic";

    public const string GameEnd = "gameEnd";
}
