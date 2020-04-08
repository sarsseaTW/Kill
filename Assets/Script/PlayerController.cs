using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float fireRate = 1f;
    float nextFire = 0;
    float swordRate = 1f;
    float nextSword = 0;

    float jumpRate = 0.8f;
    float nextJump = 0;

    float DobjumpRate = 1f;
    float nextDobJump = 0;
    bool IsWalkStop;

    float SneakRate = 3;
    float nextSneak = 0;

    static public bool IsSkill;

    public Human_NJ Human_NJ { get; private set; }
    public void Init(Human_NJ human_nj)
    {
        Human_NJ = human_nj;
        IsSkill = false;
    }
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (Human_NJ == null) return;
        if (!IsSkill)
        {
            if (Input.GetButton("Fire2") && Time.time - nextSword > swordRate)
            {
                nextSword = Time.time;
                GameEngine.Instance.Send(Message.ActionSword, new ActionSwordMessage { UserID = Human_NJ.UserID });
            }
            if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                GameEngine.Instance.Send(Message.ActionShot, new ActionShotMessage { UserID = Human_NJ.UserID });
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                Human_NJ.SetMove(h, v);
                Human_NJ.SetIsWalkStop(false);
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
                {
                    IsWalkStop = true;
                }
                else IsWalkStop = false;
                Human_NJ.SetIsWalkStop(IsWalkStop);
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
                {
                    IsWalkStop = true;
                }
                else IsWalkStop = false;
                Human_NJ.SetIsWalkStop(IsWalkStop);
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
                {
                    IsWalkStop = true;
                }
                else IsWalkStop = false;
                Human_NJ.SetIsWalkStop(IsWalkStop);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A)))
                {
                    IsWalkStop = true;
                }
                else IsWalkStop = false;
                Human_NJ.SetIsWalkStop(IsWalkStop);
            }
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                float rx = Input.GetAxis("Mouse X") * 3;
                float ry = Input.GetAxis("Mouse Y") * 3;
                Human_NJ.SetView(rx, ry);
            }
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextJump)
            {
                nextJump = Time.time + jumpRate;
                Human_NJ.SetJump();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextDobJump)
                {
                    nextDobJump = Time.time + DobjumpRate;
                    Human_NJ.SetDobJump();
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) && Time.time - nextSneak > SneakRate)
            {
                nextSneak = Time.time;
                Human_NJ.SetSneak();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Human_NJ.Hp <= 50)
                {
                    return;
                }
                IsSkill = true;
                Human_NJ.skillCAM();
                GameEngine.Instance.Send(Message.ActionSkill, new ActionSkillMessage { UserID = Human_NJ.UserID });
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (Human_NJ.Hp <= 100)
                {
                    return;
                }
                IsSkill = true;
                Human_NJ.skill2CAM();
                GameEngine.Instance.Send(Message.ActionSkill2, new ActionSkill2Message { UserID = Human_NJ.UserID });
            }
        }
    }
}
