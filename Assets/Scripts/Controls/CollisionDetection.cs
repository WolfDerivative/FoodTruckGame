using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based of:
//https://github.com/SebLague/2DPlatformer-Tutorial/blob/master/

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class CollisionDetection : MonoBehaviour {

    const float SKINWIDTH = 0.015f;

    public Vector2 NumberOfRays = new Vector2(5, 7);
    [Tooltip("velocity * RayLength - is a Length of the raycast vector.")]
    public Vector2 RayLengthIndex = new Vector2(0.2f, 1f);
    [Range(0, 90)] public float ClimbingSlope = 45;
    public LayerMask CollisionMask;

    public bool above, below, left, right;
    public bool isOnSlope;


    protected int horizontalRayCount    { get { return Mathf.FloorToInt(NumberOfRays.x); }}
    protected int verticalRayCount      { get { return Mathf.FloorToInt(NumberOfRays.y); }}
    protected Vector2 raySpacing {
        get {
            Bounds bounds = _boxCollider.bounds;
            bounds.Expand(SKINWIDTH * -2);
            Vector2 spaces = Vector2.zero;
            spaces.x = bounds.size.y / (horizontalRayCount - 1);
            spaces.y = bounds.size.x / (verticalRayCount - 1);
            return spaces;
        }//get
    }//raySpacing


    private BoxCollider2D   _boxCollider;
    private SRaycastOrigins rayOrigins;
    private float slopeAngleLastFrame, currentSlopeAngle;

    /* ****************************************************************************************** */

    struct SRaycastOrigins {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }//struct

    /* ****************************************************************************************** */


    public void Start() {
        _boxCollider = GetComponent<BoxCollider2D>();
    }//Start


    public void Move(ref Vector2 deltaMovement) {
        Reset();
        updateRaycastOrigin();
        if(deltaMovement.x != 0)
            HorizontalCollisions(ref deltaMovement);
        if(deltaMovement.y != 0)
            VerticalCollisions(ref deltaMovement);
    }//Update


    void HorizontalCollisions(ref Vector2 velocity) {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) * RayLengthIndex.x + SKINWIDTH;

        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? rayOrigins.bottomLeft : rayOrigins.bottomRight;
            rayOrigin += Vector2.up * (raySpacing.x * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

            if(i < horizontalRayCount / 2)
                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
            else
                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.green);

            if (!hit)
                continue;

            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (i == 0 && slopeAngle <= ClimbingSlope) {
                float distanceToSlopeStart = 0;
                if (slopeAngle != slopeAngleLastFrame) {
                    distanceToSlopeStart = hit.distance - SKINWIDTH;
                    velocity.x -= distanceToSlopeStart * directionX;
                }
                ClimbSlope(ref velocity, slopeAngle);
                velocity.x += distanceToSlopeStart * directionX;
            }//if

            if (!isOnSlope || slopeAngle > ClimbingSlope) {
                velocity.x = (hit.distance - SKINWIDTH) * directionX;
                rayLength = hit.distance;

                if (isOnSlope)
                    velocity.y = Mathf.Tan(currentSlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);

                left = directionX == -1;
                right = directionX == 1;
            }//if
        }//for
    }//HorizontalCollisions


    void VerticalCollisions(ref Vector2 velocity) {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) * RayLengthIndex.y + SKINWIDTH;

        for (int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1) ? rayOrigins.bottomLeft : rayOrigins.topLeft;

            rayOrigin += Vector2.right * (raySpacing.y * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit) {
                velocity.y = (hit.distance - SKINWIDTH) * directionY;
                rayLength = hit.distance;

                below = directionY == -1;
                above = directionY == 1;
            }//if
        }//for
    }//VerticalCollisions


    void ClimbSlope(ref Vector2 velocity, float slopeAngle) {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y > climbVelocityY)
            return;
        velocity.y = climbVelocityY;
        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
        below = true;
        isOnSlope = true;
        currentSlopeAngle = slopeAngle;
    }//ClimbSlope
    

    protected void updateRaycastOrigin() {
        Bounds bounds = _boxCollider.bounds;
        bounds.Expand(SKINWIDTH * -2);

        rayOrigins.topLeft      = new Vector2(bounds.min.x, bounds.max.y);
        rayOrigins.topRight     = new Vector2(bounds.max.x, bounds.max.y);
        rayOrigins.bottomLeft   = new Vector2(bounds.min.x, bounds.min.y);
        rayOrigins.bottomRight  = new Vector2(bounds.max.x, bounds.min.y);
    }//UpdateRaycastOrigin


    public void Reset() {
        above = below = left = right = false;
        isOnSlope = false;
        slopeAngleLastFrame = currentSlopeAngle;
        currentSlopeAngle = 0;
    }//Reset

}//class
