using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Des());
    }

    // Update is called once per frame
    void Update()
    {
    }
    public IEnumerator Des()
    {
        int f = 20;
        while (f > 0)
        {
            var p = transform.parent;
            transform.RotateAround(p.transform.position, Vector3.up, 20);
            f--;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
}
