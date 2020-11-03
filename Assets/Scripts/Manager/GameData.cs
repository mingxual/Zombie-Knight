using UnityEngine;

public class GameData : MonoBehaviour
{
    [Header("Enemy Parent")]
    public Transform enemyParent;

    [Header("Enemy Health")]
    public SpeedUp playerSpeedUp;
    public GameControl gameControl;

    [Header("Enemy Control")]
    public Transform playerTransform;
    public PlayerHealth playerHealth;
    public Utility utility;

    [Header("Blip Setting")]
    public RectTransform background;
    public fpsControl fps_control;
}
