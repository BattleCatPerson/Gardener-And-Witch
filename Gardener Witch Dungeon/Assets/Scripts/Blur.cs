using UnityEngine;

public class Blur : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Material m;
    void Start()
    {
        m = spriteRenderer.material;
    }

    void Update()
    {
        m.SetFloat("_BlurAmount", 0.1f);
        //spriteRenderer.material = m;
    }
}
