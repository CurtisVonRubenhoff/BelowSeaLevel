using UnityEngine;
using UnityEngine.UI;

public class PowerMeter : MonoBehaviour
{

    [SerializeField] private Image _powerMeterFill;
    [SerializeField] private float _powerMeterSpeed = 2.0f;

    public float CurrentPower {
        get { return currentPower; }
    }


    private float currentPower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnEnable()
    {
        currentPower = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        currentPower += Time.deltaTime * _powerMeterSpeed;

        if (currentPower > 1.0f) currentPower = 0f;

        _powerMeterFill.fillAmount = currentPower;
    }
}
