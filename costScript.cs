using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class costScript : MonoBehaviour
{

    public GameObject gunCost;

    public GameObject bulletCost;

    public static int[] gunCosts;

    public static int[] bulletCosts;

    // Start is called before the first frame update
    void Start()
    {
        gunCosts = new int[] { 0, 50, 150, 300 };
        bulletCosts = new int[] { 0, 25, 75, 150 };
    }

    // Update is called once per frame
    void Update()
    {
        gunCost.GetComponent<TMPro.TextMeshProUGUI>().text = "Cost: " + gunCosts[SelectGunScript.index % 4];
        bulletCost.GetComponent<TMPro.TextMeshProUGUI>().text = "Cost: " + bulletCosts[SelectBulletScript.bulletIndex % 4];
    }
}
