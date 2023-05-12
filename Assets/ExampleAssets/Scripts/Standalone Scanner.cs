using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.XR.ARFoundation;
using ZXing;


public class Scanner : MonoBehaviour
{
    [SerializeField]
    private string lastResult;

    private WebCamTexture camTexture;
    private Color32[] cameraColorData;
    private int width, height;
    private Rect screenRect;
    

    private Result result;

    private IBarcodeReader barcodeReader = new BarcodeReader
    {
        AutoRotate = false,
        Options = new ZXing.Common.DecodingOptions
        {
            TryHarder = false
        }
    };

    // Start is called before the first frame update

    private void Start()
    {
        SetupWebCamTexture();
        PlayWebcamTexture();

        lastResult = "http://www.google.com";

        cameraColorData = new Color32[width * height];
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        
    }

    private void OnEnable()
    {
        PlayWebcamTexture();
    }
    private void OnDisable()
    {
        if (camTexture != null)
        {
            camTexture.Pause();
        }
    }
    private void SetupWebCamTexture()
    {
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
    }

    private void PlayWebcamTexture()
    {
        if(camTexture != null)
        {
            camTexture.Play();
            width = camTexture.width;
            height = camTexture.height;
        }
    }
    private void Update()
    {
        camTexture.GetPixels32(cameraColorData);
        result = barcodeReader.Decode(cameraColorData, width, height);
        if(result != null)
        {
            lastResult = result.Text + " " + result.BarcodeFormat;
            Debug.Log("[Scanner] " + result.ToString());
        }
    }

    // Update is called once per frame

}
