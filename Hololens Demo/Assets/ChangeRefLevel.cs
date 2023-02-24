using Microsoft.MixedReality.Toolkit.Experimental.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static RSAAPITest;

public class ChangeRefLevel : MonoBehaviour
{
    [DllImport("rsa_api", EntryPoint = "CONFIG_SetReferenceLevel")]
    private static extern ReturnStatus CONFIG_SetReferenceLevel(double refLevel);

    [DllImport("rsa_api", EntryPoint = "CONFIG_GetReferenceLevel")]
    private static extern ReturnStatus CONFIG_GetReferenceLevel(ref double refLevel);

    [DllImport("rsa_api", EntryPoint = "DPX_Reset")]
    private static extern ReturnStatus DPX_Reset();

    private const double REF_LEVEL_MIN = -130.0;
    private const double REF_LEVEL_MAX = 30.0;
    private double refLevel;
    private MixedRealityKeyboard refLevelKeyboard;
    public DPX_Config dpxConfig;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseRefLevel()
    {
        CONFIG_GetReferenceLevel(ref refLevel);
        Debug.Log($"Reference Level is now {refLevel}");
        if ((refLevel + 1.0) <= REF_LEVEL_MAX)
        {
            CONFIG_SetReferenceLevel(refLevel + 1.0);
            DPX_Reset(); // Reset is necessary as the RSA will not update settings without resetting DPX first
        }
        Debug.Log($"Reference Level is now {refLevel}");
    }

    public void DecreaseRefLevel()
    {
        CONFIG_GetReferenceLevel(ref refLevel);
        Debug.Log($"Reference Level is now {refLevel}");
        if ((refLevel - 1.0) >= REF_LEVEL_MIN)
        {
            CONFIG_SetReferenceLevel(refLevel - 1.0);
            DPX_Reset();
        }
        Debug.Log($"Reference Level is now {refLevel}");
    }

    public void BeginUpdateRefLevel() 
    {
        CONFIG_GetReferenceLevel(ref refLevel);
        Debug.Log($"(Variable) Center Frequency Before = {refLevel}");
        refLevelKeyboard = GetComponent<MixedRealityKeyboard>();
    }

    public void FinishUpdateRefLevel()
    {
        double newRefLevel = Convert.ToDouble(refLevelKeyboard.Text);
        CONFIG_SetReferenceLevel(newRefLevel);
        GetDPXConfigParams(ref dpxConfig);
        dpxConfig.refLevel = newRefLevel;
        DPX_Reset();
        CONFIG_GetReferenceLevel(ref refLevel);
        Debug.Log($"(Variable) Center Frequency After = {refLevel} Hz");
    }
}
