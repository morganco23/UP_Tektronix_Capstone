# UP Tektronix Capstone
The Tektronix Capstone team is tasked with creating an application to display real-time spectrum data on the HoloLens 2.


## Installation Notes
There are several software and hardware prerequisites that must be met before running the application on the HoloLens 2.

### Software:
- Make sure that the latest version of [Unity Hub](https://unity.com/download) and the Editor version 2020.3.39f1 (download within Unity Hub) is installed.
- Mixed Reality Feature Tool must be installed. If not, click this [link](https://www.microsoft.com/en-us/download/confirmation.aspx?id=102778) to install it. Once the Unity project is selected
  - On Discover Features page, make sure to select all features under Mixed Reality Toolkit.
  - Make sure to selected "Mixed Reality OpenXR Plugin" under Platform support

### Hardware:
- The Real-Time Spectrum Analyzer (RSA) needs to be connected to a laptop via a USB3 connection.
- An antenna must be connected to the RF port on the RSA in order to collect .
- If there is a USB-C port on the laptop, connect the Hololens to the laptop. Otherwise, make sure that the laptop and the HoloLens are connected to the same Wi-Fi.

## Runtime Information
Currently, this application runs at about 9-10 fps since we removed the Python component from the application. The application also features built-in voice commands to change the settings of the RSA.
