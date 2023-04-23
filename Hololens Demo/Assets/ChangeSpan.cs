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

    public void IncreaseSpan()
    {
        Debug.Log($"Span was {getDPXConfigSpan()} Hz");
        double newSpan = getDPXConfigSpan() + 1e6;
        if (newSpan <= SPAN_MAX)
        {
            setDPXConfigSpan(newSpan);
            Debug.Log($"Span is now {newSpan} Hz");
        }
        else
        {
            Debug.Log("Span cannot go beyond 40 MHz. :(");
        }
    }

    public void DecreaseSpan()
    {
        Debug.Log($"Span was {getDPXConfigSpan()} Hz");
        double newSpan = getDPXConfigSpan() - 1e6;
        if (newSpan >= SPAN_MIN)
        {
            setDPXConfigSpan(newSpan);
            Debug.Log($"Span is now {newSpan} Hz");
        }
        else
        {
            Debug.Log("Span cannot be a negative number. Sorry! :(");
        }
    }
}
