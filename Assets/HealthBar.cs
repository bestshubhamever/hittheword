using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Animation anim;

    public Slider slider;
//	public Gradient gradient;
	public Image fill;
    void Start()
    {
       // anim = GetComponent<Animation>();
        //Make sure you have attached your animation in the Animations attribute
      //  anim.Play("myAnimation");
      //  anim["myAnimation"].speed = 0;
    }
    public void SetMaxHealth(int health)
	{
		slider.maxValue = health;
		slider.value = health;

		//fill.color = gradient.Evaluate(1f);
	}

    public void SetHealth(int health)
	{
		slider.value = health;
        Debug.Log("health is"+health);

		//fill.color = gradient.Evaluate(slider.normalizedValue);
	}
    public void Setbricks(int health)
    {
       //  health;
        //anim["myAnimation"].play
        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void AnimateOnSliderValue()
    {
       // anim["myAnimation"].normalizedTime = slider.value;
    }

}
