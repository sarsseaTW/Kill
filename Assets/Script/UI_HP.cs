﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_HP : MonoBehaviour
{
    public RectTransform HP_Bar, HP_Hurt;
    public GameObject Retry;
    //float hp = 400;
    // Start is called before the first frame update
    void Start()
    {
        Retry.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEngine.Instance.Player.Human_NJ == null)
        {
            return;
        }
        HP_Bar.sizeDelta = new Vector2(GameEngine.Instance.Player.Human_NJ.Hp, HP_Bar.sizeDelta.y);
        if (HP_Hurt.sizeDelta.x > HP_Bar.sizeDelta.x)
        {
            HP_Hurt.sizeDelta += new Vector2(-1, 0) * Time.deltaTime * 30;
        }
        if (GameEngine.Instance.Player.Human_NJ.IsDead)
        {
            Retry.SetActive(true);
        }
        else
        {
            Retry.SetActive(false);
        }
    }
    public void OnClickRetry()
    {
        Debug.Log("aaasda");
        GameEngine.Instance.Init();
    }
    private void OnMouseEnter()
    {
        Debug.Log("OnMouseEnter");
    }

    private void OnMouseExit()
    {
        Debug.Log("OnMouseExit");
    }

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
    }

    private void OnMouseUp()
    {
        Debug.Log("OnMouseUp");
    }
}
