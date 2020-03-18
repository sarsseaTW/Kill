using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human_NJ_Fire : MonoBehaviour
{
    public float fireRate = 1;
    float nextFire = 0;

    public GameObject Arms;
    public GameObject ArmsPoint;
    public GameObject PlayerBody;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            if (Human_NJ_Sneak.isSneak)
            {
                Human_NJ_Sneak.isSneak = false;
                Human_NJ_Sneak.MainCamera.transform.localPosition = Human_NJ_Sneak.CameraV3;
                PlayerBody.SetActive(true);
                Human_NJ_Sneak.SneakItem.SetActive(false);
            }
            
            nextFire = Time.time + fireRate;
            var createPoint = ArmsPoint.transform.position;
            GameObject Arms_Clone = Instantiate(Arms, createPoint, Human_NJ_Sneak.MainCamera.transform.rotation) as GameObject;

        }
    }
}
