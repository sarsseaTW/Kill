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
    public Quaternion ViewRotation;
    public Quaternion Rotation;
    public Camera PlayerCamera;
    public bool IsSneak;
    public string SneakName;

    public bool IsDead => Hp <= 0;
    //--------------------------------------------------------------------
    //---------------------User Walk and Jump-----------------------------
    //--------------------------------------------------------------------
    float JumpSpeed = 1;
    float WalkSpeed = 0.001f;
    //--------------------------------------------------------------------
    //---------------------User Sneak-------------------------------------
    //--------------------------------------------------------------------
    public GameObject PlayerBody;
    public GameObject SneakObj;
    public GameObject [] SneakList;
    //--------------------------------------------------------------------
    //---------------------User Attack------------------------------------
    //--------------------------------------------------------------------
    public GameObject Arms;
    public GameObject ArmsPoint;
    Vector3 CameraV3;
    //--------------------------------------------------------------------
    //---------------------Bool key---------------------------------------
    //--------------------------------------------------------------------
    //bool IsRotation;
    //bool IsWalk;
    //bool IsJump;
    //--------------------------------------------------------------------
    //---------------------User View--------------------------------------
    //--------------------------------------------------------------------
    float Cam_Y_RotationMax = 90;
    float Cam_Y_RotationMin = -30;
    float Cam_Y_RotationSum = 0;
    float RotationSpeed = 1f;
    //--------------------------------------------------------------------
    //---------------------Init-------------------------------------------
    //--------------------------------------------------------------------
    int updateTime;
    void Start()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
        PlayerCamera = GetComponentInChildren<Camera>();
        CameraV3 = PlayerCamera.transform.localPosition;
    }
    //--------------------------------------------------------------------
    //---------------------User Action------------------------------------
    //--------------------------------------------------------------------
    void Update()
    {
        if (IsDead) { return; }
        if (updateTime % 3 == 0)
        {
            MainUserAction();
        }
        updateTime++;
    }
    public void MainUserAction()
    {
        GameEngine.Instance.Send(Message.UpdatePlayerStatic, new UpdatePlayerStatic__Massage
        {
            UserID = UserID,
            UserPos = new Vector3(X, Y, Z),
            UserRotation = Rotation,
            UserViewRotation = Cam_Y_RotationSum,
            IsSneak = IsSneak,
            SneakName = SneakName
        });
    }
    //--------------------------------------------------------------------
    //---------------------User Jump--------------------------------------
    //--------------------------------------------------------------------
    public void SetJump()
    {
        GetComponent<Rigidbody>().velocity += new Vector3(0, 1, 0);
        GetComponent<Rigidbody>().AddForce(Vector3.up * JumpSpeed, ForceMode.Impulse);
    }
    //--------------------------------------------------------------------
    //---------------------User Sneak-------------------------------------
    //--------------------------------------------------------------------
    public void SetSneak()
    {
        if (!IsSneak)
        {
            SneakObj = SneakList[Random.Range(0, 2)];
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
            }
            IsSneak = true;
        }
    }
    //--------------------------------------------------------------------
    //---------------------User View Rotate-------------------------------
    //--------------------------------------------------------------------
    public Camera getCamera()
    {
        return PlayerCamera;
    }
    public void SetView(float Cam_X,float Cam_Y)
    {
        transform.Rotate(0, Cam_X * RotationSpeed, 0);
        Rotation = transform.rotation;

        //Cam_Y_RotationSum += Cam_Y;
        //Cam_Y_RotationSum = Mathf.Clamp(Cam_Y_RotationSum, Cam_Y_RotationMin, Cam_Y_RotationMax);
        //PlayerCamera.transform.localEulerAngles = new Vector3(-Cam_Y_RotationSum, 0, 0);
        //IsRotation = true;
        
        //------------------------------BUG-------------------------------
        //if (IsViewRotation)
        //{
        //    PlayerCamera.transform.localEulerAngles = new Vector3(-Cam_Y_RotationSum, 0, 0);
        //    //ViewRotation = PlayerCamera.transform.localRotation;
        //    GameEngine.Instance.Send(Message.ActionViewRotation, new ActionViewRotation__Massage { UserID = UserID, UserViewRotation = Cam_Y_RotationSum });
        //    IsViewRotation = false;
        //}
        //----------------------------------------------------------------
    }
    //--------------------------------------------------------------------
    //---------------------Server Update User Static----------------------
    //-----------------------------同期用関数------------------------------
    //--------------------------------------------------------------------
    public void UpdateOtherPlayerStatic()
    {
        //プレイヤーのRotationを同期する、必要な
        if (Rotation != transform.rotation)
        {
            transform.rotation = Rotation;
        }
        //プレイヤーのPositionを同期する、必要な
        if ((int)(X * 1000) != (int)(transform.position.x * 1000) ||
            (int)(Y * 1000) != (int)(transform.position.y * 1000) ||
            (int)(Z * 1000) != (int)(transform.position.z * 1000))
        {
            transform.position = new Vector3(X, Y, Z);
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
            }
            SneakObj.SetActive(true);
        }
        else
        {
            SneakObj.SetActive(false);
            PlayerBody.SetActive(true);
        }
        //------------------------------BUG-------------------------------
        //if(PlayerCamera.transform.localRotation != GetComponentInChildren<Camera>().transform.localRotation)
        //{
        //    GetComponentInChildren<Camera>().transform.localRotation = PlayerCamera.transform.localRotation;
        //}
        //if (ViewRotation != PlayerCamera.transform.localRotation)
        //{
        //    PlayerCamera.transform.localRotation = ViewRotation;
        //}
        //----------------------------------------------------------------
    }
    //--------------------------------------------------------------------
    //---------------------User Walk--------------------------------------
    //--------------------------------------------------------------------
    public void SetMove(float h, float v)
    {
        transform.Translate(h* WalkSpeed, 0, v * WalkSpeed);

        X = transform.position.x;
        Y = transform.position.y;
        Z = transform.position.z;
        
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
        }
        var arms = Instantiate(Arms);
        arms.SetActive(true);

        arms.transform.position = transform.position + transform.TransformDirection(new Vector3(0.0045f, 0,0.032f)) ;
        arms.transform.rotation = transform.rotation;
        
        //arms.transform.position = ArmsPoint.transform.position;
        //arms.transform.rotation = PlayerCamera.transform.rotation;
    }
    public void Damage(float dmg)
    {
        Hp -= dmg;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Arms"))
        {
            GameEngine.Instance.Send(Message.ActionDamge, new ActionDmgMessage { UserID = UserID, Dmg = Dmg });
            Destroy(collision.gameObject);
        }
    }
    public IEnumerator Dead()
    {
        while (transform.eulerAngles.z < 80)
        {
            transform.Rotate(Vector3.forward * 1.5f);
            yield return null;
        }
        Destroy(gameObject);
    }
}
