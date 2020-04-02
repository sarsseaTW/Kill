using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : MonoBehaviour
{
    float ro;
    // Start is called before the first frame update
    void Start()
    {
        ro = transform.localRotation.x;
    }
    private void FixedUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {
        ro += 1f;
        if (ro >= 20)
        {
            Destroy(gameObject);
        }
        transform.Rotate(ro, 0, 0);
    }
}
