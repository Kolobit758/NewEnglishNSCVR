using System;
using UnityEngine;

[System.Serializable]
public class MainDish
{
    public SO_Beverage beverage;
    public SO_food food;
    public SO_Topping topping;

    public taste taste;

    public bool isSpecial;

    // public void PrintData()
    // {
    //     Debug.Log("food : " + food.name + " " + "beverage : " + beverage.name + " " + "topping : " + topping.name + " " + "taste : " + taste + " " + "isSpecial : " + isSpecial);
    // }
    public int GetPrice()
    {
        int total = 0;

        if(food != null)
        {
            total += food.price;
        }
        if(beverage != null)
        {
            total += beverage.price;
        }
        if(topping != null)
        {
            total += topping.price;
        }

        if(isSpecial == true)
        {
            total += 20;
        }

        return total;
    }
}
