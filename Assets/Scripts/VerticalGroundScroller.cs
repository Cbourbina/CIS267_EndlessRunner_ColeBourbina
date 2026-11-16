using UnityEngine;

public class VerticalGroundScroller : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed;

    private Renderer quadRenderer;

    void Start()
    {
        quadRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float yOffset = Mathf.Repeat(Time.time * scrollSpeed, 1f);
        quadRenderer.material.mainTextureOffset = new Vector2(0f, yOffset);
    }
}
