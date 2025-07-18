using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    public TextMeshProUGUI scoretxt,coinstxt;
    
    public int coins;
    public int score = 0;
    void Start()
    {
        instance = this;


    }
    void Update()
    {
        scoretxt.text = "Score: " + score.ToString();
        coinstxt.text = "Coins: " + coins.ToString();
    }
}
