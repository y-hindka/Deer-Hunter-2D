using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CloudOnce;

public class HighScoresTable : MonoBehaviour
{

    public GameObject container;

    public GameObject template;

    public GameObject fadeIn;

    public GameObject fadeOut;

    public AudioSource audioSrc;

    private bool musicPlaying;

    private static List<TableEntry> tableEntries = new List<TableEntry>();

    private bool tableCreated = false;

    private int soundOn;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fadeInAnimation());
        tableCreated = false;
        template.SetActive(false); // remove example row

        if (!PlayerPrefs.HasKey("high scores"))
        {
            PlayerPrefs.SetString("high scores", JsonUtility.ToJson(new HighScores() { highScores = tableEntries }));
        }

        tableEntries = JsonUtility.FromJson<HighScores>(PlayerPrefs.GetString("high scores")).highScores;

        musicPlaying = false;

        soundOn = PlayerPrefs.GetInt("Sound");

    }

    // Update is called once per frame
    void Update()
    {
        if (!tableCreated)
        {
            sortTableEntries();
            createTable();
            tableCreated = true;
        }

        if (!musicPlaying && soundOn == 1)
        {
            StartCoroutine(playMusic());
        }
    }

    IEnumerator playMusic()
    {
        musicPlaying = true;
        audioSrc.Play();
        yield return new WaitForSeconds(audioSrc.clip.length);
        musicPlaying = false;
    }

    public static void addEntry(int score, string name)
    {
        tableEntries.Add(new TableEntry(score, name));
        // only want to save top 10 entries
        if (tableEntries.Count == 11)
        {
            sortTableEntries();
            tableEntries.RemoveAt(10);
        }

        // save new entries
        PlayerPrefs.SetString("high scores", JsonUtility.ToJson(new HighScores() { highScores = tableEntries }));
        // add highest score to leaderboard
        Leaderboards.HighScore.SubmitScore(tableEntries[0].getScore());
    }

    private void createTable()
    {
        int height = 0;
        int rank = 1;
        for (int i = tableEntries.Count - 1; i >= 0; i--)
        {
            GameObject row = Instantiate(template, container.GetComponent<RectTransform>());
            row.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, height);

            row.GetComponent<RectTransform>().Find("Rank").GetComponent<TMPro.TextMeshProUGUI>().text = rank.ToString();
            row.GetComponent<RectTransform>().Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = tableEntries[i].getName();
            row.GetComponent<RectTransform>().Find("Score").GetComponent<TMPro.TextMeshProUGUI>().text = tableEntries[i].getScore().ToString();

            row.SetActive(true);
            rank++;
            height -= 30;
        }
    }

    // selection sort
    private static void sortTableEntries()
    {
        for (int i = 0; i < tableEntries.Count; i++)
        {
            for (int j = i+1; j < tableEntries.Count; j++)
            {
                if (tableEntries[j].getScore() < tableEntries[i].getScore())
                {
                    // swap elements
                    TableEntry temp = tableEntries[j];
                    tableEntries[j] = tableEntries[i];
                    tableEntries[i] = temp;
                }
            }
        }
    }

    public static bool checkNewHighScore(int score)
    {
        sortTableEntries();
        if (tableEntries.Count == 0 || score > tableEntries[tableEntries.Count - 1].getScore())
        {
            return true;
        }
        return false;
    }

    IEnumerator fadeInAnimation()
    {
        Animator anim = fadeIn.GetComponent<Animator>();
        anim.enabled = true;
        fadeIn.SetActive(true);
        anim.Play("Start to High Scores (2)");
        yield return new WaitForSeconds(1);
        fadeIn.SetActive(false);
        anim.enabled = false;
    }

    public void menuClicked()
    {
        StartCoroutine(fadeOutAnimation());
    }

    IEnumerator fadeOutAnimation()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("Start");
        ao.allowSceneActivation = false;
        Animator anim = fadeOut.GetComponent<Animator>();
        anim.enabled = true;
        fadeOut.SetActive(true);
        anim.Play("High Scores to Start");
        yield return new WaitForSeconds(1);
        anim.enabled = false;
        ao.allowSceneActivation = true;

    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("Launched");
    }

}

class HighScores
{
    public List<TableEntry> highScores;
}

[System.Serializable]
class TableEntry
{
    [SerializeField]
    private int score;
    [SerializeField]
    private string name;

    public TableEntry(int score, string name)
    {
        this.score = score;
        this.name = name;
    }

    public int getScore()
    {
        return score;
    }

    public string getName()
    {
        return name;
    }
}
