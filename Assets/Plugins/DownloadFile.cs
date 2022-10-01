using System;
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
    public List<GameObject> GameObjectsToHide =  new();

    // optimize for many screenshots will not destroy any objects so future screenshots will be fast
    public bool optimizeForManyScreenshots = true;
    
    // private vars for screenshot
    private Rect _rect;
    private RenderTexture _renderTexture;
    [SerializeField] private Texture2D screenShot;
    private Camera camera;


    [DllImport("__Internal")]
    private static extern void FileDownload(byte[] array, int byteLength, string fileName);

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    public void DoScreenshot()
    {
        ChangeObjectsActiveState(false);

        // create screenshot objects if needed
        if (_renderTexture == null)
        {
            // creates off-screen render texture that can rendered into
            _rect = new Rect(0, 0, captureWidth, captureHeight);
            _renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
            screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        }

        //render scene into rt
        camera.targetTexture = _renderTexture;
        camera.Render();

        // read pixels will read from the currently active render texture so make our offscreen 
        // render texture active and then read the pixels
        RenderTexture.active = _renderTexture;
        screenShot.ReadPixels(_rect, 0, 0);

        // reset active camera texture and render texture
        camera.targetTexture = null;
        RenderTexture.active = null;
        
        byte[] fileData = screenShot.EncodeToPNG();
        //FileDownload(fileData, fileData.Length, "avatar400x400.png");
        File.WriteAllBytes(Application.dataPath + "/Screenshot.png", fileData);
        Destroy(screenShot);
        ChangeObjectsActiveState(true);

        // cleanup if needed
        if (optimizeForManyScreenshots == false)
        {
            Destroy(_renderTexture);
            _renderTexture = null;
            screenShot = null;
        }
    }

    private void ChangeObjectsActiveState(bool makeActive)
    {
        if (GameObjectsToHide.Count == 0) return;
        foreach (var go in GameObjectsToHide)
        {
            go.SetActive(makeActive);
        }
    }

}
    
