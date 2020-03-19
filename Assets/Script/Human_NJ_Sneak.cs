using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human_NJ_Sneak : MonoBehaviour
{
    public static bool isSneak;
    public GameObject PlayerBody;
    public GameObject[] SneakList;
    public static GameObject SneakItem;

    public static Camera MainCamera;
    public static Vector3 CameraV3;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        CameraV3 = MainCamera.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isSneak)
            {
                isSneak = true;
                SneakItem = SneakList[Random.Range(0, 2)];
                PlayerBody.SetActive(false);
                SneakItem.SetActive(true);
                switch (SneakItem.name)
                {
                    case "Tree":
                       // MainCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.04f, CameraV3.z - 0.07f);
                        break;
                    case "Rock":
                       // MainCamera.transform.localPosition = new Vector3(CameraV3.x, CameraV3.y + 0.005f, CameraV3.z - 0.04f);
                        break;
                }
            }
        }
    }
}
