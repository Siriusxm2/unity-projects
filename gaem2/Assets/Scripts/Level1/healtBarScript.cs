using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healtBarScript : MonoBehaviour {

	public Slider slider;
    public Gradient grad;
    public Image fill;

    [SerializeField] private AudioSource asrc;
    [SerializeField] private AudioSource DeathSFX;
    private AudioClip c;

    private bool first = false;

    private void Start() {
        first = true;
        c = DeathSFX.clip;
    }

    private void Update() {
        if (first == true)
        for (int i = 0 ; i < 1 ; i++)
            if (slider.value <= 0) {
                asrc.PlayOneShot(c);
                first = false;
            }
    }

    public void setMaxHealth(int hpm)
    {
        slider.maxValue = hpm;
        slider.value = hpm;

        fill.color = grad.Evaluate(1f);
    }

	public void setHealth(int hp)
    {
        slider.value = hp;

        fill.color = grad.Evaluate(slider.normalizedValue);
    }

    
}
