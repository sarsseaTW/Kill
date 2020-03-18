using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float fireRate = 1;
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
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            float Walk_X = Input.GetAxis("Horizontal") * WalkSpeed * Time.deltaTime;
            transform.Translate(Walk_X, 0, 0);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            float Walk_Z = Input.GetAxis("Vertical") * WalkSpeed * Time.deltaTime;
            transform.Translate(0, 0, Walk_Z);
        }

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
}
