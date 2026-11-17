using UnityEngine;

public class Upgrade : MonoBehaviour
{
    private float collectableSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * collectableSpeed * Time.deltaTime);
    }

    public void setSpeed(float speed)
    {
        collectableSpeed = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().upgradeCar();
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
