using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class ScreenCapture : MonoBehaviour
{

    public FirebaseManager firebaseManager;

    private PhotoCapture photoCaptureObject = null;

    // Start is called before the first frame update
    void Start()
    {
        PhotoCapture.CreateAsync(true, OnPhotoCaptureCreated);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartCamera() //Runs when capture button is pressed, Only activates camera for phototaking when needed as photo mode requires heavy resources
    {
        //StartCoroutine(StartCameraCapture());
    }

    //private IEnumerator StartCameraCapture()
    //{
    //    //checks if app has permissions to use Hololens 2 webcam, if not, it requests for permission
    //    if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
    //    {
    //        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
    //    }
    //    if (Application.HasUserAuthorization(UserAuthorization.WebCam))
    //    {
    //        Debug.Log("Creating PhotoCapture");
    //        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
    //    }
    //    else
    //    {
    //        Debug.Log("Webcam Permission not granted");
    //    }
    //}

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

        CameraParameters c = new CameraParameters();
        c.hologramOpacity = 0.0f;
        c.cameraResolutionWidth = cameraResolution.width;
        c.cameraResolutionHeight = cameraResolution.height;
        c.pixelFormat = CapturePixelFormat.BGRA32;

        captureObject.StartPhotoModeAsync(c, OnPhotoModeStarted); //Activates camera and passes in camera settings
    }

    private void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result) //function runs when photo mode ends
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result) //checks to see if photocapture mode started successfully
    {
        if (result.success) //if started, runs function to take a photo
        {
            photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
        }
        else
        {
            Debug.LogError("Unable to start photo mode!");
        }
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame) //function gets photocapture result to convert to texture
    {
        if (result.success)
        {
            // Create our Texture2D for use and set the correct resolution
            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            Texture2D texture = new Texture2D(cameraResolution.width, cameraResolution.height);
            // Copy the raw image data into our target texture
            photoCaptureFrame.UploadImageDataToTexture(texture);

            //Converts texture into base64 string data
            string imgData = ConvertToBase64(texture);
            firebaseManager.SavePhotoData(imgData); //saves img into firebaseManager list for upload when saving data

        }
        // Stops Camera after image is taken
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }
}
