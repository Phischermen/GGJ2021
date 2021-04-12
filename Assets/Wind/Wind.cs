using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float maxWind = 10;
    public float windForce;
    public float targetForce;
    public Vector2 windDirection;
    public Vector2 targetDirection;
    public Vector2 windChangeRange = new Vector2(15, 20);
    public Vector2 windVector;
    public BoatSteering boat;
    // Start is called before the first frame update
    void Start()
    {
        boat = boat == null ? GameObject.FindWithTag("Boat").GetComponent<BoatSteering>() : boat;
        boat.wind = this;
        var bdm = boat.GetComponent<BoatDamageManager>();
        bdm.sprites.Add(GetComponentInChildren<SpriteRenderer>());
        bdm.flag = gameObject;
        ChangeWind();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = boat.transform.GetChild(0).transform.position;
        windDirection = Vector3.Lerp(windDirection, targetDirection, .9f * Time.deltaTime);
        transform.up = windDirection;
        windForce = Mathf.Lerp(windForce, targetForce, .9f * Time.deltaTime);
        windVector = windForce * windDirection;
    }

    void ChangeWind()
    {
        targetForce = Random.Range(0, maxWind);
        targetDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        StartCoroutine(windChangeDelay(Random.Range(windChangeRange.x, windChangeRange.y)));
    }

    IEnumerator windChangeDelay(float seconds)
    {
        //Debug.Log("Change wind");
        yield return new WaitForSeconds(seconds);
        ChangeWind();
    }
}
