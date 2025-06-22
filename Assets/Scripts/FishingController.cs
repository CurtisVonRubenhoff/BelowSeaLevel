using UnityEngine;

public class FishingController : MonoBehaviour
{
    public enum FishingState
    {
        ZONE_SELECT,
        CASTING,
        DEFAULT
    }

    [SerializeField] private GameObject playerBody;
    [SerializeField] private Animator _animator;
    
    private FishingState _currentState;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void StartFishing()
    {
        
    }

    void SetState(FishingState state)
    {
        _currentState = state;

        switch (_currentState)
        {
            case FishingState.ZONE_SELECT:
                break;
            case FishingState.CASTING:
                break;
            case FishingState.DEFAULT:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var animState = _animator.GetInteger("AnimState");
        var oldState = animState;
        bool updateState = false;
        var mousePosition = Input.mousePosition;
        var screenWidth = Screen.width;
        var mouseHorizontalNormalized = mousePosition.x / screenWidth;

        animState = mouseHorizontalNormalized switch
        {
            < 0.33f => -1,
            (> 0.33f) and (< 0.66f) => 0,
            > 0.66f => 1,
        };

        if (Input.GetMouseButton(0))
        {
            animState = 2;
        }

        if (Input.GetMouseButtonUp(0))
        {
            animState = 1;
        }

        updateState = (oldState != animState);

        if (updateState)
        {
            _animator.SetInteger("AnimState", animState);
        }
    }
}
