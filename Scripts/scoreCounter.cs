using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreCounter : MonoBehaviour
{

    public static int score = 80;

    public static int totalScore = 0;

    private TMPro.TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
    }
}
