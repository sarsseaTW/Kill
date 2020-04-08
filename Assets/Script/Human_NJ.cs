using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Human_NJ : MonoBehaviour
{
    #region Var
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
    //---------------------Skill------------------------------------------
    //--------------------------------------------------------------------
    public GameObject CAM_Skill2Point;
    public GameObject CAM_SkillPoint;
    public GameObject Skill_Eff;
    public GameObject Skill_Eff2;
    bool isSkill;
    //--------------------------------------------------------------------
    //---------------------Init-------------------------------------------
    //--------------------------------------------------------------------
    float updateTime = 0.5f;
    float updateTime_DMG;
    int updateTime_Walk;
    IEnumerator TimerEvent_Walk;
    IEnumerator TimerEvent_Skill;
    #endregion
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
        Rotation = transform.rotation;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            tokuten += 30;
        }
        if (isShot)
        {
            shotPoint = rh.transform.position;
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
    #region Jump
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
    #endregion
    #region Sneak
    //--------------------------------------------------------------------
    //---------------------User Sneak-------------------------------------
    //--------------------------------------------------------------------
    public void SetSneak()
    {
        if (!IsSneak)
        {
            SneakObj = SneakList[Random.Range(0, 9)];
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
                case "Tortiose":
                    PlayerCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.005f, CameraV3.z - 0.04f);
                    break;
                case "Board":
                    PlayerCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.005f, CameraV3.z - 0.04f);
                    break;
            }
            IsSneak = true;
            MainUserAction();
        }
    }
    #endregion
    #region Camera View Rotate
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
        MainUserAction();

        Cam_Y_RotationSum += Cam_Y;
        Cam_Y_RotationSum = Mathf.Clamp(Cam_Y_RotationSum, Cam_Y_RotationMin, Cam_Y_RotationMax);
        CAM.transform.localPosition = new Vector3(0, (float)0.0163, (float)-0.004899);

        PlayerCamera.transform.localEulerAngles = new Vector3(-Cam_Y_RotationSum, 0, 0);
        UserViewRotation = PlayerCamera.transform.localRotation;
        CAM.transform.localRotation = UserViewRotation;
    }
    #endregion
    #region Update Other Users
    //--------------------------------------------------------------------
    //---------------------Server Update User Static----------------------
    //-----------------------------同期用関数------------------------------
    //--------------------------------------------------------------------
    public void UpdateOtherPlayerStatic(float v , float h, bool ws)
    {
        if (TimerBool)
        {
            TimerBool = false;
            StopCoroutine(TimerEvent_Walk);// stop Timer
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
            TimerEvent_Walk = GuessOtherPlayerPos(transform.position, v, h);
            StartCoroutine(TimerEvent_Walk);//start Timer
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
                case "Tortiose":
                    SneakObj = SneakList[7];
                    break;
                case "Board":
                    SneakObj = SneakList[8];
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
            F -= 1;
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
    #region Walk
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
    #endregion
    #region Attack
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
        if (!isSkill)
        {
            if ((other.gameObject.name.Contains("Skill2")))
            {
                string gameObjName = other.gameObject.name;
                gameObjName = gameObjName.Remove(gameObjName.IndexOf("S"), 6);

                if (UserID != int.Parse(gameObjName))
                {
                    if (UserID == GameEngine.Instance.GameEngineID)
                    {
                        GameEngine.Instance.Send(Message.ActionDamge, new ActionDmgMessage
                        {
                            UserID = UserID,
                            Dmg = 50,
                            killID = int.Parse(gameObjName)
                        });
                    }
                }
            }
            else if ((other.gameObject.name.Contains("Sword") || other.gameObject.name.Contains("Skill")) && Time.time - updateTime_DMG > updateTime)
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
        StopAllCoroutines();
        Destroy(gameObject);
    }
    #endregion
    #region tokutenban
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
    #endregion
    #region skill
    //--------------------------------------------------------------------
    //---------------------Update Skill-----------------------------------
    //--------------------------------------------------------------------
    public void skillCAM()
    {
        if (IsDead) { return; }
        PlayerCamera.transform.position = CAM_SkillPoint.transform.position;
        PlayerCamera.transform.localRotation = CAM_SkillPoint.transform.localRotation;
    }
    public void skill2CAM()
    {
        if (IsDead) { return; }
        PlayerCamera.transform.position = CAM_Skill2Point.transform.position;
        PlayerCamera.transform.localRotation = CAM_Skill2Point.transform.localRotation;
    }
    #region skill1
    public void ActionSkill()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            return;
        }
        isSkill = true;
        Physics.IgnoreLayerCollision(9,10, true);
        Hp -= 50;
        if (IsSneak)
        {
            IsSneak = false;
            PlayerCamera.transform.localPosition = CameraV3;
            PlayerBody.SetActive(true);
            SneakObj.SetActive(false);
            MainUserAction();
        }
        var skill = Instantiate(Skill_Eff);
        skill.SetActive(true);
        skill.gameObject.name = UserID + "Skill";
        skill.transform.SetParent(gameObject.transform, false);
        
        transform.position = new Vector3(X, Y, Z);

        StartCoroutine(Skill_1hit());
    }
    //---------- 1 hit -------------------------
    public IEnumerator Skill_1hit()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill_1hit());
            yield return null;
        }
        yield return new WaitForSeconds(3);
        transform.GetComponent<Rigidbody>().velocity += new Vector3(0, 1f, 0);
        transform.GetComponent<Rigidbody>().AddForce(transform.forward * 2, ForceMode.Impulse);
        StartCoroutine(Skill_2Read());
    }
    //---------- 2 hit -------------------------
    public IEnumerator Skill_2Read()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill_2Read());
            yield return null;
        }
        MainUserAction();
        yield return new WaitForSeconds(0.8f);
        transform.Rotate(new Vector3(0, 150, 0));
        StartCoroutine(Skill_2hit());
    }
    public IEnumerator Skill_2hit()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill_2hit());
            yield return null;
        }
        MainUserAction();
        yield return new WaitForSeconds(0.1f);
        transform.GetComponent<Rigidbody>().velocity += new Vector3(0, 1f, 0);
        transform.GetComponent<Rigidbody>().AddForce(transform.forward * 2, ForceMode.Impulse);
        StartCoroutine(Skill_3Read());
    }
    //---------- 3 hit -------------------------
    public IEnumerator Skill_3Read()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill_3Read());
            yield return null;
        }
        MainUserAction();
        yield return new WaitForSeconds(0.8f);
        transform.Rotate(new Vector3(0, 150, 0));
        StartCoroutine(Skill_3hit());
    }
    public IEnumerator Skill_3hit()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill_3hit());
            yield return null;
        }
        MainUserAction();
        yield return new WaitForSeconds(0.1f);
        transform.GetComponent<Rigidbody>().velocity += new Vector3(0, 1f, 0);
        transform.GetComponent<Rigidbody>().AddForce(transform.forward * 2, ForceMode.Impulse);
        StartCoroutine(Skill_4Read());
    }
    //---------- 4 hit -------------------------
    public IEnumerator Skill_4Read()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill_4Read());
            yield return null;
        }
        MainUserAction();
        yield return new WaitForSeconds(0.8f);
        transform.Rotate(new Vector3(0, 150, 0));
        StartCoroutine(Skill_4hit());
    }
    public IEnumerator Skill_4hit()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill_4hit());
            yield return null;
        }
        MainUserAction();
        yield return new WaitForSeconds(0.1f);
        transform.GetComponent<Rigidbody>().velocity += new Vector3(0, 1f, 0);
        transform.GetComponent<Rigidbody>().AddForce(transform.forward * 2, ForceMode.Impulse);
        StartCoroutine(Skill_5Read());
    }
    //---------- 5 hit -------------------------
    public IEnumerator Skill_5Read()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill_5Read());
            yield return null;
        }
        MainUserAction();
        yield return new WaitForSeconds(0.8f);
        transform.Rotate(new Vector3(0, 150, 0));
        StartCoroutine(Skill_5hit());
    }
    public IEnumerator Skill_5hit()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill_5hit());
            yield return null;
        }
        MainUserAction();
        yield return new WaitForSeconds(0.1f);
        transform.GetComponent<Rigidbody>().velocity += new Vector3(0, 1f, 0);
        transform.GetComponent<Rigidbody>().AddForce(transform.forward * 2, ForceMode.Impulse);
        StartCoroutine(Skill_6Read());
    }
    //---------- 6 hit -------------------------
    public IEnumerator Skill_6Read()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill_6Read());
            yield return null;
        }
        MainUserAction();
        yield return new WaitForSeconds(0.8f);
        transform.Rotate(new Vector3(0, 150, 0));
        StartCoroutine(Skill_6hit());
    }
    public IEnumerator Skill_6hit()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill_6hit());
            yield return null;
        }
        MainUserAction();
        yield return new WaitForSeconds(0.1f);
        transform.GetComponent<Rigidbody>().velocity += new Vector3(0, 1f, 0);
        transform.GetComponent<Rigidbody>().AddForce(transform.forward*1f, ForceMode.Impulse);
        StartCoroutine(Skill_7Read());
    }
    //---------- 7 hit -------------------------
    public IEnumerator Skill_7Read()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill_7Read());
            yield return null;
        }
        MainUserAction();
        yield return new WaitForSeconds(0.8f);

        StartCoroutine(Skill_7hit());
    }
    public IEnumerator Skill_7hit()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill_7hit());
            yield return null;
        }
        MainUserAction();
        yield return new WaitForSeconds(0.1f);
        transform.GetComponent<Rigidbody>().velocity += new Vector3(0, 1f, 0);
        transform.GetComponent<Rigidbody>().AddForce(transform.up*3, ForceMode.Impulse);
        StartCoroutine(Skill_CloseEff());
    }
    //---------- 7 hit -------------------------
    public IEnumerator Skill_CloseEff()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill_CloseEff());
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        isSkill = false;
        Physics.IgnoreLayerCollision(9, 10, false);
        PlayerController.IsSkill = false;

        PlayerCamera.transform.localPosition = CameraV3;
        PlayerCamera.transform.localRotation = CAM.transform.localRotation;
        MainUserAction();
    }
    #endregion
    #region skill2

    RaycastHit rh;
    Ray ray;
    Vector3 shotPoint;
    string shotname;
    int count;
    bool isShot;
    public void ActionSkill2()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            return;
        }
        isSkill = true;
        Physics.IgnoreLayerCollision(0, 9, true);
        Hp -= 200;
        if (IsSneak)
        {
            IsSneak = false;
            PlayerCamera.transform.localPosition = CameraV3;
            PlayerBody.SetActive(true);
            SneakObj.SetActive(false);
            MainUserAction();
        }

        transform.position = new Vector3(X, Y, Z);

        Vector3 fwd = CAM.transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(CAM.transform.position, fwd,out rh, 100))
        {
            isShot = true;
        }

        transform.GetComponent<Rigidbody>().useGravity = false;
        transform.GetComponent<Rigidbody>().velocity += new Vector3(0, 0.1f, 0);
        transform.GetComponent<Rigidbody>().AddForce(transform.up * 0.1f, ForceMode.Impulse);
        StartCoroutine(Skill2_UP());
    }
    public IEnumerator Skill2_UP()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill2_UP());
            yield return null;
        }
        MainUserAction();
        yield return new WaitForSeconds(2);
        while(transform.position.y <= 1.5f)
        {
            transform.GetComponent<Rigidbody>().useGravity = false;
            transform.GetComponent<Rigidbody>().velocity += new Vector3(0, 0.1f, 0);
            transform.GetComponent<Rigidbody>().AddForce(transform.up * 0.1f, ForceMode.Impulse);
        }
        transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        int f = 20;
        while (f > 0)
        {
            var skill = Instantiate(Skill_Eff2);
            skill.SetActive(true);
            skill.gameObject.name = UserID + "Skill2";
            skill.transform.SetParent(gameObject.transform, false);
            skill.transform.position = transform.position + transform.TransformDirection(new Vector3(0.0045f, 0, 0.032f));
            f -= 1;
            yield return new WaitForSeconds(0.1f);
        }
        count = transform.childCount;
        StartCoroutine(Skill2_Shot());
    }
    public IEnumerator Skill2_Shot()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill2_Shot());
            yield return null;
        }
        yield return new WaitForSeconds(1);
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            if (transform.GetChild(i).name == (UserID + "Skill2"))
            {
                transform.GetChild(i).LookAt(shotPoint);
                transform.GetChild(i).GetComponent<Rigidbody>().AddForce(transform.GetChild(i).forward * 1f, ForceMode.Impulse);
            }
        }
        StartCoroutine(Skill2_CloseEff());
    }
    public IEnumerator Skill2_CloseEff()
    {
        if (IsDead)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            PlayerController.IsSkill = false;
            StopCoroutine(Skill2_CloseEff());
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        isSkill = false;
        transform.GetComponent<Rigidbody>().useGravity = true;
        PlayerController.IsSkill = false;
        isShot = false;
        PlayerCamera.transform.localPosition = CameraV3;
        PlayerCamera.transform.localRotation = CAM.transform.localRotation;
        MainUserAction();
    }
    #endregion skill2
    #endregion skill
}
