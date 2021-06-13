using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    public Sprite ImageOn;
    public Sprite ImageOff;
    public AudioController AudioController;
    private Image Image;

    void Awake()
    {
        Image = GetComponent<Image>();
    }

    public void OnClick() 
    {
        AudioController.ToggleMute();
        Image.sprite = AudioController.IsMuted ? ImageOff : ImageOn;
    }
}
