using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public TMP_Text StartButtonText;
    private SceneManagerController SceneManagerController;

    void Awake()
    {
        SceneManagerController = Object.FindObjectOfType<SceneManagerController>();
        SceneManagerController.currentSceneIndex = 0;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) 
        {
            SceneManagerController.goToNextScene();
        }
    }

    public void OnClickStart() 
    {
        SceneManagerController.goToNextScene();
    }
}