using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionDetection))]
public class JumpControls : MonoBehaviour {

    public float JumpForce = 10f;
    public int DoubleJumps = 2;

    protected CollisionDetection _collisionDetection;

    private int currJumpCount;

    /* ************************************************************** */

    public void Start() {
        _collisionDetection = GetComponent<CollisionDetection>();
        currJumpCount = 0;
    }//Start


    public void Update() {
        if (_collisionDetection.below)
            Reset();
    }


    public void OnJump(ref Vector2 deltaMovement) {
        if (!Input.GetButtonDown("Jump"))
            return;
        if (!_collisionDetection.below && (currJumpCount >= DoubleJumps ))
            return;
        deltaMovement.y = JumpForce;
        currJumpCount++;
    }//OnJump


    public void Reset() {
        currJumpCount = 0;
    }//Reset

}//class
