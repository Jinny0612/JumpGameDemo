using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ��Ϸ��ʼ����
/// </summary>
public class GameManager : MonoBehaviour
{
    private new AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        if (audio != null)
        {
            //���button������Ч
            audio.Play();
        }
    }
}
