using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsWalk : MonoBehaviour
{
    public float Speed = 10000f;
    public float Duration = 3;

    float createdTime_;
    // Start is called before the first frame update
    void Start()
    {
        createdTime_ = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0,0,10) * Speed );
        if (createdTime_ + Duration < Time.time)
        {
            Destroy(gameObject);
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    Destroy(gameObject);
    //}
}
