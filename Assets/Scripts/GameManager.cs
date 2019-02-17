using System.Collections;
using System.Collections.Generic;
using Completed;
using UnityEngine;
using UnityScripts;

public class NewBehaviourScript : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
