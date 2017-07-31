using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour {
    private static MusicBox instance = null;
    AudioSource audio;
    public AudioClip bgm;

    public static MusicBox Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
            audio = GetComponent<AudioSource>();
            //audio.Play();
        }

        DontDestroyOnLoad(this.gameObject);
    }


}
