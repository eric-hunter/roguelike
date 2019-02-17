using UnityEngine;

/*
 * Description: Base Manager class.
 * 
 * Managers should:
 *  - Behave like a singleton.
 *  - Exists from game start to end.
 * 
 * Note: Singleton is not truly enforced here.
 *  This is a limitation of since we need Unity to call the constructor of the
 *  script.
 * 
 * This super class manages the gameObject, associated with your 
 * inheriting manager, such that it is only ever instantiated once during the 
 * life of the game.
 */

namespace UnityBaseScripts
{
    public class Manager : MonoBehaviour
    {
        #region PRIVATE PROPERTIES

        // Check to see if we're about to be destroyed.
        public static Manager Instance { get; set; }

        #endregion

        #region UNITY MESSAGES 

        public virtual void Awake()
        {
            if (Instance == null)
                Instance = this;
            //TODO: Still unsure how this would ever get executed.
            else if (Instance != this)
                Destroy(gameObject);

            //PREVENT UNITY FROM DESTROYING THE "SINGLETON" GAME OBJECT ON SCENE
            //CHANGE OR GAME END.
            DontDestroyOnLoad(gameObject);
        }

        #endregion
    }
}
