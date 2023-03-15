using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClick : MonoBehaviour
{
    SceneController controller;

    // Start is called before the first frame update
    void Start()
    {
        GameObject controllerObject = GameObject.Find("SceneManager");
        controller = controllerObject.GetComponent<SceneController>();
    }

    // Update is called once per frame
    public void Update()
    {
        if(controller.swapActivated==true)
        {
            switch(controller.battleState.ToString())
            {
                case "PlayerTurn":
                {
                        if(!controller.GetPlayer().handList.Contains(this.gameObject) && this.GetComponentInChildren<CardStats>().unique==false)
                        {
                            if(controller.swapActivated && this.transform.parent.parent.parent.name.ToString()=="PlayerField" )
                            {
                                this.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            }
                        }
                        break;

                }

                case "EnemyTurn":
                {
                        if (!controller.GetPlayer().handList.Contains(this.gameObject) && this.GetComponentInChildren<CardStats>().unique == false)
                        {
                            if (controller.swapActivated && this.transform.parent.parent.parent.name.ToString() == "EnemyField")
                            {
                                this.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            }
                        }
                        break;
                }
            }

        }
        else if(!controller.GetPlayer().handList.Contains(this.gameObject)) this.GetComponent<CanvasGroup>().blocksRaycasts = false;


    }
}
