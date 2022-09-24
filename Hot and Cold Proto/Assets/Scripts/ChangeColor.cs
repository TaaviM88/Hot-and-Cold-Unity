using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeColor : MonoBehaviour
{
    Image image;
    private float q = 0;
    bool increasing = true;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Color color = new Color(q, q, 1);
        if (increasing)
        {
            q += 0.025f;
            if (q > 1)
            {
                increasing = false;
            }
        }
        else
        {
            q -= 0.025f;
            if (q <= 0)
            {
                increasing = true;
            }
        }

        image.color = color;
    }
}
