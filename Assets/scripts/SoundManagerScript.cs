using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour {

    public static AudioClip jumpSound, jumpHSound, teleportSound, deathSound;
    static AudioSource audioSrc;

	void Start () {

        jumpSound = Resources.Load<AudioClip>("Jump");
        jumpHSound = Resources.Load<AudioClip>("JumpH");
        teleportSound = Resources.Load<AudioClip>("Teleport");
        deathSound = Resources.Load<AudioClip>("Death");
        

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


        }
    }
}
