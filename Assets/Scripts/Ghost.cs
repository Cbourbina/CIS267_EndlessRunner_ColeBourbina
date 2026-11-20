using UnityEngine;

public class Ghost : MonoBehaviour
{
    private float collectableSpeed;
    [SerializeField]
    private AudioClip collectableCollected;
    [SerializeField]
    float volume;
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
        if (collision.CompareTag("Player") || collision.CompareTag("Ghost"))
        {
            AudioSource.PlayClipAtPoint(collectableCollected, transform.position, volume);
            collision.GetComponent<PlayerController>().ghostCar();
            collision.GetComponent<PlayerController>().gameManager.GetComponent<PlayerScore>().addScore(100);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
