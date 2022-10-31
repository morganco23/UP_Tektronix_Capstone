using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFrequency : MonoBehaviour
{
    public double frequency = 2.4453e9;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncreaseFrequency()
    {
        frequency += 0.01e9;
    }

    public void DecreaseFrequency()
    {
        frequency -= 0.01e9;
    }
}
