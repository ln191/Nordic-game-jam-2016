using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Gamemanager : MonoBehaviour
{

    private List<Score> scores = new List<Score>();
    public float time;
 
    // Use this for initialization
    void Start()
    {
        LoadHighScore();  
    }

    void AddScore(string navn, float time)
    {
        Score newScore = new Score(navn, time);
        scores.Add(newScore);
    }
    void SaveHighScore(List<Score> highscore)
    {
        highscore.Sort();
        for (int i = 0; i < highscore.Count; i++)
        {
            PlayerPrefs.SetString("ScoreNavn" + i, highscore[i].Navn);
            PlayerPrefs.SetFloat("ScoreTime" + i, highscore[i].TimeScore);

        }
        PlayerPrefs.SetInt("AntalScores", highscore.Count);
        PlayerPrefs.Save();
    }
    void LoadHighScore()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("AntalScores"); i++)
        {

            Score newScore = new Score(PlayerPrefs.GetString("ScoreNavn" + i), PlayerPrefs.GetFloat("ScoreTime" + i));

            scores.Add(newScore);

        }

    }
    public void Submit(GameObject textInput)
    {
        AddScore(textInput.GetComponent<InputField>().text, time);
        SaveHighScore(scores);
        Debug.Log("is saved");
        textInput.SetActive(false);
    }
    
}
