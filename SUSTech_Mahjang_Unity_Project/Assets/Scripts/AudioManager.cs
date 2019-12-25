using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //将声音管理器写成单例模式
    public static AudioManager Am;
    //音乐播放器
    public AudioSource MusicPlayer;
    //音效播放器
    public AudioSource TileEnterSoundPlayer;

    public AudioSource TilePlaySoundPlayer;
    void Start()
    {
        Am = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //播放音乐
    public void PlayMusic()
    {
        if (MusicPlayer.isPlaying == false)
        {
            MusicPlayer.Play();
        }

    }

    //播放音效
    public void PlayTileEnterSound()
    {
        TileEnterSoundPlayer.Play();
    }



    public void PlayTilePlaySound()
    {
        TilePlaySoundPlayer.Play();
    }
}

