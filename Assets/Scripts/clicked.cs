using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image img;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private AudioClip pressClip;
    [SerializeField] private AudioClip unpressClip;
    [SerializeField] private AudioSource source;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        // if (button != null)
        // {
        //     button.onClick.AddListener(OnClicked);
        // }

        // Optional: Set default sprite at start
        if (img != null && defaultSprite != null)
        {
            img.sprite = defaultSprite;
        }
    }

    // public void OnClicked()
    // {
    //     // Optional logic for click action
    //     Debug.Log("Button clicked");
    // }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (img != null && pressedSprite != null)
            img.sprite = pressedSprite;

        if (source != null && pressClip != null)
            source.PlayOneShot(pressClip);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (img != null && defaultSprite != null)
            img.sprite = defaultSprite;

        if (source != null && unpressClip != null)
            source.PlayOneShot(unpressClip);
    }
}
