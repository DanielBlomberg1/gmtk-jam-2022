using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathScreenHandler : MonoBehaviour
{
    [SerializeField] GameObject D;
    [SerializeField] TextMeshProUGUI t;
    private string[] basicEnemyNames = new string[] { "A baby Goblin", "Goblin duo", "A singular Ant", "An apple falling from a tree", "Hitting your toe on a rock", "A local squarrel on cocaine" };
    private string[] bossEnemyNames = new string[] { "Alexstraszszaa the Life-Binder", "Sherk from Srerk", "Big bad wolf", "Big ass carcossonne dude" };


    void Start()
    {
        
        if(Random.Range(0, 2) == 0)
        {
            t.text = "You were slain by " + basicEnemyNames[Random.Range(0, basicEnemyNames.Length)] + ". Would you like to Try again?";
        }
        else
        {
            t.text = "You were slain by " + bossEnemyNames[Random.Range(0, bossEnemyNames.Length)] + ". Would you like to Try again?";
        }
        
    }
    private void Update()
    {
        D.transform.Rotate(25 * Time.deltaTime, 25 * Time.deltaTime, 0);
    }

}
