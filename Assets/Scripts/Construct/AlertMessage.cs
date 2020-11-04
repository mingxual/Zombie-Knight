using UnityEngine;
using UnityEngine.UI;

public class AlertMessage : MonoBehaviour
{
    public Text text;
    public CameraControl constructionCamera;

    public void alertMessage(string message)
    {
        text.text = message;
        constructionCamera.pause = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        constructionCamera.pause = false;
        gameObject.SetActive(false);
    }
}
