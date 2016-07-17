using UnityEngine;
using System.Collections;

public class MovingRoadTexture : MonoBehaviour {

    public float scrollSpeed = 0.5F;
    public Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    void Update()
    {
        if (rend != null)
        {
            float offset = Time.time * scrollSpeed;
            rend.material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
        }
    }
}
