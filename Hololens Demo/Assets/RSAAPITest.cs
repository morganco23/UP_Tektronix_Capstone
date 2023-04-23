using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public unsafe class RSAAPITest : MonoBehaviour
{
    public enum ReturnStatus
    {
        noError = 0
    }

    public enum TriggerMode
    {
        freeRun,
        Triggered
    }

    public enum TriggerSource
    {
        TriggerSourceExternal,
        TriggerSourceIFPowerLevel
    }

    public enum TriggerTransition
    {
        TriggerTransitionLH = 1,
        TriggerTransitionHL = 2,
        TriggerTransitionEither = 3
    }

    public enum VerticalUnitType
    {
        VerticalUnit_dBm,
        VerticalUnit_Watt,
        VerticalUnit_Volt,
        VerticalUnit_Amp
    }

    public enum TraceType
    {
        TraceTypeAverage,
        TraceTypeMax,
        TraceTypeMaxHold,
        TraceTypeMin,
        TraceTypeMinHold
    }

    public struct DPX_SettingStruct
    {
        public bool enableSpectrum;
        public bool enableSpectrogram;
        public int bitmapWidth;
        public int bitmapHeight;
        public int traceLength;
        public float decayFactor;
        public double actualRBW;
    }

    public unsafe struct DPX_FrameBuffer
    {
        
        int fftPerFrame;
        Int64 fftCount;
        Int64 frameCount;
        public double timestamp;
        uint acqDataStatus;
        double minSigDuration;
        bool minSigDurOutOfRange;
        public int spectrumBitmapWidth;
        public int spectrumBitmapHeight;
        public int spectrumBitmapSize;
        public int spectrumTraceLength;
        public int numSpectrumTraces;
        bool spectrumEnabled;
        bool spectrogramEnabled;
        public float* spectrumBitmap;
        public float** spectrumTraces;
        int sogramBitmapWidth;
        int sogramBitmapHeight;
        int sogramBitmapSize;
        int sogramBitmapNumValidLines;
        byte* sogramBitmap;
        double* sogramBitmapTimestampArray;
        double* sogramBitmapContainTriggerArray;
    }


    [DllImport("rsa_api", EntryPoint = "DEVICE_Search")]
    private static extern ReturnStatus DEVICE_Search(
            ref int numFound,
            int[] idList,
            StringBuilder name,
            StringBuilder type
        );

    

    [DllImport("rsa_api", EntryPoint = "DEVICE_Connect")]
    private static extern ReturnStatus DEVICE_Connect(int id);

    [DllImport("rsa_api", EntryPoint = "DEVICE_GetHWVersion")]
    private static extern ReturnStatus DEVICE_GetHWVersion(StringBuilder hwVersion);

    [DllImport("rsa_api", EntryPoint = "DEVICE_Run")]
    private static extern ReturnStatus DEVICE_Run();

    // Device Reset
    [DllImport("rsa_api", EntryPoint = "DEVICE_Reset")]
    private static extern ReturnStatus DEVICE_Reset(int deviceID);

    // Device Get Serial Number
    [DllImport("rsa_api", EntryPoint = "DEVICE_GetSerialNumber")]
    private static extern ReturnStatus DEVICE_GetSerialNumber(StringBuilder serialNum);

    // Device Get Nomenclature
    [DllImport("rsa_api", EntryPoint = "DEVICE_GetNomenclature")]
    private static extern ReturnStatus DEVICE_GetNomenclature(StringBuilder nomenclature);

    // Device Disconnect
    [DllImport("rsa_api", EntryPoint = "DEVICE_Disconnect")]
    private static extern ReturnStatus DEVICE_Disconnect();

    // Config Preset
    [DllImport("rsa_api", EntryPoint = "CONFIG_Preset")]
    private static extern ReturnStatus CONFIG_Preset();

    // Config Set Center Frequency
    [DllImport("rsa_api", EntryPoint = "CONFIG_SetCenterFreq")]
    private static extern ReturnStatus CONFIG_SetCenterFreq(double centerFreq);

    // Config Set Reference Level
    [DllImport("rsa_api", EntryPoint = "CONFIG_SetReferenceLevel")]
    private static extern ReturnStatus CONFIG_SetReferenceLevel(double refLevel);


    // DPX Set Enable
    [DllImport("rsa_api", EntryPoint = "DPX_SetEnable")]
    private static extern ReturnStatus DPX_SetEnable(bool enabled);

    // DPX Set Parameters
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


    // DPX Set Sogram Parameters
    [DllImport("rsa_api", EntryPoint = "DPX_SetSogramParameters")]
    private static extern ReturnStatus DPX_SetSogramParameters(
        double timePerBitmapLine,
        double timeResolution,
        double maxPower,
        double minPower);


    // DPX Configure
    [DllImport("rsa_api", EntryPoint = "DPX_Configure")]
    private static extern ReturnStatus DPX_Configure(bool enableSpectrum, bool enableSpectrogram);

    // DPX Reset
    [DllImport("rsa_api", EntryPoint = "DPX_Reset")]
    private static extern ReturnStatus DPX_Reset();

    // DPX Set Spectrum Trace Type
    [DllImport("rsa_api", EntryPoint = "DPX_SetSpectrumTraceType")]
    private static extern ReturnStatus DPX_SetSpectrumTraceType(int traceIndex, TraceType type);

    // DPX get Settings
    [DllImport("rsa_api", EntryPoint = "DPX_GetSettings")]
    private static extern ReturnStatus DPX_GetSettings(ref DPX_SettingStruct dpxSettings);

    // DPX Is Fram Buffer Available
    [DllImport("rsa_api", EntryPoint = "DPX_IsFrameBufferAvailable")]
    private static extern ReturnStatus DPX_IsFrameBufferAvailable(ref bool frameAvailable);

    // DPX Wait For Data Ready
    [DllImport("rsa_api", EntryPoint = "DPX_WaitForDataReady")]
    private static extern ReturnStatus DPX_WaitForDataReady(int timeoutMsec, ref bool ready);

    // DPX Get Frame Buffer
    [DllImport("rsa_api", EntryPoint = "DPX_GetFrameBuffer")]
    private static extern ReturnStatus DPX_GetFrameBuffer(ref DPX_FrameBuffer frameBuffer);

    // DPX Finish Frame Buffer
    [DllImport("rsa_api", EntryPoint = "DPX_FinishFrameBuffer")]
    private static extern ReturnStatus DPX_FinishFrameBuffer();

    // TRIG Set Trigger Mode
    [DllImport("rsa_api", EntryPoint = "TRIG_SetTriggerMode")]
    private static extern ReturnStatus TRIG_SetTriggerMode(TriggerMode mode);

    // TRIG Set IF Power Trigger Level


    // TRIG Set Trigger Source
    [DllImport("rsa_api", EntryPoint = "TRIG_SetTriggerSource")]
    private static extern ReturnStatus TRIG_SetTriggerSource(TriggerMode source);

    // TRIG Set Trigger Position Percent
    [DllImport("rsa_api", EntryPoint = "TRIG_SetTriggerPositionPercent")]
    private static extern ReturnStatus TRIG_SetTriggerPositionPercent(double trigPosPercent);

    bool doFrame;
    RawImage img;
    
    public struct DPX_Config
    {
        public double cf;
        public double refLevel;
        public double span;
        public double rbw;
    }

    private static DPX_Config dpxConfig;

    public static double getDPXConfigFrequency()
    {
        return dpxConfig.cf;
    }

    public static double getDPXConfigSpan()
    {
        return dpxConfig.span;
    }

    public static double getDPXConfigRefLevel()
    {
        return dpxConfig.refLevel;
    }

    public static void setDPXConfigRBW(double newRBW)
    {
        dpxConfig.rbw = newRBW;
    }

    public static void setDPXConfigSpan(double newSpan)
    {
        dpxConfig.span = newSpan;
    }

    public static void setDPXConfigFrequency(double newCf)
    {
        dpxConfig.cf = newCf;
    }

    public static void setDPXConfigRefLevel(double newRefLevel)
    {
        dpxConfig.refLevel = newRefLevel;
    }

    // Start is called before the first frame update
    void Start()
    {
        doFrame = false;
        img = GetComponent<RawImage>();
        // Search for device
        int numDevices = 0;
        int[] idList = new int[20];
        StringBuilder type = new StringBuilder(20);
        StringBuilder name = new StringBuilder(20);
        ReturnStatus error = DEVICE_Search(ref numDevices, idList, name, type);

        Debug.Log("num devices found: " + numDevices);
        error = DEVICE_Connect(idList[0]);
        Debug.Log(error);
        StringBuilder hwVersion = new StringBuilder(20);
        error = DEVICE_GetHWVersion(hwVersion);
        

        DPX_SettingStruct dpxSettings = new DPX_SettingStruct();
        DPX_Config dpxConfig = new DPX_Config();
        setDPXConfigFrequency(2400000000.00);
        setDPXConfigRBW(300000);
        setDPXConfigSpan(40000000);
        setDPXConfigRefLevel(-20.0);

        error = CONFIG_SetCenterFreq(2400000000.00);
        CONFIG_SetReferenceLevel(-20.0);

        error = DPX_SetParameters(40000000, 300000, 801, 1, 0, 0, -100, false, 1, false);
        error = DPX_SetSpectrumTraceType(0, TraceType.TraceTypeAverage);
        error = DPX_Configure(true, false);

        error = DPX_GetSettings(ref dpxSettings);
        
        DPX_SetEnable(true);

        DEVICE_Run();

    }

    // Update is called once per frame
    void Update()
    {
        // call every other frame
       

        // Get DPX Frame
        ReturnStatus rs;
        bool ready = false;
        bool available = false;
        var fb = new DPX_FrameBuffer();

        bool isDpxReady = false;
        rs = DPX_WaitForDataReady(300, ref ready);

        // If DPX is ready, check if the frame buffer is available.
        while (ready)
        {
            rs = DPX_IsFrameBufferAvailable(ref available);
            if (available)
            {
                DPX_GetFrameBuffer(ref fb);
                break;
            }
        }



            
        int bitmapWidth = fb.spectrumBitmapWidth;
        int bitmapHeight = fb.spectrumBitmapHeight;
        int bitmapSize = fb.spectrumBitmapSize;
        float* bitmap = fb.spectrumBitmap;
        if (bitmapSize > 0)
        {

            // Generate csv of bitmap data
            // convert float* bitmap to actual colors.
            Color32[] pngBytes = new Color32[bitmapSize];
            Color32 g = new Color32(0, 255, 0, 255);
            Color32 bl = new Color32(0, 0, 0, 255);

            var bitmapFile = new System.IO.StreamWriter("DPXBitmap1.csv");

            for (int nh = 0; nh < bitmapHeight; nh++)
            {
                for (int nw = 0; nw < bitmapWidth; nw++)
                {
                    bitmapFile.Write("{0},", bitmap[nh * bitmapWidth + nw]);
                }
                bitmapFile.WriteLine();
            }
            bitmapFile.Close();

            // UnityEngine.Debug.Log(bitmapSize);
            for (int i = 0; i < bitmapSize; i++)
            {
                if (bitmap[i] == 0.0)
                {
                    pngBytes[i] = bl;
                }
                else
                {
                    pngBytes[i] = Color32.Lerp(bl, g, bitmap[i]/5);
                }
            }
            // UnityEngine.Debug.Log(pngBytes.Length);
            Texture2D texture = null;
            texture = new Texture2D(801, 201);
            texture.SetPixels32(pngBytes);

            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes("Assets/image.png", bytes);

        }

        DPX_FinishFrameBuffer();

        
    }
}
