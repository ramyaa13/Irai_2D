using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    Animator anim;
    void Awake() { anim = GetComponent<Animator>(); }

    public void UpdateLocomotion(float speed01, bool grounded, float velY)
    {
        //anim.SetFloat("Speed", speed01);
        //anim.SetBool("Grounded", grounded);
        //anim.SetFloat("VelY", velY);
    }
    public void PlayJump() => anim.SetTrigger("Jump");
    public void PlayHurt() => anim.SetTrigger("Hurt");
}