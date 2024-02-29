using UnityEngine;

public class BGScroll : MonoBehaviour
{
    public float speed;
    [SerializeField] private Renderer bgRenderer;
    

    // Update is called once per frame
    private void Update()
    {
        bgRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}
