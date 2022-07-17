using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VectorGraphics;

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
    [SerializeField] private int playerMoney;
    [SerializeField] private int rerolls;

    [SerializeField] private TextMeshProUGUI rerollText;
    [SerializeField] private TextMeshProUGUI playerHpText;
    [SerializeField] private TextMeshProUGUI playerDmgText;
    [SerializeField] private TextMeshProUGUI playerMoneyText;


    [SerializeField] private GameObject damageMarkerContainer;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject backdrop;
    [SerializeField] private GameObject debuffContainer;
    [SerializeField] private Sprite[] curseList;
    private string currentDebuffName = "";
    private int fireStacks = 0;

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
        int times = 0;
        int enemyHp = enemy.hp;

        while (enemyHp > 0)
        {
            int r = UnityEngine.Random.Range(0, 101);
            if(r <= (currentDebuffName == "Wagner's Tentacle" ? 75 : 100))
            {
                enemyHp -= playerDamage;
            }
            times++;
        }


        if(times == 0) { times = 1; }
        for (int i = 0; i < times; i++)
        {
            playerHealthCur -= enemy.damage;
        }
        StartCoroutine(Delay(times * enemy.damage));
         
        UpdateHealthText();   
        if(playerHealthCur > 0)
        {
            playerMoney += UnityEngine.Random.Range(1, 4);
            UpdateMoneyText();
        }
    }
    private void Buy(int money, int health, int damage, int re)
    {
        if(playerMoney >= money)
        {
            playerHealthMax += health;
            playerHealthCur += health;
            playerDamage += damage;
            rerolls += re;
            UpdateDamageText();
            UpdateHealthText();
            SetRerollText();
            ToggleShop();
            AdvancePlayer();
        }
    }

    public void ShopOnClick(int whichButton)
    {
        switch (whichButton)
        {
            case 1:
                Buy(8, 0, 3, 0);
                break;
            case 2:
                Buy(7, 4, 0, 0);
                break;
            case 3:
                Buy(4, 0, 0, 2);
                break;
            case 4:
                ToggleShop();
                AdvancePlayer();
                break;
        }
    }

    private void ToggleShop()
    {
        backdrop.SetActive(!shop.activeInHierarchy);
        shop.SetActive(!shop.activeInHierarchy);
    }
    private void Heal()
    {
        int healAmount = currentDebuffName == "Voodoo doll" ? 2 : 3;
        if(playerHealthCur != playerHealthMax)
        {
            if(playerHealthCur + healAmount >= playerHealthMax)
            {
                playerHealthCur = playerHealthMax;
            }
            else
            {
                playerHealthCur += healAmount;
            }
        }
    }
    private void DebuffRoll()
    {
        int randomCurse = UnityEngine.Random.Range(0, 3); 
        if(randomCurse == 0)
        {
            currentDebuffName = "Nevereding fire";
            debuffContainer.SetActive(true);
            debuffContainer.GetComponent<SVGImage>().sprite = curseList[randomCurse];
            debuffContainer.GetComponentInChildren<TextMeshProUGUI>().text = currentDebuffName;
            debuffContainer.GetComponent<ToolTipInterfaces>().TextToShow = "Deals damage to the player every 4 advances";
        }else if (randomCurse == 1)
        {
            currentDebuffName = "Wagner's Tentacle";
            debuffContainer.SetActive(true);
            debuffContainer.GetComponent<SVGImage>().sprite = curseList[randomCurse];
            debuffContainer.GetComponentInChildren<TextMeshProUGUI>().text = currentDebuffName;
            debuffContainer.GetComponent<ToolTipInterfaces>().TextToShow = "Reduces the players chance to hit enemies by 25%";
        }else if(randomCurse == 2)
        {
            currentDebuffName = "Voodoo doll";
            debuffContainer.SetActive(true);
            debuffContainer.GetComponent<SVGImage>().sprite = curseList[randomCurse];
            debuffContainer.GetComponentInChildren<TextMeshProUGUI>().text = currentDebuffName;
            debuffContainer.GetComponent<ToolTipInterfaces>().TextToShow = "While active the player heals 1 less from healing tiles.";
        }
        
    }

    private void AdvancePlayer()
    {
        gameManager.PlayerHasAdvanced();
        currentTileIndex += 1;
        if(currentDebuffName == "Nevereding fire")
        {
            fireStacks += 1;
            if (fireStacks % 4 == 0)
            {
                playerHealthCur -= 1;
                UpdateHealthText();
            }
        }
    }

    private void CheckTileUnder(Tile tile)
    {
        switch (tile.tilePrefab.name)
        {
            case "Combat":
                Combat(new Enemy(UnityEngine.Random.Range(1,4), UnityEngine.Random.Range(1, 2)));
                AdvancePlayer();
                break;
            case "Shop":
                ToggleShop();
                break;
            case "Heal":
                Heal();
                AdvancePlayer();
                break;
            case "DiceTile":
                AdvancePlayer();
                break;
            case "Debuff":
                DebuffRoll();
                AdvancePlayer();
                break;
            case "Treasure":
                AdvancePlayer();
                break;
            case "GoalTile":
                AdvancePlayer();
                break;
            default:
                AdvancePlayer();
                break;
        }
    }

    public void TryToAdvance()
    {
        if (!isRightState)
        {
            return;
        }

        List<Tile> curPath = levelGen.path;

        if (curPath.Count - 1 > currentTileIndex)
        {
            float diffX = curPath[currentTileIndex + 1].position.x - curPath[currentTileIndex].position.x;
            float diffZ = curPath[currentTileIndex + 1].position.y - curPath[currentTileIndex].position.y;

            Vector3 newPos = new(diffX + gameObject.transform.position.x, gameObject.transform.position.y, diffZ + gameObject.transform.position.z);
            gameObject.transform.position = newPos;

            CheckTileUnder(curPath[currentTileIndex + 1]);
        }
        else
        {
            Debug.Log("Finished");
        }
    }

    private void UpdateHealthText()
    {
        playerHpText.text = "Health: " + playerHealthCur + " / " + playerHealthMax;   
    }
    private void UpdateDamageText()
    {
        playerDmgText.text = "Damage: " + playerDamage;
    }

    private void UpdateMoneyText()
    {
        playerMoneyText.text = "Money: " + playerMoney;
    }

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        GameManager.stateChange += GameStateHandler;
        levelGen = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        SetRerollText();
        UpdateHealthText();
        UpdateDamageText();
        UpdateMoneyText();
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
