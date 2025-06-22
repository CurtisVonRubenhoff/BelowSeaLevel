using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class ColorCycle : MonoBehaviour
{

    [SerializeField] private Image _image;
    [SerializeField] private Image _outline;
    [SerializeField] private float cycleSpeed = 2.0f;
    
    private static UnityEvent _niceTriggerEvent = new UnityEvent();

    private Tweener _tweener;

    private static float BottomPosition = -900f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        _niceTriggerEvent.AddListener(HandleNiceTrigger);
    }

    void OnDisable()
    {
        _niceTriggerEvent.RemoveListener(HandleNiceTrigger);
        
    }

    private void HandleNiceTrigger()
    { 
        _outline.color = new Color(_outline.color.r, _outline.color.g, _outline.color.b, 1);
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(800f, 1.5f).SetEase(Ease.Linear));
        sequence.Append(transform.DOMoveY(BottomPosition, 1.5f).SetEase(Ease.Linear));
        sequence.Insert(2f, _image.DOFade(0f, 1.5f)).SetEase(Ease.Linear);
        sequence.Insert(2f, _outline.DOFade(0f, 1.5f)).SetEase(Ease.Linear);
        
    }

    // Update is called once per frame
    void Update()
    {
        var current = _image.color;

         float H, S, V;

        Color.RGBToHSV(current, out H, out S, out V);

        H += Time.deltaTime * cycleSpeed;

        _image.color = Color.HSVToRGB(H, S, V);

        
    }

    public static void TriggerNice()
    {
        _niceTriggerEvent.Invoke();
    }
}
