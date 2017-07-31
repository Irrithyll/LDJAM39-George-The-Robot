using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public float maxPower = 100f;
    public float power = 100f;
    public int jumpPower = 20;
    public float movePower = 0.001f;
    private string action = "";
    public bool isGrounded = false;
    public float batteryPower = 20f;
    private float jumpHeight = 12000f;
    private bool hasKey = false;
    public bool colliding = false;
    public bool dead = false;

    public Texture2D fadeImage;

    public GameObject door;

    AnimationRenderer anim; // controls animations
    AnimationRenderer childAnim; // controls animations

    AudioSource audio;
    public AudioClip soundPickup;
    public AudioClip soundJump;
    public AudioClip soundPowerDown;
    public AudioClip soundBlip;
    public AudioClip soundActivatePortal;
    public AudioClip soundEnterPortal;

    public GameObject warning;
    public GameObject batteryText;

    private static Texture2D _staticRectTexture;
    private static GUIStyle _staticRectStyle;

    private float alpha = 0;
    private float fadeDir = -1;
    private bool fading = false;
    private bool fadeIn = true;
    private bool nextLevel = false;

    private int visiblePower;


    public GameObject powerText;

        void Awake()
    {
        audio = GetComponent<AudioSource>();
        anim = GetComponent<AnimationRenderer>();
        childAnim = GetComponentInChildren<EffectRenderer>();
        fadeIn = true;
        fading = true;
        alpha = 1;
        fadeDir = 1;

        Screen.SetResolution(1024, 768, false);

    }

    void Start () {
        anim.PlayAnimation(Animation.Idle);
        childAnim.PlayAnimation(null);
        warning = GameObject.FindGameObjectWithTag("TextWarning");
        batteryText = GameObject.FindGameObjectWithTag("BatteryPercentage");

        if (SceneManager.GetActiveScene().name == "404")
        {
            // game has ended, don't do some other stuff
            return;
        }
        for(int i = 0; i < GameStats.lives; i++)
        {
            Instantiate(Resources.Load("Prefabs/lifeIcon"), new Vector2(204 + (i*16) + 4, -169), new Quaternion(0, 0, 0, 0));
        }
    }

    void Update () {

        // game has ended, give player infinite power
        if (SceneManager.GetActiveScene().name == "404")
        {
            power = maxPower;
        }

        // if we are dead, spacebar let's us respawn
        if (dead == true) {
            if (Input.GetKeyDown(KeyCode.Space)){
                reboot();
            }
            anim.PlayAnimation(Animation.Dead);
        }

        // if we have no power, KILL THE PLAYER!
        if (dead == false && power <= 0) {
            killPlayer();
        }

        // if we are fading or dead or have no power, don't allow movement!!
        if (fading == true || dead == true || power <= 0) {
            stopMovement();
            return;
        }


        /* 
         * BASIC MOVEMENT INPUT! 
         */
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            /* MOVE LEFT */
            action = "moveLeft";
            anim.PlayAnimation(Animation.WalkLeft);
        }
        else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            /* MOVE RIGHT */
            action = "moveRight";
            anim.PlayAnimation(Animation.WalkRight);
        }
        else if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            /* FLY */
            action = "fly";
        }else
        {
            /* IDLE */
            action = "";
            anim.PlayAnimation(Animation.Idle);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            /* JUMP */
            jump();
        }

    }

    private void stopMovement()
    {
        action = "";
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        colliding = true;
        Debug.Log("collision!");
        if(other.gameObject.name == "Platform")
        {
            Debug.Log("Platform!");
            childAnim.PlayAnimation(Animation.JumpPoof, false, 0.1f);
            isGrounded = true;
        }

        if (dead == true) return;

        if (other.gameObject.name == "Spikes")
        {
            killPlayer();
        }
    }

    private void killPlayer()
    {
        GameStats.deaths++;
        GameStats.lives--;
        dead = true;
        action = "";
        audio.PlayOneShot(soundPowerDown);
        anim.PlayAnimation(Animation.Dead);
        childAnim.PlayAnimation(Animation.JumpPoof, false, 0.1f);
        batteryText.GetComponent<MeshRenderer>().enabled = false;
        warning.GetComponentInChildren<TextMeshPro>().SetText("ERROR: POWER FAILURE\nPRESS SPACE TO REBOOT");
    }

    private void reboot()
    {
        if (GameStats.lives < 0)
        {
            // game over~
            SceneManager.LoadScene("BSOD");
        }
        else
        {
            // reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void fadeOutLevel()
    {
        fading = true;
        alpha = 0;
        fadeDir = -1;
        nextLevel = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (dead == true) return;
        Debug.Log("trigger!");
        if(other.gameObject.name == "Battery")
        {
            pickUpBattery();
            Instantiate(Resources.Load("Prefabs/PlayAnimationObj"), other.gameObject.transform.position, new Quaternion(0, 0, 0, 0));
            Destroy(other.gameObject);
        }

        if(other.gameObject.name == "Key")
        {
            audio.PlayOneShot(soundActivatePortal);
            Instantiate(Resources.Load("Prefabs/PlayAnimationObj"), other.gameObject.transform.position, new Quaternion(0, 0, 0, 0));
            hasKey = true;
            door.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/doorSprite2");
            Destroy(other.gameObject);
        }

        if(other.gameObject.name == "Goal")
        {
            enterExit();
        }
    }

    void enterExit()
    {
        if (dead == true) return;
        Debug.Log("entered exit");
        if(hasKey == true)
        {
            Debug.Log("enter portal");
            audio.PlayOneShot(soundEnterPortal);
            // do end level stuff
            fadeOutLevel();
        }
        else
        {
            // do nothing
        }
    }

    void pickUpBattery()
    {
        // play a sound effect
        audio.PlayOneShot(soundPickup);
        Debug.Log("battery!");

        // add some power!
        power += batteryPower;
        if (power > maxPower) power = maxPower;
        GameStats.batteriesCollected++;
    }

    void FixedUpdate()
    {
        if(colliding == true)
        {
            colliding = false;
            return;
        }

        if(action == "jump")
        {
            jump();
        }else if(action == "moveRight")
        {
            moveDir("right");
        }else if(action == "moveLeft")
        {
            moveDir("left");
        }else if(action == "fly")
        {
            fly();
        }else
        {
        }
    }

    private void jump()
    {
        GameStats.jumps++;
        if (isGrounded == false) return;
        audio.PlayOneShot(soundJump);
        power -= jumpPower;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(0, jumpHeight));
        isGrounded = false;
    }

    private void fly()
    {
        Debug.Log("fly");
    }

    private void moveDir(string dir) {
        if (dir == "left")
        {
            transform.position = new Vector2(transform.position.x - 2f, transform.position.y);

        }else if(dir == "right")
        {
            transform.position = new Vector2(transform.position.x + 2f, transform.position.y);
        }

        move();
    }

    private void move()
    {
        power -= movePower;
    }

    void OnGUI()
    {
        drawEnergyBar();

        if(fading == true)
        {
            // idle while we fade in or out~
            anim.PlayAnimation(Animation.Idle);
            float fadeSpeed = 0.9f;
            int drawDepth = -100;

            alpha -= fadeDir * fadeSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);
            Color thisAlpha = GUI.color;
            thisAlpha.a = alpha;
            GUI.color = thisAlpha;
            GUI.depth = drawDepth;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeImage);
            if (alpha >= 1)
            {
                if (nextLevel == true)
                {
                    GameStats.levelsCompleted++;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            } else if (fadeIn == true && alpha <= 0)
            {
                fading = false;
            }

        }


    }

    private void drawEnergyBar()
    {

        if (SceneManager.GetActiveScene().name == "404")
        {
            // game has ended, don't do some other stuff
            return;
        }

        Color32 color;
        int powerPercentage = (int) ((power / maxPower) * 100);
        int powerRemaining = (int) power / 5;
        batteryText.GetComponent<TextMeshPro>().SetText(powerPercentage.ToString() + "%");
        if (powerRemaining >= 5)
        {
            // green
            color = new Color32(109, 170, 44, 255);
        }else if(powerRemaining < 10 && powerRemaining >= 5)
        {
            // orange
            color = new Color32(210, 125, 44, 255);
        }
        else
        {
            // red
            color = new Color32(208, 70, 72, 255);
        }
        if (powerRemaining <= 2)
        {
            // show warning
            warning.GetComponent<MeshRenderer>().enabled = true;
        }

        for (int i = 0; i < powerRemaining; i++)
        {
            GUIDrawRect(new Rect(90 + (i * 3), 713, 3, 15), color);
        }
        

    }

    public static void GUIDrawRect(Rect position, Color32 color)
    {
        if (_staticRectTexture == null) _staticRectTexture = new Texture2D(1, 1);

        if (_staticRectStyle == null) _staticRectStyle = new GUIStyle();

        _staticRectTexture.SetPixel(0, 0, color);
        _staticRectTexture.Apply();

        _staticRectStyle.normal.background = _staticRectTexture;

        GUI.Box(position, GUIContent.none, _staticRectStyle);
    }


}
