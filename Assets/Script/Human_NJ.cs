using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Human_NJ : MonoBehaviour
{
    public float X;
    public float Y;
    public float Z;
    public float WalkSpeed = 0.5f;
    public float RotationSpeed = 100f;
    public float Hp = 400;
    public float Dmg = 100;
    public int UserID;
    public bool IsDead => Hp <= 0;
    public bool IsSneak;
    public string SneakName;
    public bool IsJump;
    public float JumpSpeed = 10;
    Quaternion _TargetQuaternion;
    Vector3 _MovePosition;
    // Start is called before the first frame update
    public GameObject [] SneakList;
    public GameObject PlayerBody;
    public GameObject Arms;
    public GameObject ArmsPoint;
    GameObject SneakItem;
    Camera MainCamera;
    Vector3 CameraV3;
    void Start()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
        MainCamera = Camera.main;
        CameraV3 = MainCamera.transform.localPosition;
    }
    private void FixedUpdate()
    {
        if (IsDead) { return; }
        if (IsJump)
        {
            GetComponent<Rigidbody>().velocity += new Vector3(0, 1, 0);
            GetComponent<Rigidbody>().AddForce(Vector3.up * JumpSpeed * Time.deltaTime);
            IsJump = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (IsDead) { return; }


        X = transform.position.x;
        Y = transform.position.y;
        Z = transform.position.z;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Arms"))
        {
            GameEngine.Instance.Send(Message.ActionDamge, new ActionDmgMessage { UserID = UserID, Dmg = Dmg });
            Destroy(collision.gameObject);
        }
    }
    public Vector3 UpdatePos()
    {
        return transform.position;
    }
    public void SetPos(Vector3 v3)
    {
        transform.position =  v3;
    }
    public bool GetJunp(){
        return IsJump;
    }
    public void SetJump(bool jp){
        IsJump = jp;
    }
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

            SneakItem = SneakList[Random.Range(0, 2)];
            PlayerBody.SetActive(false);
            SneakItem.SetActive(true);
            SneakName = SneakItem.name;
            switch (SneakName)
            {
                case "Tree":
                    MainCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.04f, CameraV3.z - 0.07f);
                    break;
                case "Rock":
                    MainCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.005f, CameraV3.z - 0.04f);
                    break;
            }
        }
    }
    public void SetViewXL(){
        transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
    }
    public void SetViewXR(){
        transform.Rotate(0, -RotationSpeed * Time.deltaTime, 0);
    }
    public void SetMovePositionF()
    {
        transform.Translate(0, 0, WalkSpeed * Time.deltaTime);
    }
    public void SetMovePositionB()
    {
        transform.Translate(0, 0, -WalkSpeed * Time.deltaTime);
    }
    public void SetMovePositionL()
    {
        transform.Translate(-WalkSpeed * Time.deltaTime, 0, 0);
    }
    public void SetMovePositionR()
    {
        transform.Translate(WalkSpeed * Time.deltaTime, 0, 0);
    }
    public void SetMovePositionFL()
    {
        transform.Translate(-WalkSpeed * Time.deltaTime, 0, WalkSpeed * Time.deltaTime);
    }
    public void SetMovePositionFR()
    {
        transform.Translate(WalkSpeed * Time.deltaTime, 0, WalkSpeed * Time.deltaTime);
    }
    public void SetMovePositionBL()
    {
        transform.Translate(-WalkSpeed * Time.deltaTime, 0, -WalkSpeed * Time.deltaTime);
    }
    public void SetMovePositionBR()
    {
        transform.Translate(WalkSpeed * Time.deltaTime, 0, -WalkSpeed * Time.deltaTime);
    }
    public void Shot()
    {
        if (IsSneak)
        {
            IsSneak = false;
            MainCamera.transform.localPosition = CameraV3;
            PlayerBody.SetActive(true);
            SneakItem.SetActive(false);
        }
        var arms = Instantiate(Arms);
        arms.SetActive(true);
        arms.transform.position = ArmsPoint.transform.position;
        arms.transform.rotation = MainCamera.transform.rotation;
    }
    public void Damage(float dmg)
    {
        Hp -= dmg;
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
