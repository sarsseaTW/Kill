using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Skill_CloseEff());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator Skill_CloseEff()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
