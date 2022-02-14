using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using TMPro;

public class ScreenCapture : MonoBehaviour
{

    public FirebaseManager firebaseManager;
    public TextMeshPro debugText;

    private PhotoCapture photoCaptureObject = null;
    public Texture2D targetTexture = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string ConvertToBase64(Texture2D textureConvert) //function returns a base64 from input texture
    {
        byte[] imgBytes = textureConvert.EncodeToPNG();
        string data = Convert.ToBase64String(imgBytes);
        return data;
    }

    private void OnPhotoCaptureCreated(PhotoCapture captureObject) //when photo capture obj is created, function runs to set camera settings
    {
        photoCaptureObject = captureObject;

        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        CameraParameters camera = new CameraParameters();
        camera.hologramOpacity = 0.0f;
        camera.cameraResolutionWidth = cameraResolution.width;
        camera.cameraResolutionHeight = cameraResolution.height;
        camera.pixelFormat = CapturePixelFormat.BGRA32;

        debugText.text = "Creating Camera";
        photoCaptureObject.StartPhotoModeAsync(camera, OnPhotoModeStarted);
    }

    public void TakePhoto() //When button is pressed, it activates camera mode and passes in camera settings which will then take a photo
    {
        debugText.text = "Button Pressed!";
        PhotoCapture.CreateAsync(true, OnPhotoCaptureCreated); //creates photo capture object on app start
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result) //checks to see if photocapture mode started successfully
    {
        if (result.success) //if started, runs function to take a photo
        {
            photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory); //takes photo
            debugText.text = "Started Camera!";
        }
        else
        {
            Debug.LogError("Unable to start photo mode!");
            debugText.text = "Unable to start photo mode!";
        }
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame) //function gets photocapture result to convert to texture
    {
        debugText.text = "Running Captured Photo to memory";
        if (result.success)
        {
            debugText.text = "Took photo success!";
            // Create our Texture2D for use and set the correct resolution
            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
            // Copy the raw image data into our target texture
            photoCaptureFrame.UploadImageDataToTexture(targetTexture);
            debugText.text = "Save photo success";
            //Converts texture into base64 string data
            string imgData = ConvertToBase64(targetTexture);
            firebaseManager.SavePhotoData(imgData); //saves img into firebaseManager list for upload when saving data

        } else
        {
            debugText.text = "Unable to take photo!";
        }
        // Stops Camera after image is taken
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode); 
    }

    private void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result) //function runs when photo mode ends
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }
}
