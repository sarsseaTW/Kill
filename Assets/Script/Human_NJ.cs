using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Human_NJ : MonoBehaviour
{
    //--------------------------------------------------------------------
    //---------------------User-------------------------------------------
    //--------------------------------------------------------------------
    public GameObject SneakObj;
    public Camera PlayerCamera;
    public Quaternion ViewRotation;
    public Quaternion Rotation;
    public float X;
    public float Y;
    public float Z;
    public float WalkSpeed = 0.1f;
    public float RotationSpeed = 100f;
    public float Hp = 400;
    public float Dmg = 100;
    public int UserID;
    public bool IsDead => Hp <= 0;
    public bool IsSneak;
    public bool IsJump;
    public string SneakName;
    public float JumpSpeed = 10;
    //--------------------------------------------------------------------
    //---------------------User Sneak-------------------------------------
    //--------------------------------------------------------------------
    public GameObject [] SneakList;
    public GameObject PlayerBody;
    GameObject SneakItem;
    //--------------------------------------------------------------------
    //---------------------User Attack------------------------------------
    //--------------------------------------------------------------------
    public GameObject Arms;
    public GameObject ArmsPoint;
    Vector3 CameraV3;
    //--------------------------------------------------------------------
    //---------------------Bool key---------------------------------------
    //--------------------------------------------------------------------
    bool IsRotation;
    bool IsViewRotation;
    bool IsWalk;
    //--------------------------------------------------------------------
    //---------------------User View--------------------------------------
    //--------------------------------------------------------------------
    float Cam_Y_RotationMax = 90;
    float Cam_Y_RotationMin = -30;
    float Cam_Y_RotationSum = 0;
    //--------------------------------------------------------------------
    //---------------------Init-------------------------------------------
    //--------------------------------------------------------------------
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
        if (IsWalk || IsJump)
        {
            if (IsJump)
            {
                GetComponent<Rigidbody>().velocity += new Vector3(0, 1, 0);
                GetComponent<Rigidbody>().AddForce(Vector3.up * JumpSpeed * Time.deltaTime);
            }
            X = transform.position.x;
            Y = transform.position.y;
            Z = transform.position.z;
            GameEngine.Instance.Send(Message.ActionWalk, new ActionWalk__Massage { UserID = UserID, UserPos = new Vector3(X, Y, Z) });
           
            IsWalk = false;
            IsJump = false;
        }
        if (IsRotation)
        {
            Rotation = transform.rotation;
            GameEngine.Instance.Send(Message.ActionRotation, new ActionRotation__Massage { UserID = UserID, UserRotation = Rotation});
            IsRotation = false;
        }
        if (IsViewRotation)
        {
            PlayerCamera.transform.localEulerAngles = new Vector3(-Cam_Y_RotationSum, 0, 0);
            //ViewRotation = PlayerCamera.transform.localRotation;
            GameEngine.Instance.Send(Message.ActionViewRotation, new ActionViewRotation__Massage { UserID = UserID, UserViewRotation = Cam_Y_RotationSum });
            IsViewRotation = false;
        }
        if (IsSneak)
        {

        }
        if (X != transform.position.x || Y != transform.position.y || Z != transform.position.z)
        {
            SetPos(new Vector3(X, Y, Z));
        }
        if (Rotation != transform.rotation)
        {
            transform.rotation = Rotation;
        }
        //if(ViewRotation != PlayerCamera.transform.localRotation)
        //{
        //    PlayerCamera.transform.localRotation = ViewRotation;
        //}
    }
    //--------------------------------------------------------------------
    //---------------------User Jump--------------------------------------
    //--------------------------------------------------------------------
    public void SetJump()
    {
        IsJump = true;
    }
    //--------------------------------------------------------------------
    //---------------------User Sneak-------------------------------------
    //--------------------------------------------------------------------
    public bool GetSneakStatic()
    {
        return IsSneak;
    }
    public string GetSneakName()
    {
        return SneakName;
    }
    public void SetSneak()
    {
        if (!IsSneak)
        {
            IsSneak = true;

            SneakObj = SneakList[Random.Range(0, 2)];
            PlayerBody.SetActive(false);
            SneakObj.SetActive(true);
            SneakName = SneakObj.name;
            switch (SneakName)
            {
                case "Tree":
                    PlayerCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.04f, CameraV3.z - 0.07f);
                    break;
                case "Rock":
                    PlayerCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.005f, CameraV3.z - 0.04f);
                    break;
            }
        }
    }
    //--------------------------------------------------------------------
    //---------------------User View Rotate-------------------------------
    //--------------------------------------------------------------------
    public Camera getCamera()
    {
        return PlayerCamera;
    }
    public void SetViewXL(){
        transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
        IsRotation = true;
    }
    public void SetViewXR(){
        transform.Rotate(0, -RotationSpeed * Time.deltaTime, 0);
        IsRotation = true;
    }
    public void SetViewY(float Cam_Y)
    {
        Cam_Y_RotationSum += Cam_Y;
        Cam_Y_RotationSum = Mathf.Clamp(Cam_Y_RotationSum, Cam_Y_RotationMin, Cam_Y_RotationMax);
        IsViewRotation = true;
    }
    //--------------------------------------------------------------------
    //---------------------User Walk--------------------------------------
    //--------------------------------------------------------------------
    public void SetPos(Vector3 v3)
    {
        transform.position = v3;
    }
    public void SetMovePositionF()
    {
        transform.Translate(0, 0, WalkSpeed * Time.deltaTime);
        IsWalk = true;
    }
    public void SetMovePositionB()
    {
        transform.Translate(0, 0, -WalkSpeed * Time.deltaTime);
        IsWalk = true;
    }
    public void SetMovePositionL()
    {
        transform.Translate(-WalkSpeed * Time.deltaTime, 0, 0);
        IsWalk = true;
    }
    public void SetMovePositionR()
    {
        transform.Translate(WalkSpeed * Time.deltaTime, 0, 0);
        IsWalk = true;
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
        arms.transform.position = ArmsPoint.transform.position;
        arms.transform.rotation = PlayerCamera.transform.rotation;
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
