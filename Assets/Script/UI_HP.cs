using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HP : MonoBehaviour
{
    public RectTransform HP_Bar, HP_Hurt;
    float hp = 400;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) hp -= 10;
        HP_Bar.sizeDelta = new Vector2(hp, HP_Bar.sizeDelta.y);
        if (HP_Hurt.sizeDelta.x > HP_Bar.sizeDelta.x)
        {
            HP_Hurt.sizeDelta += new Vector2(-1, 0) * Time.deltaTime * 30;

        }
    }
}
