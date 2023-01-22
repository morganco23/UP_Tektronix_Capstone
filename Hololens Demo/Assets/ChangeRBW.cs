using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRBW : MonoBehaviour
{
    public double rbw = 100e3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseRBW()
    {
        rbw += 1e3;
        Debug.Log($"RBW is now {rbw}");
    }

    public void DecreaseRBW()
    {
        rbw -= 1e3;
        Debug.Log($"RBW is now {rbw}");
    }

    public void UpdateRBW() 
    {

    }
}
