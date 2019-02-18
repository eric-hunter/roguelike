using System.Collections;
using System.Collections.Generic;
using UnityBaseScripts;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    #region PUBLIC PROPERTIES

    public BoardManager Board;

    //turn time.
    public float TurnDelay = .1f;
    //initial player food points
    //somehow if I don't set this to readonly, the value gets set to zero.
    public readonly int PlayerFoodPoints = 100;
    [HideInInspector]
    public bool PlayersTurn = true;

    #endregion

    #region PRIVATE VARIABLES

    private List<Enemy> enemies;
    //using 3 bc that's when enemies appear.
    private int level = 3;
    private bool enemiesAreMoving;

    #endregion

    #region PUBLIC METHODS

    public void AddEnemy(Enemy enemyScript)
    {
        enemies.Add(enemyScript);
    }

    public void GameOver()
    {
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
        //clear out enemies from previous level.
        enemies.Clear();
        Board.SetupScene(level);
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
