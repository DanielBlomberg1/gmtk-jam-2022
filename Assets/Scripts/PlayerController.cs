using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public struct Enemy
{
    public int hp;
    public int damage;
    public Enemy(int hp, int damage)
    {
        this.hp = hp;
        this.damage = damage;
    }
}

public class PlayerController : MonoBehaviour
{
    private bool isRightState = false;
    [SerializeField] private int currentTileIndex = 0;
    [SerializeField] private int playerHealthMax;
    [SerializeField] private int playerHealthCur;
    [SerializeField] private int playerDamage;
    [SerializeField] private int rerolls;

    [SerializeField] private TextMeshProUGUI rerollText;
    [SerializeField] private TextMeshProUGUI playerHpText;
    [SerializeField] private TextMeshProUGUI playerDmgText;

    [SerializeField] private GameObject damageMarkerContainer;

    private GameManager gameManager;
    private LevelGenerator levelGen;

    public int P_rerolls => rerolls;
    public int P_playerHealthMax => playerHealthMax;
    public int P_playerHealthCur => playerHealthCur;

    private void SetRerollText()
    {
        rerollText.text = "Rerolls left: " + rerolls;
    }
    public void RerollOnce()
    {
        rerolls -= 1;
        SetRerollText();
    }

    private void Combat(Enemy enemy)
    {
        // animations could be here
        int times = playerDamage % enemy.hp;
        if(times == 0) { times = 1; }
        for (int i = 0; i < times; i++)
        {
            playerHealthCur -= enemy.damage;
        }
        StartCoroutine(Delay(times * enemy.damage));
         
        SetPlayerHealthBar();   
    }
    private void Heal()
    {
        if(playerHealthCur != playerHealthMax)
        {
            if(playerHealthCur + 2  >= playerHealthMax)
            {
                playerHealthCur = playerHealthMax;
            }
            else
            {
                playerHealthCur += 2;
            }
        }
    }

    private void CheckTileUnder(Tile tile)
    {
        switch (tile.tilePrefab.name)
        {
            case "Combat":
                Combat(new Enemy(UnityEngine.Random.Range(1,4), UnityEngine.Random.Range(1, 2)));
                break;
            case "Shop":
                break;
            case "Heal":
                Heal();
                break;
            case "DiceTile":
                break;
            case "Debuff":
                break;
            case "Treasure":
                break;
            case "GoalTile":
                break;
            default:
                break;
        }

        gameManager.PlayerHasAdvanced();
        currentTileIndex += 1;
    }

    public void TryToAdvance()
    {
        if (!isRightState)
        {
            return;
        }

        List<Tile> curPath = levelGen.path;

        if (curPath.Count > currentTileIndex)
        {
            float diffX = curPath[currentTileIndex + 1].position.x - curPath[currentTileIndex].position.x;
            float diffZ = curPath[currentTileIndex + 1].position.y - curPath[currentTileIndex].position.y;

            Vector3 newPos = new(diffX + gameObject.transform.position.x, gameObject.transform.position.y, diffZ + gameObject.transform.position.z);
            gameObject.transform.position = newPos;

            CheckTileUnder(curPath[currentTileIndex + 1]);
        }
    }

    private void SetPlayerHealthBar()
    {
        playerHpText.text = "Health: " + playerHealthCur + " / " + playerHealthMax;   
    }
    private void SetPlayerDamageBar()
    {
        playerDmgText.text = "Damage: " + playerDamage;
    }


    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        GameManager.stateChange += GameStateHandler;
        levelGen = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        SetRerollText();
        SetPlayerHealthBar();
    }

    private void GameStateHandler(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.ADVANCE_PLAYER)
        {
            isRightState = true;
        }
        else
        {
            isRightState = false;
        }
    }

    private IEnumerator Delay(int amount)
    {
        damageMarkerContainer.SetActive(true);
        TextMeshProUGUI text  = damageMarkerContainer.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "-" + amount;
        Vector3 v = Camera.main.WorldToScreenPoint(transform.position);
        v.y += 50;
        damageMarkerContainer.transform.position = v;

        yield return new WaitForSeconds(2);
        damageMarkerContainer.SetActive(false);  
    }
}
