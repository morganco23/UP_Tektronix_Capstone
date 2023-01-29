using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ChangeSpan : MonoBehaviour
{
    //This function returns the current spectrum settings
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

    //public RSAAPITest.DPX_SettingStruct specSettings;
    private RSAAPITest.DPX_Config dpxConfig;
    //public static double span = 40e6;
    public const double SPAN_MIN = 0.0;
    public const double SPAN_MAX = 40e6;
    RSAAPITest.ReturnStatus error;

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
        RSAAPITest.GetDPXConfigParams(ref dpxConfig); 
        Debug.Log($"Span is now {dpxConfig.span} Hz");
        if ((dpxConfig.span + 1e6) <= SPAN_MAX)
        {
			DPX_SetParameters(dpxConfig.span + 1e6, dpxConfig.rbw, 801, 1, 0, 0, -100, true, 1.0, false);
            //SPECTRUM_GetSettings(ref specSettings);
            //specSettings.span = span + 1e6;
            dpxConfig.span += 1e6;
            //SPECTRUM_SetSettings(specSettings);
            DPX_Configure(true, true);
        }
        Debug.Log($"Span is now {dpxConfig.span} Hz");

    }

    public void DecreaseSpan()
    {
        RSAAPITest.GetDPXConfigParams(ref dpxConfig);
        Debug.Log($"Span before was {dpxConfig.span} Hz");
        if ((dpxConfig.span - 1e6) > SPAN_MIN)
        {
            DPX_SetParameters(dpxConfig.span - 1e6, dpxConfig.rbw, 801, 1, 0, 0, -100, true, 1.0, false);
            //SPECTRUM_GetSettings(ref specSettings);
            //specSettings.span = span - 1e6;
            dpxConfig.span -= 1e6;
            //SPECTRUM_SetSettings(specSettings);
            DPX_Configure(true, true);
        }
        Debug.Log($"Span is now {dpxConfig.span} Hz");
    }

    public void UpdateSpan() 
    { 
    
    }
}
