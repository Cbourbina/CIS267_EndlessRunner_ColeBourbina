using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    private float vehicleSpeed;
    private bool changeLaneLeft;
    private bool changeLaneRight;
    private bool isMoving;
    private Vector2 targetPos;
    private Vector2 newPos;
    private Animator animator;
    private Transform gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMoving = false;
        if (changeLaneLeft)
        {
            targetPos = (Vector2)transform.position + ((new Vector2(-1, 0) * 2));
        }
        else if (changeLaneRight)
        {
            targetPos = (Vector2)transform.position + ((new Vector2(1, 0) * 2));
        }
        animator = GetComponent<Animator>();
        if (changeLaneLeft)
        {
            animator.SetBool("changeLaneLeft", true);
        }
        else if (changeLaneRight)
        {
            animator.SetBool("changeLaneRight", true);
        }
        gameManager = GameObject.FindWithTag("GameManager").transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (changeLaneLeft)
        {
            animator.SetBool("changeLaneLeft", true);
        }
        else if (changeLaneRight)
        {
            animator.SetBool("changeLaneRight", true);
        }
        if (!changeLaneLeft && !changeLaneRight)
        {
            animator.SetBool("changeLaneLeft", false);
            animator.SetBool("changeLaneRight", false);
        }
        transform.Translate(Vector2.down * vehicleSpeed * Time.deltaTime);
        if (!isMoving && transform.position.y <= 2)
        {
            if (changeLaneLeft || changeLaneRight)
            {
                newPos = transform.position;
                StartCoroutine(changeLane(targetPos));
                Debug.Log("Lane Changing, Left: " + changeLaneLeft + ", Right: " + changeLaneRight);
                isMoving = true;
            }
        }
        newPos = transform.position;
    }

    public void setVehicleSpeed(float speed)
    {
        vehicleSpeed = speed;
    }

    private IEnumerator changeLane(Vector2 targetPosition)
    {
        Debug.Log("Lane Changing, Left: " + changeLaneLeft + ", Right: " + changeLaneRight);
        while (Mathf.Abs(transform.position.x - targetPosition.x) > 0.001f)
        {
            newPos.x = Mathf.MoveTowards(transform.position.x, targetPosition.x, (vehicleSpeed * Time.deltaTime));
            transform.position = newPos;
            yield return null;
        }

        //transform.position = targetPosition;
        if (changeLaneLeft == true)
        {
            changeLaneLeft = false;
        }
        if (changeLaneRight == true)
        {
            changeLaneRight = false;
        }
    }

    public void setChangeLaneLeft(bool changeLeft)
    {
        changeLaneLeft = changeLeft;
    }

    public void setChangeLaneRight(bool changeRight)
    {
        changeLaneRight = changeRight;
    }

    public void honkHorn()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ColliderDistance2D distanceInfo = Physics2D.Distance(GetComponent<BoxCollider2D>(), collision);

        Vector2 hitSideNormal = distanceInfo.normal;
        if (collision.CompareTag("Player"))
        {
            //Bottom Side
            if (hitSideNormal.y < -0.5f)
            {
                collision.GetComponent<PlayerController>().gameManager.GetComponent<GameManager>().setHealth(0);
            }
            //Right Side
            else if (hitSideNormal.x > 0.5f)
            {
                honkHorn();
                gameManager.GetComponent<GameManager>().decreaseHealth();
            }
            //Left Side
            else if (hitSideNormal.x < -0.5f)
            {
                honkHorn();
                gameManager.GetComponent<GameManager>().decreaseHealth();
            }
        }
    }
}