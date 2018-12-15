using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*to do
* --fix camera resetting
* --rotate player with camera
* tidy up
*/
public class sc_cameraController : MonoBehaviour
{
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
    private Vector3 mouseS = new Vector3(0,0,0);

    private void Start()
    {
        //nothing doing
    }

    private void Update()
    {
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

    private void LateUpdate()
    {
        rotateUpdate();
        //applies rotations
        transform.position = target.position + rot * new Vector3(0,0,-distance);
        transform.LookAt(target.position);
        target.transform.rotation = Quaternion.Euler(target.transform.rotation.x,playerRot,target.transform.rotation.z);
    }

    private void rotateUpdate()
    {
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
    }
}

    