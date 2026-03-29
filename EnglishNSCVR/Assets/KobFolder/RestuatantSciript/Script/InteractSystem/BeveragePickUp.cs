using UnityEngine;

public class BeveragePickUp : MonoBehaviour, IInteractable
{
    public OrderNew orderNew;
    public SO_Beverage sO_Beverage;
    void Start()
    {
        orderNew = FindAnyObjectByType<OrderNew>();
    }
    public void Interact()
    {
        orderNew.AddBeverage(sO_Beverage);
        Debug.Log("Pick item: " + gameObject.name);
    }
}
