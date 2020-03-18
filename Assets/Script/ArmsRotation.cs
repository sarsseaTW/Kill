using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsRotation : MonoBehaviour
{
    public float ro;
    // Start is called before the first frame update
    void Start()
    {
        ro = transform.localRotation.x;
    }

    // Update is called once per frame
    void Update()
    {
        ro += 1f;
        if (ro >= 360) ro = 0;
        transform.Rotate(0, ro, 0);
    }
}
