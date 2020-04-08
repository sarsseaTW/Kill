using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Action : MonoBehaviour
{
    public int NPC_ID;
    public float X;
    public float Y;
    public float Z;
    public int Action;

    //-----------------
    float WalkSpeed = 1f;
    int DestroyTime = 0;

    public GameObject[] SneakList;
    bool tf;
    int number = 0;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        X = transform.position.x;
        Y = transform.position.y;
        Z = transform.position.z;
    }
    public void GetAction(int action)
    {
        switch (action)
        {
            case 0:
                {
                   // Debug.Log(NPC_ID + "  Walk");
                    transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    transform.GetComponent<Rigidbody>().AddForce(transform.forward * WalkSpeed, ForceMode.Impulse);
                }
                break;
            case 1:
                {
                   // Debug.Log(NPC_ID + "  Rotate");
                    transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    transform.Rotate(0, 45, 0);
                }
                break;
            case 2:
                {
                    //Debug.Log(NPC_ID + "  Jump");
                    transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    transform.GetComponent<Rigidbody>().velocity += new Vector3(0, 0.5f, 0);
                    transform.GetComponent<Rigidbody>().AddForce(transform.up * 0.5f, ForceMode.Impulse);
                    transform.GetComponent<Rigidbody>().AddForce(transform.forward * 1, ForceMode.Impulse);
                }
                break;
            case 3:
                {
                    //Debug.Log(NPC_ID + "  Sneak");
                    transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    SneakList[number].SetActive(false);
                    number = Random.Range(0, 10);
                    SneakList[number].SetActive(true);
                }
                break;
        }

    }
    public Vector3 setPos()
    {
        return new Vector3(X, Y, Z);
    }
}
