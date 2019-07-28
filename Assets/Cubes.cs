using UnityEngine;
using System.Collections.Generic;

public class Cubes : MonoBehaviour
{
    private bool previousForward = true;
    private float fallingSpeed = Constants.wall_AccelerationFalling;
    private bool active = false;
    public bool startWall = false;
    private Color color;
    private List<GameObject> platformes;

    public void setter(List<GameObject> platformes2)
    {
        this.platformes = platformes2;
    }

    // Start is called before the first frame update
    void Start()
    {
        enabled = false;
        if (!startWall)
            GenerateNew();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            fallingSpeed = fallingSpeed + Constants.wall_AccelerationFalling;
            float totalSpeed = fallingSpeed + Constants.wall_AccelerationFalling;
            transform.position = new Vector3(transform.position.x, transform.position.y - totalSpeed, transform.position.y - totalSpeed);
            color = GetComponent<SpriteRenderer>().color;
            color.a -= 0.05f;

            if (color.a == 0.0f)
                Destroy(gameObject);
            else GetComponent<SpriteRenderer>().color = color;
        }
    }

    public void Destroy()
    {
        if (platformes != null)
        {
            foreach(GameObject entity in platformes)
            {
                entity.GetComponent<Cubes>().Destroy();
            }
        }
        enabled = true;
        active = true;
    }

    public void forward(bool fwd)
    {
        previousForward = fwd;
    }

    private void GenerateNew()
    {
        if (Random.value < 0.2)
        {
            Sprite spr = Resources.Load("spr_crystal", typeof(Sprite)) as Sprite;

            GameObject sample = new GameObject();
            sample.AddComponent<SpriteRenderer>();
            sample.GetComponent<SpriteRenderer>().sprite = spr;
            sample.name = "Crystal";
            sample.AddComponent<Crystal>();
            sample.GetComponent<Crystal>().setPosition(transform.position.x, transform.position.y);
        }

        if (transform.position.x < Camera.main.aspect * Camera.main.orthographicSize && transform.position.y < Camera.main.orthographicSize)
        {
            Sprite spr = Resources.Load("spr_cube", typeof(Sprite)) as Sprite;

            GameObject sample = new GameObject();
            sample.name = "Wall";

            sample.AddComponent<SpriteRenderer>();
            sample.GetComponent<SpriteRenderer>().sprite = spr;

            float y = transform.position.y + 0.16f;
            if (previousForward)
                sample.transform.position = new Vector3(transform.position.x - 0.32f, y, y);
            else sample.transform.position = new Vector3(transform.position.x + 0.32f, y, y);

            sample.AddComponent<PolygonCollider2D>();
            sample.GetComponent<PolygonCollider2D>().points = new Vector2[4];
            sample.GetComponent<PolygonCollider2D>().SetPath(0, new Vector2[4] { new Vector2(0, 0.68f), new Vector2(0.32f, 0.5f),
                                                                                 new Vector2(0, 0.32f), new Vector2(-0.32f, 0.5f)});
            sample.AddComponent<Cubes>();
            sample.GetComponent<Cubes>().forward(Random.value < 0.5f ? false : true);
        }
    }

}
