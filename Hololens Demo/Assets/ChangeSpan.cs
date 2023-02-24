using Microsoft.MixedReality.Toolkit.Experimental.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ChangeSpan : MonoBehaviour
{

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

    private RSAAPITest.DPX_Config dpxConfig;
    private MixedRealityKeyboard spanKeyboard;
    public const double SPAN_MIN = 0.0;
    public const double SPAN_MAX = 40e6;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * This method increases the span of the DPX Spectrum by 1 MHz.
     */
    public void IncreaseSpan()
    {
        RSAAPITest.GetDPXConfigParams(ref dpxConfig); 
        Debug.Log($"Span is now {dpxConfig.span} Hz");
        if ((dpxConfig.span + 1e6) <= SPAN_MAX)
        {
			DPX_SetParameters(dpxConfig.span + 1e6, dpxConfig.rbw, 801, 1, 0, 0, -100, true, 1.0, false);
            dpxConfig.span += 1e6;
            DPX_Configure(true, true);
        }
        Debug.Log($"Span is now {dpxConfig.span} Hz");

    }

    /*
     * This method decreases the span of the DPX Spectrum by 1 MHz.
     */
    public void DecreaseSpan()
    {
        RSAAPITest.GetDPXConfigParams(ref dpxConfig);
        Debug.Log($"Span before was {dpxConfig.span} Hz");
        if ((dpxConfig.span - 1e6) > SPAN_MIN)
        {
            DPX_SetParameters(dpxConfig.span - 1e6, dpxConfig.rbw, 801, 1, 0, 0, -100, true, 1.0, false);
            dpxConfig.span -= 1e6;
            DPX_Configure(true, true);
        }
        Debug.Log($"Span is now {dpxConfig.span} Hz");
    }

    public void StartUpdateSpan()
    {
        MixedRealityKeyboard spanKeyboard = GetComponent<MixedRealityKeyboard>();
        spanKeyboard.ShowKeyboard();
    }

    /*
     * This method changes the span to a user-defined value.
     */
    public void FinishUpdateSpan()
    {
        double newRBW = Convert.ToDouble(spanKeyboard.Text);
        RSAAPITest.GetDPXConfigParams(ref dpxConfig);
        DPX_SetParameters(dpxConfig.span, newRBW, 801, 1, 0, 0, -100, true, 1.0, false);
        dpxConfig.rbw = newRBW;
        DPX_Configure(true, true);
    }
}
