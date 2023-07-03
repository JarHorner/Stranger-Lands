using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour
{

    #region Variables
    private static bool exists;
    private Vector3 centerPoint = new Vector3(0,0,0);
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioMixerGroup backgroundAudioMixer;
    [SerializeField] private AudioMixerGroup soundEffectAudioMixer;
    #endregion

    #region Unity Methods

    void Start()
    {
        //starts by playing background music when entering scene
        PlayBackground(backgroundMusic);
    }

    //creates a new GameObject, gives it a AudioSource component and adds the parameter as the clip then plays it.
    //Used for sound effects
    public void Play(AudioClip clip)
    {
        GameObject newSound = new GameObject();
        newSound.transform.position = this.transform.position;
        newSound.name = clip.name;
        //newSound.transform.parent = gameObject.transform;

        newSound.AddComponent(typeof(AudioSource));
        AudioSource source = newSound.GetComponent<AudioSource>();
        source.outputAudioMixerGroup = soundEffectAudioMixer;
        source.clip = clip;
        source.priority = 0;
        source.volume = 0.2f;
        source.Play();
        Destroy(newSound, 1f);
    }

    //creates a new GameObject, gives it a AudioSource component and adds the parameter as the clip then plays it.
    //Used for background music. difference between this and Play are the volume and priority levels, also looping.
    private void PlayBackground(AudioClip clip)
    {
        GameObject newSound = new GameObject();
        newSound.name = clip.name;
        newSound.transform.position = this.transform.position;
        newSound.transform.parent = gameObject.transform;

        newSound.AddComponent(typeof(AudioSource));
        AudioSource source = newSound.GetComponent<AudioSource>();
        source.outputAudioMixerGroup = backgroundAudioMixer;
        source.clip = clip;
        source.priority = 0;
        source.loop = true;
        source.Play();
    }
    #endregion
}
