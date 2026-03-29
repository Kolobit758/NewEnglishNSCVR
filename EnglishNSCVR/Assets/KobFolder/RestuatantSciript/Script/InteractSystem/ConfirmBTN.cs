using UnityEngine;

public class ConfirmBTN : MonoBehaviour, IInteractable
{
    public OrderNew orderNew;
    void Start()
    {
        orderNew = FindAnyObjectByType<OrderNew>();
    }
    public void Interact()
    {
        orderNew.ConfirmOrder();
        Debug.Log("Pick item: " + gameObject.name);
    }
}
