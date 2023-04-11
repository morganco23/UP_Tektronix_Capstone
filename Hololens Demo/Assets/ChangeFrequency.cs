using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Runtime.InteropServices;
using static RSAAPITest;

public class ChangeFrequency : MonoBehaviour
{

    [DllImport("rsa_api", EntryPoint = "CONFIG_SetCenterFreq")]
    private static extern ReturnStatus CONFIG_SetCenterFreq(double freqOffsetHz);

    [DllImport("rsa_api", EntryPoint = "DPX_Reset")]
    private static extern ReturnStatus DPX_Reset();

    private const double FREQUENCY_MIN =  0.0;
    private const double FREQUENCY_MAX = 10.0e9;


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
        DPX_Config dpxConfig = getDPXConfig();
        Debug.Log($"Frequency Before = {dpxConfig.cf} Hz");
        double newFreq = dpxConfig.cf + 0.01e9;
        if (newFreq < FREQUENCY_MAX){
            CONFIG_SetCenterFreq(newFreq);
            dpxConfig.cf += 0.01e9;
            DPX_Reset(); // Reset is necessary as the RSA will not update settings without resetting DPX first
        }
        Debug.Log($"Frequency After = {dpxConfig.cf} Hz");
    }

    public void DecreaseFrequency()
    {
        DPX_Config dpxConfig = getDPXConfig();
        Debug.Log($"Frequency Before = {dpxConfig.cf} Hz");
        double newFreq = dpxConfig.cf - 0.01e9;
        if(newFreq > FREQUENCY_MIN){
            CONFIG_SetCenterFreq(newFreq);
            dpxConfig.cf -= 0.01e9;
            DPX_Reset();
        }
        Debug.Log($"Frequency After = {dpxConfig.cf} Hz");
    }
}
