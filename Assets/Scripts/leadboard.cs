using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class leadboard : MonoBehaviour {
    
    private string mytext;
    public Text myGuiText;
	// Use this for initialization
	void Start () {
        LoadHighScore();
        
        myGuiText.enabled = true;
	}
    void LoadHighScore()
    {
        int numOfScore = PlayerPrefs.GetInt("AntalScores");
        for (int i = 0; i < numOfScore; i++)
        {
            if (PlayerPrefs.GetString("ScoreNavn" + i) != null)
            {
                Score readScore = new Score(PlayerPrefs.GetString("ScoreNavn" + i), PlayerPrefs.GetFloat("ScoreTime" + i));

                string mytexttemp = readScore.Navn + " - " + readScore.TimeScore+"\n";
                mytext = mytext + mytexttemp;
            }
            else
            {
                break;
            }
            
            
        }
        myGuiText.text = mytext;
    }
}
