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
        noError = 0,


        // Connection
        errorNotConnected = 101,
        errorIncompatibleFirmware = 102,
        errorBootLoaderNotRunning = 103,
        errorTooManyBootLoadersConnected = 104,
        errorRebootFailure = 105,

        // POST
        errorPOSTFailureFPGALoad = 201,
        errorPOSTFailureHiPower = 202,
        errorPOSTFailureI2C = 203,
        errorPOSTFailureGPIF = 204,
        errorPOSTFailureUsbSpeed = 205,
        errorPOSTDiagFailure = 206,

        // General Msmt
        errorBufferAllocFailed = 301,
        errorParameter = 302,
        errorDataNotReady = 304,

        // Spectrum
        errorParameterTraceLength = 1101,
        errorMeasurementNotEnabled = 1102,
        errorSpanIsLessThanRBW = 1103,
        errorFrequencyOutOfRange = 1104,

        // IF streaming
        errorStreamADCToDiskFileOpen = 1201,
        errorStreamADCToDiskAlreadyStreaming = 1202,
        errorStreamADCToDiskBadPath = 1203,
        errorStreamADCToDiskThreadFailure = 1204,
        errorStreamedFileInvalidHeader = 1205,
        errorStreamedFileOpenFailure = 1206,
        errorStreamingOperationNotSupported = 1207,
        errorStreamingFastForwardTimeInvalid = 1208,
        errorStreamingInvalidParameters = 1209,
        errorStreamingEOF = 1210,

        // IQ streaming
        errorIQStreamInvalidFileDataType = 1301,
        errorIQStreamFileOpenFailed = 1302,
        errorIQStreamBandwidthOutOfRange = 1303,

        // -----------------
        // Internal errors
        // -----------------
        errorTimeout = 3001,
        errorTransfer = 3002,
        errorFileOpen = 3003,
        errorFailed = 3004,
        errorCRC = 3005,
        errorChangeToFlashMode = 3006,
        errorChangeToRunMode = 3007,
        errorDSPLError = 3008,
        errorLOLockFailure = 3009,
        errorExternalReferenceNotEnabled = 3010,
        errorLogFailure = 3011,
        errorRegisterIO = 3012,
        errorFileRead = 3013,

        errorDisconnectedDeviceRemoved = 3101,
        errorDisconnectedDeviceNodeChangedAndRemoved = 3102,
        errorDisconnectedTimeoutWaitingForADcData = 3103,
        errorDisconnectedIOBeginTransfer = 3104,
        errorOperationNotSupportedInSimMode = 3015,

        errorFPGAConfigureFailure = 3201,
        errorCalCWNormFailure = 3202,
        errorSystemAppDataDirectory = 3203,
        errorFileCreateMRU = 3204,
        errorDeleteUnsuitableCachePath = 3205,
        errorUnableToSetFilePermissions = 3206,
        errorCreateCachePath = 3207,
        errorCreateCachePathBoost = 3208,
        errorCreateCachePathStd = 3209,
        errorCreateCachePathGen = 3210,
        errorBufferLengthTooSmall = 3211,
        errorRemoveCachePath = 3212,
        errorGetCachingDirectoryBoost = 3213,
        errorGetCachingDirectoryStd = 3214,
        errorGetCachingDirectoryGen = 3215,
        errorInconsistentFileSystem = 3216,

        errorWriteCalConfigHeader = 3301,
        errorWriteCalConfigData = 3302,
        errorReadCalConfigHeader = 3303,
        errorReadCalConfigData = 3304,
        errorEraseCalConfig = 3305,
        errorCalConfigFileSize = 3306,
        errorInvalidCalibConstantFileFormat = 3307,
        errorMismatchCalibConstantsSize = 3308,
        errorCalConfigInvalid = 3309,

        // flash
        errorFlashFileSystemUnexpectedSize = 3401,
        errorFlashFileSystemNotMounted = 3402,
        errorFlashFileSystemOutOfRange = 3403,
        errorFlashFileSystemIndexNotFound = 3404,
        errorFlashFileSystemReadErrorCRC = 3405,
        errorFlashFileSystemReadFileMissing = 3406,
        errorFlashFileSystemCreateCacheIndex = 3407,
        errorFlashFileSystemCreateCachedDataFile = 3408,
        errorFlashFileSystemUnsupportedFileSize = 3409,
        errorFlashFileSystemInsufficentSpace = 3410,
        errorFlashFileSystemInconsistentState = 3411,
        errorFlashFileSystemTooManyFiles = 3412,
        errorFlashFileSystemImportFileNotFound = 3413,
        errorFlashFileSystemImportFileReadError = 3414,
        errorFlashFileSystemImportFileError = 3415,
        errorFlashFileSystemFileNotFoundError = 3416,
        errorFlashFileSystemReadBufferTooSmall = 3417,
        errorFlashWriteFailure = 3418,
        errorFlashReadFailure = 3419,
        errorFlashFileSystemBadArgument = 3420,
        errorFlashFileSystemCreateFile = 3421,

        // Aux monitoring
        errorMonitoringNotSupported = 3501,
        errorAuxDataNotAvailable = 3502,

        // battery
        errorBatteryCommFailure = 3601,
        errorBatteryChargerCommFailure = 3602,
        errorBatteryNotPresent = 3603,

        // EST
        errorESTOutputPathFile = 3701,
        errorESTPathNotDirectory = 3702,
        errorESTPathDoesntExist = 3703,
        errorESTUnableToOpenLog = 3704,
        errorESTUnableToOpenLimits = 3705,

        // Revision information
        errorRevisionDataNotFound = 3801,

        // alignment
        error112MHzAlignmentSignalLevelTooLow = 3901,
        error10MHzAlignmentSignalLevelTooLow = 3902,
        errorInvalidCalConstant = 3903,
        errorNormalizationCacheInvalid = 3904,
        errorInvalidAlignmentCache = 3905,

        // acq status
        errorADCOverrange = 9000,  // must not change the location of these error codes without coordinating with MFG TEST
        errorOscUnlock = 9001,

        errorNotSupported = 9901,

        errorPlaceholder = 9999,
        notImplemented = -1
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

    /*public enum SpectrumWindows
    {
        SpectrumWindow_Kaiser,
        SpecturmWindow_Mil6dB,
        SpectrumWindow_BlackmanHarris,
        SpectrumWindow_Rectangular,
        SpectrumWindow_FlatTop,
        SpectrumWindow_Hann
    }

    public enum SpectrumVerticalUnits 
    {
        SpectrumVerticalUnit_dBm,
        SpectrumVerticalUnit_Watt,
        SpectrumVerticalUnit_Volt,
        SpectrumVerticalUnit_Amp,
        SpectrumVerticalUnit_dBmV
    }*/

    public struct DPX_SettingStruct
    {
        bool enableSpectrum;
        bool enableSpectrogram;
        public int bitmapWidth;
        public int bitmapHeight;
        int traceLength;
        float decayFactor;
        double actualRBW;
    }

    /*public struct Spectrum_Settings
    {
        public double span;
        public double rbw;
        bool enableVBW;
        double vbw;
        public int traceLength;
        SpectrumWindows window;
        SpectrumVerticalUnits verticalUnit;
        public double actualStartFreq;
        double actualStopFreq;
        public double actualFreqStepSize;
        double actualRBW;
        double VBW;
        int actualNumIQSamples;
    }*/

    public struct DPX_FrameBuffer
    {
        int fftPerFrame;
        Int64 fftCount;
        Int64 frameCount;
        double timestamp;
        uint acqDataStatus;
        double minSigDuration;
        bool minSigDurOutOfRange;
        public int spectrumBitmapWidth;
        public int spectrumBitmapHeight;
        public int spectrumBitmapSize;
        public int spectrumTraceLength;
        int numSpectrumTraces;
        bool spectrumEnabled;
        bool spectrogramEnabled;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 161001)]
        public float[] spectrumBitmap;
        [MarshalAs(UnmanagedType.LPArray, SizeConst = 161001)]
        public float[][] spectrumTraces;
        public int sogramBitmapWidth;
        public int sogramBitmapHeight;
        public int sogramBitmapSize;
        public int sogramBitmapNumValidLines;
        public byte[] sogramBitmap;
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

    public struct DPX_Config
    {
        public double cf { get; set; }
        public double refLevel { get; set; }
        public double span { get; set; }
        public double rbw { get; set; }
    }

    public static DPX_FrameBuffer fb;

    public static DPX_Config GetDPXConfigParams(ref DPX_Config dpxConfig) 
    {
        return dpxConfig;
    }

    public static DPX_FrameBuffer GetCurrentFrameBuffer()
    {
        return fb;
    }

    // Start is called before the first frame update (Automatically called by Unity on start)
    void Start()
    {
        // Search for device
        int numDevices = 0;
        int[] idList = new int[20];
        StringBuilder type = new StringBuilder(20);
        StringBuilder name = new StringBuilder(20);
        ReturnStatus error = DEVICE_Search(ref numDevices, idList, name, type);

        Debug.Log("num devices found: " + numDevices);
        Debug.Log(idList[1]);
        error = DEVICE_Connect(idList[0]);
        Debug.Log(error);
        StringBuilder hwVersion = new StringBuilder(20);
        error = DEVICE_GetHWVersion(hwVersion);
        // UnityEngine.Debug.Log("error: " + error);
        Debug.Log(name);
        error = DEVICE_Run();
        Debug.Log(error);

        DPX_SettingStruct dpxSettings = new DPX_SettingStruct();
        error = CONFIG_SetCenterFreq(2400000000.00);
        error = CONFIG_SetReferenceLevel(0.00);
        Debug.Log(error);
        
        error = DPX_SetParameters(40000000, 300000, 801, 1, 0, 0, -100, true, 1.0, false);
        error = DPX_Configure(true, false);
        error = DPX_SetSpectrumTraceType(0, TraceType.TraceTypeMaxHold);
        error = DPX_SetSpectrumTraceType(1, TraceType.TraceTypeMinHold);
        error = DPX_SetSpectrumTraceType(2, TraceType.TraceTypeAverage);
        DPX_Config dpxConfig = new DPX_Config();
        dpxConfig.cf = 2400000000.00;
        dpxConfig.refLevel = 0.00;
        dpxConfig.span = 40000000;
        dpxConfig.rbw = 300000;
        error = DPX_GetSettings(ref dpxSettings);
        //UnityEngine.Debug.Log(dpxSettings);
        Debug.Log(DPX_SetEnable(true));

    }

    // Update is called once per frame (auto-loops; called after Start())
    void Update()
    {
            // TODO: 
            // 1. Get the latest DPX settings (span, frequency, amplitude).
            // 2. Call graph function which uses ScottPlot to graph the given settings

            bool doFrame = true;
            // only update once every 2 frames
            if(doFrame)
            {
                ReturnStatus rs;
                bool frameReady = false;
                bool isAvailable = false;
                DPX_FrameBuffer fb = new DPX_FrameBuffer();

                rs = DEVICE_Run();
                if(rs != 0){
                    Debug.Log($"ERROR: DEVICE_Run error code {rs}");
                }

                rs = DPX_Reset();
                if(rs != 0){
                    Debug.Log($"ERROR: DPX_Reset error code {rs}");
                }

                rs = DPX_IsFrameBufferAvailable(ref isAvailable);
                if(rs != 0){
                    Debug.Log($"ERROR: DPX_IsFrameBufferAvailable error code {rs}");
                }

                Debug.Log($"STATUS: Is Frame Buffer available: {isAvailable}");

                if(rs == 0 && isAvailable)
                {
                    rs = DPX_WaitForDataReady(100, ref frameReady);
                    if(rs != 0){
                        Debug.Log($"ERROR: DPX_IsFrameBufferAvailable error code {rs}");
                    } 
                }

                if(frameReady)
                {
                    rs = DPX_GetFrameBuffer(ref fb); // DOES NOT WORK YET
                    Debug.Log("Error is: ");
                }

                // generate bmp file
                doFrame = !doFrame;

            } else {
                DPX_FinishFrameBuffer();
            }
        }
        

}

 
