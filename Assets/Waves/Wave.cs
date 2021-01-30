using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    new public Rigidbody2D rigidbody2D;
    public Vector3 velocity;
    public AnimationCurve waveCurve;
    //enum WaveState
    //{
    //    rising,
    //    falling
    //}
    //WaveState state;
    public float waveProgress;
    public float waveDuration = 2f;
    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        waveProgress += Time.deltaTime;
        transform.localScale = Vector3.one * waveCurve.Evaluate(waveProgress / waveDuration);
        rigidbody2D.MovePosition(transform.position + velocity * Time.deltaTime);
        if (waveProgress >= waveDuration)
        {
            Debug.Log("Destroyed");
            Destroy(gameObject);
        }
        //switch (state)
        //{
        //    case WaveState.rising:

        //}

    }
    private void OnCollisionEnter(Collision collision)
    {
        /*
        var boat = collision.gameObject.GetComponent<Boat>();
        if (boat != null)
        {
            if boat.velocity.Dot(velocity) > 0f{
                boat.velocity += boost * boat.velocity.Normalized();
            }
            else
            {
                boat.takeDamage();
            }
        }
        */
    }
}
