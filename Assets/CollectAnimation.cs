using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectAnimation : MonoBehaviour {
    AnimationRenderer anim;

    void Start () {
        anim = GetComponent<AnimationRenderer>();
        anim.PlayAnimation(Animation.Collect, false, 0.1f, true);
    }
    
}
