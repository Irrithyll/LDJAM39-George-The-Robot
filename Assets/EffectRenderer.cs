using UnityEngine;
using System.Collections;

public class EffectRenderer : AnimationRenderer
{

    void Start()
    {
        spriteFrames = Resources.LoadAll<Sprite>("Sprites/effectsAnimation");
        spriteRenderer = GetComponent<SpriteRenderer>();
        currAnim = null;
    }

}
