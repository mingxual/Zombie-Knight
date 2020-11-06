using UnityEngine;
using UnityEngine.UI;

public class BleedingBorderEffect : MonoBehaviour
{
    public Image border;
    public float bleedingTime;
    private float timer = 0;
    private bool increase = true;

    void Update()
    {
        timer += Time.deltaTime;
        Color color = border.color;
        color.a = Mathf.Lerp(0.2f, 0.5f, increase? timer/bleedingTime : (1 - timer/bleedingTime));
        border.color = color;
        if (timer >= bleedingTime)
        {
            timer = 0;
            increase = !increase;
        }
    }
}
