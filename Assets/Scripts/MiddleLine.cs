using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLine : MonoBehaviour
{
    public int dashes;
    public float dashLength;
    public float dashWidth;
    public float gapLength;
    public float yOffset;
    public Color dashColor;

    void Draw()
    {
        GameObject template = new("Dash Template");
        SpriteRenderer sr = template.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "Background";
        sr.sortingOrder = 1;
        sr.color = dashColor;
        sr.sprite = Resources.Load<Sprite>("Square");
        template.transform.localScale = new Vector3(dashWidth, dashLength, 1);

        for (int i = 0; i < dashes; i++)
        {
            GameObject dash = Instantiate(template, transform);
            dash.name = "Dash " + i;
            dash.transform.parent = transform;
            dash.transform.localPosition = new Vector3(0, yOffset + i * (dashLength + gapLength), 0);
        }

        Destroy(template);
    }

    void Start()
    {
        Draw();
    }
}
