using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏开始管理
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
            //点击button触发音效
            audio.Play();
        }
    }
}
