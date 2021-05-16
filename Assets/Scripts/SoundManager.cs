using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource sfxPlayer;

    public GameObject audioPrefab;

    public AudioClip au_Xsound, au_Osound, au_ClickSound, au_EndSound;

    void Start()
    {
        instance = this;
    }

    public void AudioPlay(State checkState) 
    {
        switch (checkState)
        {
            case State.O:
                sfxPlayer.clip = au_Osound;
                sfxPlayer.PlayOneShot(au_Osound);
                break;

            case State.X:
                sfxPlayer.clip = au_Xsound;
                sfxPlayer.PlayOneShot(au_Xsound);
                break;
        }
    }

    public void CreateSound(AudioClip newClip)
    {
        SoundPrefab soundPrefabClass = Instantiate(audioPrefab).GetComponent<SoundPrefab>();
        soundPrefabClass.soundSource.clip = newClip;
        soundPrefabClass.soundSource.PlayOneShot(newClip);
    }
}
