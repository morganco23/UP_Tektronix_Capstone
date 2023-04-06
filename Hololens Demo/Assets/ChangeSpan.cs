using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static RSAAPITest;

public class ChangeSpan : MonoBehaviour
{
    [DllImport("rsa_api", EntryPoint = "DPX_Configure")]
    private static extern ReturnStatus DPX_Configure(bool enableSpectrum, bool enableSpectrogram);

    [DllImport("rsa_api", EntryPoint = "DPX_SetParameters")]
    private static extern ReturnStatus DPX_SetParameters(
       double fspan,
       double rbw,
       int bitmapWidth,
       int tracePtsPerPixel,
       VerticalUnitType yUnit,
       double yTop,
       double yBottom,
       bool infinitePersistence,
       double persistenceTimeSec,
       bool showOnlyTrigFrame
       );

    private DPX_Config dpxConfig;
    public const double SPAN_MIN = 0.0;
    public const double SPAN_MAX = 40e6;
    // Start is called before the first frame update
    void Start()
    {
        dpxConfig = getDPXConfig();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseSpan()
    {
        Debug.Log($"Span was {dpxConfig.span} Hz");
        Debug.Log($"New span = {dpxConfig.span + 1e6} Hz");
        double newSpan = dpxConfig.span + 1e6;
        if (newSpan <= SPAN_MAX)
        {
            //DPX_SetParameters(newSpan, dpxConfig.rbw, 801, 1, 0, 0, -100, true, 1.0, false);
            dpxConfig.span += 1e6;
            //DPX_Configure(true, true);
        }
        Debug.Log($"Span is now {dpxConfig.span} Hz");
    }

    public void DecreaseSpan()
    {
        Debug.Log($"Span was {dpxConfig.span} Hz");
        if ((dpxConfig.span - 1e6) >= SPAN_MIN)
        {
            //DPX_SetParameters(dpxConfig.span - 1e6, dpxConfig.rbw, 801, 1, 0, 0, -100, true, 1.0, false);
            dpxConfig.span -= 1e6;
            //DPX_Configure(true, true);
        }
        Debug.Log($"Span is now {dpxConfig.span} Hz");
    }
}
