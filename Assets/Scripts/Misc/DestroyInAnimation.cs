using UnityEngine;

public class DestroyInAnimation : MonoBehaviour
{
    private Animator animator;
    public bool isPlayAdd = true;
    public bool isnormal = false;
    private void Awake()
    {
        if (isnormal) { return; }
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
        if (isPlayAdd)
            animator?.Play("Add");
    }
}
