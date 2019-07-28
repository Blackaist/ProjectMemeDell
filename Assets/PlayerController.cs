using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private List<GameObject> platformes;
    private Text ui_score;
    private static bool isGameBegun = false;
    private bool forward = true, gameOver = false, trueEnd = false;
    private float fallingSpeed = Constants.player_AccelerationFalling;
    private float chance = 33.4f;
    private static int score = 0;
    private int collides = 0;

    void Start()
    {
        if (isGameBegun) {
            ui_score = GameObject.Find("ui_Score").GetComponent<Text>();
            ui_score.text = "Score: " + score;

            platformes = new List<GameObject>();
            for (int i = 0, j = 0; j != 3; i++)
            {
                 Sprite spr = Resources.Load("spr_cube", typeof(Sprite)) as Sprite;

                 GameObject sample = new GameObject();
                 sample.AddComponent<SpriteRenderer>();
                 sample.name = "StartWall";
                 sample.GetComponent<SpriteRenderer>().sprite = spr;

                 sample.AddComponent<PolygonCollider2D>();
                 sample.GetComponent<PolygonCollider2D>().points = new Vector2[4];
                 sample.GetComponent<PolygonCollider2D>().SetPath(0, new Vector2[4] { new Vector2(0, 0.68f), new Vector2(0.32f, 0.5f),
                                                                                  new Vector2(0, 0.32f), new Vector2(-0.32f, 0.5f)});

                 sample.transform.position = new Vector3(gameObject.transform.position.x - (i - j) * 0.32f, gameObject.transform.position.y + (i + j) * 0.16f - 0.60f, gameObject.transform.position.y + (i + j) * 0.16f - 0.60f);

                 sample.AddComponent<Cubes>();
                 sample.GetComponent<Cubes>().startWall = true;
                 platformes.Add(sample);

                 if (i == 2)
                 {
                     j++;
                     i = -1;
                     if (Random.Range(0, 100) < chance * j)
                     {
                         sample.name = "Wall";
                         sample.GetComponent<Cubes>().startWall = false;
                         sample.GetComponent<Cubes>().forward(true);
                         sample.GetComponent<Cubes>().setter(platformes);
                         platformes.Remove(sample);
                         chance = 0;
                     }
                 }
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            if (isGameBegun && (!gameOver || !trueEnd))
                forward = forward ? false : true;
            else if (!isGameBegun)
            {
                isGameBegun = true;
                Start();
            }
            else SceneManager.LoadScene(0);

        if (!gameOver && !trueEnd)
        {
            Move();
            //trueEnd = false;
        }
        else if (!trueEnd)
            trueEnd = true;
        else MoveDown();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collides--;

        if (collides == 0)
            gameOver = true;

        if (collision.name == "Wall")
        {
            collision.GetComponent<Cubes>().Destroy();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameOver = false;
        collides++;

        if (collision.name == "Shadow")
        {
            score++;
            ui_score.text = "Score: " + score;
            Destroy(collision.gameObject);
        }
    }

    private void Move()
    {
        if (forward)
            transform.position = new Vector3(transform.position.x - Constants.player_speedX, transform.position.y + Constants.player_speedY, transform.position.y + Constants.player_speedY - 0.8f);
        else transform.position = new Vector3(transform.position.x + Constants.player_speedX, transform.position.y + Constants.player_speedY, transform.position.y + Constants.player_speedY - 0.8f);
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

    private void MoveDown()
    {
        fallingSpeed = fallingSpeed + Constants.player_AccelerationFalling;
        float totalSpeed = fallingSpeed + Constants.player_AccelerationFalling;
        transform.position = new Vector3(transform.position.x, transform.position.y - totalSpeed, transform.position.z);
    }

}
