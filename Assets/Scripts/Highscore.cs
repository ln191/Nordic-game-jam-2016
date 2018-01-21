using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{

    private List<Score> scores = new List<Score>();

    [SerializeField]
    GameObject textInput;
    private bool haveBeenSaved = false;

    public float TimeScore { get; set; }

    /// <summary>
    /// Adds a new score to the score list
    /// </summary>
    /// <param name="name"></param>
    /// <param name="time"></param>
    public void AddScore(string name, float time)
    {
        Score newScore = new Score(name, time);
        scores.Add(newScore);
    }
    public void SaveHighScore(List<Score> highscore)
    {
        highscore.Sort();
        for (int i = 0; i < highscore.Count; i++)
        {
            PlayerPrefs.SetString("ScoreName" + i, highscore[i].Navn);
            PlayerPrefs.SetFloat("ScoreTime" + i, highscore[i].TimeScore);

        }
        PlayerPrefs.SetInt("AntalScores", highscore.Count);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Loads all scores from PlayerPrefs highscore
    /// </summary>
    public void LoadHighScore()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("AntalScores"); i++)
        {
            Score newScore = new Score(PlayerPrefs.GetString("ScoreName" + i), PlayerPrefs.GetFloat("ScoreTime" + i));
            scores.Add(newScore);
        }

    }
    /// <summary>
    /// Submits a new score to the PlayerPrefs highscore
    /// Use this if you have an text InputField
    /// </summary>
    /// <param name="time"></param>
    public void Submit(GameObject textInput)
    {
        if (!textInput)
            Debug.LogError("No textInput Obj has been assigned to the script");
        else
            AddScore(textInput.GetComponent<InputField>().text, TimeScore);
            SaveHighScore(scores);
            Debug.Log("is saved");
            textInput.SetActive(false);

    }
    /// <summary>
    /// Submits a new score to the PlayerPrefs highscore
    /// Use this to 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="time"></param>
    public void Submit(string name, float time)
    {
        if (!textInput)
            Debug.LogError("No textInput Obj has been assigned to the script");
        else
            AddScore(name, time);
            SaveHighScore(scores);
            Debug.Log("is saved");
            textInput.SetActive(false);

    }
}
