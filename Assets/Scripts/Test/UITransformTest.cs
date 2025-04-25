using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITransformTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float width = GetComponent<RectTransform>().rect.width;
        float height = GetComponent<RectTransform>().rect.height;
        Debug.Log("Width: " + width);
        Debug.Log("Height: " + height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
