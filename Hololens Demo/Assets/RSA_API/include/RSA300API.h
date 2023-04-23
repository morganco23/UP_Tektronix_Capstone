/**************************************************************************************************
*  RSA300_API.h -- RSA306 API SW interface definition -- LEGACY SUPPORT ONLY !!!
*
*  !!! DEPRECATION WARNING !!! DEPRECATION WARNING !!! DEPRECATION WARNING !!! DEPRECATION WARNING !!! 
*  !!! PLEASE READ !!! PLEASE READ !!! PLEASE READ !!! PLEASE READ !!! PLEASE READ !!! PLEASE READ !!!
*
*  >>>  The RSA300 API interface (AKA "V1") defined in this file is DEPRECATED.  
*  >>>  It should NOT BE USED for new work.
*  >>>  This file provides legacy support for applications already using the RSA300 V1 API definition.
*  >>>
*  >>>  V1 support may be removed in a future release.  API users are strongly encouraged to 
*  >>>  migrate applications to the V2 API definition provided in "RSA_API.h".  The V2 API is 
*  >>>  compatible with all RSA306/RSA306B/RSA500A/RSA600A devices.  See the API Programming
*  >>>  Reference manual for information on how to migrate V1 usage to V2 usage.
*
*  !!! PLEASE READ !!! PLEASE READ !!! PLEASE READ !!! PLEASE READ !!! PLEASE READ !!! PLEASE READ !!!
*  !!! DEPRECATION WARNING !!! DEPRECATION WARNING !!! DEPRECATION WARNING !!! DEPRECATION WARNING !!!
*
*  Copyright (c) Tektronix Incorporated 2016.  All rights reserved. 
*  Licensed software products are owned by Tektronix or its subsidiaries or suppliers, 
*  and are protected by national copyright laws and international treaty provisions.
***************************************************************************************************/

#ifndef RSA300_API_H
#define RSA300_API_H

// --- Define import macro for Windows builds
#ifdef _WIN32
#define RSA300_API_DLL __declspec(dllimport)
#else  // non-Windows builds
#define RSA300_API_DLL
#endif

#include <stdint.h>
#include <time.h>

#ifdef __cplusplus
namespace RSA300_API    // V1 namespace
{
    extern "C"
    {
#endif //__cplusplus

        // Status and Error Reporting enums
        #define RETURNSTATUS_ONLY   // include RSA_API.h with this macro flag set to get only the ReturnStatus type def
        #include "RSA_API.h"

        ///////////////////////////////////////////////////////////
        // Global Type Definitions
        ///////////////////////////////////////////////////////////

#ifndef __cplusplus
        // Create a bool type for "plain" C
        typedef uint8_t bool;
#ifndef false
#define false (0)
#endif
#ifndef true 
#define true (1)
#endif
#endif

        // Complex data type definitions
        typedef struct
        {
            float i;
            float q;
        } Cplx32;
        typedef struct
        {
            int32_t i;
            int32_t q;
        } CplxInt32;
        typedef struct
        {
            int16_t i;
            int16_t q;
        } CplxInt16;


        //
        // AcqDataStatus enumeration gives the bit-field defininitions for the .acqDataStatus member in the following info structs:
        //   * IQHeader (returned by GetIQHeader()
        //   * Spectrum_TraceInfo( returned by SPECTRUM_GetTraceInfo())
        //   * DPX_FrameBuffer (returned by DPX_GetFrameBuffer())
        //  NOTE: Any active (1) bits in the status word other than defined below are for internal use only and should be ignored.
        //
        typedef enum
        {
            adcOverrange = 0x1,				// Bit 0: Overrange - Input to the ADC was outside of its operating range.
            refFreqUnlock = 0x2,			// Bit 1: Reference oscillator unlocked - Loss of locked status on the reference oscillator.
            lo1Unlock = 0x4,				// Bit 2: Internal use only
            lo2Unlock = 0x8,				// Bit 3: Internal use only
            lowSupplyVoltage = 0x10,		// Bit 4: Power fail - Power (5V and Usb) failure detected.
            adcDataLost = 0x20,				// Bit 5: Dropped frame - Loss of ADC data frame samples
            event1pps = 0x40,				// Bit 6: Internal use only
            eventTrig1 = 0x80,				// Bit 7: Internal use only
            eventTrig2 = 0x100,				// Bit 8: Internal use only
        } AcqDataStatus;

        ///////////////////////////////////////////////////////////
        // Device Connection and Info
        ///////////////////////////////////////////////////////////

        RSA300_API_DLL const char* GetErrorString(ReturnStatus status);
        RSA300_API_DLL ReturnStatus Search(long deviceIDs[], wchar_t* deviceSerial[], int* numDevicesFound); //returns array of valid deviceIDs
        RSA300_API_DLL ReturnStatus Connect(long deviceID); //connect to specific deviceID
        RSA300_API_DLL ReturnStatus ResetDevice(long deviceID);
        RSA300_API_DLL ReturnStatus Disconnect();
        RSA300_API_DLL ReturnStatus GetAPIVersion(char* apiVersion);
        RSA300_API_DLL ReturnStatus GetFirmwareVersion(char* fwVersion);
        RSA300_API_DLL ReturnStatus GetFPGAVersion(char* fpgaVersion);
        RSA300_API_DLL ReturnStatus GetHWVersion(char* hwVersion);
        RSA300_API_DLL ReturnStatus GetDeviceSerialNumber(char* serialNum);
        RSA300_API_DLL ReturnStatus GetDeviceNomenclature(char* nomenclature);
        RSA300_API_DLL ReturnStatus POST_QueryStatus();

        ///////////////////////////////////////////////////////////
        // Device Configuration (global)
        ///////////////////////////////////////////////////////////

        RSA300_API_DLL ReturnStatus Preset();
        RSA300_API_DLL ReturnStatus SetReferenceLevel(double refLevel);
        RSA300_API_DLL ReturnStatus GetReferenceLevel(double* refLevel);
        RSA300_API_DLL ReturnStatus GetMaxCenterFreq(double* maxCF);
        RSA300_API_DLL ReturnStatus GetMinCenterFreq(double* minCF);
        RSA300_API_DLL ReturnStatus SetCenterFreq(double cf);
        RSA300_API_DLL ReturnStatus GetCenterFreq(double* cf);
        RSA300_API_DLL ReturnStatus GetTunedCenterFreq(double* cf);
        RSA300_API_DLL ReturnStatus SetExternalRefEnable(bool exRefEn);
        RSA300_API_DLL ReturnStatus GetExternalRefEnable(bool* exRefEn);


        ///////////////////////////////////////////////////////////
        // Trigger Configuration 
        ///////////////////////////////////////////////////////////

        typedef enum
        {
            freeRun = 0,
            triggered = 1
        } TriggerMode;

        typedef enum
        {
            TriggerSourceExternal = 0,		//  external 
            TriggerSourceIFPowerLevel = 1	//  IF power level
        } TriggerSource;

        typedef enum
        {
            TriggerTransitionLH = 1,		//  Low to High transition		
            TriggerTransitionHL = 2,		//  High to Low transition
            TriggerTransitionEither = 3		//  either Low to High or High to Low transition	
        } TriggerTransition;

        RSA300_API_DLL ReturnStatus ForceTrigger();
        RSA300_API_DLL ReturnStatus SetTriggerPositionPercent(double trigPosPercent);
        RSA300_API_DLL ReturnStatus GetTriggerPositionPercent(double* trigPosPercent);
        RSA300_API_DLL ReturnStatus SetTriggerMode(TriggerMode mode);
        RSA300_API_DLL ReturnStatus GetTriggerMode(TriggerMode* mode);
        RSA300_API_DLL ReturnStatus SetTriggerTransition(TriggerTransition transition);
        RSA300_API_DLL ReturnStatus GetTriggerTransition(TriggerTransition *transition);
        RSA300_API_DLL ReturnStatus SetTriggerSource(TriggerSource source);
        RSA300_API_DLL ReturnStatus GetTriggerSource(TriggerSource *source);
        RSA300_API_DLL ReturnStatus SetIFPowerTriggerLevel(double level);
        RSA300_API_DLL ReturnStatus GetIFPowerTriggerLevel(double *level);


        ///////////////////////////////////////////////////////////
        // Device Alignment
        ///////////////////////////////////////////////////////////

        RSA300_API_DLL ReturnStatus IsAlignmentNeeded(bool *needed);
        RSA300_API_DLL ReturnStatus RunAlignment();
        RSA300_API_DLL double GetDeviceTemperature();


        ///////////////////////////////////////////////////////////
        // Device Operation (global)
        ///////////////////////////////////////////////////////////

        typedef enum
        {
            stopped = 0,
            running = 1
        } RunMode;
        RSA300_API_DLL ReturnStatus GetRunState(RunMode* runMode);
        RSA300_API_DLL ReturnStatus Run();
        RSA300_API_DLL ReturnStatus Stop();
        RSA300_API_DLL ReturnStatus PrepareForRun();
        RSA300_API_DLL ReturnStatus StartFrameTransfer();

        ///////////////////////////////////////////////////////////
        // System/Reference Time 
        ///////////////////////////////////////////////////////////

        RSA300_API_DLL  ReturnStatus REFTIME_GetTimestampRate(uint64_t* o_refTimestampRate);
        RSA300_API_DLL  ReturnStatus REFTIME_GetCurrentTime(time_t* o_timeSec, uint64_t* o_timeNsec, uint64_t* o_timestamp);
        RSA300_API_DLL  ReturnStatus REFTIME_GetTimeFromTimestamp(uint64_t i_timestamp, time_t* o_timeSec, uint64_t* o_timeNsec);
        RSA300_API_DLL  ReturnStatus REFTIME_GetTimestampFromTime(time_t i_timeSec, uint64_t i_timeNsec, uint64_t* o_timestamp);
        RSA300_API_DLL  ReturnStatus REFTIME_GetIntervalSinceRefTimeSet(double* sec);

        ///////////////////////////////////////////////////////////
        // IQ Block Data aquisition
        ///////////////////////////////////////////////////////////

        RSA300_API_DLL ReturnStatus GetMaxIQBandwidth(double* maxBandwidth);
        RSA300_API_DLL ReturnStatus GetMinIQBandwidth(double* minBandwidth);
        RSA300_API_DLL ReturnStatus GetMaxAcquisitionSamples(unsigned long* maxSamples);

        RSA300_API_DLL ReturnStatus SetIQBandwidth(double iqBandwidth);
        RSA300_API_DLL ReturnStatus GetIQBandwidth(double* iqBandwidth);
        RSA300_API_DLL ReturnStatus GetIQSampleRate(double* iqSampleRate);
        RSA300_API_DLL ReturnStatus SetIQRecordLength(long recordLength);
        RSA300_API_DLL ReturnStatus GetIQRecordLength(long* recordLength);

        RSA300_API_DLL ReturnStatus WaitForIQDataReady(int timeoutMsec, bool* ready);
        RSA300_API_DLL ReturnStatus GetIQData(float* iqData, int startIndex, int length);
        RSA300_API_DLL ReturnStatus GetIQDataDeinterleaved(float* iData, float*	qData, int startIndex, int length);
        RSA300_API_DLL ReturnStatus GetIQDataCplx(Cplx32* iqData, int startIndex, int length);

        // Query IQ block acquisition status
        typedef struct
        {
            uint16_t acqDataStatus;				// See AcqDataStatus enumeration for bit definitions
            uint64_t acquisitionTimestamp;
            uint32_t frameID;
            uint16_t trigger1Index;
            uint16_t trigger2Index;
            uint16_t timeSyncIndex;
        } IQHeader;

        RSA300_API_DLL ReturnStatus GetIQHeader(IQHeader* header);

        ///////////////////////////////////////////////////////////
        // Spectrum Trace acquisition
        ///////////////////////////////////////////////////////////

        //  Spectrum windowing functions
        typedef enum
        {
            SpectrumWindow_Kaiser = 0,
            SpectrumWindow_Mil6dB = 1,
            SpectrumWindow_BlackmanHarris = 2,
            SpectrumWindow_Rectangle = 3,
            SpectrumWindow_FlatTop = 4,
            SpectrumWindow_Hann = 5
        } SpectrumWindows;

        //  Spectrum traces
        typedef enum
        {
            SpectrumTrace1 = 0,
            SpectrumTrace2 = 1,
            SpectrumTrace3 = 2
        } SpectrumTraces;

        //  Spectrum trace detector
        typedef enum
        {
            SpectrumDetector_PosPeak = 0,
            SpectrumDetector_NegPeak = 1,
            SpectrumDetector_AverageVRMS = 2,
            SpectrumDetector_Sample = 3
        } SpectrumDetectors;

        //  Spectrum trace output vertical unit
        typedef enum
        {
            SpectrumVerticalUnit_dBm = 0,
            SpectrumVerticalUnit_Watt = 1,
            SpectrumVerticalUnit_Volt = 2,
            SpectrumVerticalUnit_Amp = 3,
            SpectrumVerticalUnit_dBmV = 4
        } SpectrumVerticalUnits;

        //  Spectrum settings structure
        //  The actual values are returned from SPECTRUM_GetSettings() function
        //  Use SPECTRUM_GetLimits() to get the limits of the settings
        typedef struct
        {
            double span;
            double rbw;
            bool enableVBW;
            double vbw;
            int traceLength;					//  MUST be odd number
            SpectrumWindows window;
            SpectrumVerticalUnits verticalUnit;

            //  additional settings return from SPECTRUM_GetSettings()
            double actualStartFreq;
            double actualStopFreq;
            double actualFreqStepSize;
            double actualRBW;
            double actualVBW;
            int actualNumIQSamples;
        } Spectrum_Settings;

        //  Spectrum limits
        typedef struct
        {
            double maxSpan;
            double minSpan;
            double maxRBW;
            double minRBW;
            double maxVBW;
            double minVBW;
            int maxTraceLength;
            int minTraceLength;
        } Spectrum_Limits;

        //  Spectrum result trace information
        typedef struct
        {
            uint64_t timestamp;			//  timestamp of the first acquisition sample
            uint16_t acqDataStatus;		// See AcqDataStatus enumeration for bit definitions
        } Spectrum_TraceInfo;

        //  Enable/disable Spectrum measurement
        RSA300_API_DLL ReturnStatus SPECTRUM_SetEnable(bool enable);
        RSA300_API_DLL ReturnStatus SPECTRUM_GetEnable(bool *enable);

        //  Set spectrum settings to default values
        RSA300_API_DLL ReturnStatus SPECTRUM_SetDefault();

        //  Set/get spectrum settings
        RSA300_API_DLL ReturnStatus SPECTRUM_SetSettings(Spectrum_Settings settings);
        RSA300_API_DLL ReturnStatus SPECTRUM_GetSettings(Spectrum_Settings *settings);

        //  Set/get spectrum trace settings
        RSA300_API_DLL ReturnStatus SPECTRUM_SetTraceType(SpectrumTraces trace, bool enable, SpectrumDetectors detector);
        RSA300_API_DLL ReturnStatus SPECTRUM_GetTraceType(SpectrumTraces trace, bool *enable, SpectrumDetectors *detector);

        //  Get spectrum setting limits
        RSA300_API_DLL ReturnStatus SPECTRUM_GetLimits(Spectrum_Limits *limits);

        //  Wait for spectrum trace ready
        RSA300_API_DLL ReturnStatus SPECTRUM_WaitForDataReady(int timeoutMsec, bool *ready);

        //  Get spectrum trace data
        RSA300_API_DLL ReturnStatus SPECTRUM_GetTrace(SpectrumTraces trace, int maxTracePoints, float *traceData, int *outTracePoints);

        //  Get spectrum trace result information
        RSA300_API_DLL ReturnStatus SPECTRUM_GetTraceInfo(Spectrum_TraceInfo *traceInfo);


        ///////////////////////////////////////////////////////////
        // DPX Bitmap, Traces and Spectrogram
        ///////////////////////////////////////////////////////////

        typedef struct
        {
            int32_t fftPerFrame;
            int64_t fftCount;
            int64_t frameCount;
            double timestamp;
            uint32_t acqDataStatus;		// See AcqDataStatus enumeration for bit definitions

            double minSigDuration;
            bool minSigDurOutOfRange;

            int32_t spectrumBitmapWidth;
            int32_t spectrumBitmapHeight;
            int32_t spectrumBitmapSize;
            int32_t spectrumTraceLength;
            int32_t numSpectrumTraces;

            bool spectrumEnabled;
            bool spectrogramEnabled;

            float* spectrumBitmap;
            float** spectrumTraces;

            int32_t sogramBitmapWidth;
            int32_t sogramBitmapHeight;
            int32_t sogramBitmapSize;
            int32_t sogramBitmapNumValidLines;
            uint8_t* sogramBitmap;
            double* sogramBitmapTimestampArray;
            int16_t* sogramBitmapContainTriggerArray;

        } DPX_FrameBuffer;

        typedef struct
        {
            int32_t bitmapWidth;
            int32_t bitmapHeight;
            double sogramTraceLineTime;
            double sogramBitmapLineTime;
        } DPX_SogramSettingsStruct;

        typedef struct
        {
            bool enableSpectrum;
            bool enableSpectrogram;
            int32_t bitmapWidth;
            int32_t bitmapHeight;
            int32_t traceLength;
            float decayFactor;
            double actualRBW;
        } DPX_SettingsStruct;

        typedef enum
        {
            TraceTypeAverage = 0,
            TraceTypeMax = 1,
            TraceTypeMaxHold = 2,
            TraceTypeMin = 3,
            TraceTypeMinHold = 4
        } TraceType;

        typedef enum
        {
            VerticalUnit_dBm = 0,
            VerticalUnit_Watt = 1,
            VerticalUnit_Volt = 2,
            VerticalUnit_Amp = 3
        } VerticalUnitType;

        RSA300_API_DLL ReturnStatus GetDPXEnabled(bool* enabled);
        RSA300_API_DLL ReturnStatus SetDPXEnabled(bool enabled);

        RSA300_API_DLL ReturnStatus DPX_SetParameters(double fspan, double rbw, int32_t bitmapWidth, int32_t tracePtsPerPixel,
            VerticalUnitType yUnit, double yTop, double yBottom,
            bool infinitePersistence, double persistenceTimeSec, bool showOnlyTrigFrame);
        RSA300_API_DLL ReturnStatus DPX_Configure(bool enableSpectrum, bool enableSpectrogram);
        RSA300_API_DLL ReturnStatus DPX_GetSettings(DPX_SettingsStruct *pSettings);

        enum { DPX_TRACEIDX_1 = 0, DPX_TRACEIDX_2 = 1, DPX_TRACEIDX_3 = 2 };   // traceIndex enumerations
        RSA300_API_DLL ReturnStatus DPX_SetSpectrumTraceType(int32_t traceIndex, TraceType type);

        RSA300_API_DLL ReturnStatus DPX_FindRBWRange(double fspan, double* minRBW, double* maxRBW);
        RSA300_API_DLL ReturnStatus DPX_ResetDPx();
        RSA300_API_DLL ReturnStatus WaitForDPXDataReady(int timeoutMsec, bool* ready);

        RSA300_API_DLL ReturnStatus DPX_GetFrameInfo(int64_t* frameCount, int64_t* fftCount);

        //  Spectrogram 
        RSA300_API_DLL ReturnStatus DPX_SetSogramParameters(double timePerBitmapLine, double timeResolution, double maxPower, double minPower);
        RSA300_API_DLL ReturnStatus DPX_SetSogramTraceType(TraceType traceType);
        RSA300_API_DLL ReturnStatus DPX_GetSogramSettings(DPX_SogramSettingsStruct *pSettings);

        RSA300_API_DLL ReturnStatus DPX_GetSogramHiResLineCountLatest(int32_t* lineCount);
        RSA300_API_DLL ReturnStatus DPX_GetSogramHiResLineTriggered(bool* triggered, int32_t lineIndex);
        RSA300_API_DLL ReturnStatus DPX_GetSogramHiResLineTimestamp(double* timestamp, int32_t lineIndex);
        RSA300_API_DLL ReturnStatus DPX_GetSogramHiResLine(int16_t* vData, int32_t* vDataSize, int32_t lineIndex, double* dataSF, int32_t tracePoints, int32_t firstValidPoint);

        //  Frame Buffer
        RSA300_API_DLL ReturnStatus DPX_GetFrameBuffer(DPX_FrameBuffer* frameBuffer);

        //  The client is required to call FinishFrameBuffer() before the next frame can be available.
        RSA300_API_DLL ReturnStatus DPX_FinishFrameBuffer();
        RSA300_API_DLL ReturnStatus DPX_IsFrameBufferAvailable(bool* frameAvailable);


        ///////////////////////////////////////////////////////////
        // Audio Demod
        ///////////////////////////////////////////////////////////

        // Get/Set the demod mode to one of:
        typedef enum
        {
            ADM_FM_8KHZ = 0,
            ADM_FM_13KHZ = 1,
            ADM_FM_75KHZ = 2,
            ADM_FM_200KHZ = 3,
            ADM_AM_8KHZ = 4,
            ADM_NONE	// internal use only
        } AudioDemodMode;
        RSA300_API_DLL ReturnStatus AUDIO_SetMode(AudioDemodMode mode);
        RSA300_API_DLL ReturnStatus AUDIO_GetMode(AudioDemodMode *mode);

        // Get/Set the volume 0.0 -> 1.0. 
        RSA300_API_DLL ReturnStatus AUDIO_SetVolume(float volume);
        RSA300_API_DLL ReturnStatus AUDIO_GetVolume(float *_volume);

        // Mute/unmute the speaker output.  This does not stop the processing or data callbacks.
        RSA300_API_DLL ReturnStatus AUDIO_SetMute(bool mute);
        RSA300_API_DLL ReturnStatus AUDIO_GetMute(bool* _mute);

        RSA300_API_DLL ReturnStatus AUDIO_StartAudio();
        RSA300_API_DLL ReturnStatus AUDIO_StopAudio();
        RSA300_API_DLL ReturnStatus AUDIO_Running(bool *_running);

        // Get data from audio ooutput
        // User must allocate data to inSize before calling
        // Actual data returned is in outSize and will not exceed inSize
        RSA300_API_DLL ReturnStatus AUDIO_GetData(int16_t* data, uint16_t inSize, uint16_t *outSize);


        ///////////////////////////////////////////////////////////
        // IF(ADC) Data Streaming to disk
        ///////////////////////////////////////////////////////////

        typedef enum
        {
            StreamingModeRaw = 0,
            StreamingModeFramed = 1
        } StreamingMode;
        RSA300_API_DLL ReturnStatus SetStreamADCToDiskEnabled(bool enabled);
        RSA300_API_DLL ReturnStatus GetStreamADCToDiskActive(bool *enabled);
        RSA300_API_DLL ReturnStatus SetStreamADCToDiskMode(StreamingMode mode);
        RSA300_API_DLL ReturnStatus SetStreamADCToDiskPath(const char *path);
        RSA300_API_DLL ReturnStatus SetStreamADCToDiskFilenameBase(const char *filename);
        RSA300_API_DLL ReturnStatus SetStreamADCToDiskMaxTime(long milliseconds);
        RSA300_API_DLL ReturnStatus SetStreamADCToDiskMaxFileCount(int maximum);

        ///////////////////////////////////////////////////////////
        // IQ Data Streaming to client or disk
        ///////////////////////////////////////////////////////////

        RSA300_API_DLL ReturnStatus IQSTREAM_SetAcqBandwidth(double bwHz_req);
        RSA300_API_DLL ReturnStatus IQSTREAM_GetAcqParameters(double* bwHz_act, double* srSps);

        typedef enum { IQSOD_CLIENT = 0, IQSOD_FILE_TIQ = 1, IQSOD_FILE_SIQ = 2, IQSOD_FILE_SIQ_SPLIT = 3 } IQSOUTDEST;
        typedef enum { IQSODT_SINGLE = 0, IQSODT_INT32 = 1, IQSODT_INT16 = 2 } IQSOUTDTYPE;
        RSA300_API_DLL ReturnStatus IQSTREAM_SetOutputConfiguration(IQSOUTDEST dest, IQSOUTDTYPE dtype);

        RSA300_API_DLL ReturnStatus IQSTREAM_SetIQDataBufferSize(int reqSize);
        RSA300_API_DLL ReturnStatus IQSTREAM_GetIQDataBufferSize(int* maxSize);

        RSA300_API_DLL ReturnStatus IQSTREAM_SetDiskFilenameBaseW(const wchar_t* filenameBaseW);
        RSA300_API_DLL ReturnStatus IQSTREAM_SetDiskFilenameBase(const char* filenameBase);
        enum { IQSSDFN_SUFFIX_INCRINDEX_MIN = 0, IQSSDFN_SUFFIX_TIMESTAMP = -1, IQSSDFN_SUFFIX_NONE = -2 };  // enums for the special fileSuffixCtl values
        RSA300_API_DLL ReturnStatus IQSTREAM_SetDiskFilenameSuffix(int suffixCtl);
        RSA300_API_DLL ReturnStatus IQSTREAM_SetDiskFileLength(int msec);

        RSA300_API_DLL ReturnStatus IQSTREAM_Start();
        RSA300_API_DLL ReturnStatus IQSTREAM_Stop();
        RSA300_API_DLL ReturnStatus IQSTREAM_GetEnabled(bool* enabled);

        enum {
            IQSTRM_STATUS_OVERRANGE = (1 << 0),         // RF input overrange detected (non-sticky(client): in this block; sticky(client+file): in entire run)
            IQSTRM_STATUS_XFER_DISCONTINUITY = (1 << 1),// Continuity error (gap) detected in IF frame transfers 
            IQSTRM_STATUS_IBUFF75PCT = (1 << 2),        // Input buffer >= 75 % full, indicates IQ processing may have difficulty keeping up with IF sample stream
            IQSTRM_STATUS_IBUFFOVFLOW = (1 << 3),       // Input buffer overflow, IQ processing cannot keep up with IF sample stream, input samples dropped
            IQSTRM_STATUS_OBUFF75PCT = (1 << 4),        // Output buffer >= 75% full, indicates output sink (disk or client) may have difficulty keeping up with IQ output stream
            IQSTRM_STATUS_OBUFFOVFLOW = (1 << 5),       // Output buffer overflow, IQ unloading not keeping up with IA sample stream, output samples dropped
            IQSTRM_STATUS_NONSTICKY_SHIFT = 0,          // Non-sticky status bits are shifted this many bits left from LSB
            IQSTRM_STATUS_STICKY_SHIFT = 16             // Sticky status bits are shifted this many bits left from LSB
        };

        enum { IQSTRM_MAXTRIGGERS = 100 };  // max size of IQSTRMIQINFO triggerIndices array 
        typedef struct
        {
            uint64_t  timestamp;            // timestamp of first IQ sample returned in block
            int       triggerCount;         // number of triggers detected in this block
            int*      triggerIndices;       // array of trigger sample indices in block (overwritten on each new block query)
            double    scaleFactor;          // sample scale factor for Int16, Int32 data types (scales to volts into 50-ohms)
            uint32_t  acqStatus;            // 0:acq OK, >0:acq issues; see IQSTRM_STATUS enums to decode...
        } IQSTRMIQINFO;

        RSA300_API_DLL ReturnStatus IQSTREAM_GetIQData(void* iqdata, int* iqlen, IQSTRMIQINFO* iqinfo);

        RSA300_API_DLL ReturnStatus IQSTREAM_GetDiskFileWriteStatus(bool* isComplete, bool *isWriting);

        enum { IQSTRM_FILENAME_DATA_IDX = 0, IQSTRM_FILENAME_HEADER_IDX = 1 };
        typedef struct
        {
            uint64_t  numberSamples;              // number of samples written to file
            uint64_t  sample0Timestamp;           // timestamp of first sample in file 
            uint64_t  triggerSampleIndex;         // if triggering enabled, sample index of 1st trigger event in file
            uint64_t  triggerTimestamp;           // if triggering enabled, timestamp of trigger event
            uint32_t  acqStatus;                  // 0=acq OK, >0 acq issues; see IQSTRM_STATUS enums to decode...
            wchar_t** filenames;                  // [0]:data filename, [1]:header filename
        } IQSTRMFILEINFO;
        RSA300_API_DLL ReturnStatus IQSTREAM_GetFileInfo(IQSTRMFILEINFO* fileinfo);

        RSA300_API_DLL void IQSTREAM_ClearAcqStatus();


        ///////////////////////////////////////////////////////////
        // Stored IF Data File Playback
        ///////////////////////////////////////////////////////////

        RSA300_API_DLL ReturnStatus PLAYBACK_OpenDiskFile(
            const wchar_t * fileName,
            int startPercentage,
            int stopPercentage,
            double skipTimeBetweenFullAcquisitions,
            bool loopAtEndOfFile,
            bool emulateRealTime);

        RSA300_API_DLL ReturnStatus PLAYBACK_HasReplayCompleted(bool * isCompleted);

#ifdef __cplusplus
    }  //extern "C"
}  //namespace 
#endif //__cplusplus

#endif //RSA300_API_H
