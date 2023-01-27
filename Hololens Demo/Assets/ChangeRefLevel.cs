using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ChangeRefLevel : MonoBehaviour
{
    [DllImport("rsa_api", EntryPoint = "CONFIG_SetReferenceLevel")]
    private static extern RSAAPITest.ReturnStatus CONFIG_SetReferenceLevel(double refLevel);

    [DllImport("rsa_api", EntryPoint = "CONFIG_GetReferenceLevel")]
    private static extern RSAAPITest.ReturnStatus CONFIG_GetReferenceLevel(ref double refLevel);

    [DllImport("rsa_api", EntryPoint = "DPX_Reset")]
    private static extern RSAAPITest.ReturnStatus DPX_Reset();

    private const double REF_LEVEL_MIN = -130.0;
    private const double REF_LEVEL_MAX = 30.0;
    public double refLevel;
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

    public void UpdateRefLevel() 
    { 

    }
}
