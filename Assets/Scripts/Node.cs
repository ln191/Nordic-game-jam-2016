using UnityEngine;
using System.Collections;

public enum NodeType
{
    Dirt, Rock, Seed, FinishLevel
}

public class Node : MonoBehaviour
{
    public Sprite tileSprite;
    private SpriteRenderer spriteRenderer;
    public int xPos;
    public int yPos;
    public bool unwalkable = false;
    public bool isSeed = false;
    public bool isFinishTile = false;
    public NodeType nodeType;
	
	void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = tileSprite;
	}
	
    public void UpdateSprite()
    {
        spriteRenderer.sprite = tileSprite;
    }
}
