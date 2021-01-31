using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harbor : MonoBehaviour
{
    public GameMaster gameMaster;

    private void Start()
    {
        
    }

    

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Boat")
        {
            GameMaster.endScene end = collider.GetComponent<Captain>().onBorad ? GameMaster.endScene.goodEnding : GameMaster.endScene.neutralEnding;
            Debug.Log("Trigger win");
            gameMaster.EndGame(true, end);
        }
    }
}
