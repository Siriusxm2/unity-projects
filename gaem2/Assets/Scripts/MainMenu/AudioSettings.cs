using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    private static readonly string BGMPref = "BGMPref";
    private static readonly string BruhPref = "BruhPref";
    private static readonly string SfxPref = "SfxPref";

    private float bgmFloat, bruhFloat, sfxFloat;
    public AudioSource bgmAudio;
    public AudioSource bruhAudio;
    public AudioSource endAudio;
    public AudioSource[] sfxAudio;

    void Awake()
    {
        ContinueSettings();
    }
    
    private void ContinueSettings() {

        bgmFloat = PlayerPrefs.GetFloat(BGMPref);
        bruhFloat = PlayerPrefs.GetFloat(BruhPref);
        sfxFloat = PlayerPrefs.GetFloat(SfxPref);

        bgmAudio.volume = bgmFloat;
        endAudio.volume = bgmFloat;
        bruhAudio.volume = bruhFloat;
        for (int i = 0 ; i < sfxAudio.Length ; i++)
            sfxAudio[i].volume = sfxFloat;
    }
}
