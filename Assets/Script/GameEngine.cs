using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

   // Dictionary<int, Human_NJ> user_id_kado = new Dictionary<int, Human_NJ>();

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
        _client = new WsClient("ws://192.168.8.85:3000");
        _client.OnMessage = onMessage;
    }
    void onMessage(Message msg)
    {
        _message.Push(msg);
        //Debug.Log("GameEngine - msg => " + msg);
        //Debug.Log("GameEngine - msg.Data => " + msg.Data);
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
            //Debug.Log("Update - msg => " + msg);
            //Debug.Log("Update - msg.Type => " + msg.Type);
            //Debug.Log("Update - msg.Data => " + msg.Data);
            if (GameStarted)
            {
                updateMessageForGameStarted(msg);
            }
            else
            {
                updateMessage(msg);
            }
        }
        if (GameStarted)
        {
            updateServerUser();
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
                    GameStarted = true;
                }
                break;
            case Message.ActionWalk_F:
                {
                    var data = JsonUtility.FromJson<WalkF__Message>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);
                    human_nj.SetMovePositionF();
                    //human_nj.X = human_nj.UpdatePos().x;
                    //human_nj.Y = human_nj.UpdatePos().y;
                    //human_nj.Z = human_nj.UpdatePos().z;
                    //Debug.Log("human_nj.X " + human_nj.X);
                    //Debug.Log("human_nj.Y " + human_nj.Y);
                    //Debug.Log("human_nj.Z " + human_nj.Z);
                }
                break;
            case Message.ActionWalk_B:
                {
                    var data = JsonUtility.FromJson<WalkB__Message>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);
                    human_nj.SetMovePositionB();
                }
                break;
            case Message.ActionWalk_L:
                {
                    var data = JsonUtility.FromJson<WalkL__Message>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);
                    human_nj.SetMovePositionL();
                }
                break;
            case Message.ActionWalk_R:
                {
                    var data = JsonUtility.FromJson<WalkR__Message>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);
                    human_nj.SetMovePositionR();
                }
                break;
            case Message.UpdateUser:
                {
                    var data = JsonUtility.FromJson<UpdateUserMessage>(msg.Data);
                    var human_nj = FindHumanNJ(data.User.ID);
                    if (human_nj != null)
                    {
                        //human_nj.X = human_nj.UpdatePos().x;
                        //human_nj.Y = human_nj.UpdatePos().y;
                        //human_nj.Z = human_nj.UpdatePos().z;
                        //Debug.Log("human_nj.X " + human_nj.X);
                        //Debug.Log("human_nj.Y " + human_nj.Y);
                        //Debug.Log("human_nj.Z " + human_nj.Z);
                        //Debug.Log("human_nj_List.Count =>" + human_nj_List.Count);
                        //for (int i = 0; i < human_nj_List.Count; i++)
                        //{
                        //    human_nj_List[i].X = human_nj_List[i].UpdatePos().x;
                        //    human_nj_List[i].Y = human_nj_List[i].UpdatePos().y;
                        //    human_nj_List[i].Z = human_nj_List[i].UpdatePos().z;
                        //    human_nj_List[i].SetPos(new Vector3(human_nj_List[i].X, human_nj_List[i].Y, human_nj_List[i].Z));
                        //    //Player.UpdatePos(new Vector3(human_nj_List[i].X, human_nj_List[i].Y, human_nj_List[i].Z));
                         
                        //    //Debug.Log("human_nj_List.Count UserID " + i.ToString() + " => " + human_nj_List[i].UserID);
                        //    //Debug.Log("human_nj_List.Count UserID " + i.ToString() + " X => " + human_nj_List[i].X);
                        //    //Debug.Log("human_nj_List.Count UserID " + i.ToString() + " Y => " + human_nj_List[i].Y);
                        //    //Debug.Log("human_nj_List.Count UserID " + i.ToString() + " Z => " + human_nj_List[i].Z);
                        //}
                    }
                    else
                    {
                        _users.Add(data.User);
                        human_nj_List.Add(createPlayer(data.User));
                    }
                    //Debug.Log("data => " + data);
                    //Debug.Log("data.User => " + data.User);
                    //Debug.Log("data.User.X => " + data.User.X);
                    //Debug.Log("data.User.Y => " + data.User.Y);
                    //Debug.Log("data.User.Z => " + data.User.Z);
                    //Debug.Log("data.User.ID => " + data.User.ID);
                    //Debug.Log("data.User.Name => " + data.User.Name);
                    //Debug.Log("data.User.WsName => " + data.User.WsName);
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
            case Message.ActionShot:
                {
                    var data = JsonUtility.FromJson<ActionShotMessage>(msg.Data);
                    var human_nj = FindHumanNJ(data.UserID);
                    human_nj.Shot();
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
                    }
                    StartCoroutine(human_nj.Dead());
                }
                break;
        }
    }
    void updateMessage(Message msg)
    {
        switch (msg.Type)
        {
            case Message.Join:
                _client.SendMessage(Message.GameStart, "Human_NJ");
                break;
            case Message.GameStart:
                {
                    var data = JsonUtility.FromJson<GameStartMessage>(msg.Data);
                    _users = data.Users;
                    foreach(var user in _users)
                    {
                        var obj = createPlayer(user);
                        human_nj_List.Add(obj);
                        if(user.ID == data.Player.ID)
                        {
                            MainCamera.transform.SetParent(obj.transform, false);
                            MainCamera.transform.localPosition = new Vector3(0, (float)0.0163, (float)-0.004899);
                            Player.Init(obj);
                        }
                    }
                    GameStarted = true;
                }
                break;
        }
    }
    void updateServerUser()
    {
    
        if (_frameCount % 100 == 0)
        {
            var msg = new UpdateUserMessage();
            var c = FindUser(Player.Human_NJ.UserID);//User.Data 
            c.X = Player.Human_NJ.transform.position.x;
            c.Y = Player.Human_NJ.transform.position.y;
            c.Z = Player.Human_NJ.transform.position.z;
            c.Hp = Player.Human_NJ.Hp;
            c.Dmg = Player.Human_NJ.Dmg;
            c.IsSneak = Player.Human_NJ.isSneak;
            msg.User = c;
            //Debug.Log("c => " + c);
            //Debug.Log("c.X => " + c.X);
            //Debug.Log(" c.Y => " + c.Y);
            //Debug.Log(" c.Z => " + c.Z);
            //Debug.Log("c.ID => " + c.ID);
            //Debug.Log("c.Name => " + c.Name);
            //Debug.Log("c.WsName => " + c.WsName);
            //Debug.Log("GameEngine - msg => " + msg);
            //Debug.Log("GameEngine - msg.User => " + msg.User);
            //Debug.Log("GameEngine - msg.User.X => " + msg.User.X);
            //Debug.Log("GameEngine - msg.User.Y => " + msg.User.Y);
            //Debug.Log("GameEngine - msg.User.Z => " + msg.User.Z);
            //Debug.Log("GameEngine - msg.User.ID => " + msg.User.ID);
            //Debug.Log("GameEngine - msg.User.Name => " + msg.User.Name);
            //Debug.Log("GameEngine - msg.User.WsName => " + msg.User.WsName);
            Send(Message.UpdateUser, msg);
        }
        _frameCount++;
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
        human_nj.UserID = u.ID;
        human_nj.Hp = u.Hp;
        human_nj.Dmg = u.Dmg;
        //human_nj.SetMovePosition(pos);

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
struct UpdateUserMessage
{
    public UserData User;
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
    public int Dmg;
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
public struct WalkF__Message
{
    public int UserID;
}
public struct WalkB__Message
{
    public int UserID;
}
public struct WalkL__Message
{
    public int UserID;
}
public struct WalkR__Message
{
    public int UserID;
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

    public const string ActionSneak = "actionSneak";
}