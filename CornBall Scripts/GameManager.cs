using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    public TextMeshProUGUI test;
    public int score = 0;
    void Start()
    {
        instance = this;


    }
    void Update()
    {
        test.text = "Score: " + score.ToString();
    }
}
