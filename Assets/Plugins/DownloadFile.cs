using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;

public class DownloadFile : MonoBehaviour
{

    public int captureWidth = 400;
    public int captureHeight = 400;

    // optional game object to hide during screenshots (usually your scene canvas hud)
    public GameObject hideGameObject;

    // optimize for many screenshots will not destroy any objects so future screenshots will be fast
    public bool optimizeForManyScreenshots = true;

    // configure with raw, jpg, png, or ppm (simple raw format)
    public enum Format { RAW, JPG, PNG, PPM };
    public Format format = Format.PPM;

    // folder to write output (defaults to data path)
    public string folder;

    // private vars for screenshot
    private Rect rect;
    private RenderTexture renderTexture;
    [SerializeField] private Texture2D screenShot;
    private int counter = 0; // image #

    // commands
    private bool captureScreenshot = false;
    private bool captureVideo = false;
    
    
    [DllImport("__Internal")]
    private static extern void FileDownload(byte[] array, int byteLength, string fileName);
    
    private void Start()
    {
        DoScreenshot();
    }

    public void CaptureScreenshot()
    {
        captureScreenshot = true;
    }

    private void DoScreenshot()
    {
        // check keyboard 'k' for one time screenshot capture and holding down 'v' for continious screenshots
        //captureScreenshot |= Input.GetKeyDown("k");
        //captureVideo = Input.GetKey("v");

        //if (!captureScreenshot && !captureVideo) return;
        captureScreenshot = false;

        // hide optional game object if set
        if (hideGameObject != null) hideGameObject.SetActive(false);

        // create screenshot objects if needed
        if (renderTexture == null)
        {
            // creates off-screen render texture that can rendered into
            rect = new Rect(0, 0, captureWidth, captureHeight);
            renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
            screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        }

        // get main camera and manually render scene into rt
        Camera camera = Camera.main; // NOTE: added because there was no reference to camera in original script; must add this script to Camera
        camera.targetTexture = renderTexture;
        camera.Render();

        // read pixels will read from the currently active render texture so make our offscreen 
        // render texture active and then read the pixels
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);

        // reset active camera texture and render texture
        camera.targetTexture = null;
        RenderTexture.active = null;

        // get our unique filename
        //string filename = uniqueFilename((int)rect.width, (int)rect.height);

        // pull in our file header/data bytes for the specified image format (has to be done from main thread)
        byte[] fileHeader = null;
        byte[] fileData = null;
        // if (format == Format.RAW)
        // {
        //     fileData = screenShot.GetRawTextureData();
        // }
        // else if (format == Format.PNG)
        // {
        // }
        // else if (format == Format.JPG)
        // {
        //     fileData = screenShot.EncodeToJPG();
        // }
        // else // ppm
        // {
        //     // create a file header for ppm formatted file
        //     string headerStr = string.Format("P6\n{0} {1}\n255\n", rect.width, rect.height);
        //     fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
        //     fileData = screenShot.GetRawTextureData();
        // }
        fileData = screenShot.EncodeToPNG();
        print(fileData);
        FileDownload(fileData, fileData.Length, "screenshot.png");

        // unhide optional game object if set
        if (hideGameObject != null) hideGameObject.SetActive(true);

        // cleanup if needed
        if (optimizeForManyScreenshots == false)
        {
            Destroy(renderTexture);
            renderTexture = null;
            screenShot = null;
        }
    }

}
    
