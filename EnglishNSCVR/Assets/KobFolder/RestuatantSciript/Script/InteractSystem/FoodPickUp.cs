using UnityEngine;

public class FoodPickUp : MonoBehaviour,IInteractable
{
    public OrderNew orderNew;
    public SO_food sO_Food;
    void Start()
    {
        orderNew = FindAnyObjectByType<OrderNew>();
    }
    public void Interact()
    {
        orderNew.AddFood(sO_Food);
        Debug.Log("Pick item: " + gameObject.name );
    }
}
