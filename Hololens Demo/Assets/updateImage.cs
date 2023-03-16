using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class updateImage : MonoBehaviour
{
    public string imagePath;
    public Texture2D img;
    public Material plane;
    public  MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        GetComponent<MeshRenderer>().material.mainTexture = img;
        LoadImageFromFile(imagePath);
    }

    private void LoadImageFromFile(string path){
        if(File.Exists(path)){
            byte[] bytes = File.ReadAllBytes(path);
            img = new Texture2D(2,2);
            img.LoadImage(bytes);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(File.Exists(imagePath)){
            byte[] bytes = File.ReadAllBytes(imagePath);
            img = new Texture2D(2,2);
            img.LoadImage(bytes);
            plane.SetTexture("_MainTex", img);
        }

        meshRenderer.material.SetTexture("_MainTex", img);
    }
}
