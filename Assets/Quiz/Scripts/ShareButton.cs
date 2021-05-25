using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShareButton : MonoBehaviour
{
    public void sharemeappnow() {
        StartCoroutine(TakeScreenshotAndShare());
    }
    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath)
            .SetSubject("Hit The word").SetText("you wont believe this game is so amazing check out my score, i am learning arabic words while having fun!").SetUrl("https://www.google.com/search?q=hit+the+word&btnK=Google+Search&sxsrf=ALeKk01L6SIvtwY3iXQTioiWCDzJK_QSrw%3A1619697396401&source=hp&ei=9J6KYPKJFuG-3LUPl7-YwAI&iflsig=AINFCbYAAAAAYIqtBDZYPBCs4SbqQq94RsYeBtlvJm_r")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();

        // Share on WhatsApp only, if installed (Android only)
        //if( NativeShare.TargetExists( "com.whatsapp" ) )
        //	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
    }
}
