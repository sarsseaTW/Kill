using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float fireRate = 1;
    float nextFire = 0;
    public float WalkSpeed = 0.1f;

    public Human_NJ Human_NJ { get; private set; }
    int _frameCountWalk;
    public void Init(Human_NJ human_nj)
    {
        Human_NJ = human_nj;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Human_NJ == null) return;
        if (_frameCountWalk % 4 == 0)
        {
            if (Input.GetKey(KeyCode.W))
            {
                GameEngine.Instance.Send(Message.ActionWalk_F, new WalkF__Message { UserID = Human_NJ.UserID });
            }
            if (Input.GetKey(KeyCode.S))
            {
                GameEngine.Instance.Send(Message.ActionWalk_B, new WalkB__Message { UserID = Human_NJ.UserID });
            }
            if (Input.GetKey(KeyCode.A))
            {
                GameEngine.Instance.Send(Message.ActionWalk_L, new WalkL__Message { UserID = Human_NJ.UserID });
            }
            if (Input.GetKey(KeyCode.D))
            {
                GameEngine.Instance.Send(Message.ActionWalk_R, new WalkR__Message { UserID = Human_NJ.UserID });
            }
        }
        _frameCountWalk++;
        
        //if (Input.GetKey(KeyCode.W))
        //{
        //    var move = Human_NJ.transform.TransformDirection(Vector3.forward);
        //    Human_NJ.SetMovePosition(Human_NJ.transform.position + move);
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    var move = Human_NJ.transform.TransformDirection(-Vector3.forward);
        //    Human_NJ.SetMovePosition(Human_NJ.transform.position + move);
        //}

        //if (Input.GetKeyDown(KeyCode.Space) && nextFire + fireRate < Time.time)
        //{
        //    nextFire = Time.time;
        //    GameEngine.Instance.Send(Message.ActionShot, new ActionShotMessage { UserID = Human_NJ.UserID });
        //}
        //if (Input.GetKeyDown(KeyCode.LeftControl))
        //{
        //    if (!isSneak)
        //    {
        //        isSneak = true;
        //        SneakItem = SneakList[Random.Range(0, 2)];
        //        PlayerBody.SetActive(false);
        //        SneakItem.SetActive(true);
        //        switch (SneakItem.name)
        //        {
        //            case "Tree":
        //                MainCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.04f, CameraV3.z - 0.07f);
        //                break;
        //            case "Rock":
        //                MainCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.005f, CameraV3.z - 0.04f);
        //                break;
        //        }
        //    }
        //}
    }
    public void UpdatePos(Vector3 v3)
    {
        Human_NJ.X = v3.x;
        Human_NJ.Y = v3.y;
        Human_NJ.Z = v3.z;
    }
}
