using UnityEngine;

public class DestroyInAnimation : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    public void InActiveObject()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        animator.Play("Add");
    }
}
