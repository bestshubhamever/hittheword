using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickFillerAnimation : MonoBehaviour
{
    public Animator anim1,anim2,anim3,anim4;
    // Start is called before the first frame update
    void Start()
    {
        //anim1.SetTrigger("start");
        //  anim = GetComponent<Animation>();
        //Make sure you have attached your animation in the Animations attribute
        //  anim.Play("myAnimation");
        //anim["myAnimation"].speed = 0;

    }

    // Update is called once per frame
    void Update()
    {
       // Setbricks(PlayerPrefs.GetInt("wrong"));
        // call set brick method
        //if health = 1 then anim1.settrigger start
    }
    public void Setbricks(int health)
    {
        if (health == 1) {
            anim1.SetTrigger("start");
        }
        if (health == 2)
        {
            anim2.SetTrigger("start");
        }
        if (health == 3)
        {
            anim3.SetTrigger("start");
        }
        if (health == 4)
        {
            anim4.SetTrigger("start");
        }
        //  health;
        //anim["myAnimation"].play  // m_Animator.SetTrigger("Jump");
        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
