using UnityEngine;

public class ToppingPickUp : MonoBehaviour
{
    public OrderNew orderNew;
    public SO_Topping sO_Topping;
    void Start()
    {
        orderNew = FindAnyObjectByType<OrderNew>();
    }
    public void Interact()
    {
        orderNew.AddTopping(sO_Topping);
        Debug.Log("Pick item: " + gameObject.name);
    }
}
