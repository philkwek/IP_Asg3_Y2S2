using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using TMPro;

public class NewScreenCapture : MonoBehaviour
{
    private PhotoCapture photoCaptureObject = null;
    public TextMeshProUGUI debugText;
    // Start is called before the first frame update
    public void StartCap()
    {
        PhotoCapture.CreateAsync(true, OnPhotoCaptureCreated);
    }

    void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        photoCaptureObject = captureObject;

        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        CameraParameters c = new CameraParameters();
        c.hologramOpacity = 0.0f;
        c.cameraResolutionWidth = cameraResolution.width;
        c.cameraResolutionHeight = cameraResolution.height;
        c.pixelFormat = CapturePixelFormat.BGRA32;

        captureObject.StartPhotoModeAsync(c, OnPhotoModeStarted);
    }
    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            debugText.text = "Photo mode started, taking picture";
            photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
        }
        else
        {
            Debug.LogError("Unable to start photo mode!");
        }
    }
    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {
            // Create our Texture2D for use and set the correct resolution
            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
            // Copy the raw image data into our target texture
            photoCaptureFrame.UploadImageDataToTexture(targetTexture);
            // Do as we wish with the texture such as apply it to a material, etc.
            string imgData = ConvertToBase64(targetTexture);
            debugText.text = imgData;
        }
        // Clean up
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    public string ConvertToBase64(Texture2D textureConvert) //function returns a base64 from input texture
    {
        byte[] imgBytes = textureConvert.EncodeToPNG();
        string data = Convert.ToBase64String(imgBytes);
        return data;
    }
}
