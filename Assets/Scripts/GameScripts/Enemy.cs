using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    #region PUBLIC PROPERTIES

    public int DamageDealtToPlayer;

    #endregion

    #region PRIVATE VARIABLES

    private Animator animator;
    //USED TO STORE PLAYER'S POSITION TO TELL ENEMY WHERE TO ATTACK
    private Transform target;
    private bool skipMove;

    #endregion

    #region PUBLIC METHODS

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        //IF PLAYER IS AT A DIFFERENT VERTICAL POSITION
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            //THEN MOVE CLOSER TO THEM VERTICALLY BY 1 TILE
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            //OTHERWISE MOVE CLOSER TO THEM HORIZONTALLY BY 1 TILE
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }

    #endregion

    #region PROTECTED IMPLEMENTED METHODS

    protected override void AttemptMove<ExpectedT>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<ExpectedT>(xDir, yDir);
        skipMove = true;
    }

    protected override void OnCantMove<ExpectedT>(
        ExpectedT component)
    {
        Player playerThatWasHit = component as Player;

        animator.SetTrigger("enemyAttack");

        playerThatWasHit.DamagePlayer(DamageDealtToPlayer);
    }


    #endregion

    #region MESSAGES

    protected override void Start()
    {
        //register self to the game manager
        GameManager.Instance.AddEnemy(this);

        //get access to animation control
        animator = GetComponent<Animator>();

        //identify the Player game object.
        target = GameObject.FindGameObjectWithTag("Player").transform;

        base.Start();
    }

    #endregion
}
