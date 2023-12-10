using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private Sound[] sfx = null;
    [SerializeField] private Sound[] bgm = null;

    [SerializeField] private AudioSource bgmPlayer = null;
    [SerializeField] private AudioSource[] sfxPlayer = null;

    private int bgmCount = 0;
    #region Unity Callback
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        if (bgmCount == 2)
        {
            bgmCount = 0;
        }
        if (bgmPlayer.isPlaying) return;
        PlayBGM(bgm[bgmCount].name);
    }
    #endregion

    public void PlayBGM(string p_bgmName)
    {
        for(int i = 0; i < bgm.Length; i++)
        {
            if(p_bgmName == bgm[i].name)
            {
                bgmPlayer.clip = bgm[i].clip;
                bgmPlayer.Play();
                bgmCount++;
            }
        }
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(string p_sfxName)
    {
        for(int i = 0; i < sfx.Length; i++)
        {
            if(p_sfxName == sfx[i].name)
            {
                for(int j = 0; j < sfxPlayer.Length; j++)
                {
                    if(!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = sfx[i].clip;
                        sfxPlayer[j].PlayOneShot(sfx[i].clip);
                        return;
                    }
                }
                return;
            }
        }
        return;
    }
}
