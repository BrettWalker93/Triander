using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*to do
* --fix camera resetting
* --rotate player with camera
* tidy up
* allow zooming
*       make offset (distance) public
*       have scroll wheel control distance
* terrain repositioning
*       cast ray from camera to player
*       if ray hits something other than player, change offset (distance) to prevent this
*       unclamp rotation in RotateUpdate to allow looking up
*/
public class CameraController : MonoBehaviour
{
    Rigidbody targetRigidBody;
    public Transform target;
    public Transform camTransform;
    public float sensitivity;    

    private float tHoldx = 0;
    private float tHoldy = 0;
    private float playerRot = 0;

    bool rocheck = false;

    private float distance = 10f;

    //rotation
    private Quaternion rot;

    //holds mouse position
    private Vector3 mouseS = Vector3.zero;

    void Start()
    {
        targetRigidBody = target.GetComponent<Rigidbody>();
    }

    void Update()
    {
        //get input
        if (Input.GetMouseButton(1) && !rocheck)
        { 
            rocheck = true;
            //starting mouse position
            mouseS = Input.mousePosition;
            //starting transform
            tHoldx = transform.rotation.eulerAngles.x;
            tHoldy = transform.rotation.eulerAngles.y;
        }
    }

    void LateUpdate()
    {
        RotateUpdate();
    }

    //calculate rotation according to input
    void RotateUpdate()
    {
        //calculates new rotation
        if (!Input.GetMouseButton(1))
            rocheck = false;
        else if (rocheck)
        {
            float n0 = (mouseS - Input.mousePosition).y*sensitivity/10 + tHoldx;
            float n1 = (Input.mousePosition - mouseS).x*sensitivity/10 + tHoldy;
            //sets rotation based on mouse movement after right clicking
            rot = Quaternion.Euler(Mathf.Clamp(n0,5,85),n1, 0);
            playerRot = n1;
        }

        //applies rotations
        transform.position = targetRigidBody.position + rot * new Vector3(0, 0, -distance);
        transform.LookAt(targetRigidBody.position);
        targetRigidBody.transform.rotation = Quaternion.Euler(targetRigidBody.transform.rotation.x, playerRot, targetRigidBody.transform.rotation.z);
    }
}

    