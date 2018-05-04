using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class movimiento : MonoBehaviour
{

    //Use this for initialization
    private float direction; //X component
    private Rigidbody2D controller;
    private float distSuelo;
    private float verticalHeight;
    private float distPared;
    private bool extraJump;
    private double doubleJumpRef;
    private bool rWallJump;
    private bool lWallJump;
    //Layers
    private int groundLayer;
    private int boteParedLayer;
    private int dobleSalto;
    private int reboteLayer;
    private int deathLayer;
    private int nextLevelLayer;
    //Respawn
    private float xRes;
    private float yRes;
    private Vector2 position;
    private Vector2 positionL;
    private Vector2 positionR;

    void Start()
    {
        verticalHeight = 5.75f;
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
        rWallJump = true;
        lWallJump = false;
        //Respawn coordinates
        xRes = -9.78f;
        yRes = -3.17f;
        position.x = xRes;
        position.y = yRes;
        controller.position = position;
    }


    void Update()
    {
        //Update vector
        positionL.x = controller.position.x - 0.3f;
        positionL.y = controller.position.y;
        positionR.x = controller.position.x + 0.3f;
        positionR.y = controller.position.y;

        mainMenu();
        nextLevel();
        if (isDead())
        {
            controller.position = position;
        }
        if (Grounded() || platWallJump() || doubleJump() || rebote())
        {
            if(platWallJump())
            {

            }
            if (Grounded())
            {
                verticalHeight = 5.75f;
                rWallJump = true;
                lWallJump = true;
            }
            if (rebote())
            {
                verticalHeight = 10;
                controller.velocity = new Vector2(controller.velocity.x / 5, verticalHeight);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                controller.velocity = new Vector2(controller.velocity.x, verticalHeight);
            }
            else
            {
                direction = Input.GetAxis("Horizontal");
                controller.AddForce(new Vector2(direction * 0.1f, 0));
            }
            if (doubleJump())
            {
                extraJump = true;
                doubleJumpRef = Time.realtimeSinceStartup;
            }
            else
                extraJump = false;
        }
        else if (Grounded() == false)
        {
            direction = Input.GetAxis("Horizontal");
            controller.AddForce(new Vector2(direction * 0.01f, 0));
            if (direction == 0 && Grounded() == false)
            {
                if (controller.velocity.x > 1)
                    controller.AddForce(new Vector2(controller.velocity.x - 5, controller.velocity.y));
                else if (controller.velocity.x < -1)
                    controller.AddForce(new Vector2(controller.velocity.x + 5, controller.velocity.y));
            }
            if (extraJump && Input.GetKey(KeyCode.Space) && Time.realtimeSinceStartup >= doubleJumpRef + 0.35f)
            {
                controller.velocity = new Vector2(controller.velocity.x, verticalHeight);
                extraJump = false;
            }
        }
        if ((controller.velocity.x < 5 && direction == 1) || (controller.velocity.x > -5 && direction == -1))     //aceleración
        {
            controller.AddForce(new Vector2(controller.velocity.x + direction * 10, 0));
        }
        if ((direction == 0 && (Grounded() || doubleJump())) || (controller.velocity.x > 5 || controller.velocity.x < -5))
        {
            if (controller.velocity.x > 1)
                controller.AddForce(new Vector2(controller.velocity.x - 40, controller.velocity.y));
            else if (controller.velocity.x < -1)
                controller.AddForce(new Vector2(controller.velocity.x + 40, controller.velocity.y));
        }
        Debug.Log(Grounded());
    }

    bool Grounded()
    {
        if (Physics2D.Raycast(positionL, Vector2.down, distSuelo, groundLayer) || Physics2D.Raycast(positionR, Vector2.down, distSuelo, groundLayer))
            return true;
        return false;
    }

    bool isDead()
    {
        if (Physics2D.Raycast(positionL, Vector2.down, distSuelo, deathLayer) || Physics2D.Raycast(positionR, Vector2.down, distSuelo, deathLayer) || positionR.y < -10)
            return true;
        return false;
    }

    bool platWallJump()
    {
        if (Physics2D.Raycast(transform.position, Vector2.right, distPared, boteParedLayer) && rWallJump && Input.GetKey(KeyCode.Space))
        {
            controller.velocity = new Vector2(0, 5.75f);
            rWallJump = false;
            lWallJump = true;
            return true;
        }
        else if (Physics2D.Raycast(transform.position, Vector2.left, distPared, boteParedLayer) && lWallJump && Input.GetKey(KeyCode.Space))
        {
            controller.velocity = new Vector2(0, 5.75f);
            lWallJump = false;
            rWallJump = true;
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
            }
        }
    }
    void mainMenu()
    {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene("Menu_1");
    }
}

