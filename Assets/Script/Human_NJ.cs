using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Human_NJ : MonoBehaviour
{
    //--------------------------------------------------------------------
    //---------------------User Init--------------------------------------
    //--------------------------------------------------------------------
    public int UserID;
    public float Hp = 400;
    public float Dmg = 100;
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
    public int killID;
    public int tokuten;

    public bool IsDead => Hp <= 0;
    //--------------------------------------------------------------------
    //---------------------User Walk and Jump-----------------------------
    //--------------------------------------------------------------------
    float JumpSpeed = 1;
    float WalkSpeed = 0.002f;
    //--------------------------------------------------------------------
    //---------------------User Sneak-------------------------------------
    //--------------------------------------------------------------------
    public GameObject PlayerBody;
    public GameObject SneakObj;
    public GameObject[] SneakList;
    //--------------------------------------------------------------------
    //---------------------User Attack------------------------------------
    //--------------------------------------------------------------------
    public GameObject Arms;
    public GameObject Sword;
    Vector3 CameraV3;
    //--------------------------------------------------------------------
    //---------------------Bool key---------------------------------------
    //--------------------------------------------------------------------
    bool TimerBool;
    //bool IsRotation;
    //bool IsWalk;
    //bool IsJump;
    //bool IsDobJump;
    //--------------------------------------------------------------------
    //---------------------User View--------------------------------------
    //--------------------------------------------------------------------
    float Cam_Y_RotationMax = 90;
    float Cam_Y_RotationMin = -30;
    float Cam_Y_RotationSum = 0;
    float RotationSpeed = 1f;
    public GameObject CAM;
    //--------------------------------------------------------------------
    //---------------------Tokutenbann------------------------------------
    //--------------------------------------------------------------------
    public Text tokutenbenn;
    //--------------------------------------------------------------------
    //---------------------Init-------------------------------------------
    //--------------------------------------------------------------------
    float updateTime = 0.5f;
    float updateTime_DMG;
    int updateTime_Walk;
    IEnumerator TimerEvent;
    void Start()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
        PlayerCamera = GetComponentInChildren<Camera>();
        CameraV3 = PlayerCamera.transform.localPosition;

        MainUserAction();
    }
    //--------------------------------------------------------------------
    //---------------------User Action------------------------------------
    //--------------------------------------------------------------------
    void Update()
    {
        if (IsDead) { return; }

        X = transform.position.x;
        Y = transform.position.y;
        Z = transform.position.z;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            tokuten += 30;
        }
    }
    public void MainUserAction()
    {
        GameEngine.Instance.Send(Message.UpdatePlayerStatic, new UpdatePlayerStatic__Massage
        {
            UserID = UserID,
            UserPos = new Vector3(X, Y, Z),
            UserRotation = Rotation,
            UserViewRotation = UserViewRotation,
            IsSneak = IsSneak,
            SneakName = SneakName,
            tokuten = tokuten,
            MoveH = MoveH,
            MoveV = MoveV,
            IsWalkStop = IsWalkStop
        });
    }
    //--------------------------------------------------------------------
    //---------------------User Jump--------------------------------------
    //--------------------------------------------------------------------
    public void SetJump()
    {
        transform.GetComponent<Rigidbody>().velocity += new Vector3(0, 0.5f, 0);
        transform.GetComponent<Rigidbody>().AddForce(transform.up * JumpSpeed, ForceMode.Impulse);
        X = transform.position.x;
        Y = transform.position.y;
        Z = transform.position.z;
        MainUserAction();
    }
    public void SetDobJump()
    {
        transform.GetComponent<Rigidbody>().AddForce(transform.forward * JumpSpeed, ForceMode.Impulse);
        X = transform.position.x;
        Y = transform.position.y;
        Z = transform.position.z;
        MainUserAction();
    }
    //--------------------------------------------------------------------
    //---------------------User Sneak-------------------------------------
    //--------------------------------------------------------------------
    public void SetSneak()
    {
        if (!IsSneak)
        {
            SneakObj = SneakList[Random.Range(0, 7)];
            SneakName = SneakObj.name;
            PlayerBody.SetActive(false);
            SneakObj.SetActive(true);
            switch (SneakName)
            {
                case "Tree":
                    PlayerCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.04f, CameraV3.z - 0.07f);
                    break;
                case "Rock":
                    PlayerCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.005f, CameraV3.z - 0.04f);
                    break;
                case "Yuki":
                    PlayerCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.04f, CameraV3.z - 0.07f);
                    break;
                case "Light":
                    PlayerCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.04f, CameraV3.z - 0.07f);
                    break;
                case "Forcer":
                    PlayerCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.04f, CameraV3.z - 0.07f);
                    break;
                case "Rabbit":
                    PlayerCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.005f, CameraV3.z - 0.04f);
                    break;
                case "Chicken":
                    PlayerCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.005f, CameraV3.z - 0.04f);
                    break;
            }
            IsSneak = true;
            MainUserAction();
        }
    }
    //--------------------------------------------------------------------
    //---------------------User View Rotate-------------------------------
    //--------------------------------------------------------------------
    public Camera getCamera()
    {
        return GetComponentInChildren<Camera>();
    }
    public void SetView(float Cam_X, float Cam_Y)
    {
        transform.Rotate(0, Cam_X * RotationSpeed, 0);
        Rotation = transform.rotation;
        MainUserAction();

        Cam_Y_RotationSum += Cam_Y;
        Cam_Y_RotationSum = Mathf.Clamp(Cam_Y_RotationSum, Cam_Y_RotationMin, Cam_Y_RotationMax);
        CAM.transform.localPosition = new Vector3(0, (float)0.0163, (float)-0.004899);

        PlayerCamera.transform.localEulerAngles = new Vector3(-Cam_Y_RotationSum, 0, 0);
        UserViewRotation = PlayerCamera.transform.localRotation;
        CAM.transform.localRotation = UserViewRotation;
    }
    //--------------------------------------------------------------------
    //---------------------Server Update User Static----------------------
    //-----------------------------同期用関数------------------------------
    //--------------------------------------------------------------------
    public void UpdateOtherPlayerStatic(float v , float h, bool ws)
    {
        if (TimerBool)
        {
            TimerBool = false;
            StopCoroutine(TimerEvent);// stop Timer
        }
        if (ws)
        {
            ws = false;
            v = 0;
            h = 0;
        }
        if (IsDead)
        {
            return;
        }
        //プレイヤーのRotationを同期する、必要な
        if (Rotation != transform.rotation)
        {
            transform.rotation = Rotation;
        }
        if (UserViewRotation != CAM.transform.localRotation)
        {
            CAM.transform.localRotation = UserViewRotation;
        }
        //プレイヤーのPositionを同期する、必要な
        if ((int)(X * 1000) != (int)(transform.position.x * 1000) ||
            (int)(Y * 1000) != (int)(transform.position.y * 1000) ||
            (int)(Z * 1000) != (int)(transform.position.z * 1000))
        {
            transform.position = new Vector3(X, Y, Z);
            TimerEvent = GuessOtherPlayerPos(transform.position, v, h);
            StartCoroutine(TimerEvent);//start Timer
        }
        if (IsSneak)
        {
            PlayerBody.SetActive(false);
            switch (SneakName)
            {
                case "Tree":
                    SneakObj = SneakList[0];
                    break;
                case "Rock":
                    SneakObj = SneakList[1];
                    break;
                case "Yuki":
                    SneakObj = SneakList[2];
                    break;
                case "Light":
                    SneakObj = SneakList[3];
                    break;
                case "Forcer":
                    SneakObj = SneakList[4];
                    break;
                case "Rabbit":
                    SneakObj = SneakList[5];
                    break;
                case "Chicken":
                    SneakObj = SneakList[6];
                    break;
            }
            SneakObj.SetActive(true);
        }
        else
        {
            if (SneakObj == null) return;
            SneakObj.SetActive(false);
            PlayerBody.SetActive(true);
        }
    }
    IEnumerator GuessOtherPlayerPos(Vector3 v3, float v, float h)
    {
        TimerBool = true;
        int F = 30;
        while (F > 0)
        {
            var addSpeed = (tokuten * 0.0000075f);
            transform.Translate(h * ( WalkSpeed + addSpeed ) , 0, v * ( WalkSpeed + addSpeed ));
            X = transform.position.x;
            Y = transform.position.y;
            Z = transform.position.z;
            //transform.position = new Vector3(X, Y, Z);
            F -= 1;
            yield return new WaitForEndOfFrame();
        }
    }
    //--------------------------------------------------------------------
    //---------------------User Walk--------------------------------------
    //--------------------------------------------------------------------
    public void SetIsWalkStop(bool ws)
    {
        IsWalkStop = ws;
    }
    public void SetMove(float h, float v)
    {
        var addSpeed = (tokuten * 0.000075f);
        transform.Translate(h * (WalkSpeed + addSpeed), 0, v * (WalkSpeed + addSpeed));

        X = transform.position.x;
        Y = transform.position.y;
        Z = transform.position.z;

        if (updateTime_Walk % 30 == 0)
        {
            MoveV = v;
            MoveH = h;
            MainUserAction();
        }
        updateTime_Walk++;
    }
    //--------------------------------------------------------------------
    //-------------------User Attack--------------------------------------
    //--------------------------------------------------------------------
    public void Shot()
    {
        if (IsSneak)
        {
            IsSneak = false;
            PlayerCamera.transform.localPosition = CameraV3;
            PlayerBody.SetActive(true);
            SneakObj.SetActive(false);
            MainUserAction();
        }
        var arms = Instantiate(Arms);
        arms.SetActive(true);
        arms.gameObject.name = UserID + "Arms";
        arms.transform.position = transform.position + transform.TransformDirection(new Vector3(0.0045f, 0, 0.032f));
        //arms.transform.rotation = transform.rotation;
        CAM.transform.localRotation = UserViewRotation;
        arms.transform.rotation = CAM.transform.rotation;
    }
    public void Giri()
    {
        if (IsSneak)
        {
            IsSneak = false;
            PlayerCamera.transform.localPosition = CameraV3;
            PlayerBody.SetActive(true);
            SneakObj.SetActive(false);
            MainUserAction();
        }
        var sword = Instantiate(Sword);
        sword.SetActive(true);
        sword.gameObject.name = UserID + "Sword";
        sword.transform.SetParent(gameObject.transform, false);
        CAM.transform.localRotation = UserViewRotation;
        sword.transform.rotation = CAM.transform.rotation;
    }
    public void Damage(float dmg)
    {
        Hp -= dmg;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Sword") && Time.time - updateTime_DMG > updateTime)
        {
            updateTime_DMG = Time.time;

            string gameObjName = other.gameObject.name;
            gameObjName = gameObjName.Remove(gameObjName.IndexOf("S"), 5);
            if (UserID != int.Parse(gameObjName))
            {
                if (UserID == GameEngine.Instance.GameEngineID)
                {
                    GameEngine.Instance.Send(Message.ActionDamge, new ActionDmgMessage
                    {
                        UserID = UserID,
                        Dmg = 400,
                        killID = int.Parse(gameObjName)
                    });
                }
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Arms") && Time.time - updateTime_DMG > updateTime)
        {
            updateTime_DMG = Time.time;
            string gameObjName = collision.gameObject.name;
            gameObjName = gameObjName.Remove(gameObjName.IndexOf("A"), 4);
            if (UserID != int.Parse(gameObjName))
            {
                if (UserID == GameEngine.Instance.GameEngineID)
                {
                    GameEngine.Instance.Send(Message.ActionDamge, new ActionDmgMessage
                    {
                        UserID = UserID,
                        Dmg = Dmg,
                        killID = int.Parse(gameObjName)
                    });
                }
                Destroy(collision.gameObject);
            }
        }
    }
    public IEnumerator Dead()
    {
        if (transform == null || gameObject == null)
        {
            StopCoroutine(Dead());
            yield return null;
        }

        while (transform.eulerAngles.z < 80)
        {
            transform.Rotate(Vector3.forward * 1.5f);
            yield return null;
        }
        Destroy(gameObject);
    }
    //--------------------------------------------------------------------
    //---------------------Update tokutenbann-----------------------------
    //--------------------------------------------------------------------
    public int GetTokuten()
    {
        return tokuten;
    }
    public void SetTokuten(int _tokuten)
    {
        tokuten = _tokuten;

    }
    public void UpdateTokutenbann(string tokutenbannText)
    {
        tokutenbenn.text = tokutenbannText;
    }
}
