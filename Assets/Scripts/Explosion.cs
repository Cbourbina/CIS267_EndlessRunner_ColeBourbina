using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    AudioClip explosion;
    [SerializeField]
    private float volume;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            AudioSource.PlayClipAtPoint(explosion, transform.position, volume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);    
    }

    public void radiusEffectComplete()
    {
        Destroy(this.gameObject);
    }
}
