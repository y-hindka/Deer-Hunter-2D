using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuButtonScript : MonoBehaviour, IPointerDownHandler

{
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        // do nothing if new high score window active
        if (TimerScript.newHighScoreActive) { }
        else
        {
            if (SceneManager.GetActiveScene().name.Equals("Level 1"))
            {
                PlayerPrefs.SetInt("Coins", CoinScript.totalCoinCount);
                totalScoreScript.tempTimeHolder = (int)TimerScript.time;
                totalScoreScript.tempRetriesHolder = TimerScript.retriesLeft;
                totalScoreScript.tempLevelHolder = levelScript.level;
            }
        }

    }

}
