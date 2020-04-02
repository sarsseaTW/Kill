using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Action : MonoBehaviour
{
    float WalkSpeed = 0.02f;
    int DestroyTime = 0;
    GameObject NowObj;
    GameObject SneakObj;
    public GameObject body;
    public GameObject[] SneakList;
    bool tf;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().freezeRotation = true;

        NowObj = SneakList[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (DestroyTime % 60 == 0 && DestroyTime<3000)
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    {
                        //Debug.Log("Walk");
                        int F = 100;
                        while (F > 0)
                        {
                            transform.GetComponent<Rigidbody>().AddForce(transform.forward * WalkSpeed, ForceMode.Impulse);
                            F -= 1;
                        }
                    }
                    break;
                case 1:
                    {
                        //Debug.Log("Rotate");
                        var ro = transform.localRotation.x;
                        int F = Random.Range(0,90);
                        while (F > 0)
                        {
                            ro += 1f;
                            if (ro >= 360) ro = 0;
                            transform.Rotate(0, ro, 0);
                            F -= 1;
                        }
                    }
                    break;
                case 2:
                    {
                        //Debug.Log("Jump");
                        transform.GetComponent<Rigidbody>().velocity += new Vector3(0, 0.5f, 0);
                        transform.GetComponent<Rigidbody>().AddForce(transform.up * 0.5f, ForceMode.Impulse);
                        transform.GetComponent<Rigidbody>().AddForce(transform.forward * 1, ForceMode.Impulse);
                    }
                    break;
                case 3:
                    {
                        //Debug.Log("Sneak");
                        SneakObj = SneakList[Random.Range(0, 8)];

                        NowObj.SetActive(false);
                        SneakObj.SetActive(true);
                        NowObj = SneakObj;
                        SneakObj.SetActive(false);
                        NowObj.SetActive(true);
                    }
                    break;
            }
        }
        DestroyTime++;
        if (DestroyTime == 3000)
        {
            var arms = Instantiate(body);
            StartCoroutine(Dead());
            arms.SetActive(true);
            var x = (float)(-0.1 + Random.Range(0, 0.2f));
            var z = (float)(-0.1 + Random.Range(0, 0.2f));
            arms.transform.position = new Vector3(x, 1.5f, z);
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
