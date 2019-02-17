using System.Collections;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    #region PUBLIC PROPERTIES

    public float MoveTime = .1f;
    public LayerMask BlockingLayer;

    #endregion

    #region PRIVATE VARIABLES

    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidBody;
    //to make movement calculations more efficient.
    private float inverseMoveTime;

    #endregion

    #region MESSAGES

    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        //TODO: no movement == 1?
        inverseMoveTime = 1f / MoveTime;
    }

    #endregion

    #region PROTECTED METHDOS

    protected bool Move(int xDir, int yDir, out RaycastHit2D hitRayCast)
    {
        //calculate start and end.
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        //DETECT HIT
        //NOTE: WE MUST TURN OFF OUR OWN COLLIDER OR OUR 2D LINECAST WILL HIT.
        boxCollider.enabled = false;
        //draws imaginary line between two vectors in the layer called Blocking.
        hitRayCast = Physics2D.Linecast(start, end, BlockingLayer);
        boxCollider.enabled = true;

        if (hitRayCast.transform == null)
        {
            //PERFORM ACTUAL MOVEMENT
            //this works because the IEnumerable-while-loop contents are 
            //functioning like a function. TODO: refactor below later.
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        //if linecast detected another box collider, then return false to indicate
        //that this Move is not valid.
        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(
                rigidBody.position,
                end,
                inverseMoveTime * Time.deltaTime
                );

            rigidBody.MovePosition(newPosition);

            //update for while condition
            //transform.position will use the new position set by previous line.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            //yield will prevent next loop through while until the Iterator 
            //method finishes its work.
            yield return null;
        }
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
            return;

        //get the component containing the box collider that the linecast hit.
        T hitComponent = hit.transform.GetComponent<T>();

        //execute the hit component's implementation of OnCantMove.
        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);

    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;

    #endregion
}
