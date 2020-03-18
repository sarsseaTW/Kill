using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewController : MonoBehaviour
{
    public float Cam_Y_RotationMax = 90;
    public float Cam_Y_RotationMin = -30;
    public float Cam_Y_RotationSum = 0;
    //public float Cam_X_RotationSum = 0;
    // Start is called before the first frame update
    void Start()
    {
       // GetComponent<Rigidbody>().freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
       // Cam_X_RotationSum += Input.GetAxis("Mouse X") * 3;
       // Cam_X_RotationSum = Mathf.Clamp(Cam_X_RotationSum, -45, 45);
        Cam_Y_RotationSum += Input.GetAxis("Mouse Y") * 3;
        Cam_Y_RotationSum = Mathf.Clamp(Cam_Y_RotationSum, Cam_Y_RotationMin, Cam_Y_RotationMax);
        transform.localEulerAngles = new Vector3(-Cam_Y_RotationSum, 0, 0);

        //transform.localEulerAngles = new Vector3(-Cam_Y_RotationSum, Cam_X_RotationSum, 0);
    }
}
