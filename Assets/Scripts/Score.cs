using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Score : MonoBehaviour, IComparable<Score>
{

    private string navn;
    public string Navn
    {
        get { return navn; }
        set { navn = value; }
    }
    private float timeScore;
    public float TimeScore
    {
        get { return timeScore; }
        set { timeScore = value; }
    }
    internal Score(string navn, float timeScore)
    {
        this.navn = navn;
        this.timeScore = timeScore;
    }
    public int CompareTo(Score other)
    {
        if (timeScore < other.timeScore)
        {
            return -1;
        }
        if (timeScore > other.timeScore)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
	
}
