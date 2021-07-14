using UnityEngine;

public class AnimTesting : MonoBehaviour
{
    private Animator animator;
    private bool toggle;
    [SerializeField] private float timer = 0.0f;
    private float tick = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= tick)
        {
            if(!toggle)
                animator.SetTrigger("Close");
            else
                animator.SetTrigger("Open");

            timer -= tick;
            toggle = !toggle;
        }

        timer += Time.deltaTime;
    }
}
