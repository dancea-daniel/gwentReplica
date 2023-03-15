using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audioClips;

    public static AudioManager instance;
    public AudioSource source;

    public float duration;

    int r;

    void Awake()
    {
        if(instance==null)
        {
            instance = this;
            source = GetComponent<AudioSource>();
            audioClips = Resources.LoadAll<AudioClip>("Audio/Music");
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);


    }

    private void Start()
    {
        PlayNextMusic();
        source.volume = .2f;
    }

    public void StopMusic()
    {
        CancelInvoke("PlayNextMusic");
        source.Stop();
    }

    public void VolumeChange()
    {
        source.volume = (float)GameObject.Find("PausePanel").GetComponent<MenuButtons>().optionsButtons.GetComponentInChildren<Slider>().value;
    }

    void PlayNextMusic()
    {
        Debug.Log("belepbelep");
        if(source.clip == null)
        {
            Debug.Log("eredetileg nincs audio");
            r = Random.Range(0, audioClips.Length);
            source.clip = audioClips[r];
        }
        else
        {
            if(r + 1 >= audioClips.Length)
            {
                r=0;
                source.clip = audioClips[0];
            }
            else
            source.clip = audioClips[++r];
        }



        source.Play();

        duration = source.clip.length;
        Debug.Log("a mostani zene hossza " + source.clip.length);

        Invoke("PlayNextMusic", duration + 0.5f);
    }

}
