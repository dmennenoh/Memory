using System;
using UnityEngine;
using UnityEngine.UI;


/*
 * Corner Clicker is typically used for opening a configuration panel when a BA
 * clicks a set number of times in a corner of the screen.
 * 
 * Raises a cornerClickerActivated event when a certain number of clicks have
 * been registered in one, or more, corners. You can set the number of clicks
 * required in all four corners as well as the maximum time between clicks for each 
 * corner and the maximum time between successive corners.
 * 
 * There is no 'order' if UL = 3 clicks and LR = 2 clicks the BA/User may
 * double click LR first or triple click UL first
 * 
 * Possible TODO's: allow changing corner size - currently 200x200 which seems fine
 */


public enum ClickIndexes
{
    UL,//0
    UR,//1
    LR,//2
    LL //3
}

public class CornerClicker : MonoBehaviour
{
    [Header("Click Combination")]
    [SerializeField] private int ULClicksNeeded = 0;
    [SerializeField] private int URClicksNeeded = 4;
    [SerializeField] private int LRClicksNeeded = 0;
    [SerializeField] private int LLClicksNeeded = 0;

    [Header("Click Timing")]
    [SerializeField] private float cornerClickMaxTime = 0.5f; //max time between successive clicks in the same corner
    [SerializeField] private float betweenCornerMaxTime = 2f;//max time between corners if more than one corner needs clicking

    private int[] theClicks;//total number of clicks in each corner ul,ur,lr,ll  

    private float lastClickTime;//for measuring elapsed time between clicks

    public event Action cornerClickerActivated;//raised whenever theClicks match the Click Combination



    //Set corners inactive if no clicks are required on them
    //Initialize theCLicks to [0,0,0,0]
    private void Start()
    {
        theClicks = new int[4];  
        
        if(ULClicksNeeded == 0)
        {
            transform.Find("UL").gameObject.SetActive(false);
        }
        if (URClicksNeeded == 0)
        {
            transform.Find("UR").gameObject.SetActive(false);
        }
        if (LRClicksNeeded == 0)
        {
            transform.Find("LR").gameObject.SetActive(false);
        }
        if (LLClicksNeeded == 0)
        {
            transform.Find("LL").gameObject.SetActive(false);
        }
    }


    //Called by the button at UL in the prefab
    public void ULClicked()
    {
        CancelInvoke("resetAllClicks");

        if (theClicks[(int)ClickIndexes.UL] == 0)
        {
            theClicks[(int)ClickIndexes.UL] += 1;
        }
        else
        {
            if(Time.time - lastClickTime < cornerClickMaxTime)
            {
                CancelInvoke("resetULClicks");
                theClicks[(int)ClickIndexes.UL] += 1;
            }
        }        

        if (theClicks[(int)ClickIndexes.UL] == ULClicksNeeded)
        {   
            checkForDone();
        }
        else
        {
            Invoke("resetULClicks", cornerClickMaxTime);
        }

        Invoke("resetAllClicks", betweenCornerMaxTime);
        lastClickTime = Time.time;
    }


    //Called by the button at UR in the prefab
    public void URClicked()
    {
        CancelInvoke("resetAllClicks");

        if (theClicks[(int)ClickIndexes.UR] == 0)
        {
            theClicks[(int)ClickIndexes.UR] += 1;
        }
        else
        {
            if (Time.time - lastClickTime < cornerClickMaxTime)
            {
                CancelInvoke("resetURClicks");
                theClicks[(int)ClickIndexes.UR] += 1;
            }
        }

        if (theClicks[(int)ClickIndexes.UR] == URClicksNeeded)
        {   
            checkForDone();
        }
        else
        {
            Invoke("resetURClicks", cornerClickMaxTime);
        }

        Invoke("resetAllClicks", betweenCornerMaxTime);
        lastClickTime = Time.time;
    }

    //Called by the button at LR in the prefab
    public void LRClicked()
    {
        CancelInvoke("resetAllClicks");

        if (theClicks[(int)ClickIndexes.LR] == 0)
        {
            theClicks[(int)ClickIndexes.LR] += 1;
        }
        else
        {
            if (Time.time - lastClickTime < cornerClickMaxTime)
            {
                CancelInvoke("resetLRClicks");
                theClicks[(int)ClickIndexes.LR] += 1;
            }
        }

        if (theClicks[(int)ClickIndexes.LR] == LRClicksNeeded)
        {   
            checkForDone();
        }
        else
        {
            Invoke("resetLRClicks", cornerClickMaxTime);
        }

        Invoke("resetAllClicks", betweenCornerMaxTime);
        lastClickTime = Time.time;
    }

    //Called by the button at LL in the prefab
    public void LLClicked()
    {
        CancelInvoke("resetAllClicks");

        if (theClicks[(int)ClickIndexes.LL] == 0)
        {
            theClicks[(int)ClickIndexes.LL] += 1;
        }
        else
        {
            if (Time.time - lastClickTime < cornerClickMaxTime)
            {
                CancelInvoke("resetLLClicks");
                theClicks[(int)ClickIndexes.LL] += 1;
            }
        }

        if (theClicks[(int)ClickIndexes.LL] == LLClicksNeeded)
        {   
            checkForDone();
        }
        else
        {
            Invoke("resetLLClicks", cornerClickMaxTime);
        }

        Invoke("resetAllClicks", betweenCornerMaxTime);
        lastClickTime = Time.time;
    }


    
    //Called by Invokes()
    private void resetULClicks()
    {
        theClicks[(int)ClickIndexes.UL] = 0;
    }
    private void resetURClicks()
    {
        theClicks[(int)ClickIndexes.UR] = 0;
    }
    private void resetLRClicks()
    {
        theClicks[(int)ClickIndexes.LR] = 0;
    }
    private void resetLLClicks()
    {
        theClicks[(int)ClickIndexes.LL] = 0;
    }

    private void resetAllClicks()
    {
        Array.Clear(theClicks, 0, theClicks.Length);
    }


    private void checkForDone()
    {
        if (theClicks[(int)ClickIndexes.UL] == ULClicksNeeded && theClicks[(int)ClickIndexes.UR] == URClicksNeeded && theClicks[(int)ClickIndexes.LR] == LRClicksNeeded && theClicks[(int)ClickIndexes.LL] == LLClicksNeeded)
        {
            resetAllClicks();
            cornerClickerActivated?.Invoke();
        }
    }

}
