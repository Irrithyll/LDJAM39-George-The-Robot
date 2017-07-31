using UnityEngine;
using System.Collections;

public class Animation
{
    public static Animation Idle = new Animation(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 });
    public static Animation WalkRight = new Animation(new int[] { 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34 });
    public static Animation WalkLeft = new Animation(new int[] { 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50});

    public static Animation PowerUp = new Animation(new int[] { 0, 1, 2, 3, 4, 5, 6, 16 });
    public static Animation JumpPoof = new Animation(new int[] { 7, 8, 9, 11, 16 });
    public static Animation Collect = new Animation(new int[] { 12, 13, 14, 15, 16 });

    public static Animation None = new Animation(new int[] { });
    public static Animation Dead = new Animation(new int[] { 51});

    public int[] frames;
    public Animation(int[] frames)
    {
        this.frames = frames;
    }
}

public class AnimationRenderer : MonoBehaviour
{
    public Sprite[] spriteFrames;
    public Animation currAnim = Animation.Idle;
    public SpriteRenderer spriteRenderer;
    public float frameElapsed;
    public int frameIndex;
    private float frameSpeed = 0.1f;
    private bool loop = true;
    private bool destroyAfter = false;


    public void PlayAnimation(Animation anim)
    {
        currAnim = anim;
    }

    public void PlayAnimation(Animation anim, bool loop)
    {
        this.loop = loop;
        if(currAnim != anim) frameIndex = 0;
        currAnim = anim;
    }

    public void PlayAnimation(Animation anim, bool loop, float frameSpeed)
    {
        this.loop = loop;
        this.frameSpeed = frameSpeed;
        if (currAnim != anim) frameIndex = 0;
        currAnim = anim;
    }

    public void PlayAnimation(Animation anim, bool loop, float frameSpeed, bool destroyAfter)
    {
        this.loop = loop;
        this.frameSpeed = frameSpeed;
        this.destroyAfter = destroyAfter;
        if (currAnim != anim) frameIndex = 0;
        currAnim = anim;
    }

    public void StopAnimation()
    {
        currAnim = Animation.Idle;
    }

    void Start()
    {
        spriteFrames = Resources.LoadAll<Sprite>("Sprites/Player/player_spritesheet");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        if (currAnim == null) return;
        // check if enough time has elapsed before
        // we move onto the next frame of the sprite
        frameElapsed += Time.deltaTime;
        if (frameElapsed > frameSpeed)
        {
            frameIndex += 1;
            frameElapsed = 0;
        }

        if (frameIndex >= currAnim.frames.Length)
        {
            // check if we should loop
            if (loop == false)
            {
                if(destroyAfter == true)
                {
                    Destroy(this.gameObject);
                }
                currAnim = Animation.None;
                return;
            }
            frameIndex = 0;
        }

        // set the current frame of the sprite!
        spriteRenderer.sprite = spriteFrames[currAnim.frames[frameIndex]];
    }
}
