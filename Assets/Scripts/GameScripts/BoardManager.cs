using System;
using System.Collections.Generic;

using UnityEngine;
//USE UNITY RANDOM CLASS INSTEAD OF SYSTEM
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    #region CLASSES
    //SERIALIZE THIS BECAUSE OF THE SUB-PROPERTY NATURE TO DISPLAY IN UNITY GUI.
    [Serializable]
    public class RandomCount
    {
        public int Minimum;
        public int Maximum;

        public RandomCount(int min, int max)
        {
            Minimum = min;
            Maximum = max;
        }
    }

    #endregion

    #region PUBLIC PROPERTIES

    //GAME BOARD SIZE
    public int Columns = 8;
    public int Rows = 8;

    //RANDOM ITEM COUNTS
    public RandomCount WallCount = new RandomCount(5, 9);
    public RandomCount FoodCount = new RandomCount(1, 5);

    //GAME OBJECTS
    public GameObject ExitSign;
    public GameObject[] FloorTiles;
    public GameObject[] WallTiles;
    public GameObject[] FoodTiles;
    public GameObject[] EnemyTiles;
    public GameObject[] OuterWallTiles;

    #endregion

    #region PRIVATE PROPERTIES

    //TODO: BOARD HOLDER??
    private Transform boardHolder;
    private List<Vector3> gridPositionsToFill = new List<Vector3>();

    #endregion

    #region PUBLIC METHODS

    //START
    public void SetupScene(int level)
    {
        //initialize private list of grid position vectors.
        InitializeGridPositionVectors();

        //setup walls
        BoardSetup();

        //randomly spread out walls.
        LayoutObjectsAtRandom(WallTiles, WallCount.Minimum, WallCount.Maximum);

        //randomly spread out food.
        LayoutObjectsAtRandom(FoodTiles, FoodCount.Minimum, FoodCount.Maximum);

        //set difficulty level (where difficulty increases logarithmically).
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectsAtRandom(EnemyTiles, enemyCount, enemyCount);

        //finally, instantiate the exit tile at the top right tile.
        Instantiate(
            ExitSign,
            new Vector3(Columns - 1, Rows - 1, 0f),
            Quaternion.identity
            );
    }

    #endregion

    #region PRIVATE METHODS

    //BOARD SETUP METHODS
    private void InitializeGridPositionVectors()
    {
        gridPositionsToFill.Clear();

        for (int col = 0; col < Columns - 1; col++)
        {
            for (int row = 0; row < Rows - 1; row++)
            {
                gridPositionsToFill.Add(new Vector3(row, col, 0f));
            }
        }
    }

    private void BoardSetup()
    {
        //TODO: WHAT IS THIS??
        boardHolder = new GameObject("Board").transform;

        for (int col = -1; col < Columns + 1; col++)
        {
            for (int row = -1; row < Rows + 1; row++)
            {
                //decide whether tile is an outside wall tile.
                GameObject chosenWall =
                    col == -1 || row == -1 || col == Columns || row == Rows ?
                    OuterWallTiles[Random.Range(0, OuterWallTiles.Length)]
                    : FloorTiles[Random.Range(0, FloorTiles.Length)];

                //clone chosen object and instantiate it in game.
                GameObject clonedWall = Instantiate(
                    chosenWall,
                    new Vector3(row, col, 0f),
                    Quaternion.identity);

                //TODO: Figure out why we needed to do this??
                clonedWall.transform.SetParent(boardHolder);

            }
        }
    }

    private void LayoutObjectsAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            //get random position.
            Vector3 randomPosition = TakeRandomGridPosition();
            //get random type of tile.
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            //CALL INSTANTIATE TO ADD GAME OBJECT TO BOARD (at randomePosition).
            Instantiate(tileChoice, randomPosition, Quaternion.identity);

        }
    }

    #endregion

    #region HELPERS

    //HELPERS
    private Vector3 TakeRandomGridPosition()
    {
        int randomIndex = Random.Range(0, gridPositionsToFill.Count);
        Vector3 randomPosition = gridPositionsToFill[randomIndex];

        //because we don't want to add whatever we're adding in the same place twice.
        //this is ok to call multiple times, bc we can't have food and walls in same position.
        gridPositionsToFill.RemoveAt(randomIndex);

        return randomPosition;
    }

    #endregion
}

