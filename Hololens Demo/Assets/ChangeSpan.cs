using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpan : MonoBehaviour
{
    public double span = 40e6;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseSpan()
    {
        span += 1e6;
        Debug.Log($"Span is now {span}");
    }

    public void DecreaseSpan()
    {
        span -= 1e6;
        Debug.Log($"Span is now {span}");
    }

    public void UpdateSpan() 
    { 

    }
}
