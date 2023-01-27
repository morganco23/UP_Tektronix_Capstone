using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using UnityEngine.UI;
using ScottPlot;
using System.Runtime.InteropServices;
using System.Linq;

public class fetchDPXFrame : MonoBehaviour
{
    [DllImport("rsa_api", EntryPoint = "CONFIG_SetReferenceLevel")]
    private static extern RSAAPITest.ReturnStatus CONFIG_GetReferenceLevel(ref double refLevel);

    [DllImport("rsa_api", EntryPoint = "DPX_GetSettings")]
    private static extern RSAAPITest.ReturnStatus DPX_GetSettings(ref RSAAPITest.DPX_SettingStruct settings);

    RawImage img;
    //public Texture frame;
    //public string imgPath = "Assets/exe.png";
    //public int timer;
    public Plot plt;
    public double referenceLevel;
    public RSAAPITest.DPX_SettingStruct dpxSettings;
    // Start is called before the first frame update
    void Start()
    {
        //start script
        //UnityEngine.Debug.Log("starting script");
        //run_cmd();

        //apply initial texture
        plt = new Plot(15, 10);
		dpxSettings = new RSAAPITest.DPX_SettingStruct();
        //img = GetComponent<RawImage>();
        //img.texture = frame;
        //img.mainTexture = LoadPNG("Assets/exe_axis.png");
        UnityEngine.Debug.Log("initial texture loaded");

        //for delay
        //timer = 0;

    }

    public static Texture2D LoadPNG(string path)
    {
        Texture2D texture = null;
        byte[] fileData;

        if(File.Exists(path))
        {
           
            fileData = File.ReadAllBytes(path);
            texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); 
        }
        return texture;
    }

    // Update is called once per frame
    void Update()
    {
        //img.texture = frame;
        /*if(timer == 0)
        {
            img.texture = LoadPNG("Assets/exe.png");
            UnityEngine.Debug.Log("update called");
            timer = 10;
        }
        else
        {
            timer = timer - 1;
        }*/
        plt.Title("DPX Spectrum Trace");
        plt.YLabel("Amplitude (dBm)");
        plt.XLabel("Frequency (Hz)");
		plt.AxisAuto();
        DPX_GetSettings(ref dpxSettings);
        CONFIG_GetReferenceLevel(ref referenceLevel);
        plt.SetAxisLimitsY(referenceLevel - 100, referenceLevel);
        plt.SaveFig("Assets/exe_new.png");
        img.texture = LoadPNG("Assets/exe_new.png");
    }

    void createTexture()
    {
        
    }

    void run_cmd()
    {
        ProcessStartInfo start = new ProcessStartInfo();
        //used complete file path for testing but works with shortened path
        //start.FileName = "D:\up_tek_capstone\UP_Tektronix_Capstone\Unity\Assets\exe";
        start.FileName = "Assets/exe/rsa_dpx.exe"; //independent of complete file path
        start.Arguments = "2.4453e9 -30 40e6 100e3"; //default arguments
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        UnityEngine.Debug.Log("Starting rsa_dpx.exe");
        Process.Start(start);

        // string argument = "2.4453e9 -30 40e6 100e3";

        // var myProcess= new System.Diagnostics.Process();
        // myProcess.StartInfo = new ProcessStartInfo(argument)
        // {
        //     FileName="Assets/exe/rsa_dpx.exe",
        //     Arguments = "/C"+argument,
        //     CreateNoWindow = true,
        //     UseShellExecute = false,
        //     RedirectStandardOutput = true,
        //     RedirectStandardInput = true,
        //     RedirectStandardError = true
        // };
        // myProcess.Start();

        

        // // Create a process
        // System.Diagnostics.Process process = new System.Diagnostics.Process();

        // // Set the StartInfo of process
        // process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        // process.StartInfo.FileName = "Assets/exe/rsa_dpx.exe";
        // process.StartInfo.Arguments = argument;

        // // Start the process
        // process.Start();
        // process.WaitForExit();



        /*
        Process myProcess = new Process();
        myProcess.StartInfo.FileName = "C:/Users/mader22/My project/Assets/fetchInt.exe";
        myProcess.StartInfo.FileName = "Assets/exe/rsa_dpx.exe";
        myProcess.StartInfo.UseShellExecute = false;
        myProcess.StartInfo.RedirectStandardOutput = true;
        myProcess.StartInfo.Arguments = "/C" + "2.4453e9 -30 40e6 100e3";

        myProcess.Start();

        start.Arguments = "2.4453e9 -30 40e6 100e3";

        StreamReader reader = myProcess.StandardOutput;
        var output = reader.ReadToEnd();
        UnityEngine.Debug.Log("hello" + output);
        //start.WaitForExit();
        */
    }

}
