using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour {

    public static AudioClip jumpSound, jumpHSound, teleportSound, deathSound, bounceSound, wallSound, exitSound, keySound;
    static AudioSource audioSrc;

	void Start () {

        jumpSound = Resources.Load<AudioClip>("Jump");
        jumpHSound = Resources.Load<AudioClip>("JumpH");
        teleportSound = Resources.Load<AudioClip>("Teleport");
        deathSound = Resources.Load<AudioClip>("Death");
        bounceSound = Resources.Load<AudioClip>("Bounce");
        wallSound = Resources.Load<AudioClip>("Wall");
        keySound = Resources.Load<AudioClip>("Key");
        exitSound = Resources.Load<AudioClip>("Exit");

        audioSrc = GetComponent<AudioSource>();
    }
	
	
	void Update () {
		
	}


    public static void PlaySound (string clip)
    {
        switch (clip)
        {
            case "Jump":
                audioSrc.PlayOneShot(jumpSound);
                break;
            case "JumpH":
                audioSrc.PlayOneShot(jumpHSound);
                break;
            case "Teleport":
                audioSrc.PlayOneShot(teleportSound);
                break;
            case "Death":
                audioSrc.PlayOneShot(deathSound);
                break;
            case "Bounce":
                audioSrc.PlayOneShot(bounceSound);
                break;
            case "Wall":
                audioSrc.PlayOneShot(wallSound);
                break;
            case "Key":
                audioSrc.PlayOneShot(keySound);
                break;
            case "Exit":
                audioSrc.PlayOneShot(exitSound);
                break;
        }
    }
}
