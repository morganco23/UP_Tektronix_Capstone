using Microsoft.MixedReality.Toolkit.Experimental.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ChangeRBW : MonoBehaviour
{
    // This function returns the current spectrum settings
    //[DllImport("rsa_api", EntryPoint = "SPECTRUM_GetSettings")]
    //private static extern RSAAPITest.ReturnStatus SPECTRUM_GetSettings(ref RSAAPITest.Spectrum_Settings settings);

    // This function modifies the spectrum settings
    //[DllImport("rsa_api", EntryPoint = "SPECTRUM_SetSettings")]
    //private static extern RSAAPITest.ReturnStatus SPECTRUM_SetSettings(RSAAPITest.Spectrum_Settings settings);

    [DllImport("rsa_api", EntryPoint = "DPX_Configure")]
    private static extern RSAAPITest.ReturnStatus DPX_Configure(bool enableSpectrum, bool enableSpectrogram);
	
	[DllImport("rsa_api", EntryPoint = "DPX_SetParameters")]
    private static extern RSAAPITest.ReturnStatus DPX_SetParameters(
       double fspan,
       double rbw,
       int bitmapWidth,
       int tracePtsPerPixel,
       RSAAPITest.VerticalUnitType yUnit,
       double yTop,
       double yBottom,
       bool infinitePersistence,
       double persistenceTimeSec,
       bool showOnlyTrigFrame
       );

    //public static double rbw = 300e3;
	public const double RBW_MIN = 0.0;
    public RSAAPITest.DPX_Config dpxConfig;
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
        RSAAPITest.GetDPXConfigParams(ref dpxConfig);
        Debug.Log($"The resolution bandwidth before was {dpxConfig.rbw}");
		DPX_SetParameters(dpxConfig.span, dpxConfig.rbw+1e3, 801, 1, 0, 0, -100, true, 1.0, false);
        //SPECTRUM_GetSettings(ref specSettings);
        //specSettings.rbw = rbw + 1e3;
        //SPECTRUM_SetSettings(specSettings);
        dpxConfig.rbw += 1e3;
        DPX_Configure(true, true);
        Debug.Log($"The resolution bandwidth is now {dpxConfig.rbw}");
    }

    public void DecreaseRBW()
    {
        Debug.Log($"The resolution bandwidth before was {dpxConfig.rbw}");
		if (dpxConfig.rbw-1e3 > 0)
        {
            RSAAPITest.GetDPXConfigParams(ref dpxConfig);
            DPX_SetParameters(dpxConfig.span, dpxConfig.rbw-1e3, 801, 1, 0, 0, -100, true, 1.0, false);
            //SPECTRUM_GetSettings(ref specSettings);
            //specSettings.rbw = rbw - 1e3;
            //SPECTRUM_SetSettings(specSettings);
            dpxConfig.rbw -= 1e3;
            DPX_Configure(true, true);
		}
        Debug.Log($"The resolution bandwidth is now {dpxConfig.rbw}");
    }

    public void UpdateRBW() 
    {
        MixedRealityKeyboard RBWkeyboard = GetComponent<MixedRealityKeyboard>();
        double newRBW = Convert.ToDouble(RBWkeyboard.Text);
        RSAAPITest.GetDPXConfigParams(ref dpxConfig);
        DPX_SetParameters(dpxConfig.span, newRBW, 801, 1, 0, 0, -100, true, 1.0, false);
        dpxConfig.rbw = newRBW;
        DPX_Configure(true, true);
    }
}
