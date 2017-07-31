using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public float xMoveSpeed = 0.6f;
    public float yMoveSpeed = 0;
    public float moveDistance = 100f;
    public float distanceMoved = 0;
    public int dir = 1;

    public float repeatDelay = 0f;
    public float framesElapsed = 0f;

    private bool continueMoving = true;

    List<GameObject> AttachedObjects = new List<GameObject>();


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        framesElapsed += Time.deltaTime;
        if (framesElapsed >= repeatDelay)
        {
            continueMoving = true;
            framesElapsed = 0;
        }
    }

    void FixedUpdate()
    {

        if (distanceMoved >= moveDistance)
        {
            distanceMoved = 0;
            dir = dir * -1;
            continueMoving = false;
        }

        if (continueMoving == false && repeatDelay != 0f) return;

        transform.position = new Vector3(transform.position.x + (xMoveSpeed * dir), transform.position.y + (yMoveSpeed * dir), 40);
        distanceMoved += xMoveSpeed + yMoveSpeed;


        foreach (GameObject item in AttachedObjects)
        {
            
            Debug.Log("transform the thing");
            item.transform.position = new Vector2(item.transform.position.x + (xMoveSpeed * dir), item.transform.position.y + (yMoveSpeed * dir));
        }
        AttachedObjects.Clear();
    }

    void OnCollisionStay2D(Collision2D c)
    {
        Debug.Log("On collision stay");
        Debug.Log(c);

        bool Unique = true;
        foreach (GameObject item in AttachedObjects)
        {
            Unique = item == c.gameObject || Unique;
        }
        if (Unique)
        {
            AttachedObjects.Add(c.gameObject);
        }

    }

}
