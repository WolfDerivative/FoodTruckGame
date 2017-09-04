using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(CollisionDetection) )]
public class PlatformerControls2d : MonoBehaviour {

    public float Acceleration = 30f;
    public float MaxSpeed = 10f;
    public Vector2 velocity;
    public float Gravity = 9.8f;

    protected Rigidbody2D           _rigidBody;
    protected BoxCollider2D         _boxCollider;
    protected CollisionDetection    _collisionDetection;

    private bool bIsSprinting;

    /* ************************************************************* */


	void Start () {
        _rigidBody          = GetComponent<Rigidbody2D>();
        _boxCollider        = GetComponent<BoxCollider2D>();
        _collisionDetection = GetComponent<CollisionDetection>();
	}//Start
	
	
	void Update () {
        Vector2 deltaMovement = velocity;
        deltaMovement.y -= Gravity * Time.deltaTime;

        deltaMovement.x = Input.GetAxis("Horizontal") * Time.deltaTime * Acceleration;

        _collisionDetection.Move(ref deltaMovement);
        transform.Translate(deltaMovement);

        velocity = deltaMovement;
	}//Update

}//class
