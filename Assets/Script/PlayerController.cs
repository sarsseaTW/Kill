using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float fireRate = 0.5f;
    float nextFire = 0;
    public float WalkSpeed = 0.1f;

    public Human_NJ Human_NJ { get; private set; }
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
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            //&& Time.time > nextFire
            nextFire = Time.time + fireRate;
            GameEngine.Instance.Send(Message.ActionShot, new ActionShotMessage { UserID = Human_NJ.UserID });
        }
        if (Input.GetKey(KeyCode.W))
        {
            Human_NJ.SetMovePositionF();
        }
        if (Input.GetKey(KeyCode.S))
        {
            Human_NJ.SetMovePositionB();
        }
        if (Input.GetKey(KeyCode.A))
        {
            Human_NJ.SetMovePositionL();
        }
        if (Input.GetKey(KeyCode.D))
        {
            Human_NJ.SetMovePositionR();
        }
        if (Input.GetAxis("Mouse X") > 0)
        {
            Human_NJ.SetViewXL();
        }
        if (Input.GetAxis("Mouse X") < 0)
        {
            Human_NJ.SetViewXR();
        }
        if(Input.GetAxis("Mouse Y") != 0)
        {
            float ry = Input.GetAxis("Mouse Y") * 3;
            Human_NJ.SetViewY(ry);
        }
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Human_NJ.SetJump();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Human_NJ.SetSneak();
           // GameEngine.Instance.Send(Message.ActionSneak, new Sneak__Message { UserID = Human_NJ.UserID, IsSneak = Human_NJ.IsSneak });
        }
    }
}
