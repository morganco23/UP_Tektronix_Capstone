using UnityEngine;
using System.Diagnostics;
using System.IO;
using UnityEngine.UI;
using ScottPlot;
using System.Runtime.InteropServices;
using static RSAAPITest;

public class fetchDPXFrame : MonoBehaviour
{
    [DllImport("rsa_api", EntryPoint = "CONFIG_SetReferenceLevel")]
    private static extern ReturnStatus CONFIG_GetReferenceLevel(ref double refLevel);

    [DllImport("rsa_api", EntryPoint = "DPX_GetSettings")]
    private static extern RSAAPITest.ReturnStatus DPX_GetSettings(ref RSAAPITest.DPX_SettingStruct settings);

    RawImage img;
    //public Texture frame;
    //public string imgPath = "Assets/exe.png";
    //public int timer;
    public Plot plt;
    public double referenceLevel;
    public DPX_Config dpxConfig;
    public DPX_SettingStruct dpxSettings;
    // Start is called before the first frame update
    void Start()
    {
        //start script
        //UnityEngine.Debug.Log("starting script");
        //run_cmd();

        //apply initial texture
        //      plt = new Plot(15, 10);
        //dpxSettings = new DPX_SettingStruct();
        //      DPX_GetSettings(ref dpxSettings);
        //      GetDPXConfigParams(ref dpxConfig);
        //      Tuple<NDarray, NDarray> dpxData = ConfigDPX(dpxConfig.cf, dpxConfig.refLevel, dpxConfig.span, dpxSettings);
        //      DPX_FrameBuffer fb = GetCurrentFrameBuffer();
        //      Tuple<NDarray, float[][]> dpxTraceData = ExtractDPXSpectrum(fb);
        //      NDarray dpxogram = ExtractDPXogram(fb);
        //      NDarray plotFreq = np.linspace(dpxConfig.cf - dpxConfig.span / 2.0, dpxConfig.cf + dpxConfig.span / 2.0, 11) / 1e9;
        //      string[] xTicks = np.around(plotFreq, 4).GetData<string>();
        //      plt.XTicks(np.linspace(0, fb.spectrumBitmapWidth, 11).GetData<double>(), xTicks);
        //      string[] yTicks = np.linspace(dpxConfig.refLevel, dpxConfig.refLevel - 100, 11).GetData<string>();
        //      plt.YTicks(np.linspace(0, fb.spectrumBitmapHeight, 11).GetData<double>(), yTicks);
        //      string[] colorCodes = { "#440154", "#39568C", "#1F968B", "#73D055" };
        //      System.Drawing.Color[] colors = colorCodes.Select(x => ColorTranslator.FromHtml(x)).ToArray();
        //      var dpxPlot = plt.AddSignal(plotFreq.GetData<double>());
        //      dpxPlot.DensityColors = colors;
        //      dpxPlot.Color = colors[0];
        //      //CONFIG_GetReferenceLevel(ref referenceLevel);
        //      //plt.SetAxisLimitsY(referenceLevel - 100, referenceLevel);
        //      plt.Title("DPX Spectrum Trace");
        //      plt.YLabel("Amplitude (dBm)");
        //      plt.XLabel("Frequency (Hz)");
        //      plt.AxisAuto();
        img = GetComponent<RawImage>();
        img.texture = LoadPNG("Assets/exe_axis.png");
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
        //Tuple<NDarray, NDarray> dpxData = ConfigDPX(dpxConfig.cf, dpxConfig.refLevel, dpxConfig.span, dpxSettings);
        //DPX_FrameBuffer fb = GetCurrentFrameBuffer();
        //Tuple<NDarray, float[][]> dpxTraceData = ExtractDPXSpectrum(fb);
        //NDarray dpxogram = ExtractDPXogram(fb);
        //NDarray plotFreq = np.linspace(dpxConfig.cf - dpxConfig.span / 2.0, dpxConfig.cf + dpxConfig.span / 2.0, 11) / 1e9;
        //string[] xTicks = np.around(plotFreq, 4).GetData<string>();
        //plt.XTicks(np.linspace(0, fb.spectrumBitmapWidth, 11).GetData<double>(), xTicks);
        //string[] yTicks = np.linspace(dpxConfig.refLevel, dpxConfig.refLevel - 100, 11).GetData<string>();
        //plt.YTicks(np.linspace(0, fb.spectrumBitmapHeight, 11).GetData<double>(), yTicks);
        //string[] colorCodes = { "#440154", "#39568C", "#1F968B", "#73D055" };
        //System.Drawing.Color[] colors = colorCodes.Select(x => ColorTranslator.FromHtml(x)).ToArray();
        //var dpxPlot = plt.AddSignal(plotFreq.GetData<double>());
        //dpxPlot.DensityColors = colors;
        //dpxPlot.Color = colors[0];
        ////CONFIG_GetReferenceLevel(ref referenceLevel);
        ////plt.SetAxisLimitsY(referenceLevel - 100, referenceLevel);
        //plt.Title("DPX Spectrum Trace");
        //plt.YLabel("Amplitude (dBm)");
        //plt.XLabel("Frequency (Hz)");
        ////plt.AxisAuto();
        //plt.SaveFig("./Assets/exe_new.png");
        img.texture = LoadPNG("./Assets/exe_new.png");
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
