using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string BGMPref = "BGMPref";
    private static readonly string BruhPref = "BruhPref";
    private static readonly string SfxPref = "SfxPref";

    private int firstPlayInt;
    public Slider bgmS, bruhS, sfxS;
    private float bgmFloat, bruhFloat, sfxFloat;
    public AudioSource bgmAudio, endAudio;
    public AudioSource bruhAudio;
    public AudioSource[] sfxAudio;

    void Start()
    {
        bgmAudio = GameObject.FindObjectOfType<DontDestroy>().GetComponent<AudioSource>();

        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
        {
            bgmFloat = 1f;
            bruhFloat = 1f;
            sfxFloat = 1f;

            bgmS.value = bgmFloat;
            bruhS.value = bruhFloat;
            sfxS.value = sfxFloat;

            PlayerPrefs.SetFloat(BGMPref, bgmFloat);
            PlayerPrefs.SetFloat(BruhPref, bruhFloat);
            PlayerPrefs.SetFloat(SfxPref, sfxFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);

        }

        else
        {
            bgmFloat = PlayerPrefs.GetFloat(BGMPref);
            bgmS.value = bgmFloat;
            bruhFloat = PlayerPrefs.GetFloat(BruhPref);
            bruhS.value = bruhFloat;
            sfxFloat = PlayerPrefs.GetFloat(SfxPref);
            sfxS.value = sfxFloat;
        }

    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(BGMPref, bgmS.value);
        PlayerPrefs.SetFloat(BruhPref, bruhS.value);
        PlayerPrefs.SetFloat(SfxPref, sfxS.value);
    }

    //Exit on Quit without saving
    void OnAppFocus(bool inFocus)
    {
        if (!inFocus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound() {
        bgmAudio.volume = bgmS.value;
        endAudio.volume = bgmS.value;
        bruhAudio.volume = bruhS.value;
        for (int i = 0 ; i < sfxAudio.Length ; i++) {
            sfxAudio[i].volume = sfxS.value;
        }
    }
}
