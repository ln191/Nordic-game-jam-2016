using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Grid : MonoBehaviour
{
    public GameObject NodePrefab;
    public int tileWidthCount;
    public int tileHeightCount;
    private GameObject nodeMother;
    public GameObject[,] nodeArray;
    public List<Sprite> tileTypes = new List<Sprite>();
    string[] mapData;

    // Use this for initialization
    void Awake()
    {
        nodeArray = new GameObject[tileHeightCount, tileWidthCount];
        nodeMother = new GameObject("Node Mother");
        mapData = ReadLvLText();
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateGrid()
    {      
        for (int i = 0; i < tileHeightCount; i++)
        {
            for (int j = 0; j < tileWidthCount; j++)
            {
                GameObject gridNode = Instantiate(NodePrefab);
                gridNode.name = "Node: " + j + ", " + i;
                gridNode.transform.position = new Vector2(j, -i);
                gridNode.transform.SetParent(nodeMother.transform);
                Node node = gridNode.transform.GetComponent<Node>();
                node.tileSprite = tileTypes[FindTileType(i, j)];

                int fileType = FindTileType(i, j);

                switch (fileType)
                {
                    case 1:
                        node.unwalkable = true;
                        node.nodeType = NodeType.Dirt;
                        break;
                    case 5:
                        node.nodeType = NodeType.Seed;
                        break;
                    case 7:
                        node.nodeType = NodeType.FinishLevel;
                        break;
                    default:
                        break;
                }

                if (fileType != 7)               
                    node.GetComponent<Animator>().enabled = false;

                node.xPos = j;
                node.yPos = i;

                nodeArray[i, j] = gridNode;
            }
        }
    }
    /// <summary>
    /// Find the numbertype to the specified tile en the grid from a text file
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    private int FindTileType(int i , int j)
    {
        char[] horizontalTiles = mapData[i].ToCharArray();
        int tileIndex = int.Parse(horizontalTiles[j].ToString());
        return tileIndex;

    }

    private string[] ReadLvLText()
    {
        TextAsset levelData = Resources.Load("level") as TextAsset;
        string data = levelData.text.Replace(Environment.NewLine, string.Empty);
        return data.Split('-');
    }
}
