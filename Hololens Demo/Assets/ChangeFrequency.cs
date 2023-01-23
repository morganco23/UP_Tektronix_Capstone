using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Runtime.InteropServices;

public class ChangeFrequency : MonoBehaviour
{

    [DllImport("rsa_api", EntryPoint = "AUDIO_SetFrequencyOffset")]
    private static extern RSAAPITest.ReturnStatus AUDIO_SetFrequencyOffset(double freqOffsetHz);

    [DllImport("rsa_api", EntryPoint = "DPX_Reset")]
    private static extern RSAAPITest.ReturnStatus DPX_Reset();

    private const double FREQUENCY_MIN =  0.0;
    private const double FREQUENCY_MAX = 10.0e9;
    private double frequency = 2.4453e9;


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
        Debug.Log($"Frequency Before = {frequency} Hz");
        if ((frequency + 0.01e9) < FREQUENCY_MAX){
            AUDIO_SetFrequencyOffset(frequency + 0.01e9);
            DPX_Reset(); // Reset is necessary as the RSA will not update settings without resetting DPX first
        }
        Debug.Log($"Frequency After = {frequency} Hz");
    }

    public void DecreaseFrequency()
    {
        Debug.Log($"Frequency Before = {frequency} Hz");
        if((frequency - 0.01e9) > FREQUENCY_MIN){
            AUDIO_SetFrequencyOffset(frequency - 0.01e9);
            DPX_Reset();
        }
        Debug.Log($"Frequency After = {frequency} Hz");
    }
}
