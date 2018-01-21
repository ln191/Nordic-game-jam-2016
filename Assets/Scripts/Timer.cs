using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class Timer: MonoBehaviour
{
    public Text text;
    public Player player;
    public Image cooldownPortrait;
    public Text timer;
    public Text scoreTimer;
    public Sprite winSprite;
    public Sprite loseSprite;
    float timeLeft;
    bool activateTimer;
    float timestamp;
    public bool activateScoreTimer;
    public float currentTime;
    public string[] highscores;
    public Text Highscore;
    public GameObject HighscorePanel;
    public GameObject InputfieldPanel;
    private GameObject gamemanager;

    // Use this for initialization
    void Start()
    {
        gamemanager = GameObject.Find("Gamemanager");
        HighscorePanel.SetActive(false);
        player = GameObject.Find("Player").GetComponent<Player>();
        cooldownPortrait = gameObject.GetComponentInChildren<Image>();
        timer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = player.distanceScore + " M";

        if (activateTimer)
        {
            timeLeft -= Time.deltaTime;

            timer.text = Round(timeLeft, 1).ToString();
            if (timeLeft < 0)
            {
                ControlCoolDownUI(false);
            }
        }
        if (activateScoreTimer)
        {
             currentTime = Time.time - timestamp;
            scoreTimer.text = Round(currentTime, 2).ToString();
        }

        if (player.isDead)
        {
            cooldownPortrait.sprite = loseSprite;
        }
        else if (player.hasWon)
        {
            cooldownPortrait.sprite = winSprite;
        }
    }

    public void ControlCoolDownUI(bool activate)
    {
        if (activate)
        {
            cooldownPortrait.color = Color.grey;
            timeLeft = player.eatingCD;
            activateTimer = true;
            timer.enabled = true;
        }
        else
        {
            cooldownPortrait.color = Color.white;
            activateTimer = false;
            timer.enabled = false;
        }
    }

    public void StartEndTimeScore(bool activate)
    {
        if (activate)
        {
            timestamp = Time.time;
            activateScoreTimer = true;
        }
        else
        {
            HighscorePanel.SetActive(true);
            activateScoreTimer = false;
            gamemanager.GetComponent<Gamemanager>().time = Round(currentTime,2);
            //// save score
            
            //scoreTimer.gameObject.AddComponent<SelectWobble>();

            //highscores = ReadHighscreText();

            //for (int i = 0; i < highscores.Length; i++)
            //{
            //    Highscore.text += highscores[i] + "\n";
            //}         
        }
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    //private string[] ReadHighscreText()
    //{
    //    TextAsset highscoreData = Resources.Load("highscore") as TextAsset;
    //    string data = highscoreData.text.Replace(System.Environment.NewLine, string.Empty);
    //    return data.Split('-');
    //}

    //public void WriteToHighscore(string name)
    //{
    //    InputfieldPanel.SetActive(false);

    //    string path = @"Assets/Resources/highscore.txt";

    //    using (StreamWriter sw = File.AppendText(path))
    //    {
    //        sw.WriteLine(name + " " + Round(currentTime, 2) + "-");
    //    }

    //    Highscore.text = string.Empty;
    //    highscores = ReadHighscreText();

    //    for (int i = 0; i < highscores.Length; i++)
    //    {
    //        Highscore.text += highscores[i] + "\n";
    //    }
    //}
}
