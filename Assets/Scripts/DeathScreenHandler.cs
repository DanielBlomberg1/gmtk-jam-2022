using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathScreenHandler : MonoBehaviour
{
    [SerializeField] GameObject D;
    GameObject GM;
    TextMeshProUGUI dm;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("Game Controller");
        dm.text = GM.GetComponent<GameManager>().deathMessage;
    }
    private void Update()
    {
        D.transform.Rotate(25 * Time.deltaTime, 25 * Time.deltaTime, 0);
    }

    public void ClickYes()
    {
        GameManager gm = GM.GetComponent<GameManager>();
        gm.StartGame();
    }
    public void ClickYES()
    {
        GameManager gm = GM.GetComponent<GameManager>();
        gm.GotoMenu();
    }

}
