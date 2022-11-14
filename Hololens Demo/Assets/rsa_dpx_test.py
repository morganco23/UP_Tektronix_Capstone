"""
Tektronix RSA_API Example
Author: Morgan Allison
Date created: 6/15
Date edited: 9/17
Windows 7 64-bit
RSA API version 3.11.0047
Python 3.6.1 64-bit (Anaconda 4.4.0)
NumPy 1.13.1, MatPlotLib 2.0.2
Download Anaconda: http://continuum.io/downloads
Anaconda includes NumPy and MatPlotLib
Download the RSA_API: http://www.tek.com/model/rsa306-software
Download the RSA_API Documentation:
http://www.tek.com/spectrum-analyzer/rsa306-manual-6

YOU WILL NEED TO REFERENCE THE API DOCUMENTATION
####################################
TEKTRONIX CAPSTONE 2021-22
-using pieces of demo code for API as testing ground/prototyping
-modified to display only a DPX frame in dpx example, as well as save to png when connected to by client

-server/client functionality removed, sticking to single device so networking is not needed
-timers added to check speed of connection/write
-loops etc added to get multiple images per run
-program can pull dpx frames indefinitely until told to quit
-TODO: integrate threading/joining for connection to Hololens
####################################
"""

from ctypes import *
from os import chdir
from time import sleep
import os
import time
import numpy as np
import matplotlib.pyplot as plt
from RSA_API import *
import timeit
from threading import *
import sys
import re

#timer to check program runtime
start = timeit.default_timer()

from matplotlib import __version__ as __mversion__
print('Matplotlib Version:', __mversion__)
print('Numpy Version:', np.__version__)

#get path name to save dpx images to correct directory
pathName = os.getcwd()

# C:\Tektronix\RSA_API\lib\x64 needs to be added to the
# PATH system environment variable
chdir(".\\RSA_API\\lib\\x64")
rsa = cdll.LoadLibrary("./RSA_API.dll")

#turn interactive plotting off (to help with img error)
plt.ioff()

"""################CLASSES AND FUNCTIONS################"""
def err_check(rs):
    if ReturnStatus(rs) != ReturnStatus.noError:
        raise RSAError(ReturnStatus(rs).name)

def search_connect():
    numFound = c_int(0)
    intArray = c_int * DEVSRCH_MAX_NUM_DEVICES
    deviceIDs = intArray()
    deviceSerial = create_string_buffer(DEVSRCH_SERIAL_MAX_STRLEN)
    deviceType = create_string_buffer(DEVSRCH_TYPE_MAX_STRLEN)
    apiVersion = create_string_buffer(DEVINFO_MAX_STRLEN)

    rsa.DEVICE_GetAPIVersion(apiVersion)
    print('API Version {}'.format(apiVersion.value.decode()))

    err_check(rsa.DEVICE_Search(byref(numFound), deviceIDs,
                                deviceSerial, deviceType))

    if numFound.value < 1:
        # rsa.DEVICE_Reset(c_int(0))
        print('No instruments found. Exiting script.')
        exit()
    elif numFound.value == 1:
        print('One device found.')
        print(f'Device ID: {deviceIDs[0]}')
        print('Device type: {}'.format(deviceType.value.decode()))
        print('Device serial number: {}'.format(deviceSerial.value.decode()))
        err_check(rsa.DEVICE_Connect(deviceIDs[0]))
    else:
        # corner case
        print('2 or more instruments found. Enumerating instruments, please wait.')
        for inst in deviceIDs:
            rsa.DEVICE_Connect(inst)
            rsa.DEVICE_GetSerialNumber(deviceSerial)
            rsa.DEVICE_GetNomenclature(deviceType)
            print('Device {}'.format(inst))
            print('Device Type: {}'.format(deviceType.value))
            print('Device serial number: {}'.format(deviceSerial.value))
            rsa.DEVICE_Disconnect()
        # note: the API can only currently access one at a time
        selection = 1024
        while (selection > numFound.value - 1) or (selection < 0):
            selection = int(raw_input('Select device between 0 and {}\n> '.format(numFound.value - 1)))
        err_check(rsa.DEVICE_Connect(deviceIDs[selection]))
    rsa.CONFIG_Preset()

"""################DPX EXAMPLE################"""
def config_DPX(cf=1e9, refLevel=0, span=40e6, rbw=300e3):
    yTop = refLevel
    yBottom = yTop - 100
    yUnit = VerticalUnitType.VerticalUnit_dBm

    dpxSet = DPX_SettingStruct()
    rsa.CONFIG_SetCenterFreq(c_double(cf))
    rsa.CONFIG_SetReferenceLevel(c_double(refLevel))

    rsa.DPX_SetEnable(c_bool(True))
    rsa.DPX_SetParameters(c_double(span), c_double(rbw), c_int(801), c_int(1),
                          yUnit, c_double(yTop), c_double(yBottom), c_bool(False),
                          c_double(1.0), c_bool(False))
    rsa.DPX_SetSogramParameters(c_double(1e-3), c_double(1e-3),
                                c_double(refLevel), c_double(refLevel - 100))
    rsa.DPX_Configure(c_bool(True), c_bool(True))

    rsa.DPX_SetSpectrumTraceType(c_int32(0), c_int(2))
    rsa.DPX_SetSpectrumTraceType(c_int32(1), c_int(4))
    rsa.DPX_SetSpectrumTraceType(c_int32(2), c_int(0))

    rsa.DPX_GetSettings(byref(dpxSet))
    dpxFreq = np.linspace((cf - span / 2), (cf + span / 2), dpxSet.bitmapWidth)
    dpxAmp = np.linspace(yBottom, yTop, dpxSet.bitmapHeight)
    return dpxFreq, dpxAmp


def acquire_dpx_frame():
    frameAvailable = c_bool(False)
    ready = c_bool(False)
    fb = DPX_FrameBuffer()

    rsa.DEVICE_Run()
    rsa.DPX_Reset()

    while not frameAvailable.value:
        rsa.DPX_IsFrameBufferAvailable(byref(frameAvailable))
        while not ready.value:
            rsa.DPX_WaitForDataReady(c_int(100), byref(ready))
    rsa.DPX_GetFrameBuffer(byref(fb))
    rsa.DPX_FinishFrameBuffer()
    #rsa.DEVICE_Stop()
    return fb


def extract_dpx_spectrum(fb):
    # When converting a ctypes pointer to a numpy array, we need to
    # explicitly specify its length to dereference it correctly
    dpxBitmap = np.array(fb.spectrumBitmap[:fb.spectrumBitmapSize])
    dpxBitmap = dpxBitmap.reshape((fb.spectrumBitmapHeight,
                                   fb.spectrumBitmapWidth))
    #dpxBitmap = dpxBitmap.resize((200,800))

    # Grab trace data and convert from W to dBm
    # http://www.rapidtables.com/convert/power/Watt_to_dBm.htm
    # Note: fb.spectrumTraces is a pointer to a pointer, so we need to
    # go through an additional dereferencing step
    traces = []
    for i in range(3):
        traces.append(10 * np.log10(1000 * np.array(
            fb.spectrumTraces[i][:fb.spectrumTraceLength])) + 30)
    # specTrace2 = 10 * np.log10(1000*np.array(
    #     fb.spectrumTraces[1][:fb.spectrumTraceLength])) + 30
    # specTrace3 = 10 * np.log10(1000*np.array(
    #     fb.spectrumTraces[2][:fb.spectrumTraceLength])) + 30

    # return dpxBitmap, specTrace1, specTrace2, specTrace3
    return dpxBitmap, traces


def extract_dpxogram(fb):
    # When converting a ctypes pointer to a numpy array, we need to
    # explicitly specify its length to dereference it correctly
    dpxogram = np.array(fb.sogramBitmap[:fb.sogramBitmapSize])
    dpxogram = dpxogram.reshape((fb.sogramBitmapHeight,
                                 fb.sogramBitmapWidth))
    dpxogram = dpxogram[:fb.sogramBitmapNumValidLines, :]

    return dpxogram


def dpx_example():
    print('\n\n########DPX Example########')
    search_connect()

    connecttime = timeit.default_timer()
    print("Time to connect: ", connecttime - start)

    cf = float(sys.argv[1])
    refLevel = int(sys.argv[2])
    span = float(sys.argv[3])
    rbw = float(sys.argv[4])

    #parameters
    # cf = 2.4453e9
    # refLevel = -30
    # span = 40e6
    # rbw = 100e3

    #set up quit event
    quit = Event()

    #draw background (once)
    graph_axis(cf, refLevel, span, rbw)

    #spawn new thread to continuously graph dpx frames
    #dpx_thread = Thread(target=dpx_loop,args=(quit, cf, refLevel, span, rbw))
    #dpx_thread.start()
    #print("thread started")
    dpx_loop(quit, cf, refLevel, span, rbw)
    #loop to see if graph params are changed
    #to change, in command line type the variable to change followed by
    #an eqauls, followed by the number
    #i.e: "span = 42e6" or "reflevel = -45" or "cf = 2.44e8" or "rbw = 100e2"

def update_settings_loop(quit, input_str, cf, refLevel, span, rbw):
    #if want to change cf
    if "cf" in input_str:
        print("input was cf")
        num = input_str.split('=')
        print("setting cf to " + num[1])
        cf = float(num[1])
        rsa.DEVICE_Stop()
        print("Stopped Device")

    #if want to change reflevel
    elif "reflevel" in input_str:
        print("input was refLevel")
        num = input_str.split('=')
        print("setting refLevel to " + num[1])
        refLevel = float(num[1])
        rsa.DEVICE_Stop()

    #if want to change span
    elif "span" in input_str:
        print("input was span")
        num = input_str.split('=')
        print("setting span to " + num[1])
        span = float(num[1])
        rsa.DEVICE_Stop()

    #if want to change rbw
    elif "rbw" in input_str:
        print("input was rbw")
        num = input_str.split('=')
        print("setting rbw to " + num[1])
        rbw = float(num[1]) 
        
        rsa.DEVICE_Stop()
    
    #print out the changed values
    print("cf = " + str(cf) + " refLevel = " + str(refLevel) + " span = " + str(span) + " rbw = " + str(rbw))
    


def dpx_loop(quit, cf, refLevel, span, rbw):
    while(True):
    #for x in range(100):
        #print("##########################")
        #draw graph (every time)
        # input_str = input().lower()
        # if input_str is not None:
        #     settings_thread = Thread(target=update_settings_loop,args=(quit,input_str,cf,refLevel,span,rbw))
        #     settings_thread.start()
        

        
        #if the character typed was q, quit break out of loop and 
        #send signal to stop thread
        graph_axis(cf, refLevel, span, rbw)
        graph_dpx(cf, refLevel, span, rbw)
        #check if quit was triggered
        if quit.isSet():
            print("quitting...")
            break




def graph_dpx(cf, refLevel, span, rbw):

    dpxFreq, dpxAmp = config_DPX(cf, refLevel, span, rbw)
    fb = acquire_dpx_frame()

    dpxBitmap, traces = extract_dpx_spectrum(fb)
    dpxogram = extract_dpxogram(fb)
    numTicks = 11
    plotFreq = np.linspace(cf - span / 2.0, cf + span / 2.0, numTicks) / 1e9

    """################PLOT################"""
    #Show the colorized DPX display
    graphstart = timeit.default_timer()
    fig = plt.figure(1)
    ax2 = fig.add_subplot()
    plt.axis("off")
    ax2.imshow(dpxBitmap, cmap='gist_stern')
    ax2.set_aspect(4)

    plt.tight_layout()
    ts = time.time()
    #filename = pathName + str(ts) + ".png" #for multiple img files
    filename = pathName + "\exe.png" #for a single img file updated repeatedly
    #filename = "exe.png"
    try:
        plt.savefig(filename)
        plt.close()
        graphstop = timeit.default_timer()
        graphtime = graphstop - graphstart
        #print("Time to graph: ", graphtime)
    except:
        print("failed")

    
def graph_axis(cf, refLevel, span, rbw):

    dpxFreq, dpxAmp = config_DPX(cf, refLevel, span, rbw)
    fb = acquire_dpx_frame()

    #dpxBitmap, traces = extract_dpx_spectrum(fb)
    dpxogram = extract_dpxogram(fb)
    numTicks = 11
    plotFreq = np.linspace(cf - span / 2.0, cf + span / 2.0, numTicks) / 1e9

    """################PLOT################"""
    #graph the axis by themselves
    fig = plt.figure(1)
    ax2 = fig.add_subplot()
    #ax2.imshow(dpxBitmap, cmap='gist_stern')
    ax2.set_aspect(4)
    ax2.set_title('DPX Bitmap')
    ax2.set_xlabel('Frequency (GHz)')
    ax2.set_ylabel('Amplitude (dBm)')
    xTicks = map('{:.4}'.format, plotFreq)
    plt.xticks(np.linspace(0, fb.spectrumBitmapWidth, numTicks), xTicks)
    yTicks = map('{}'.format, np.linspace(refLevel, refLevel - 100, numTicks))
    plt.yticks(np.linspace(0, fb.spectrumBitmapHeight, numTicks), yTicks)

    plt.tight_layout()
    filename = pathName + "_axis.png"
    plt.savefig(filename)
    plt.close()

"""################MISC################"""
def config_trigger(trigMode=TriggerMode.triggered, trigLevel=-10,
                   trigSource=TriggerSource.TriggerSourceIFPowerLevel):
    rsa.TRIG_SetTriggerMode(trigMode)
    rsa.TRIG_SetIFPowerTriggerLevel(c_double(trigLevel))
    rsa.TRIG_SetTriggerSource(trigSource)
    rsa.TRIG_SetTriggerPositionPercent(c_double(10))


def peak_power_detector(freq, trace):
    peakPower = np.amax(trace)
    peakFreq = freq[np.argmax(trace)]

    return peakPower, peakFreq



def main():
    dpx_example()

    #stops timer and prints out program runtime
    stop = timeit.default_timer()
    print("Total Time: ", stop - start)

if __name__ == '__main__':
    main()
