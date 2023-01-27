using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Runtime.InteropServices;

public class ChangeFrequency : MonoBehaviour
{

    [DllImport("rsa_api", EntryPoint = "CONFIG_SetCenterFreq")]
    private static extern RSAAPITest.ReturnStatus CONFIG_SetCenterFreq(double centerFreq);

    [DllImport("rsa_api", EntryPoint = "CONFIG_GetCenterFreq")]
    private static extern RSAAPITest.ReturnStatus CONFIG_GetCenterFreq(ref double centerFreq);

    [DllImport("rsa_api", EntryPoint = "DPX_Reset")]
    private static extern RSAAPITest.ReturnStatus DPX_Reset();

    private const double FREQUENCY_MIN =  0.0;
    private const double FREQUENCY_MAX = 10.0e9;
    private double frequency;


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
        CONFIG_GetCenterFreq(ref frequency);
        Debug.Log($"Center Frequency Before = {frequency} Hz");
        if ((frequency + 0.01e9) <= FREQUENCY_MAX){
            CONFIG_SetCenterFreq(frequency + 0.01e9);
            DPX_Reset(); // Reset is necessary as the RSA will not update settings without resetting DPX first
        }
        Debug.Log($"Center Frequency After = {frequency} Hz");
    }

    public void DecreaseFrequency()
    {
        CONFIG_GetCenterFreq(ref frequency);
        Debug.Log($"Center Frequency Before = {frequency} Hz");
        if((frequency - 0.01e9) > FREQUENCY_MIN){
            CONFIG_SetCenterFreq(frequency - 0.01e9);
            DPX_Reset();
        }
        Debug.Log($"Center Frequency After = {frequency} Hz");
    }
}
