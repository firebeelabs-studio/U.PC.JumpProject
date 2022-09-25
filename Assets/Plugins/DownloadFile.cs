using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DownloadFile : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void FileDownload(byte[] array, int byteLength, string fileName);

    private void CreateTexture()
    {
        var texture = TakeScreenshot.CaptureScreenshot();
        byte[] textureBytes = texture.EncodeToPNG();
        FileDownload(textureBytes, textureBytes.Length, "screenshot.png");
        Destroy(texture);
    }

    private void Start()
    {
        CreateTexture();
    }

}
    
