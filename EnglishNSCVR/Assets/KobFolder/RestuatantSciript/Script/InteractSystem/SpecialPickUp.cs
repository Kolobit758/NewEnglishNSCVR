using UnityEngine;

public class SpecialPickUp : MonoBehaviour
{
    public OrderNew orderNew;
    public bool isSpecial;
    void Start()
    {
        orderNew = FindAnyObjectByType<OrderNew>();
    }
    public void Interact()
    {
        orderNew.NewThickSpecial(isSpecial);
        Debug.Log("Pick item: " + gameObject.name);
    }
}
