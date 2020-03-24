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
    bool isDobCli = false;
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
        //if (Input.GetKey(KeyCode.W))
        //{
        //    Human_NJ.SetMovePositionF();
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    Human_NJ.SetMovePositionB();
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    Human_NJ.SetMovePositionL();
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    Human_NJ.SetMovePositionR();
        //}
        //if(Input.GetAxis("Mouse X") > 0)
        //{
        //    Human_NJ.SetViewXL();
        //}
        //if(Input.GetAxis("Mouse X") < 0)
        //{
        //    Human_NJ.SetViewXR();
        //}

        if (_frameCountWalk % 3 == 0)
        {
            if (Input.GetAxis("Mouse X") > 0)
            {
                GameEngine.Instance.Send(Message.ActionView_XL, new ViewXL__Message { UserID = Human_NJ.UserID });
            }
            if (Input.GetAxis("Mouse X") < 0)
            {
                GameEngine.Instance.Send(Message.ActionView_XR, new ViewXR__Message { UserID = Human_NJ.UserID });
            }
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                GameEngine.Instance.Send(Message.ActionWalk_FL, new WalkFL__Message { UserID = Human_NJ.UserID });
                isDobCli = true;
            }
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                GameEngine.Instance.Send(Message.ActionWalk_FR, new WalkFR__Message { UserID = Human_NJ.UserID });
                isDobCli = true;
            }
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                GameEngine.Instance.Send(Message.ActionWalk_BL, new WalkBL__Message { UserID = Human_NJ.UserID });
                isDobCli = true;
            }
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                GameEngine.Instance.Send(Message.ActionWalk_BR, new WalkBR__Message { UserID = Human_NJ.UserID });
                isDobCli = true;
            }
            if (!isDobCli)
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
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameEngine.Instance.Send(Message.ActionJump, new Jump__Message { UserID = Human_NJ.UserID });
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            GameEngine.Instance.Send(Message.ActionSneak, new Sneak__Message { UserID = Human_NJ.UserID, IsSneak = Human_NJ.IsSneak });
        }
        if (Input.GetButtonDown("Fire1") )
        {
            //&& Time.time > nextFire
            nextFire = Time.time + fireRate;
            GameEngine.Instance.Send(Message.ActionShot, new ActionShotMessage { UserID = Human_NJ.UserID });
        }

        isDobCli = false;
        _frameCountWalk++;
    }
}
