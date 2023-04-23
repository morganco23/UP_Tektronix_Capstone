/**************************************************************************************************
*  RSA_API.h -- RSA API SW interface definition for RSA306/306B/500A/600A devices 
*
*  NOTE:
*    This file contains the RSA API "V2" SW interface definition.
*    The V2 API is compatible with all RSA306/RSA306B/RSA500A/RSA600A devices.
*
*    The initial RSA300_API interface (AKA "V1") is defined in the "RSA300API.h" file.
*    RSA300_API (V1) is DEPRECATED and support may be removed in a future release.
*    API users are strongly encouraged to migrate applications to the V2 API.  
*    See the API Programming Reference manual for information on how to migrate V1 usage to V2 usage.
*
*  Copyright (c) Tektronix Incorporated 2016.  All rights reserved.
*  Licensed software products are owned by Tektronix or its subsidiaries or suppliers,
*  and are protected by national copyright laws and international treaty provisions.
***************************************************************************************************/

#ifndef RSA_API_H
#define RSA_API_H

/******************/

#ifndef RETURNSTATUS_ONLY   // wnen def'd, only ReturnStatus definition is enabled (special use only)

// --- Define import/export macro for Windows builds
#ifdef _WIN32
#ifdef RSA_API_EXPORTS
#define RSA_API_DLL __declspec(dllexport)
#else
#define RSA_API_DLL __declspec(dllimport)
#endif
#else  // non-Windows builds
#define RSA_API_DLL
#endif

//#define MATLAB_COMPAT		// when this macro is def'd, it removes some content that causes Matlab loadlibrary() compatibility issues

#include <stdint.h>
#ifndef MATLAB_COMPAT
#include <time.h>
#else
typedef long long time_t;
#endif

#ifdef __cplusplus  // for C++ compilation...
namespace RSA_API   // ...use V2 namespace
{
extern "C"          // ...prevent C++ function name mangling
{
#endif //__cplusplus

#endif // #ifndef RETURNSTATUS_ONLY

    ///////////////////////////////////////////////////////////
	// Status and Error Reporting enums
	///////////////////////////////////////////////////////////

	typedef enum
	{
		//-----------------
		// User errors
		//-----------------

        noError = 0,    // most API functions return this value when successful

        // Connection
        errorNotConnected = 101,
        errorIncompatibleFirmware,
        errorBootLoaderNotRunning,
        errorTooManyBootLoadersConnected,
        errorRebootFailure,
        errorGNSSNotInstalled,
        errorGNSSNotEnabled,

        // POST
        errorPOSTFailureFPGALoad = 201,
        errorPOSTFailureHiPower,
        errorPOSTFailureI2C,
        errorPOSTFailureGPIF,
        errorPOSTFailureUsbSpeed,
        errorPOSTDiagFailure,
		errorPOSTFailure3P3VSense,
		errorPOSTLinkFailure,

        // General Msmt
        errorBufferAllocFailed = 301,
        errorParameter,
        errorDataNotReady,

		// Spectrum
		errorParameterTraceLength = 1101,
		errorMeasurementNotEnabled,
		errorSpanIsLessThanRBW,
		errorFrequencyOutOfRange,

		// IF streaming
		errorStreamADCToDiskFileOpen = 1201,
        errorStreamADCToDiskAlreadyStreaming,
        errorStreamADCToDiskBadPath,
        errorStreamADCToDiskThreadFailure,
        errorStreamedFileInvalidHeader,
        errorStreamedFileOpenFailure,
        errorStreamingOperationNotSupported,
        errorStreamingFastForwardTimeInvalid,
        errorStreamingInvalidParameters,
        errorStreamingEOF,
        errorStreamingIfReadTimeout,
        errorStreamingIfNotEnabled,

		// IQ streaming
		errorIQStreamInvalidFileDataType = 1301,
		errorIQStreamFileOpenFailed,
		errorIQStreamBandwidthOutOfRange,
        errorIQStreamingNotEnabled,


		//-----------------
		// Internal errors
		//-----------------
		errorTimeout = 3001,
		errorTransfer,
		errorFileOpen,
		errorFailed,
		errorCRC,
		errorChangeToFlashMode,
		errorChangeToRunMode,
		errorDSPLError,
		errorLOLockFailure,
		errorExternalReferenceNotEnabled,
		errorLogFailure,
		errorRegisterIO,
        errorFileRead,
        errorConsumerNotActive,

		errorDisconnectedDeviceRemoved = 3101,
		errorDisconnectedDeviceNodeChangedAndRemoved,
		errorDisconnectedTimeoutWaitingForADcData,
		errorDisconnectedIOBeginTransfer,
		errorOperationNotSupportedInSimMode,
		errorDisconnectedIOFinishTransfer,

		errorFPGAConfigureFailure = 3201,
        errorCalCWNormFailure,
        errorSystemAppDataDirectory,
        errorFileCreateMRU,
        errorDeleteUnsuitableCachePath,
        errorUnableToSetFilePermissions,
        errorCreateCachePath,
        errorCreateCachePathBoost,
        errorCreateCachePathStd,
        errorCreateCachePathGen,
        errorBufferLengthTooSmall,
        errorRemoveCachePath,
        errorGetCachingDirectoryBoost,
        errorGetCachingDirectoryStd,
        errorGetCachingDirectoryGen,
        errorInconsistentFileSystem,

		errorWriteCalConfigHeader = 3301,
		errorWriteCalConfigData,
		errorReadCalConfigHeader,
		errorReadCalConfigData,
		errorEraseCalConfig,
		errorCalConfigFileSize,
		errorInvalidCalibConstantFileFormat,
		errorMismatchCalibConstantsSize,
		errorCalConfigInvalid,

        // flash
        errorFlashFileSystemUnexpectedSize = 3401,
        errorFlashFileSystemNotMounted,
        errorFlashFileSystemOutOfRange,
        errorFlashFileSystemIndexNotFound,
        errorFlashFileSystemReadErrorCRC,
        errorFlashFileSystemReadFileMissing,
        errorFlashFileSystemCreateCacheIndex,
        errorFlashFileSystemCreateCachedDataFile,
        errorFlashFileSystemUnsupportedFileSize,
        errorFlashFileSystemInsufficentSpace,
        errorFlashFileSystemInconsistentState,
        errorFlashFileSystemTooManyFiles,
        errorFlashFileSystemImportFileNotFound,
        errorFlashFileSystemImportFileReadError,
        errorFlashFileSystemImportFileError,
        errorFlashFileSystemFileNotFoundError,
        errorFlashFileSystemReadBufferTooSmall,
        errorFlashWriteFailure,
        errorFlashReadFailure,
        errorFlashFileSystemBadArgument,
        errorFlashFileSystemCreateFile,
		errorArchiveDirectoryNotFound,
		errorArchiveDirectoryNotWriteable,
		errorArchiveWriteFile,
		errorArchiveGenerateFilename,
		errorArchiveBoost,
		errorArchiveStd,
		errorArchiveGeneric,

        // Aux monitoring
        errorMonitoringNotSupported = 3501,
        errorAuxDataNotAvailable,

        // battery
        errorBatteryCommFailure = 3601,
		errorBatteryChargerCommFailure = 3602,
		errorBatteryNotPresent = 3603,

		//EST
		errorESTOutputPathFile = 3701,
		errorESTPathNotDirectory,
		errorESTPathDoesntExist,
		errorESTUnableToOpenLog,
		errorESTUnableToOpenLimits,

		// Revision information
		errorRevisionDataNotFound = 3801,
		
		// alignment
		error112MHzAlignmentSignalLevelTooLow = 3901,
		error10MHzAlignmentSignalLevelTooLow,
		errorInvalidCalConstant,
        errorNormalizationCacheInvalid,
        errorInvalidAlignmentCache,
		errorLockExtRefAfterAlignment,

		// Triggering
		errorTriggerSystem = 4000,

		// VNA
        errorVNAUnsupportedConfiguration = 4100,

		// MFC
		errorMFCHWNotPresent = 4200,
		errorMFCWriteCalFile,
		errorMFCReadCalFile,
		errorMFCFileFormatError,
		errorMFCFlashCorruptDataError,

		// acq status
        errorADCOverrange = 9000,	// must not change the location of these error codes without coordinating with MFG TEST
        errorOscUnlock = 9001,      

		errorNotSupported = 9901,

        errorPlaceholder = 9999,
        notImplemented = -1

	} ReturnStatus;
    

#ifndef RETURNSTATUS_ONLY   // if def'd, all content below is disabled

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
	//   - Spectrum_TraceInfo (returned by SPECTRUM_GetTraceInfo())
	//   - DPX_FrameBuffer (returned by DPX_GetFrameBuffer())
	//  NOTE: Any active (1) bits in the status word other than defined below are for internal use only and should be ignored.
	//
	enum
	{ 
		AcqDataStatus_ADC_OVERRANGE		= 0x1,		// Bit 0: Overrange - Input to the ADC was outside of its operating range.
		AcqDataStatus_REF_OSC_UNLOCK	= 0x2,		// Bit 1: RefOscUnlocked - Loss of locked status on the reference oscillator.
		AcqDataStatus_LOW_SUPPLY_VOLTAGE = 0x10,	// Bit 4: PowerFail - Power (5V and Usb) failure detected.
		AcqDataStatus_ADC_DATA_LOST		= 0x20,		// Bit 5: Dropped frame - Loss of ADC data frame samples
		AcqDataStatus_VALID_BITS_MASK	= (AcqDataStatus_ADC_OVERRANGE | AcqDataStatus_REF_OSC_UNLOCK | AcqDataStatus_LOW_SUPPLY_VOLTAGE | AcqDataStatus_ADC_DATA_LOST)
	};  // AcqDataStatus


	///////////////////////////////////////////////////////////
	// Device Connection and Info
	///////////////////////////////////////////////////////////

	RSA_API_DLL const char*  DEVICE_GetErrorString(ReturnStatus status);		// translate ReturnStatus code into text (char) string

	// Device Search maximum sizes:
	enum { DEVSRCH_MAX_NUM_DEVICES = 20, DEVSRCH_SERIAL_MAX_STRLEN = 100, DEVSRCH_TYPE_MAX_STRLEN = 20 };

	// Device Search, Type 1 (Client provides storage buffers)
	// Example: 
	// int numDev;
	// int devID[RSA_API::DEVSRCH_MAX_NUM_DEVICES];
	// {char|wchar_t} devSN[RSA_API::DEVSRCH_MAX_NUM_DEVICES][RSA_API::DEVSRCH_SERIAL_MAX_STRLEN];
	// {char|wchar_t} devType[RSA_API::DEVSRCH_MAX_NUM_DEVICES][RSA_API::DEVSRCH_TYPE_MAX_STRLEN];
	// rs = RSA_API::DEVICE_Search{W}(&numDev, devID, devSN, devType);
	RSA_API_DLL ReturnStatus DEVICE_Search(int* numDevicesFound, int deviceIDs[], char deviceSerial[][DEVSRCH_SERIAL_MAX_STRLEN], char deviceType[][DEVSRCH_TYPE_MAX_STRLEN]);
	RSA_API_DLL ReturnStatus DEVICE_SearchW(int* numDevicesFound, int deviceIDs[], wchar_t deviceSerial[][DEVSRCH_SERIAL_MAX_STRLEN], wchar_t deviceType[][DEVSRCH_TYPE_MAX_STRLEN]);	
	
	// Device Search, Type 2 (API provides internal static storage buffers, client may copy if needed)
	// Example: 
	// int numDev;
	// int* devID;						// ptr to devID array
	// const {char|wchar_t}** devSN;	// ptr to array of ptrs to devSN strings
	// const {char|wchar_t}** devType;	// ptr to array of ptrs to devType strings
	// rs = RSA_API::DEVICE_SearchInt{W}(&numDev, &devID, &devSN, &devType);
#ifndef MATLAB_COMPAT
	RSA_API_DLL ReturnStatus DEVICE_SearchInt(int* numDevicesFound, int* deviceIDs[], const char** deviceSerial[], const char** deviceType[]);
	RSA_API_DLL ReturnStatus DEVICE_SearchIntW(int* numDevicesFound, int* deviceIDs[], const wchar_t** deviceSerial[], const wchar_t** deviceType[]);
#endif

	RSA_API_DLL ReturnStatus DEVICE_Connect(int deviceID);
	RSA_API_DLL ReturnStatus DEVICE_Reset(int deviceID);
	RSA_API_DLL ReturnStatus DEVICE_Disconnect();

	// Version Info of connected device
	// Example:  char xyzInfo[DEVINFO_MAX_STRLEN];	DEVICE_Get<xyz>Version(xyzInfo);
	enum { DEVINFO_MAX_STRLEN = 100 };
	RSA_API_DLL ReturnStatus DEVICE_GetNomenclature(char* nomenclature);
	RSA_API_DLL ReturnStatus DEVICE_GetNomenclatureW(wchar_t* nomenclature);
	RSA_API_DLL ReturnStatus DEVICE_GetSerialNumber(char* serialNum);
	RSA_API_DLL ReturnStatus DEVICE_GetAPIVersion(char* apiVersion);
	RSA_API_DLL ReturnStatus DEVICE_GetFWVersion(char* fwVersion);
	RSA_API_DLL ReturnStatus DEVICE_GetFPGAVersion(char* fpgaVersion);
	RSA_API_DLL ReturnStatus DEVICE_GetHWVersion(char* hwVersion);
	// Get all device info strings at once
#ifndef MATLAB_COMPAT
	typedef struct
	{
		char nomenclature[DEVINFO_MAX_STRLEN];
		char serialNum[DEVINFO_MAX_STRLEN];
		char apiVersion[DEVINFO_MAX_STRLEN];
		char fwVersion[DEVINFO_MAX_STRLEN];
		char fpgaVersion[DEVINFO_MAX_STRLEN];
		char hwVersion[DEVINFO_MAX_STRLEN];
	} DEVICE_INFO;
	RSA_API_DLL ReturnStatus DEVICE_GetInfo(DEVICE_INFO* devInfo);
#endif

	RSA_API_DLL ReturnStatus DEVICE_GetOverTemperatureStatus(bool* overTemperature);

	
    ///////////////////////////////////////////////////////////
	// Device Configuration (global)
	///////////////////////////////////////////////////////////

	RSA_API_DLL ReturnStatus CONFIG_Preset();

	RSA_API_DLL ReturnStatus CONFIG_SetReferenceLevel(double refLevel);
	RSA_API_DLL ReturnStatus CONFIG_GetReferenceLevel(double* refLevel);
	RSA_API_DLL ReturnStatus CONFIG_GetMaxCenterFreq(double* maxCF);
	RSA_API_DLL ReturnStatus CONFIG_GetMinCenterFreq(double* minCF);
	RSA_API_DLL ReturnStatus CONFIG_SetCenterFreq(double cf);
	RSA_API_DLL ReturnStatus CONFIG_GetCenterFreq(double* cf);

	RSA_API_DLL ReturnStatus CONFIG_GetAutoAttenuationEnable(bool *enable);
	RSA_API_DLL ReturnStatus CONFIG_SetAutoAttenuationEnable(bool enable);
	RSA_API_DLL ReturnStatus CONFIG_GetRFPreampEnable(bool *enable);
	RSA_API_DLL ReturnStatus CONFIG_SetRFPreampEnable(bool enable);
	RSA_API_DLL ReturnStatus CONFIG_GetRFAttenuator(double *value);
	RSA_API_DLL ReturnStatus CONFIG_SetRFAttenuator(double value);

    RSA_API_DLL ReturnStatus CONFIG_SetExternalRefEnable(bool exRefEn);     // use CONFIG_SetFrequencyReferenceSource instead
    RSA_API_DLL ReturnStatus CONFIG_GetExternalRefEnable(bool* exRefEn);
    RSA_API_DLL ReturnStatus CONFIG_GetExternalRefFrequency(double* extFreq);

    // Frequency Reference Source selection control/status (access to GNSS and USER sources)
    typedef enum { FRS_INTERNAL = 0, FRS_EXTREF = 1, FRS_GNSS = 2, FRS_USER = 3 } FREQREF_SOURCE;
    RSA_API_DLL ReturnStatus CONFIG_SetFrequencyReferenceSource(FREQREF_SOURCE src);
    RSA_API_DLL ReturnStatus CONFIG_GetFrequencyReferenceSource(FREQREF_SOURCE* src);
    // GNSS-based Frequency Reference controls and status
    typedef enum { GFRM_OFF = 0, GFRM_FREQTRACK = 2, GFRM_PHASETRACK = 3, GFRM_HOLD = 4 } GFR_MODE;
    RSA_API_DLL ReturnStatus CONFIG_SetModeGnssFreqRefCorrection(GFR_MODE mode);
    RSA_API_DLL ReturnStatus CONFIG_GetModeGnssFreqRefCorrection(GFR_MODE* mode);
    typedef enum { GFRS_OFF = 0, GFRS_ACQUIRING = 1, GFRS_FREQTRACKING = 2, GFRS_PHASETRACKING = 3, GFRS_HOLDING = 4 } GFR_STATE;
    typedef enum { GFRQ_INVALID = 0, GFRQ_LOW = 1, GFRQ_MEDIUM = 2, GFRQ_HIGH = 3 } GFR_QUALITY;
    RSA_API_DLL ReturnStatus CONFIG_GetStatusGnssFreqRefCorrection(GFR_STATE* state, GFR_QUALITY* quality);
    // GNSS Timing Ref alignment controls
    RSA_API_DLL ReturnStatus CONFIG_SetEnableGnssTimeRefAlign(bool enable);
    RSA_API_DLL ReturnStatus CONFIG_GetEnableGnssTimeRefAlign(bool* enable);
    RSA_API_DLL ReturnStatus CONFIG_GetStatusGnssTimeRefAlign(bool* aligned);
    // Manage USER Frequency Reference setting set/get
    RSA_API_DLL ReturnStatus CONFIG_SetFreqRefUserSetting(const char* i_usstr);
    RSA_API_DLL ReturnStatus CONFIG_GetFreqRefUserSetting(char* o_usstr);
#ifndef MATLAB_COMPAT
	typedef struct
    {
        bool    isvalid;
        unsigned int dacValue;
        char    datetime[DEVINFO_MAX_STRLEN];  // storage d/t as yyyy-mm-ddThh:mm:ss
        double  temperature;    // storage temperature degC
    } FREQREF_USER_INFO;
    RSA_API_DLL ReturnStatus CONFIG_DecodeFreqRefUserSettingString(const char* i_usstr, FREQREF_USER_INFO* o_fui);
#endif

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
		TriggerSourceIFPowerLevel = 1,	//  IF power level
		TriggerSourceTime = 2			//  time-based trigger
	} TriggerSource;

	typedef enum
	{
		TriggerTransitionLH = 1,		//  Low to High transition		
		TriggerTransitionHL = 2,		//  High to Low transition
		TriggerTransitionEither = 3		//  either Low to High or High to Low transition	
	} TriggerTransition;

	RSA_API_DLL ReturnStatus TRIG_SetTriggerMode(TriggerMode mode);
	RSA_API_DLL ReturnStatus TRIG_GetTriggerMode(TriggerMode* mode);
	RSA_API_DLL ReturnStatus TRIG_SetTriggerSource(TriggerSource source);
	RSA_API_DLL ReturnStatus TRIG_GetTriggerSource(TriggerSource *source);
	RSA_API_DLL ReturnStatus TRIG_SetTriggerTransition(TriggerTransition transition);
	RSA_API_DLL ReturnStatus TRIG_GetTriggerTransition(TriggerTransition *transition);
	RSA_API_DLL ReturnStatus TRIG_SetIFPowerTriggerLevel(double level);
	RSA_API_DLL ReturnStatus TRIG_GetIFPowerTriggerLevel(double *level);
	RSA_API_DLL ReturnStatus TRIG_SetTriggerPositionPercent(double trigPosPercent);
	RSA_API_DLL ReturnStatus TRIG_GetTriggerPositionPercent(double* trigPosPercent);
	RSA_API_DLL ReturnStatus TRIG_SetTriggerTime(time_t startTimeSec, uint64_t startTimeNsec, uint64_t repeatTimeNsec);
	RSA_API_DLL ReturnStatus TRIG_GetTriggerTime(time_t* startTimeSec, uint64_t* startTimeNsec, uint64_t* repeatTimeNsec);

	RSA_API_DLL ReturnStatus TRIG_ForceTrigger();
	
	
    ///////////////////////////////////////////////////////////
	// Device Alignment
	///////////////////////////////////////////////////////////
	
    RSA_API_DLL ReturnStatus ALIGN_GetWarmupStatus(bool* warmedUp);
	RSA_API_DLL ReturnStatus ALIGN_GetAlignmentNeeded(bool* needed);
	RSA_API_DLL ReturnStatus ALIGN_RunAlignment();


	///////////////////////////////////////////////////////////
	// Device Operation (global)
	///////////////////////////////////////////////////////////
	
	RSA_API_DLL ReturnStatus DEVICE_GetEnable(bool* enable);
	RSA_API_DLL ReturnStatus DEVICE_Run();
	RSA_API_DLL ReturnStatus DEVICE_Stop();
	RSA_API_DLL ReturnStatus DEVICE_PrepareForRun();
	RSA_API_DLL ReturnStatus DEVICE_StartFrameTransfer();

	enum { DEVEVENT_OVERRANGE = 0, DEVEVENT_TRIGGER = 1, DEVEVENT_1PPS = 2 };
	RSA_API_DLL ReturnStatus DEVICE_GetEventStatus(int eventID, bool* eventOccurred, uint64_t* eventTimestamp);

	
	///////////////////////////////////////////////////////////
	// System/Reference Time 
	///////////////////////////////////////////////////////////

	RSA_API_DLL ReturnStatus REFTIME_GetTimestampRate(uint64_t* o_refTimestampRate);
	RSA_API_DLL ReturnStatus REFTIME_GetCurrentTime(time_t* o_timeSec, uint64_t* o_timeNsec, uint64_t* o_timestamp);
	RSA_API_DLL ReturnStatus REFTIME_GetTimeFromTimestamp(uint64_t i_timestamp, time_t* o_timeSec, uint64_t* o_timeNsec);
	RSA_API_DLL ReturnStatus REFTIME_GetTimestampFromTime(time_t i_timeSec, uint64_t i_timeNsec, uint64_t* o_timestamp);
	RSA_API_DLL ReturnStatus REFTIME_GetIntervalSinceRefTimeSet(double* sec);
	RSA_API_DLL ReturnStatus REFTIME_SetReferenceTime(time_t refTimeSec, uint64_t refTimeNsec, uint64_t refTimestamp);
	RSA_API_DLL ReturnStatus REFTIME_GetReferenceTime(time_t* refTimeSec, uint64_t* refTimeNsec, uint64_t* refTimestamp);
    typedef enum { RTSRC_NONE = 0, RTSRC_SYSTEM = 1, RTSRC_GNSS = 2, RTSRC_USER = 3, RTSRC_IRIGBAM = 4, RTSRC_IRIGBDC = 5, RTSRC_PPS = 6} REFTIME_SRC;
    RSA_API_DLL ReturnStatus REFTIME_GetReferenceTimeSource(REFTIME_SRC* source);

	///////////////////////////////////////////////////////////
	// IQ Block Data aquisition
	///////////////////////////////////////////////////////////

	RSA_API_DLL ReturnStatus IQBLK_GetMaxIQBandwidth(double* maxBandwidth);
	RSA_API_DLL ReturnStatus IQBLK_GetMinIQBandwidth(double* minBandwidth);

	RSA_API_DLL ReturnStatus IQBLK_SetIQBandwidth(double iqBandwidth);
	RSA_API_DLL ReturnStatus IQBLK_GetIQBandwidth(double* iqBandwidth);

    RSA_API_DLL ReturnStatus IQBLK_GetIQSampleRate(double* iqSampleRate);   // Set IQBLK BW before calling
    RSA_API_DLL ReturnStatus IQBLK_GetMaxIQRecordLength(int* maxSamples);   // Set IQBLK BW before calling
    
    RSA_API_DLL ReturnStatus IQBLK_SetIQRecordLength(int recordLength);
	RSA_API_DLL ReturnStatus IQBLK_GetIQRecordLength(int* recordLength);

	RSA_API_DLL ReturnStatus IQBLK_AcquireIQData();
	RSA_API_DLL ReturnStatus IQBLK_WaitForIQDataReady(int timeoutMsec, bool* ready);

	RSA_API_DLL ReturnStatus IQBLK_GetIQData(float* iqData, int* outLength, int reqLength);
	RSA_API_DLL ReturnStatus IQBLK_GetIQDataDeinterleaved(float* iData, float* qData, int* outLength, int reqLength);
	RSA_API_DLL ReturnStatus IQBLK_GetIQDataCplx(Cplx32* iqData, int* outLength, int reqLength);
	
	RSA_API_DLL ReturnStatus IQBLK_FinishedIQData(void);

	// Bit field definitions for "acqStatus" word of IQBLK_ACQINFO struct
	enum {
		IQBLK_STATUS_INPUT_OVERRANGE = (1 << 0),
		IQBLK_STATUS_FREQREF_UNLOCKED = (1 << 1),  // frequency reference unlocked
		IQBLK_STATUS_ACQ_SYS_ERROR = (1 << 2),		// acquisition system error within block 
		IQBLK_STATUS_DATA_XFER_ERROR = (1 << 3),   // data transfer error within block
	};
	typedef struct
	{
		uint64_t  sample0Timestamp;         // timestamp of first sample in block 
		uint64_t  triggerSampleIndex;       // sample index of trigger event in block
		uint64_t  triggerTimestamp;         // timestamp of trigger event in block
		uint32_t  acqStatus;				// 0:no errors, >0:acq events/issues; see IQBLK_STATUS_* bit enums to decode...
	} IQBLK_ACQINFO;
	RSA_API_DLL ReturnStatus IQBLK_GetIQAcqInfo(IQBLK_ACQINFO* acqInfo);   // Query IQ block acquisition info

	
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
	RSA_API_DLL ReturnStatus SPECTRUM_SetEnable(bool enable);
	RSA_API_DLL ReturnStatus SPECTRUM_GetEnable(bool *enable);

	//  Set spectrum settings to default values
	RSA_API_DLL ReturnStatus SPECTRUM_SetDefault();

	//  Set/get spectrum settings
	RSA_API_DLL ReturnStatus SPECTRUM_SetSettings(Spectrum_Settings settings);
	RSA_API_DLL ReturnStatus SPECTRUM_GetSettings(Spectrum_Settings *settings);

	//  Set/get spectrum trace settings
	RSA_API_DLL ReturnStatus SPECTRUM_SetTraceType(SpectrumTraces trace, bool enable, SpectrumDetectors detector);
	RSA_API_DLL ReturnStatus SPECTRUM_GetTraceType(SpectrumTraces trace, bool *enable, SpectrumDetectors *detector);

	//  Get spectrum setting limits
	RSA_API_DLL ReturnStatus SPECTRUM_GetLimits(Spectrum_Limits *limits);

	//  Start Trace acquisition
	RSA_API_DLL ReturnStatus SPECTRUM_AcquireTrace();
	//  Wait for spectrum trace ready
	RSA_API_DLL ReturnStatus SPECTRUM_WaitForTraceReady(int timeoutMsec, bool *ready);

	//  Get spectrum trace data
	RSA_API_DLL ReturnStatus SPECTRUM_GetTrace(SpectrumTraces trace, int maxTracePoints, float *traceData, int *outTracePoints);

	//  Get spectrum trace result information
	RSA_API_DLL ReturnStatus SPECTRUM_GetTraceInfo(Spectrum_TraceInfo *traceInfo);

	
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

	RSA_API_DLL ReturnStatus DPX_GetEnable(bool* enable);
	RSA_API_DLL ReturnStatus DPX_SetEnable(bool enable);

	RSA_API_DLL ReturnStatus DPX_SetParameters(double fspan, double rbw, int32_t bitmapWidth, int32_t tracePtsPerPixel, 
		                                       VerticalUnitType yUnit, double yTop, double yBottom, 
											   bool infinitePersistence, double persistenceTimeSec, bool showOnlyTrigFrame);
	RSA_API_DLL ReturnStatus DPX_Configure(bool enableSpectrum, bool enableSpectrogram);
	RSA_API_DLL ReturnStatus DPX_GetSettings(DPX_SettingsStruct *pSettings);

	enum { DPX_TRACEIDX_1 = 0, DPX_TRACEIDX_2 = 1, DPX_TRACEIDX_3 = 2 };   // traceIndex enumerations
	RSA_API_DLL ReturnStatus DPX_SetSpectrumTraceType(int32_t traceIndex, TraceType type);

	RSA_API_DLL ReturnStatus DPX_GetRBWRange(double fspan, double* minRBW, double* maxRBW);
	RSA_API_DLL ReturnStatus DPX_Reset();
	RSA_API_DLL ReturnStatus DPX_WaitForDataReady(int timeoutMsec, bool* ready);

	RSA_API_DLL ReturnStatus DPX_GetFrameInfo(int64_t* frameCount, int64_t* fftCount);

	//  Spectrogram 
	RSA_API_DLL ReturnStatus DPX_SetSogramParameters(double timePerBitmapLine, double timeResolution, double maxPower, double minPower);
	RSA_API_DLL ReturnStatus DPX_SetSogramTraceType(TraceType traceType);
	RSA_API_DLL ReturnStatus DPX_GetSogramSettings(DPX_SogramSettingsStruct *pSettings);

	RSA_API_DLL ReturnStatus DPX_GetSogramHiResLineCountLatest(int32_t* lineCount);
	RSA_API_DLL ReturnStatus DPX_GetSogramHiResLineTriggered(bool* triggered, int32_t lineIndex);
	RSA_API_DLL ReturnStatus DPX_GetSogramHiResLineTimestamp(double* timestamp, int32_t lineIndex);
	RSA_API_DLL ReturnStatus DPX_GetSogramHiResLine(int16_t* vData, int32_t* vDataSize, int32_t lineIndex, double* dataSF, int32_t tracePoints, int32_t firstValidPoint);

	//  Frame Buffer
	RSA_API_DLL ReturnStatus DPX_GetFrameBuffer(DPX_FrameBuffer* frameBuffer);

	//  The client is required to call FinishFrameBuffer() before the next frame can be available.
	RSA_API_DLL ReturnStatus DPX_FinishFrameBuffer();
	RSA_API_DLL ReturnStatus DPX_IsFrameBufferAvailable(bool* frameAvailable);

	
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
	RSA_API_DLL ReturnStatus AUDIO_SetMode(AudioDemodMode mode);
	RSA_API_DLL ReturnStatus AUDIO_GetMode(AudioDemodMode *mode);

	// Get/Set the volume 0.0 -> 1.0. 
	RSA_API_DLL ReturnStatus AUDIO_SetVolume(float volume);
	RSA_API_DLL ReturnStatus AUDIO_GetVolume(float *_volume);

	// Mute/unmute the speaker output.  This does not stop the processing or data callbacks.
	RSA_API_DLL ReturnStatus AUDIO_SetMute(bool mute);
	RSA_API_DLL ReturnStatus AUDIO_GetMute(bool* _mute);

	// Tune Offset from center frequency (default is 0 Hz offset)
	RSA_API_DLL ReturnStatus AUDIO_SetFrequencyOffset(double freqOffsetHz);
	RSA_API_DLL ReturnStatus AUDIO_GetFrequencyOffset(double* freqOffsetHz);

	RSA_API_DLL ReturnStatus AUDIO_Start();						// Start the audio demod output generation
	RSA_API_DLL ReturnStatus AUDIO_Stop();						// Stop the audio demod output generation
	RSA_API_DLL ReturnStatus AUDIO_GetEnable(bool *enable);		// Query the audio demod state

	// Get data from audio ooutput
	// User must allocate data to inSize before calling
	// Actual data returned is in outSize and will not exceed inSize
	RSA_API_DLL ReturnStatus AUDIO_GetData(int16_t* data, uint16_t inSize, uint16_t *outSize);

	
	///////////////////////////////////////////////////////////
	// IF(ADC) Data Streaming to client or disk file
	///////////////////////////////////////////////////////////
	
    // Note: IFSTREAM_SetOutputConfiguration() function and IFSOUTDEST type are now preferred setting IF streaming output.
    // They should be used in place of IFSTREAM_SetDiskFileMode() function and StreamingMode type in new work.
    typedef enum {
        IFSOD_CLIENT = 0,               // output direct to client, no file written
        IFSOD_FILE_R3F = 1,             // R3F file output, equivalent to IFSTREAM_SetDiskFileMode(StreamingModeFramed)
        IFSOD_FILE_R3HA_DET = 3,        // R3H+R3A file output, equivalent to IFSTREAM_SetDiskFileMode(StreamingModeRaw)
        IFSOD_FILE_MIDAS = 11,          // Midas CDIF file output, combined header+data file
        IFSOD_FILE_MIDAS_DET = 12       // Midas CDIF+DET file output, separate header and data files
    } IFSOUTDEST;
    RSA_API_DLL ReturnStatus IFSTREAM_SetOutputConfiguration(IFSOUTDEST dest);  // may be used for all output types (file or client)
    
    // Configure IF data disk file output 
	// Note: IFSTREAM_SetDiskFileMode() will be maintained for legacy support.
    // Use IFSTREAM_SetOutputConfiguration() for new work.
    typedef enum {
		StreamingModeRaw = 0,       // output to R3H+R3A files (separate header + raw data)
		StreamingModeFramed = 1     // output to R3F file (combined header + framed data)
	} StreamingMode;
	RSA_API_DLL ReturnStatus IFSTREAM_SetDiskFileMode(StreamingMode mode);  // legacy: for file output type
	RSA_API_DLL ReturnStatus IFSTREAM_SetDiskFilePath(const char *path);
	RSA_API_DLL ReturnStatus IFSTREAM_SetDiskFilenameBase(const char *base);
	enum { IFSSDFN_SUFFIX_INCRINDEX_MIN = 0, IFSSDFN_SUFFIX_TIMESTAMP = -1, IFSSDFN_SUFFIX_NONE = -2 };  // enums for the special fileSuffixCtl values
	RSA_API_DLL ReturnStatus IFSTREAM_SetDiskFilenameSuffix(int fileSuffixCtl);
	RSA_API_DLL ReturnStatus IFSTREAM_SetDiskFileLength(int msec);
	RSA_API_DLL ReturnStatus IFSTREAM_SetDiskFileCount(int count);

    // Queries for IF data parameters (client use)
    RSA_API_DLL ReturnStatus IFSTREAM_GetAcqParameters(double* bwHz_act, double* srSps, double* cfAtIfHz);
    RSA_API_DLL ReturnStatus IFSTREAM_GetScalingParameters(double* scaleFactor, double* scaleFreq);
    RSA_API_DLL ReturnStatus IFSTREAM_GetEQParameters(int* numPts, float** freq, float** ampl, float** phase);
    RSA_API_DLL ReturnStatus IFSTREAM_GetIFDataBufferSize(int* buffSize, int* numSamples);

    // IF stream Start/Stop/Status access
    RSA_API_DLL ReturnStatus IFSTREAM_SetEnable(bool enable);
    RSA_API_DLL ReturnStatus IFSTREAM_GetActiveStatus(bool *active);

    // IF data output to client
    enum { IFSTRM_MAXTRIGGERS = 32 };  // max size of IFSTRMDATAINFO triggerIndices array 
    enum {
        IFSTRM_STATUS_OVERRANGE = (1 << 0),         // ADC input overrange detected
        IFSTRM_STATUS_XFER_DISCONTINUITY = (1 << 1),// Continuity error (gap) detected in IF frames
    };
    typedef struct
    {
        uint64_t  timestamp;            // timestamp of first IF sample returned in block
        int       triggerCount;         // number of triggers detected in this block
        int*      triggerIndices;       // internal array of trigger sample indices in block (overwritten on each new block query)
        uint32_t  acqStatus;            // 0:acq OK, >0:acq issues; see IFQSTRM_STATUS enums to decode...
    } IFSTRMDATAINFO;
    RSA_API_DLL ReturnStatus IFSTREAM_GetIFData(int16_t* data, int* datalen, IFSTRMDATAINFO* datainfo);
    RSA_API_DLL ReturnStatus IFSTREAM_GetIFFrames(uint8_t** data, int* numBytes, int* numFrames);

	///////////////////////////////////////////////////////////
	// IQ Data Streaming to client or disk file
	///////////////////////////////////////////////////////////

	RSA_API_DLL ReturnStatus IQSTREAM_GetMaxAcqBandwidth(double* maxBandwidthHz);
	RSA_API_DLL ReturnStatus IQSTREAM_GetMinAcqBandwidth(double* minBandwidthHz);
	RSA_API_DLL ReturnStatus IQSTREAM_SetAcqBandwidth(double bwHz_req);
    RSA_API_DLL ReturnStatus IQSTREAM_GetAcqParameters(double* bwHz_act, double* srSps);

	typedef enum { 
        IQSOD_CLIENT = 0,               // output direct to client, no file written
        IQSOD_FILE_TIQ = 1,             // TIQ file output
        IQSOD_FILE_SIQ = 2,             // SIQ file output, combined header+data
        IQSOD_FILE_SIQ_SPLIT = 3,       // SIQH+SIQD file output, split header and data
        IQSOD_FILE_MIDAS = 11,          // Midas CDIF file output, combined header+data file
        IQSOD_FILE_MIDAS_DET = 12       // Midas CDIF+DET file output, separate header and data files
    } IQSOUTDEST;
	typedef enum { 
        IQSODT_SINGLE = 0,				// float32, scaled to volts/50 ohms
        IQSODT_INT32 = 1,				// int32 
        IQSODT_INT16 = 2,				// int16
		IQSODT_SINGLE_SCALE_INT32 = 3	// float32, scaled to int32 range
	} IQSOUTDTYPE;
	RSA_API_DLL ReturnStatus IQSTREAM_SetOutputConfiguration(IQSOUTDEST dest, IQSOUTDTYPE dtype);

	RSA_API_DLL ReturnStatus IQSTREAM_SetIQDataBufferSize(int reqSize);
	RSA_API_DLL ReturnStatus IQSTREAM_GetIQDataBufferSize(int* maxSize);

    RSA_API_DLL ReturnStatus IQSTREAM_SetDiskFilenameBase(const char* filenameBase);
    RSA_API_DLL ReturnStatus IQSTREAM_SetDiskFilenameBaseW(const wchar_t* filenameBaseW);
	enum {  // enums for IQSTREAM_SetDiskFilenameSuffix() suffixCtl values
        IQSSDFN_SUFFIX_INCRINDEX_MIN = 0, 
        IQSSDFN_SUFFIX_TIMESTAMP = -1,      // default at connect
        IQSSDFN_SUFFIX_NONE = -2 };     
	RSA_API_DLL ReturnStatus IQSTREAM_SetDiskFilenameSuffix(int suffixCtl);
	RSA_API_DLL ReturnStatus IQSTREAM_SetDiskFileLength(int msec);

	RSA_API_DLL ReturnStatus IQSTREAM_Start();
	RSA_API_DLL ReturnStatus IQSTREAM_Stop();
	RSA_API_DLL ReturnStatus IQSTREAM_GetEnable(bool* enable);

    enum { IQSTRM_MAXTRIGGERS = 100 };  // max size of IQSTRMIQINFO triggerIndices array 
    enum {
		IQSTRM_STATUS_OVERRANGE = (1 << 0),         // RF input overrange detected (non-sticky(client): in this block; sticky(client+file): in entire run)
		IQSTRM_STATUS_XFER_DISCONTINUITY = (1 << 1),// Continuity error (gap) detected in IF frame transfers 
		IQSTRM_STATUS_IBUFF75PCT = (1 << 2),        // Input buffer >= 75 % full, indicates IQ processing may have difficulty keeping up with IF sample stream
		IQSTRM_STATUS_IBUFFOVFLOW = (1 << 3),       // Input buffer overflow, IQ processing cannot keep up with IF sample stream, input samples dropped
		IQSTRM_STATUS_OBUFF75PCT = (1 << 4),        // Output buffer >= 75% full, indicates output sink (disk or client) may have difficulty keeping up with IQ output stream
		IQSTRM_STATUS_OBUFFOVFLOW = (1 << 5),       // Output buffer overflow, IQ unloading not keeping up with IF sample stream, output samples dropped
		IQSTRM_STATUS_NONSTICKY_SHIFT = 0,          // Non-sticky status bits are shifted this many bits left from LSB
		IQSTRM_STATUS_STICKY_SHIFT = 16             // Sticky status bits are shifted this many bits left from LSB
	};
	typedef struct
	{
		uint64_t  timestamp;            // timestamp of first IQ sample returned in block
		int       triggerCount;         // number of triggers detected in this block
		int*      triggerIndices;       // array of trigger sample indices in block (overwritten on each new block query)
		double    scaleFactor;          // sample scale factor for Int16, Int32 data types (scales to volts into 50-ohms)
		uint32_t  acqStatus;            // 0:acq OK, >0:acq issues; see IQSTRM_STATUS enums to decode...
	} IQSTRMIQINFO;

    RSA_API_DLL ReturnStatus IQSTREAM_WaitForIQDataReady(int timeoutMsec, bool* ready);
	RSA_API_DLL ReturnStatus IQSTREAM_GetIQData(void* iqdata, int* iqlen, IQSTRMIQINFO* iqinfo);

	RSA_API_DLL ReturnStatus IQSTREAM_GetDiskFileWriteStatus(bool* isComplete, bool *isWriting);

	enum { IQSTRM_FILENAME_DATA_IDX = 0, IQSTRM_FILENAME_HEADER_IDX = 1 };  // indices to IQSTRMFILEINFO filenames
	typedef struct
	{
		uint64_t  numberSamples;              // number of samples written to file
		uint64_t  sample0Timestamp;           // timestamp of first sample in file 
		uint64_t  triggerSampleIndex;         // if triggering enabled, sample index of 1st trigger event in file
		uint64_t  triggerTimestamp;           // if triggering enabled, timestamp of trigger event
		uint32_t  acqStatus;                  // 0=acq OK, >0 acq issues; see IQSTRM_STATUS enums to decode...
		wchar_t** filenames;                  // [0]:data filename, [1]:header filename
	} IQSTRMFILEINFO;
	RSA_API_DLL ReturnStatus IQSTREAM_GetDiskFileInfo(IQSTRMFILEINFO* fileinfo);

	RSA_API_DLL void IQSTREAM_ClearAcqStatus();


	///////////////////////////////////////////////////////////
    // Stored IF Data File Playback
    ///////////////////////////////////////////////////////////

	RSA_API_DLL ReturnStatus PLAYBACK_OpenDiskFile(
        const wchar_t * fileName,
        int startPercentage,
        int stopPercentage,
        double skipTimeBetweenFullAcquisitions,
        bool loopAtEndOfFile,
        bool emulateRealTime);

	RSA_API_DLL ReturnStatus PLAYBACK_GetReplayComplete(bool* complete);


	///////////////////////////////////////////////////////////
	// Tracking Generator control
	///////////////////////////////////////////////////////////

	RSA_API_DLL ReturnStatus TRKGEN_GetHwInstalled(bool *installed);
	RSA_API_DLL ReturnStatus TRKGEN_SetEnable(bool enable);
	RSA_API_DLL ReturnStatus TRKGEN_GetEnable(bool *enable);
	RSA_API_DLL ReturnStatus TRKGEN_SetOutputLevel(double leveldBm);
	RSA_API_DLL ReturnStatus TRKGEN_GetOutputLevel(double *leveldBm);


    ///////////////////////////////////////////////////////////
	// GNSS Rx Control and Output
	///////////////////////////////////////////////////////////

	typedef enum { GNSS_NOSYS = 0, GNSS_GPS_GLONASS = 1, GNSS_GPS_BEIDOU = 2, GNSS_GPS = 3, GNSS_GLONASS = 4, GNSS_BEIDOU = 5, } GNSS_SATSYS;

	RSA_API_DLL ReturnStatus GNSS_GetHwInstalled(bool *installed);
	RSA_API_DLL ReturnStatus GNSS_SetEnable(bool enable);
	RSA_API_DLL ReturnStatus GNSS_GetEnable(bool* enable);
	RSA_API_DLL ReturnStatus GNSS_SetSatSystem(GNSS_SATSYS satSystem);
	RSA_API_DLL ReturnStatus GNSS_GetSatSystem(GNSS_SATSYS *satSystem);
	RSA_API_DLL ReturnStatus GNSS_SetAntennaPower(bool powered);
	RSA_API_DLL ReturnStatus GNSS_GetAntennaPower(bool* powered);
	RSA_API_DLL ReturnStatus GNSS_GetNavMessageData(int* msgLen, const char** message);
	RSA_API_DLL ReturnStatus GNSS_ClearNavMessageData();
	RSA_API_DLL ReturnStatus GNSS_Get1PPSTimestamp(bool* isValid, uint64_t* timestamp1PPS);
    RSA_API_DLL ReturnStatus GNSS_GetStatusRxLock(bool* lock);

	///////////////////////////////////////////////////////////
	// Power and Battery Status
	///////////////////////////////////////////////////////////

	typedef struct
	{
		bool externalPowerPresent;
		bool batteryPresent;
		double batteryChargeLevel;		//  in percent
		bool batteryCharging;
		bool batteryOverTemperature;
		bool batteryHardwareError;
	} POWER_INFO;
	RSA_API_DLL ReturnStatus POWER_GetStatus(POWER_INFO* powerInfo);

#ifdef __cplusplus
}  //extern "C"
}  //namespace RSA_API
#endif //__cplusplus

#endif // #ifndef RETURNSTATUS_ONLY

#endif // RSA_API_H
