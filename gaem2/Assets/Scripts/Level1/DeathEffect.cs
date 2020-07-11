using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    [SerializeField] private AudioSource asrc;
    [SerializeField] private AudioSource DeathSFX;

    void Awake() {
        if (gameObject.activeSelf)
            PlayEffect();
    }

    private void PlayEffect() {
        DeathSFX.Play();
    }

}
