using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Audio : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip openingSound;
    public AudioClip duringGameSound;

    bool soundIsChanged = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = openingSound;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(FindObjectOfType<Opening>() == null)
        {
            if(!soundIsChanged)
            {
                //StartCoroutine(FadeOutMusic(3f));

                audioSource.clip = duringGameSound;
                audioSource.loop = true;
                audioSource.Play();
                soundIsChanged = true;
            }
        }
    }

    IEnumerator FadeOutMusic(float fadeTime)
    {
        float startVolume = audioSource.volume;

        while(audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
