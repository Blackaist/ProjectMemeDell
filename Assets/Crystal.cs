using UnityEngine;

public class Crystal : MonoBehaviour
{
    private bool goUp = true;
    float x = 0, y = 0, y2 = 0;
    private GameObject shadow;

    void Start()
    {
        shadow = new GameObject();
        Sprite spr = Resources.Load("spr_shadow", typeof(Sprite)) as Sprite;
        shadow.AddComponent<SpriteRenderer>();
        shadow.GetComponent<SpriteRenderer>().sprite = spr;
        shadow.name = "Shadow";
        shadow.AddComponent<BoxCollider2D>();

        shadow.transform.position = new Vector3(x, y - 0.10f, y - 0.71f);
        transform.position = new Vector3(x, y, -9);
    }


    void Update()
    {
        if (goUp)
            y2 = y2 + Constants.crystal_speed;
        else y2 = y2 - Constants.crystal_speed;

        transform.position = new Vector3(x, y2, transform.position.z);

        if (y2 > y + Constants.crystal_scatter && goUp) goUp = false;
        else if (y2 < y - Constants.crystal_scatter && !goUp) goUp = true;

        if (!shadow)
            Destroy(gameObject);

    }

    public void setPosition (float x, float y)
    {
        this.x = x;
        this.y = y + Constants.crystal_offsetShadow;
        y2 = this.y;
    }
}
