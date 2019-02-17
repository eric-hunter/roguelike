using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite WallDamageSprite;
    public int HitPoints = 4;

    //reference to this Wall component's Sprite Renderer component.
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        spriteRenderer.sprite = WallDamageSprite;
        HitPoints -= loss;

        //if player finally destroys the wall.
        if (HitPoints <= 0)
            gameObject.SetActive(false);
    }

}
