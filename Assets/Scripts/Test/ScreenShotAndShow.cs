using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotAndShow : MonoBehaviour
{
    [SerializeField] private RawImage displayImages;
    [SerializeField] private int captureWidth = 300;
    [SerializeField] private int captureHeight = 500;

    private Texture2D croppedTexture;

    private void Update()
    {
        /*        if (Input.GetKeyDown(KeyCode.P))
                {
                    TakePictureAndShow();
                }*/
    }

    public void TakePictureAndShow()
    {
        StartCoroutine(TakeScreenShotAndShow());
    }

    private IEnumerator TakeScreenShotAndShow()
    {
        yield return new WaitForEndOfFrame();
        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();

        int startX = (Screen.width - captureWidth) / 2;
        int startY = (Screen.height - captureHeight) / 2;

        croppedTexture = new Texture2D(captureWidth, captureHeight + 400);
        croppedTexture.SetPixels(screenshot.GetPixels(startX, startY, captureWidth, captureHeight + 400));
        croppedTexture.Apply();

        displayImages.texture = croppedTexture;

        Destroy(screenshot);
    }
}
