using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Runtime.InteropServices;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using static RSAAPITest;

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
    RSAAPITest.ReturnStatus error;
    private double updatedFreq = 0;
    TouchScreenKeyboard freqKeyboard;
    public static string keyboardText = "";
    public DPX_Config dpxConfig;

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
            error = CONFIG_SetCenterFreq(frequency + 0.01e9);
            if(error == 0) {
                DPX_Reset(); // Reset is necessary as the RSA will not update settings without resetting DPX first
                CONFIG_GetCenterFreq(ref updatedFreq);
                CONFIG_GetCenterFreq(ref frequency);
                GetDPXConfigParams(ref dpxConfig);
                dpxConfig.cf = frequency;
            } else {
                Debug.Log($"ERROR: CONFIG_SetCenterFreq returned error code: {error}.");
            }
        }
        Debug.Log($"(Variable) Center Frequency After = {frequency} Hz");
        Debug.Log($"(RSA) Updated Frequency = {updatedFreq} Hz");

    }

    public void DecreaseFrequency()
    {
        CONFIG_GetCenterFreq(ref frequency);
        Debug.Log($"Center Frequency Before = {frequency} Hz");
        if((frequency - 0.01e9) > FREQUENCY_MIN){
            CONFIG_SetCenterFreq(frequency - 0.01e9);
            if(error == 0) {
                DPX_Reset(); // Reset is necessary as the RSA will not update settings without resetting DPX first
                CONFIG_GetCenterFreq(ref updatedFreq);
                CONFIG_GetCenterFreq(ref frequency);
                GetDPXConfigParams(ref dpxConfig);
                dpxConfig.cf = frequency;
            } else {
                Debug.Log($"ERROR: CONFIG_SetCenterFreq returned error code: {error}.");
            }
        }

        Debug.Log($"(Variable) Center Frequency After = {frequency} Hz");
        Debug.Log($"(RSA) Updated Frequency = {updatedFreq} Hz");
    }

    public void UpdateFrequency()
    {
        //Calls a touch screen keyboard for user to input value in Ghz
        CONFIG_GetCenterFreq(ref frequency);
        Debug.Log($"(Variable) Center Frequency Before = {frequency} Hz");
        freqKeyboard = TouchScreenKeyboard.Open("");
        if (freqKeyboard.status == TouchScreenKeyboard.Status.Done)
        {
            double newFrequency = Convert.ToDouble(freqKeyboard.text);
            CONFIG_SetCenterFreq(newFrequency);
            GetDPXConfigParams(ref dpxConfig);
            dpxConfig.cf = frequency;
            DPX_Reset();
        }
        CONFIG_GetCenterFreq(ref frequency);
        Debug.Log($"(Variable) Center Frequency After = {frequency} Hz");
    }
}
