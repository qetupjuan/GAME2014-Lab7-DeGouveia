using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Movement")]
    public MovingDirection dir;

    [Range(0.1f, 10.0f)]
    public float speed;

    [Range(1.0f, 25.0f)]
    public float dist;

    [Range(0.01f, 0.1f)]
    public float distOffSet;

    public bool isLooping;

    private Vector2 startPos;
    private bool motionActive;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        motionActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        movePlatform();

        if (isLooping)
        {
            motionActive = true;
        }
    }

    private void movePlatform()
    {
        float movingValue = (motionActive) ? Mathf.PingPong(Time.time * speed, dist) : dist;

        if ((!isLooping) && (movingValue >= dist - distOffSet))
        {
            motionActive = false;
        }

        switch (dir)
        {
            case MovingDirection.HORIZONTAL:
                transform.position = new Vector2(startPos.x + movingValue, transform.position.y);
                break;
            case MovingDirection.VERTICAL:
                transform.position = new Vector2(transform.position.x, startPos.y + movingValue);
                break;
            case MovingDirection.DIAGONAL_UP:
                transform.position = new Vector2(startPos.x + movingValue, startPos.y + movingValue);
                break;
            case MovingDirection.DIAGONAL_DOWN:
                transform.position = new Vector2(startPos.x + movingValue, startPos.y - movingValue);
                break;
        }
    }
}
