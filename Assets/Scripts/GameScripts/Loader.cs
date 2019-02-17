using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject GameManagerScript;


    #region UNITY MESSAGES

    public void Awake()
    {
        if (GameManager.Instance == null)
            Instantiate(GameManagerScript);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
}
