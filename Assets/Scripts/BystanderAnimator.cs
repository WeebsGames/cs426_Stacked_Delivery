using UnityEngine;
//
// Triggers a cheer animation when the player gets close to the bystander.
//
public class BystanderAnimator : MonoBehaviour
{
    private Animator animator;
    private bool hasCheered = false;
    [SerializeField] private float triggerRange = 20f;
    [SerializeField] private Transform player;
    //
    // Initializes the animator component.
    //
    public void FindCar()
	{
		player = GameObject.FindWithTag("Player").GetComponent<Transform>();
	}
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    //
    // Triggers the cheer animation when the player enters the trigger range.
    //
    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < triggerRange && !hasCheered)
        {
            hasCheered = true;
            animator.SetTrigger("Cheer");
        }
    }
}