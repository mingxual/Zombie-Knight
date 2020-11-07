using UnityEngine;
using UnityEngine.UI;

public class Bandage : MonoBehaviour
{
    public GameObject bandageMenu, crossHair;
    public Image fillIn;
    private PlayerHealth health;
    private Rigidbody body;

    public float bandagingTime;
    private float timer;

    private bool finishBandage, canBandage, isBandaging;

    void Start()
    {
        health = GetComponent<PlayerHealth>();
        body = GetComponent<Rigidbody>();
        finishBandage = false;
        canBandage = true;
        isBandaging = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canBandage)
        {
            timer = 0;
            isBandaging = true;
            canBandage = false;
            finishBandage = false;
            bandageMenu.SetActive(true);
            crossHair.SetActive(false);
        }
        if (Input.GetKey(KeyCode.E) && isBandaging && hasBandage())
        {
            if (body.velocity != Vector3.zero || health.attacked || Input.GetMouseButton(0))
            {
                finishBandage = true;
            }
            else 
            {
                timer += Time.deltaTime;
                fillIn.fillAmount = timer / bandagingTime;

                if (timer >= bandagingTime)
                {
                    health.Recover(50);
                    consumeOneBandage();
                    finishBandage = true;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.E) || finishBandage)
        {
            canBandage = true;
            fillIn.fillAmount = 0;
            isBandaging = false;
            finishBandage = false;
            bandageMenu.SetActive(false);
            crossHair.SetActive(true);
        }
    }

    // Check if there is bandage in bag
    bool hasBandage()
    {
        return WeaponSwitch.instance.hasMedicineBag();
    }

    // remove one bandage from bag
    void consumeOneBandage()
    {
        WeaponSwitch.instance.consumeMedicineBag();
    }
}
