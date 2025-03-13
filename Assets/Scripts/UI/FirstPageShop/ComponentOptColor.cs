using System;
using UnityEngine;
using UnityEngine.UI;

public class ComponentOptColor : MonoBehaviour
{
    public static EventHandler<int> OnChangePart;
    [SerializeField] private GridLayoutGroup groupBtn;
    [SerializeField] private GameObject[] num_component_Color;
    [SerializeField] private UIGeneratePress[] uiPress;
    private void OnEnable()
    {
        OnChangePart += CheckActiveComponent;
    }

    private void CheckActiveComponent(object sender, int e)
    {
        uiPress[e].ShowAndHiddenGameObject();
    }

    public void ChangeComponent(int num)
    {
        if (num == 2)
        {
            for (int i = 0; i < num_component_Color.Length; i++)
            {
                num_component_Color[i].gameObject.SetActive(true);
                if (i == 2) { num_component_Color[i].gameObject.SetActive(false); }
            }
            groupBtn.spacing = new Vector2(130, 0);
        }
        else
        {
            for (int i = 0; i < num_component_Color.Length; i++)
            {
                num_component_Color[i].gameObject.SetActive(true);

            }
            groupBtn.spacing = new Vector2(30, 0);
        }

    }
    public void SetNumCurrentChoice1()
    {
        OnChangePart?.Invoke(this, 0);
    }
    public void SetNumCurrentChoice2()
    {
        OnChangePart?.Invoke(this, 1);
    }
    public void SetNumCurrentChoice3()
    {
        OnChangePart?.Invoke(this, 2);
    }
    private void OnDisable()
    {
        OnChangePart -= CheckActiveComponent;
    }
}
