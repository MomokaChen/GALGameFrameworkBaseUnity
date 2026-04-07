using UnityEngine;
using UnityEngine.UI;

public class BackgroundLoop : MonoBehaviour
{
    private RectTransform rect;
    private float length;
    private float startX;

    public float speed = -200f;

    void Start()
    {
        // UI 专用
        rect = GetComponent<RectTransform>();

        float left = rect.rect.xMin;
        float right = rect.rect.xMax;
        float actualWidth = right - left;
        
        length = actualWidth;
        startX = rect.anchoredPosition.x;

    }

    void Update()
    {
        rect.anchoredPosition += new Vector2(speed * Time.deltaTime, 0);
        float moved = rect.anchoredPosition.x - startX;
        
        if (moved <= -length)
        {
            rect.anchoredPosition = new Vector2(startX, rect.anchoredPosition.y);
        }
    }
}