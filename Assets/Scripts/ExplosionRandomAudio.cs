using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ر�ը�������Ч
/// </summary>
public class ExplosionRandomAudio : MonoBehaviour
{
    public AudioClip[] audioClips;//Ԥ�����úõ���Ч�б�

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();

        int randSound = Random.Range(0, audioClips.Length);
        audio.clip = audioClips[randSound];
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
