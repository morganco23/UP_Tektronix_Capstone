using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RSAAPITest : MonoBehaviour
{

    [DllImport("rsa_api")]
    private static extern int DEVICE_Search(ref int numFound, ref int[] idList, ref string[] names, ref string[] types);
    [DllImport("rsa_api")]
    private static extern int DEVICE_Connect(int id);
    [DllImport("rsa_api")]
    private static extern int DEVICE_GetHWVersion(char* hwVersion);


    // Start is called before the first frame update
    void Start()
    {
        int numDevices = -1;
        int[] idList = new int[100];
        string[] names = new string[100];
        //UnityEngine.Debug.Log("initial id:" + names[0]);
        string[] types = new string[100];
        string id = "B012107";
        //int error = DEVICE_Search(ref numDevices, ref idList, ref names, ref types);
        //UnityEngine.Debug.Log("id:" + names[0]);
        int error = DEVICE_Connect(0);
        UnityEngine.Debug.Log("error: " + error);
        error = DEVICE_GetHWVersion(ref hwVersion);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
