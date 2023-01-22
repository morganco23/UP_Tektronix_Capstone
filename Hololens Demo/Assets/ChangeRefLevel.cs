using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRefLevel : MonoBehaviour
{
    public double refLevel = -30.0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseRefLevel()
    {
        refLevel += 1.0;
        Debug.Log($"Reference Level is now {refLevel}");

    }

    public void DecreaseRefLevel()
    {
        refLevel -= 1.0;
        Debug.Log($"Reference Level is now {refLevel}");
    }

    public void UpdateRefLevel() 
    { 

    }
}
