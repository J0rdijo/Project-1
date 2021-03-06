﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class movimiento : MonoBehaviour
{
    //Use this for initialization
    private float direction; //X component
    private bool jump;
    private Rigidbody2D controller;
    private float distSuelo;
    private float verticalHeight;
    private float distPared;
    private bool extraJump;
    private double doubleJumpRef;
    private double portalRef;
    private bool rWallJump;
    private bool lWallJump;
    //Layers
    private int groundLayer;
    private int boteParedLayer;
    private int dobleSalto;
    private int reboteLayer;
    private int deathLayer;
    private int nextLevelLayer;
    private int portalLayer;
    private int switchLayer;
    //Respawn
    private float xRes;
    private float yRes;
    private Vector2 position;
    private Vector2 resVel;
    private Vector2 positionL;
    private Vector2 positionR;
    private bool resSound;
    private double resSoundDelay;

    //Game Objects
    private Vector2 portalPos;
    private Vector3 portalAngle;

    //HUD reference
    private double wallJumpHUDRef;
    private double doubleJumpHUDRef;
    private double reboteHUDRef;
    private double portalHUDRef;

    void Start()
    {
        verticalHeight = 7.625f;
        controller = GetComponent<Rigidbody2D>();
        distSuelo = 0.6f;
        distPared = 0.45f;
        groundLayer = LayerMask.GetMask("ground");
        deathLayer = LayerMask.GetMask("muerte");
        boteParedLayer = LayerMask.GetMask("bote pared");
        dobleSalto = LayerMask.GetMask("doble salto");
        extraJump = false;
        reboteLayer = LayerMask.GetMask("rebotador");
        nextLevelLayer = LayerMask.GetMask("Next Level");
        portalLayer = LayerMask.GetMask("portal");
        switchLayer = LayerMask.GetMask("switch");
        rWallJump = true;
        lWallJump = false;
        //Respawn coordinates and velocity
        resVel.x = 0;
        resVel.y = 0;
        xRes = -9.78f;
        yRes = -3.17f;
        position.x = xRes;
        position.y = yRes;
        controller.position = position;
        jump = false;
        resSound = true;
        //partículas meta
        if (GameObject.Find("Textura Meta Desactivada") != null)
        {
            GameObject.Find("Meta").layer = 8;
            GameObject.Find("Textura Meta Desactivada").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/Textura Meta Desactivada");
            GameObject.Find("Particle Meta 1").GetComponent<ParticleSystem>().Stop();
            GameObject.Find("Particle Meta 2").GetComponent<ParticleSystem>().Stop();
            GameObject.Find("Particle Meta 3").GetComponent<ParticleSystem>().Stop();
            GameObject.Find("Particle Key 1").GetComponent<ParticleSystem>().Play();
            GameObject.Find("Particle Key 2").GetComponent<ParticleSystem>().Play();
            GameObject.Find("Particle Key 3").GetComponent<ParticleSystem>().Play();
            GameObject.Find("Particle Key 4").GetComponent<ParticleSystem>().Play();
        }
        //Skin
        switch (PlayerPrefs.GetInt("Player Skin"))
        {
            case 1:
                GameObject.Find("Robot").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/s2");
                break;
            case 2:
                GameObject.Find("Robot").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/skin1pixel");
                break;
            case 3:
                GameObject.Find("Robot").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/skin3-t");
                break;
            case 4:
                GameObject.Find("Robot").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/skin4pixel");
                break;
            case 5:
                GameObject.Find("Robot").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/skin-5");
                break;

        }
        //HUD
        GameObject.Find("Platform HUD").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/empty HUD");

        //Explosion Particles
        GameObject.Find("Particle Respawn").GetComponent<ParticleSystem>().Play();
        GameObject.Find("Particle Respawn").GetComponent<Rigidbody2D>().position = controller.position;
        GameObject.Find("Particle Muerte").GetComponent<ParticleSystem>().Stop();
        GameObject.Find("Particle Muerte").GetComponent<Rigidbody2D>().position = controller.position;
        GameObject.Find("Particle Entrada Portal").GetComponent<ParticleSystem>().Stop();
        GameObject.Find("Particle Entrada Portal").GetComponent<Rigidbody2D>().position = controller.position;
        GameObject.Find("Particle Salida Portal").GetComponent<ParticleSystem>().Stop();
        GameObject.Find("Particle Salida Portal").GetComponent<Rigidbody2D>().position = controller.position;


        //SaveFile
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (PlayerPrefs.GetInt("Levels Unlocked") < currentSceneIndex)
            PlayerPrefs.SetInt("Levels Unlocked", currentSceneIndex - 12);
        Debug.Log(PlayerPrefs.GetInt("Levels Unlocked"));
    }


    void Update()
    {
        //Update vector
        positionL.x = controller.position.x - 0.3f;
        positionL.y = controller.position.y;
        positionR.x = controller.position.x + 0.3f;
        positionR.y = controller.position.y;
        if (Input.GetAxis("Horizontal") == 0)
            direction = Input.GetAxis("Horizontal J");
        else
            direction = Input.GetAxis("Horizontal");
        if (Input.GetAxis("Vertical") == 1)
            jump = true;
        else if (Input.GetAxis("Vertical J") == 1)
            jump = true;
        else
            jump = false;
        if (direction < 0.2f || direction > 0.8f)
            direction = (int)direction;
            
        mainMenu();
        nextLevel();
        switchPlat();
        showHUD();
        if (resSound)
        {
            SoundManagerScript.PlaySound("Exit");
            resSound = false;
        }

        if (portal() != 0)
        {
            SoundManagerScript.PlaySound("Teleport");
            GameObject.Find("Particle Entrada Portal").GetComponent<ParticleSystem>().Play();
            GameObject.Find("Particle Entrada Portal").GetComponent<Rigidbody2D>().position = controller.position;
            portalPos = GameObject.Find("morado (1)").transform.position;
            portalAngle = GameObject.Find("morado (1)").transform.eulerAngles;
            portalRef = Time.realtimeSinceStartup;
            portalHUDRef = Time.realtimeSinceStartup;
            switch ((int)portalAngle.z)
            {
                case 90:
                    portalPos.y += 0.8f;
                    controller.position = portalPos;
                    GameObject.Find("Particle Salida Portal").GetComponent<ParticleSystem>().Play();
                    GameObject.Find("Particle Salida Portal").GetComponent<Rigidbody2D>().position = controller.position;
                    switch (portal())
                    {
                        case 1:
                            controller.velocity = new Vector2(controller.velocity.x, controller.velocity.y * -1);
                            break;
                        case 2:
                            controller.velocity = new Vector2(controller.velocity.x, controller.velocity.y);
                            break;
                        case 3:
                            controller.velocity = new Vector2(0, controller.velocity.x*1.5f);
                            break;
                        case 4:
                            controller.velocity = new Vector2(0, (controller.velocity.x * 1.5f) * -1);
                            break;
                        default:
                            break;
                    }
                    break;
                case 270:
                    portalPos.y -= 0.8f;
                    controller.position = portalPos;
                    GameObject.Find("Particle Salida Portal").GetComponent<ParticleSystem>().Play();
                    GameObject.Find("Particle Salida Portal").GetComponent<Rigidbody2D>().position = controller.position;
                    switch (portal())
                    {
                        case 1:
                            controller.velocity = new Vector2(controller.velocity.x, controller.velocity.y);
                            break;
                        case 2:
                            controller.velocity = new Vector2(controller.velocity.x, controller.velocity.y * -1);
                            break;
                        case 3:
                            controller.velocity = new Vector2(0, (controller.velocity.x * 1.5f) * -1);
                            break;
                        case 4:
                            controller.velocity = new Vector2(0, controller.velocity.x * 1.5f);
                            break;
                        default:
                            break;
                    }
                    break;
                case 0:
                    portalPos.x += 0.75f;
                    controller.position = portalPos;
                    GameObject.Find("Particle Salida Portal").GetComponent<ParticleSystem>().Play();
                    GameObject.Find("Particle Salida Portal").GetComponent<Rigidbody2D>().position = controller.position;
                    switch (portal())
                    {
                        case 1:
                            controller.velocity = new Vector2(controller.velocity.y * -1, 0);
                            break;
                        case 2:
                            controller.velocity = new Vector2(controller.velocity.y, 0);
                            break;
                        case 3:
                            controller.velocity = new Vector2(controller.velocity.x, 0);
                            break;
                        case 4:
                            controller.velocity = new Vector2(controller.velocity.x * -1, 0);
                            break;
                        default:
                            break;
                    }                  
                    break;
                case 180:
                    portalPos.x -= 0.75f;
                    controller.position = portalPos;
                    GameObject.Find("Particle Salida Portal").GetComponent<ParticleSystem>().Play();
                    GameObject.Find("Particle Salida Portal").GetComponent<Rigidbody2D>().position = controller.position;
                    switch (portal())
                    {
                        case 1:
                            controller.velocity = new Vector2(controller.velocity.y, 0);
                            break;
                        case 2:
                            controller.velocity = new Vector2(controller.velocity.y * -1, 0);
                            break;
                        case 3:
                            controller.velocity = new Vector2(controller.velocity.x * -1, 0);
                            break;
                        case 4:
                            controller.velocity = new Vector2(controller.velocity.x, 0);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        if (Grounded() || platWallJump() || doubleJump() || rebote())
        {
            if (Grounded())
            {
                verticalHeight = 7.625f;
                rWallJump = true;
                lWallJump = true;
            }
            if (rebote())
            {
                SoundManagerScript.PlaySound("Bounce");
                verticalHeight = 14;
                controller.velocity = new Vector2(controller.velocity.x / 5, verticalHeight);
                reboteHUDRef = Time.realtimeSinceStartup;
            }
            if (jump)
            {
                SoundManagerScript.PlaySound("Jump");
                controller.velocity = new Vector2(controller.velocity.x, verticalHeight);
            }
            else
            {
                controller.AddForce(new Vector2(direction * 0.1f, 0));
            }
            if (doubleJump())
            {
                extraJump = true;
                doubleJumpRef = Time.realtimeSinceStartup;
                doubleJumpHUDRef = Time.realtimeSinceStartup;
            }
            else
                extraJump = false;
        }
        else if (Grounded() == false)
        {
            controller.AddForce(new Vector2(direction, 0));
            if ((direction == 0 && Grounded() == false) || Time.realtimeSinceStartup <= portalRef + 0.65f)
            {
                if (controller.velocity.x > 1)
                    controller.AddForce(new Vector2(controller.velocity.x, controller.velocity.y));
                else if (controller.velocity.x < -1)
                    controller.AddForce(new Vector2(controller.velocity.x, controller.velocity.y));
            }
            if (extraJump && jump && Time.realtimeSinceStartup >= doubleJumpRef + 0.35f)
            {
                SoundManagerScript.PlaySound("JumpH");
                controller.velocity = new Vector2(controller.velocity.x, verticalHeight);
                extraJump = false;
            }
        }
        if ((controller.velocity.x < 5 && direction == 1) || (controller.velocity.x > -5 && direction == -1))     //aceleración
        {
            controller.AddForce(new Vector2(controller.velocity.x + direction * 15, 0));
        }
        if ((direction == 0 && (Grounded() || doubleJump())) || (controller.velocity.x > 5 || controller.velocity.x < -5))
        {
            if (controller.velocity.x > 1)
                controller.AddForce(new Vector2(controller.velocity.x - 40, controller.velocity.y));
            else if (controller.velocity.x < -1)
                controller.AddForce(new Vector2(controller.velocity.x + 40, controller.velocity.y));
        }
        if (isDead())
        {
            SoundManagerScript.PlaySound("Death");
            GameObject.Find("Particle Muerte").GetComponent<ParticleSystem>().Play();
            GameObject.Find("Particle Muerte").GetComponent<Rigidbody2D>().position = controller.position;
            controller.position = position;
            controller.velocity = resVel;
            GameObject.Find("Particle Respawn").GetComponent<ParticleSystem>().Play();
            GameObject.Find("Particle Respawn").GetComponent<Rigidbody2D>().position = controller.position;
        }
    }

    bool Grounded()
    {
        if (Physics2D.Raycast(positionL, Vector2.down, distSuelo, groundLayer) || Physics2D.Raycast(positionR, Vector2.down, distSuelo, groundLayer))
            return true;
        return false;
    }

    bool isDead()
    {
        if (Physics2D.Raycast(positionL, Vector2.down, distSuelo, deathLayer) ||
            Physics2D.Raycast(positionR, Vector2.down, distSuelo, deathLayer) ||
            Physics2D.Raycast(positionL, Vector2.up, distSuelo, deathLayer) ||
            (Physics2D.Raycast(positionR, Vector2.up, distSuelo, deathLayer)) ||
            Physics2D.Raycast(transform.position, Vector2.right, distPared, deathLayer) ||
            Physics2D.Raycast(transform.position, Vector2.left, distPared, deathLayer) || transform.position.y < -10)
        {
            if (GameObject.Find("Textura Meta Desactivada") != null)
            {
                GameObject.Find("Meta").layer = 8;
                GameObject.Find("Textura Meta Desactivada").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/Textura Meta Desactivada");
                GameObject.Find("Particle Meta 1").GetComponent<ParticleSystem>().Stop();
                GameObject.Find("Particle Meta 2").GetComponent<ParticleSystem>().Stop();
                GameObject.Find("Particle Meta 3").GetComponent<ParticleSystem>().Stop();
                GameObject.Find("Particle Key 1").GetComponent<ParticleSystem>().Play();
                GameObject.Find("Particle Key 2").GetComponent<ParticleSystem>().Play();
                GameObject.Find("Particle Key 3").GetComponent<ParticleSystem>().Play();
                GameObject.Find("Particle Key 4").GetComponent<ParticleSystem>().Play();
            }
            return true;
        }
        return false;
    }

    bool platWallJump()
    {
        if (Physics2D.Raycast(transform.position, Vector2.right, distPared, boteParedLayer) && rWallJump && jump)
        {
            SoundManagerScript.PlaySound("Wall");
            controller.velocity = new Vector2(0, 5.75f);
            rWallJump = false;
            lWallJump = true;
            wallJumpHUDRef = Time.realtimeSinceStartup;
            return true;
        }
        else if (Physics2D.Raycast(transform.position, Vector2.left, distPared, boteParedLayer) && lWallJump && jump)
        {
            SoundManagerScript.PlaySound("Wall");
            controller.velocity = new Vector2(0, 5.75f);
            lWallJump = false;
            rWallJump = true;
            wallJumpHUDRef = Time.realtimeSinceStartup;
            return true;
        }
        return false;
    }

    bool doubleJump()
    {
        if (Physics2D.Raycast(positionL, Vector2.down, distSuelo, dobleSalto) || Physics2D.Raycast(positionR, Vector2.down, distSuelo, dobleSalto))
            return true;
        return false;
    }
    bool rebote()
    {
        if (Physics2D.Raycast(positionL, Vector2.down, distSuelo, reboteLayer) || Physics2D.Raycast(positionR, Vector2.down, distSuelo, reboteLayer))
            return true;
        return false;
    }

    void nextLevel()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, distSuelo, nextLevelLayer))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            {
                SceneManager.LoadScene(currentSceneIndex + 1);
                resSoundDelay = Time.realtimeSinceStartup;
            }
        }
    }

    int portal()
    {
        if (Physics2D.Raycast(positionL, Vector2.down, distSuelo, portalLayer) || (Physics2D.Raycast(positionR, Vector2.down, distSuelo, portalLayer)))
            return 1;
        else if (Physics2D.Raycast(positionL, Vector2.up, distSuelo, portalLayer) || (Physics2D.Raycast(positionR, Vector2.up, distSuelo, portalLayer)))
            return 2;
        else if (Physics2D.Raycast(transform.position, Vector2.right, distPared, portalLayer))
            return 3;
        else if (Physics2D.Raycast(transform.position, Vector2.left, distPared, portalLayer))
            return 4;
        return 0;
    }

    void switchPlat()
    {
        if ((Physics2D.Raycast(positionL, Vector2.down, distSuelo, switchLayer) ||
            Physics2D.Raycast(positionR, Vector2.down, distSuelo, switchLayer) ||
            Physics2D.Raycast(positionL, Vector2.up, distSuelo, switchLayer) ||
            (Physics2D.Raycast(positionR, Vector2.up, distSuelo, switchLayer)) ||
            Physics2D.Raycast(transform.position, Vector2.right, distPared, switchLayer) ||
            Physics2D.Raycast(transform.position, Vector2.left, distPared, switchLayer)) &&
            GameObject.Find("Meta").layer == 8)
        {
            SoundManagerScript.PlaySound("Key");
            GameObject.Find("Meta").layer = 13;
            GameObject.Find("Textura Meta Desactivada").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/Textura Meta");
            GameObject.Find("Particle Meta 1").GetComponent<ParticleSystem>().Play();
            GameObject.Find("Particle Meta 2").GetComponent<ParticleSystem>().Play();
            GameObject.Find("Particle Meta 3").GetComponent<ParticleSystem>().Play();
            GameObject.Find("Particle Key 1").GetComponent<ParticleSystem>().Stop();
            GameObject.Find("Particle Key 2").GetComponent<ParticleSystem>().Stop();
            GameObject.Find("Particle Key 3").GetComponent<ParticleSystem>().Stop();
            GameObject.Find("Particle Key 4").GetComponent<ParticleSystem>().Stop();
        }
    }

    void showHUD()
    {
        if(Time.realtimeSinceStartup <= wallJumpHUDRef + 0.5f)
            GameObject.Find("Platform HUD").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/Salto Pared HUD");
        else if (Time.realtimeSinceStartup <= doubleJumpHUDRef + 0.5f)
            GameObject.Find("Platform HUD").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/Doble Salto HUD");
        else if (Time.realtimeSinceStartup <= reboteHUDRef + 0.5f)
            GameObject.Find("Platform HUD").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/Rebote HUD");
        else if (Time.realtimeSinceStartup <= portalHUDRef + 0.5f)
            GameObject.Find("Platform HUD").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/Portal HUD");
        else
            GameObject.Find("Platform HUD").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/empty HUD");

    }


    void mainMenu()
    {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene("Menu_1");
        else if (Input.GetKey(KeyCode.Joystick1Button7))
            SceneManager.LoadScene("Menu_1");
    }
}

