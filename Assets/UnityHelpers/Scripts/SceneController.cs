using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 * Attached to SceneTransitioner
 * Works with the SceneTransitionController
 * Shows the transition image, async loads the new scene, then hides the transition image
 * in order to reveal the new scene.
 */

public class SceneController : MonoBehaviour
{
    [SerializeField] private SceneTransitionController sceneTransitionController;

    private string _sceneNameBeingLoaded;


    public void LoadNewScene(string sceneName)
    {
        _sceneNameBeingLoaded = sceneName;
        //wait for transition to finish animating on before starting load
        sceneTransitionController.transitionShowing += startAsyncLoad;
        sceneTransitionController.Show(); 
    }


    private void startAsyncLoad()
    {
        StartCoroutine(LoadSceneAsyncCoroutine());
    }


    private IEnumerator LoadSceneAsyncCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneNameBeingLoaded);

        //prevent scene from auto starting once loaded
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)//won't trigger until allowSceneActivation = true
        {
            //Unity splits async progress into two parts:
            //0.0 to 0.9 = Loading assets and data
            //0.9 to 1.0 = Activating the scene
            //float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            //all assets done loading once progress >= .9
            if (asyncLoad.progress >= 0.9f)
            {
                sceneTransitionController.Hide();
                sceneTransitionController.transitionShowing -= startAsyncLoad;
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }        
    }
}
