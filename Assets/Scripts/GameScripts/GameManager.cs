using System.Collections;
using System.Collections.Generic;
using UnityBaseScripts;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Manager<GameManager>
{
    #region PUBLIC PROPERTIES

    public BoardManager Board;

    public float LevelStartDelay = 2f;
    //turn time.
    public float TurnDelay = 2f;
    //initial player food points
    //somehow if I don't set this to readonly, the value gets set to zero.
    public readonly int PlayerFoodPoints = 100;
    [HideInInspector]
    public bool PlayersTurn = true;

    #endregion

    #region PRIVATE VARIABLES

    private Text levelText;
    private GameObject levelImage;
    private List<Enemy> enemies;
    //using 3 bc that's when enemies appear.
    private int level = 1;
    private bool enemiesAreMoving;
    private bool doingSetup;

    #endregion

    #region PUBLIC METHODS

    public void AddEnemy(Enemy enemyScript)
    {
        enemies.Add(enemyScript);
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, starved.";
        levelImage.SetActive(true);

        enabled = false;
    }

    #endregion

    #region MESSAGES

    public void Update()
    {
        //IF PLAYER IS MOVING OR ENEMIES ARE ALREADY MOVING, TAKE NO ACTION.
        if (PlayersTurn || enemiesAreMoving)
            return;
        //OTHERWISE, START ENEMY MOVEMENT COROUTINE.
        StartCoroutine(MoveEnemies());
    }

    private void OnLevelWasLoaded(int level)
    {
        level++;

        InitGame();
    }

    #endregion

    #region OVERRIDDEN MESSAGES

    public override void Awake()
    {
        base.Awake();

        enemies = new List<Enemy>();
        Board = GetComponent<BoardManager>();
        InitGame();
    }

    #endregion

    #region PRIVATE METHODS

    private void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        //PRESENT USER WITH LEVEL INFO
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", LevelStartDelay);

        //clear out enemies from previous level.
        enemies.Clear();
        Board.SetupScene(level);
    }

    private void HideLevelImage() 
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    //ENEMY MOVEMENT COROUTINE
    private IEnumerator MoveEnemies()
    {
        //START ENEMY MOVEMENT
        enemiesAreMoving = true;
        yield return new WaitForSeconds(TurnDelay);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(TurnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].MoveTime);
        }

        PlayersTurn = true;
        enemiesAreMoving = false;
    }


    #endregion
}
