using UnityEngine;

public class TastePickUp : MonoBehaviour
{
    public OrderNew orderNew;
    public taste taste;
    void Start()
    {
        orderNew = FindAnyObjectByType<OrderNew>();
    }
    public void Interact()
    {
        orderNew.NewChooseTatse(taste);
        Debug.Log("Pick item: " + gameObject.name);
    }
}
