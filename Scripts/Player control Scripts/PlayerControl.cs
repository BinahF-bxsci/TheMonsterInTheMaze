using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
   
    public Transform start;
    public Slider healthbar;
    public Animator animator;
    public UIcontroller ui;
    public MinotaurController mino;
    public FirstPersonLook sight;
    public FirstPersonMovement life;
    public GameObject fight;

    private const string atk = "atk";


    int health = 100;

    public void Start() //reset health and position
    {
        health = 100;
        healthbar.SetValueWithoutNotify(health);
        transform.SetPositionAndRotation(start.position, start.rotation);
        
    }

    void Update() 
    {
        if (Input.GetMouseButtonDown(0)) //sword
        { animator.SetTrigger("atk");  }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyWeapon") && mino.GetHealth() >= 0) //if mino hits - but dont thake damage if hes already dead
            {
                Hit(5);
            }
        if (other.CompareTag("Finish") && mino.GetHealth() <= 0) //exit & mino is dead
        {
            ui.WinMenu();
        }
    }
    public void Hit(int damage)
    {
        health -= damage;
        healthbar.SetValueWithoutNotify(health);
        if (health <= 0) { //die.
            ui.LoseMenu();
            life.enabled = false;
            sight.enabled = false;
            fight.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
