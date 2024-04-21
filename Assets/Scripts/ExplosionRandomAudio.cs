using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物体落地爆炸，随机音效
/// </summary>
public class ExplosionRandomAudio : MonoBehaviour
{
    public AudioClip[] audioClips;//预先设置好的音效列表

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
