using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Animator anim;
    int sceneInt;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void singlePlayer()
    {
        sceneInt = 1;
        anim.Play("FadeOut");
    }

    public void multiplayButton()
    {
        sceneInt = 2;
        anim.Play("FadeOut");
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    //Event
    void LoadGameScene()
    {
        SceneManager.LoadScene(sceneInt);
    }
}
