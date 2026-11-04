using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

/*
 * Uses LeanTween (V2.50) for animation
 */

public class SceneTransitionController : MonoBehaviour
{
    [SerializeField] private Sprite transitionImage;

    [SerializeField] private RectTransform leftMask;
    [SerializeField] private Image leftImage;
    [SerializeField] private RectTransform rightMask;
    [SerializeField] private Image rightImage;

    private string currScene;

    public event Action transitionHidden;
    public event Action transitionShowing;



    //Called in the Editor whenever a serialized value changes in the Inspector
    //Shows the updated transition image without having to hit Play first
    private void OnValidate()
    {
        if (leftImage != null) leftImage.sprite = transitionImage;
        if (rightImage != null) rightImage.sprite = transitionImage;
    }


    private void Awake()
    {
        currScene = SceneManager.GetActiveScene().name;

        leftImage.sprite = transitionImage;
        rightImage.sprite = transitionImage;

        Hide(false);
    }
        

    public void Show()
    {
        LeanTween.moveX(leftMask, -852, 0.5f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() => transitionShowing?.Invoke());
        LeanTween.moveX(rightMask, 870, 0.5f).setEase(LeanTweenType.easeInOutCubic);
    }


    public void Hide(bool animate = true)
    {
        if (animate)
        {
            LeanTween.value(gameObject, leftMask.anchoredPosition.x, -2200f, 0.5f)
                .setEase(LeanTweenType.easeInOutCubic)
                .setOnUpdate((float x) =>
                {
                    Vector2 pos = leftMask.anchoredPosition;
                    pos.x = x;
                    leftMask.anchoredPosition = pos;
                })
                .setOnComplete(() => transitionHidden?.Invoke());

            LeanTween.value(gameObject, rightMask.anchoredPosition.x, 2200f, 0.5f)
                .setEase(LeanTweenType.easeInOutCubic)
                .setOnUpdate((float x) =>
                {
                    Vector2 pos = rightMask.anchoredPosition;
                    pos.x = x;
                    rightMask.anchoredPosition = pos;
                });
        }
        else
        {
            //just set the position with no animation
            Vector2 pos = leftMask.anchoredPosition;
            pos.x = -2200f;
            leftMask.anchoredPosition = pos;

            pos = rightMask.anchoredPosition;
            pos.x = 2200f;
            rightMask.anchoredPosition = pos;

            transitionHidden?.Invoke();
        }
    }
}
