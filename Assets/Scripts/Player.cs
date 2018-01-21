using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SpecifyDirection
{
    Continue, Up, Down, Right, Left
}

public class Player : MonoBehaviour
{
    public int distanceTraveled;
    public int distanceScore;
    public Grid grid;
    public List<Node> visitedNodes;
    Node currentNode;
    [SerializeField]
    private float baseSpeed = 3f;
    
    private float speed;
    public float seedBoost = 0.5f;
    public float returnSpeedMultiplier = 8;
    public bool isDead;
    public bool hasWon;
    public GameObject MainCamera;
    public bool gameShouldPlay;
    public GameObject currentTargetNode;
    public SpriteRenderer spriteRenderer;
    public GameObject LavaPrefab;
    public GameObject PartialPrefab;
    public Sprite damagedSprite;
    public Sprite winSprite;
    private Sprite originalSprite;
    public Sprite eatingSprite;
    private bool isEating;
    public Timer timer;

    SpecifyDirection previousDirection = SpecifyDirection.Down;
    SpecifyDirection queuedDirection = SpecifyDirection.Down;
    private float fireEyesCD = 0.5f;
    public float eatingCD = 4.0f;
    private float eatingTime = 0.5f;
    public bool playerIsOnCD;


    // Use this for initialization
    void Start()
    {
        speed = baseSpeed;
        visitedNodes = new List<Node>();
        grid = GameObject.Find("GridManager").GetComponent<Grid>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        distanceScore = grid.tileHeightCount;
        MainCamera = GameObject.Find("Main Camera");
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || hasWon)
        {
            WinLoseGame();
        }
        else if (gameShouldPlay)
        {
            GetInput();
            UpdateDirect();

            if (!spriteRenderer.isVisible)
            {
                if (!isDead && !hasWon)
                {
                    isDead = true;
                    MainCamera.GetComponent<CamaraFollow>().hasLost = true;
                }
            }
        }
    }


    public void SpawnPlayer()
    {
        spriteRenderer.enabled = true;
        gameShouldPlay = true;
        int tileStart = grid.tileWidthCount / 2;
        transform.position = grid.nodeArray[0, tileStart].transform.position;

        Node tmpNode = grid.nodeArray[0, tileStart].GetComponent<Node>();
        tmpNode.tileSprite = grid.tileTypes[2];
        tmpNode.UpdateSprite();

        currentTargetNode = grid.nodeArray[1, tileStart];
        visitedNodes.Add(currentTargetNode.GetComponent<Node>());
        timer.StartEndTimeScore(true);
    }

    void GetInput()
    {

        if (Input.GetKeyDown(KeyCode.D) && queuedDirection != SpecifyDirection.Left && queuedDirection != SpecifyDirection.Right)
        {
            previousDirection = queuedDirection;
            queuedDirection = SpecifyDirection.Right;
        }
        else if (Input.GetKeyDown(KeyCode.A) && queuedDirection != SpecifyDirection.Right && queuedDirection != SpecifyDirection.Left)
        {
            previousDirection = queuedDirection;
            queuedDirection = SpecifyDirection.Left;
        }
        else if (Input.GetKeyDown(KeyCode.W) && queuedDirection != SpecifyDirection.Down && queuedDirection != SpecifyDirection.Up)
        {
            previousDirection = queuedDirection;
            queuedDirection = SpecifyDirection.Up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && queuedDirection != SpecifyDirection.Up && queuedDirection != SpecifyDirection.Down)
        {
            previousDirection = queuedDirection;
            queuedDirection = SpecifyDirection.Down;
        }

        if (Input.GetKey(KeyCode.Space) && !playerIsOnCD && !isEating)
        {
            isEating = true;
            playerIsOnCD = true;
            spriteRenderer.sprite = eatingSprite;
            StartCoroutine(NoLongerHungry());
        }
    }

    void UpdateDirect()
    {
        //Apply input as direct velocity
        transform.position = Vector2.MoveTowards(transform.position, currentTargetNode.transform.position, speed * Time.deltaTime);

        if (transform.position == currentTargetNode.transform.position)
        {
            currentNode = currentTargetNode.GetComponent<Node>();

            if (currentNode.nodeType == NodeType.Seed)
            {
                speed += seedBoost;
                iTween.ShakePosition(MainCamera, new Vector3(0.1f, 0.1f), 0.4f);
                spriteRenderer.sprite = winSprite;
                StartCoroutine(ChangeBackPlayer());
            }

            if (currentNode.unwalkable && !isEating)
            {
                iTween.ShakePosition(MainCamera, new Vector3(0.3f, 0.3f), 0.5f);
                timer.activateScoreTimer = false;
                isDead = true;
                MainCamera.GetComponent<CamaraFollow>().hasLost = true;
                spriteRenderer.sprite = damagedSprite;
                return;
            }
            else if (currentNode.unwalkable && isEating && currentNode.nodeType == NodeType.Rock)
            {
                iTween.ShakePosition(MainCamera, new Vector3(0.6f, 0.6f), 0.8f);
                currentNode.tileSprite = grid.tileTypes[2];
                currentNode.UpdateSprite();
                currentNode.unwalkable = true;
            }

            if (currentNode.nodeType == NodeType.FinishLevel)
            {
                timer.StartEndTimeScore(false);
                hasWon = true;
                MainCamera.GetComponent<CamaraFollow>().hasLost = true;
                spriteRenderer.sprite = winSprite;
                return;
            }
            else if (currentNode.nodeType == NodeType.Dirt)
            {
                currentNode.tileSprite = grid.tileTypes[2];
                currentNode.UpdateSprite();
                currentNode.unwalkable = true;
            }


            visitedNodes.Add(currentNode);
            currentTargetNode = FindDirectionNode();
            CalculateTurnSprite();
            CalculateDistance();
        }
    }

    public IEnumerator ChangeBackPlayer()
    {
        yield return new WaitForSeconds(fireEyesCD);
        spriteRenderer.sprite = originalSprite;
    }

    public IEnumerator NoLongerHungry()
    {
        yield return new WaitForSeconds(eatingTime);
        spriteRenderer.sprite = originalSprite;
        isEating = false;
        StartCoroutine(EatingCooldown());
        timer.ControlCoolDownUI(true);
    }

    public IEnumerator EatingCooldown()
    {
        yield return new WaitForSeconds(eatingCD);
        playerIsOnCD = false;
    }

    GameObject FindDirectionNode()
    {
        int targetNodePosX = currentTargetNode.GetComponent<Node>().xPos;
        int targetNodePosY = currentTargetNode.GetComponent<Node>().yPos;

        switch (queuedDirection)
        {
            case SpecifyDirection.Up:
                transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                return grid.nodeArray[targetNodePosY - 1, targetNodePosX];

            case SpecifyDirection.Down:
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                return grid.nodeArray[targetNodePosY + 1, targetNodePosX];

            case SpecifyDirection.Right:
                transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                if (targetNodePosX + 1 < grid.tileWidthCount)
                {
                    return grid.nodeArray[targetNodePosY, targetNodePosX + 1];
                }
                else
                {
                    transform.position = grid.nodeArray[targetNodePosY, 0].transform.position;
                    Node tmpNode = grid.nodeArray[targetNodePosY, 0].GetComponent<Node>();
                    tmpNode.tileSprite = grid.tileTypes[2];
                    tmpNode.UpdateSprite();
                    tmpNode.gameObject.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                    tmpNode.unwalkable = true;
                    visitedNodes.Add(tmpNode);
                    return grid.nodeArray[targetNodePosY, 1];
                }

            case SpecifyDirection.Left:
                transform.rotation = Quaternion.AngleAxis(270, Vector3.forward);
                if (targetNodePosX > 0)
                {
                    return grid.nodeArray[targetNodePosY, targetNodePosX - 1];
                }
                else
                {
                    transform.position = grid.nodeArray[targetNodePosY, grid.tileWidthCount - 1].transform.position;
                    Node tmpNode2 = grid.nodeArray[targetNodePosY, grid.tileWidthCount - 1].GetComponent<Node>();
                    tmpNode2.tileSprite = grid.tileTypes[2];
                    tmpNode2.UpdateSprite();
                    tmpNode2.gameObject.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                    tmpNode2.unwalkable = true;
                    visitedNodes.Add(tmpNode2);
                    return grid.nodeArray[targetNodePosY, grid.tileWidthCount - 2];
                }
        }
        return null;
    }

    public void CalculateTurnSprite()
    {
        if (queuedDirection == SpecifyDirection.Up && previousDirection == SpecifyDirection.Right)
        {
            // choose right sprite
            ChangeSprite(4, 0);
            previousDirection = queuedDirection;
        }
        else if (queuedDirection == SpecifyDirection.Up && previousDirection == SpecifyDirection.Left)
        {
            // choose left sprite
            ChangeSprite(3, 0);
            previousDirection = queuedDirection;
        }
        else if (queuedDirection == SpecifyDirection.Right && previousDirection == SpecifyDirection.Down)
        {
            // choose right sprite
            ChangeSprite(4, 90);
            previousDirection = queuedDirection;
        }
        else if (queuedDirection == SpecifyDirection.Right && previousDirection == SpecifyDirection.Up)
        {
            // choose left sprite
            ChangeSprite(3, 90);
            previousDirection = queuedDirection;
        }
        else if (queuedDirection == SpecifyDirection.Down && previousDirection == SpecifyDirection.Left)
        {
            // choose right sprite
            ChangeSprite(4, 180);
            previousDirection = queuedDirection;
        }
        else if (queuedDirection == SpecifyDirection.Down && previousDirection == SpecifyDirection.Right)
        {
            ChangeSprite(3, 180);
            previousDirection = queuedDirection;
        }
        else if (queuedDirection == SpecifyDirection.Left && previousDirection == SpecifyDirection.Up)
        {
            // choose right sprite
            ChangeSprite(4, 270);
            previousDirection = queuedDirection;
        }
        else if (queuedDirection == SpecifyDirection.Left && previousDirection == SpecifyDirection.Down)
        {
            // choose left sprite
            ChangeSprite(3, 270);
            previousDirection = queuedDirection;
        }
        else if (queuedDirection == SpecifyDirection.Left && previousDirection == SpecifyDirection.Left)
        {
            // choose left sprite
            ChangeSprite(2, 90);
            previousDirection = queuedDirection;
            Instantiate(PartialPrefab, new Vector2(currentNode.xPos, -currentNode.yPos), Quaternion.AngleAxis(270, Vector3.back));
        }
        else if (queuedDirection == SpecifyDirection.Right && previousDirection == SpecifyDirection.Right)
        {
            // choose left sprite
            ChangeSprite(2, 90);
            previousDirection = queuedDirection;
            Instantiate(PartialPrefab, new Vector2(currentNode.xPos, -currentNode.yPos), Quaternion.AngleAxis(90, Vector3.back));
        }
        else if (queuedDirection == SpecifyDirection.Up && previousDirection == SpecifyDirection.Up)
        {
            // choose left sprite
            ChangeSprite(2, 0);
            previousDirection = queuedDirection;
            Instantiate(PartialPrefab, new Vector2(currentNode.xPos, -currentNode.yPos), Quaternion.AngleAxis(0, Vector3.back));
        }
        else if (queuedDirection == SpecifyDirection.Down && previousDirection == SpecifyDirection.Down)
        {
            // choose left sprite
            ChangeSprite(2, 0);
            previousDirection = queuedDirection;
            Instantiate(PartialPrefab, new Vector2(currentNode.xPos, -currentNode.yPos), Quaternion.AngleAxis(180, Vector3.back));
        }
    }

    void ChangeSprite(int spriteIndex, float rotation)
    {
        currentNode.tileSprite = grid.tileTypes[spriteIndex];
        currentNode.UpdateSprite();
        currentNode.gameObject.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.back);
    }

    void WinLoseGame()
    {
        if (visitedNodes.Count > 1)
        {
            Node loseTarget = visitedNodes[visitedNodes.Count - 1];

            if (Mathf.Abs(transform.position.x - loseTarget.transform.position.x) < 5)
            {
                transform.position = Vector2.MoveTowards(transform.position, loseTarget.gameObject.transform.position, baseSpeed * returnSpeedMultiplier * Time.deltaTime);
                transform.rotation = loseTarget.transform.rotation;
            }
            else
            {
                transform.position = loseTarget.gameObject.transform.position;
            }


            if (loseTarget != visitedNodes[0].gameObject)
            {
                if (transform.position == loseTarget.gameObject.transform.position)
                {
                    if (isDead)
                    {
                        loseTarget.tileSprite = grid.tileTypes[0];
                        loseTarget.UpdateSprite();
                        visitedNodes.Remove(loseTarget);
                    }
                    else
                    {
                        Instantiate(LavaPrefab, loseTarget.transform.position, Quaternion.identity);
                        visitedNodes.RemoveAt(visitedNodes.Count - 1);
                    }
                }
            }
        }
        else
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Application.LoadLevel(0);
            }
        }
    }

    public void CalculateDistance()
    {
        if (currentNode.yPos > distanceTraveled)
        {
            distanceScore = grid.tileHeightCount - currentNode.yPos - 2;
            distanceTraveled = currentNode.yPos;
        }
    }
}
