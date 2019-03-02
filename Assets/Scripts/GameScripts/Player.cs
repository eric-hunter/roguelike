using UnityEngine;
using UnityEngine.UI;

public class Player : MovingObject
{
    #region PUBLIC PROPERTIES

    public int DealtPointsToWall = 1;
    public int GainedPointsFromFood = 10;
    public int GainedPointsFromSoda = 20;
    public float RestartLevelDelay = 1f;
    public Text foodText;

    #endregion

    #region PRIVATE VARIABLES

    private Animator animator;
    private int foodLevel;
    private readonly string FOOD_TEXT = "Food: {0:d}";
    private readonly string FOOD_CHANGE_TEXT = "Food: {0:d} ({1:d} pts)";

    #endregion

    #region PUBLIC METHODS

    public void DamagePlayer(int points)
    {
        animator.SetTrigger("playerHit");
        foodLevel -= points;
        foodText.text = string.Format(FOOD_CHANGE_TEXT, foodLevel, points);
        CheckFoodLevel();
    }

    #endregion

    #region PROTECTED IMPLEMENTED METHODS

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        foodLevel--;
        foodText.text = string.Format(FOOD_CHANGE_TEXT, foodLevel, -1);

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        if (Move(xDir, yDir, out hit))
        {
            //call sound stuff
        }

        CheckFoodLevel();

        GameManager.Instance.PlayersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall wallThatWasHit = component as Wall;

        wallThatWasHit.DamageWall(DealtPointsToWall);

        animator.SetTrigger("playerChop");
    }

    #endregion


    #region IMPLEMENTED MESSAGES

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        foodLevel = GameManager.Instance.PlayerFoodPoints;
        foodText.text = string.Format(FOOD_TEXT, foodLevel);

        base.Start();
    }

    #endregion

    #region MESSAGES
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.PlayersTurn)
            return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        //PREVENTS PLAYER FROM MOVING DIAGONALLY
        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);

    }

    //CALLED WHEN PLAYER GAME OBJECT HAS COLLIDED WITH ANOTHER OBJECT.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Exit"))
        {
            Invoke("Restart", RestartLevelDelay);
            enabled = false;
        }
        else if (other.tag.Equals("Food"))
        {
            foodLevel += GainedPointsFromFood;
            foodText.text = string.Format(FOOD_CHANGE_TEXT, foodLevel, GainedPointsFromFood);
            other.gameObject.SetActive(false);
        }
        else if (other.tag.Equals("Soda"))
        {
            foodLevel += GainedPointsFromSoda;
            foodText.text = string.Format(FOOD_CHANGE_TEXT, foodLevel, GainedPointsFromSoda);
            other.gameObject.SetActive(false); 
        }
    }

    #endregion

    #region PRIVATE HELPER METHODS

    private void CheckFoodLevel()
    {
        if (foodLevel <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    #endregion
}
