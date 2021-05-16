using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Animator anim;
    int sceneInt;
    public AudioSource clickSound;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void singlePlayer()
    {
        clickSound.Play();
        sceneInt = 1;
        anim.Play("FadeOut");
    }

    public void multiplayButton()
    {
        clickSound.Play();
        sceneInt = 2;
        anim.Play("FadeOut");
    }

    public void LeaveGame()
    {
        clickSound.Play();
        Application.Quit();
    }

    //Event
    void LoadGameScene()
    {
        SceneManager.LoadScene(sceneInt);
    }
}
