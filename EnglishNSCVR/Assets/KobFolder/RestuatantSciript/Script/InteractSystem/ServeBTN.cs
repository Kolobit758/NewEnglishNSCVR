using UnityEngine;

public class ServeBTN : MonoBehaviour, IInteractable
{
    public OrderNew orderNew;
    void Start()
    {
        orderNew = FindAnyObjectByType<OrderNew>();
    }
    public void Interact()
    {
        orderNew.serve();
        Debug.Log("Pick item: " + gameObject.name);
    }
}
