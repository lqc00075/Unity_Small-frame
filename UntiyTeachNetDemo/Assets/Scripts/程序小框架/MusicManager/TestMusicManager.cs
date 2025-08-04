using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestMusicManager : MonoBehaviour
{
    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Slider slider;

    public Button btn4;
    public Button btn5;
    public Slider slider2;

    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        btn1.onClick.AddListener(() => {
            MusicManager.GetInstance().PlayBackgroundMusic("Bg_1");
        });
        btn2.onClick.AddListener(() => {
            MusicManager.GetInstance().PauseBackgroundMusic();
        });
        btn3.onClick.AddListener(() => {
            MusicManager.GetInstance().StopBackgroundMusic();
        });

        btn4.onClick.AddListener(() => {
            MusicManager.GetInstance().PlaySound("Sound", false, (clip) => {
                audio = clip;
            });
        });
        btn5.onClick.AddListener(() => {
            MusicManager.GetInstance().StopSound(audio);
        });
    }
    private void Change() {
        MusicManager.GetInstance().ChangeBackgroundMusicValue(slider.value);
        MusicManager.GetInstance().ChangeSoundValue(slider2.value);
    }
    // Update is called once per frame
    void Update()
    {
        Change();
    }
}
