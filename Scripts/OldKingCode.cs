using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldKingCode : MonoBehaviour
{
    public UIcontroller UI;
    public FirstPersonLook look;
    public FirstPersonMovement move;
    public GameObject self;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //disappear on mouse click and enable player movement
        {
            UI.GameMenu();
            look.enabled = true;
            move.enabled = true;
            self.SetActive(false);
        }
    }
}
