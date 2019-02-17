using UnityBaseScripts;
using UnityEngine;

public class GameManager : Manager
{
    #region PUBLIC PROPERTIES

    public BoardManager BoardScript;

    public int PlayerFoodPoints;

    [HideInInspector] 
    public bool PlayersTurn = true;

    #endregion

    #region PUBLIC METHODS

    public void GameOver()
    {
        enabled = false;
    }

    #endregion

    #region PRIVATE VARIABLES

    //using 3 bc that's when enemies appear.
    private int level = 3;

    #endregion

    #region MESSAGES

    public override void Awake()
    {
        base.Awake();
        BoardScript = GetComponent<BoardManager>();
        InitGame();
    }

    #endregion

    #region PRIVATE METHODS

    private void InitGame()
    {
        BoardScript.SetupScene(level);
    }

    #endregion
}
