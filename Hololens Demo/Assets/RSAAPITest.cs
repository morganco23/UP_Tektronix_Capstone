using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System;
using UnityEngine;

public class RSAAPITest : MonoBehaviour
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
        bool enableSpectrum;
        bool enableSpectrogram;
        int bitmapWidth;
        int bitmapHeight;
        int traceLength;
        float decayFactor;
        double actualRBW;
    }

    private struct DPX_FrameBuffer
    {
        
        int fftPerFrame;
        Int64 fftCount;
        Int64 frameCount;
        double timestamp;
        uint acqDataStatus;
        double minSigDuration;
        bool minSigDurOutOfRange;
        int spectrumBitmapWidth;
        int spectrumBitmapHeight;
        int spectrumBitmapSize;
        int spectrumTraceLength;
        int numSpectrumTraces;
        bool spectrumEnabled;
        bool spectrogramEnabled;
        [MarshalAs(UnmanagedType.LPArray , SizeConst = 161001)]
        float[] spectrumBitmap;
        [MarshalAs(UnmanagedType.LPArray, SizeConst = 161001)]
        float[][] spectrumTraces;
        int sogramBitmapWidth;
        int sogramBitmapHeight;
        int sogramBitmapSize;
        int sogramBitmapNumValidLines;
        byte[] sogramBitmap;
        double[] sogramBitmapTimestampArray;
        double[] sogramBitmapContainTriggerArray;
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


    // Start is called before the first frame update
    void Start()
    {
        // Search for device
        int numDevices = 0;
        int[] idList = new int[20];
        StringBuilder type = new StringBuilder(20);
        StringBuilder name = new StringBuilder(20);
        ReturnStatus error = DEVICE_Search(ref numDevices, idList, name, type);

        UnityEngine.Debug.Log("num devices found: " + numDevices);
        UnityEngine.Debug.Log(idList[1]);
        error = DEVICE_Connect(idList[0]);
        UnityEngine.Debug.Log(error);
        StringBuilder hwVersion = new StringBuilder(20);
        error = DEVICE_GetHWVersion(hwVersion);
        // UnityEngine.Debug.Log("error: " + error);
        UnityEngine.Debug.Log(name);
        error = DEVICE_Run();
        UnityEngine.Debug.Log(error);

        DPX_SettingStruct dpxSettings = new DPX_SettingStruct();
        error = CONFIG_SetCenterFreq(2400000.00);
        error = CONFIG_SetCenterFreq(0.00);
        UnityEngine.Debug.Log(error);
        
        error = DPX_SetParameters(40000000, 300000, 801, 1, 0, 0, -100, true, 1.0, false);
        error = DPX_Configure(true, false);
        error = DPX_SetSpectrumTraceType(0, TraceType.TraceTypeMax);
        error = DPX_SetSpectrumTraceType(1, TraceType.TraceTypeMin);
        error = DPX_SetSpectrumTraceType(2, TraceType.TraceTypeAverage);

        error = DPX_GetSettings(ref dpxSettings);
        //UnityEngine.Debug.Log(dpxSettings);
        UnityEngine.Debug.Log(DPX_SetEnable(true));

    }

    // Update is called once per frame
    void Update()
    {


        
        bool doFrame = true;
        // only update once every 2 frames
        if(doFrame)
        {
            ReturnStatus rs;
            bool frameAvailable = false;
            bool ready = false;
            bool available = false;
            DPX_FrameBuffer fb = new DPX_FrameBuffer();

            rs = DEVICE_Run();
            rs = DPX_Reset();

            rs = DPX_IsFrameBufferAvailable(ref ready);
            UnityEngine.Debug.Log(ready);
            if(rs == 0 && ready)
            {
                rs = DPX_IsFrameBufferAvailable(ref available);
                
                
            }
            if(available)
            {
                rs = DPX_GetFrameBuffer(ref fb); // DOES NOT WORK YET

                UnityEngine.Debug.Log("Error is: ");
            }
            
            // generate bmp file


        }
        else
        {
           DPX_FinishFrameBuffer();
        }
        doFrame = !doFrame;
        

    }
}
