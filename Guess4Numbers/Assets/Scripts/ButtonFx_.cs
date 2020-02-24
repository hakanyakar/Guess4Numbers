using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFx_ : MonoBehaviour
{
    public AudioSource myFx;
    public AudioClip setFx;
    public AudioClip setBackFx;
    public AudioClip buttonFx;

    public void SetSound()
    {
        myFx.PlayOneShot(setFx);
    }
    public void SetBackSound()
    {
        myFx.PlayOneShot(setBackFx);
    }
    public void ButtonSound()
    {
        myFx.PlayOneShot(buttonFx);
    }
}
