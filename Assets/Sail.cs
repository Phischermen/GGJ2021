using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sail : MonoBehaviour
{
    BoatSteering boat;
    public bool open = true;
    public bool atSail = false;
    public Vector2 rotationRange = new Vector2(-180, 0);
    public float turnSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        boat = transform.parent.GetComponent<BoatSteering>();
        boat.sail = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (atSail)
        {
            float raiselower = Input.GetAxis("Vertical");
            if (Mathf.Abs(raiselower) > .1)
            {
                bool opening = raiselower > 0 ? true : false;
                OpenClose(opening);
            }
            float rotation = Input.GetAxis("Horizontal");
            if (Mathf.Abs(rotation) > .1)
            {
                transform.Rotate(Vector3.forward * rotation * turnSpeed * Time.deltaTime);
                SailClamp();
            }
        }
    }

    public void OpenClose()
    {
        open = !open;
    }

    public void OpenClose(bool opening)
    {
        open = opening;
    }

    void SailClamp()
    {
        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, Mathf.Clamp(transform.rotation.z, rotationRange.x, rotationRange.y), transform.rotation.w);
    }
}
