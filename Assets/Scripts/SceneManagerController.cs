using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController: MonoBehaviour
{
    private Animator Animator;
    public int currentSceneIndex = 0;
    
    void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void reloadCurrentScene()
    {
        Animator.SetTrigger("FadeOut");
    }

    public void goToNextScene()
    {
        this.currentSceneIndex += 1;
        Animator.SetTrigger("FadeOut");
    }

    public void goToPreviousScene()
    {
        this.currentSceneIndex -= 1;
        Animator.SetTrigger("FadeOut");
    }

    public void OnFadeOutFinished()
    {
        SceneManager.LoadScene(this.currentSceneIndex);
    }
}