using UnityBaseScripts;

public class GameManager : Manager
{

    #region PUBLIC PROPERTIES

    public BoardManager BoardScript { get; set; }

    #endregion

    #region PRIVATE VARIABLES

    //using 3 bc that's when enemies appear.
    private int level = 3;

    #endregion

    #region PRIVATE METHODS

    private void Awake()
    {
        BoardScript = GetComponent<BoardManager>();
        InitGame();
    }

    private void InitGame()
    {
        BoardScript.SetupScene(level);
    }

    #endregion

    #region UNITY HOOKS

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
}
