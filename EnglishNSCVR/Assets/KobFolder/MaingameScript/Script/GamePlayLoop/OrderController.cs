using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using NUnit.Framework;

public class OrderController : MonoBehaviour
{

    public MainDish currentOrder;
    public Gamesubmary gamesubmary;
    public NPC_Controller nPC_Controller;
    public OrderData newOrder;
    public ConverRulebase_Manager converRulebase_Manager;

    orderState currentOrderState;

    public int currentPage;


    public Image timerUI;
    [Header("Money")]
    public int total_salses = 0; // total price that player sell
    public int total_price = 0;// total price of 1 NPC


    [Header("LIST MENU")]
    [SerializeField] List<SO_food> foods = new List<SO_food>();
    [SerializeField] List<SO_Beverage> beverages = new List<SO_Beverage>();
    [SerializeField] List<SO_Topping> toppings = new List<SO_Topping>();
    taste thisTaste;
    // ==================================================================================

    [Header("Screen")]
    [SerializeField] GameObject categoryScreen;
    [SerializeField] GameObject foodScreen;
    [SerializeField] GameObject beverageScreen;
    [SerializeField] GameObject toppingScreen;
    [SerializeField] GameObject submaryScreen;
    [SerializeField] List<GameObject> screens = new List<GameObject>();
    [Header("More UI")]
    [SerializeField] TMP_Text total_price_text;
    [SerializeField] TMP_Text total_selses_text;

    [Header("SpawnPoint")]
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    [Header("Real Object")]
    public GameObject currentFood;
    public GameObject currentBeverage;

    public GameObject buttonPrefab;
    public Button specialBtn;
    [SerializeField] List<TasteBtnData> tasteButtons = new List<TasteBtnData>();



    void OnEnable()
    {
        GameEvent.OnPlayerConfirmedOrder += WaitTime;
        GameEvent.OnNPCPaid += NPCPaidMoney;
    }
    void Start()
    {
        SetState(orderState.Idle);

        GenerateFoodButtons();
        GenerateBeverageButtons();
        GenerateToppingButtons();

        OnSalesChanged();
        OnCostChanged();
    }

    // ==================================================================================

    #region Order function
    public void AddFood(SO_food food)
    {
        currentOrder.food = food;

        Debug.Log("food : " + currentOrder.food.name);
        OnCostChanged();
    }
    public void AddBeverage(SO_Beverage beverage)
    {
        currentOrder.beverage = beverage;
        Debug.Log("food : " + currentOrder.beverage.name);
        OnCostChanged();
    }
    public void AddTopping(SO_Topping topping)
    {
        currentOrder.topping = topping;
        Debug.Log("food : " + currentOrder.topping.name);
        OnCostChanged();
    }
    public void thickSpecial()
    {
        currentOrder.isSpecial = !currentOrder.isSpecial;

        if (currentOrder.isSpecial == true)
        {
            specialBtn.GetComponent<Image>().color = Color.green;
        }
        else
        {
            specialBtn.GetComponent<Image>().color = Color.grey;
        }
        OnCostChanged();

    }
    public void ChooseTatse(int index)
    {
        foreach (TasteBtnData tasteBtn in tasteButtons)
        {
            tasteBtn.button.GetComponent<Image>().color = Color.gray;
        }

        currentOrder.taste = tasteButtons[index].taste;
        tasteButtons[index].button.GetComponent<Image>().color = Color.green;
        OnCostChanged();
    }
    public void ConfirmOrder()
    {

        Debug.Log("total_price : " + total_price);
        OnCostChanged();
        GameEvent.OnPlayerConfirmedOrder?.Invoke();
        converRulebase_Manager.updateTotalCost(total_price);
    }

    public void testOrder()
    {

        Debug.Log("Test order");
    }

    public void NPCPaidMoney()
    {
        //NPC animation

        OnSalesChanged();
        Debug.Log("paid money : " + total_price + "total player money : " + total_salses);
        total_price = 0;
        total_price_text.text = "Cost : " + total_price;
    }


    void orderLoopCase()
    {
        switch (currentOrderState)
        {
            case orderState.Idle:

                break;
            case orderState.Choose_Category:

                break;
            case orderState.Choose_Item:

                break;
            case orderState.Choose_Options:

                break;
            case orderState.Confirm:

                break;
        }
    }

    #endregion

    #region UI function
    public void ChangeScreen(int index)
    {
        OnCostChanged();
        if (index < 0 || index >= screens.Count) return;

        screens[currentPage].SetActive(false);
        screens[index].SetActive(true);
        currentPage = index;
    }

    public void instantBTN(GameObject parent)
    {

        GameObject newBtn = Instantiate(buttonPrefab, parent.transform);

    }
    public void GenerateFoodButtons()
    {
        foreach (var food in foods)
        {
            GameObject btnObj = Instantiate(buttonPrefab, foodScreen.transform);
            btnObj.GetComponentInChildren<TMP_Text>().text = food.name;

            // ดึงคอมโพเนนต์ Button แล้วใส่ Listener
            Button btn = btnObj.GetComponent<Button>();
            if (btn != null)
            {
                // ใช้ Lambda Expression เพื่อส่งค่า food เข้าไปในฟังก์ชัน
                btn.onClick.AddListener(() => AddFood(food));
            }
        }
    }
    public void GenerateBeverageButtons()
    {
        foreach (var beverage in beverages)
        {
            GameObject btnObj = Instantiate(buttonPrefab, beverageScreen.transform);

            // สมมติปุ่มมี TMP_Text
            btnObj.GetComponentInChildren<TMP_Text>().text = beverage.name;
            if (beverage.ItemImage != null)
            {
                btnObj.GetComponentInChildren<Image>().sprite = beverage.ItemImage;
            }


            // ผูก event
            Button btn = btnObj.GetComponent<Button>();
            if (btn != null)
            {
                // ใช้ Lambda Expression เพื่อส่งค่า food เข้าไปในฟังก์ชัน
                btn.onClick.AddListener(() => AddBeverage(beverage));
            }
        }
    }
    public void GenerateToppingButtons()
    {
        foreach (var topping in toppings)
        {
            GameObject btnObj = Instantiate(buttonPrefab, toppingScreen.transform);

            // สมมติปุ่มมี TMP_Text
            btnObj.GetComponentInChildren<TMP_Text>().text = topping.name;

            // ผูก event
            Button btn = btnObj.GetComponent<Button>();
            if (btn != null)
            {
                // ใช้ Lambda Expression เพื่อส่งค่า food 
                btn.onClick.AddListener(() => AddTopping(topping));
            }
        }
    }

    public void OnCostChanged()
    {
        total_price = currentOrder.GetPrice();
        total_price_text.text = "Cost : " + total_price;
    }
    public void OnSalesChanged()
    {
        total_salses += total_price;
        total_selses_text.text = "your money : " + total_salses;
    }
    #endregion

    // ==================================================================================
    public void SetState(orderState newState)
    {
        currentOrderState = newState;
        Debug.Log("orderState -> " + newState);
        Debug.Log("========================================");
        orderLoopCase();
    }
    public void WaitTime()
    {
        StartCoroutine(WaitforCooking());
    }

    public void CookFinished()
    {
        if (currentOrder.food != null)
        {
            if (currentFood != null)
                Destroy(currentFood.gameObject);
            GameObject food = Instantiate(currentOrder.food.prefab, spawnPoints[0]);
            currentFood = food;
            Debug.Log("Spawn food");
        }
        if (currentOrder.beverage != null)
        {
            if (currentBeverage != null)
                Destroy(currentBeverage.gameObject);
            currentBeverage = Instantiate(currentOrder.food.prefab, spawnPoints[1]);
            Debug.Log("Spawn beverage");
        }


    }
    public void serve()
    {


        // 1. เรียกเช็คแค่ครั้งเดียว แล้วเก็บผลลัพธ์ไว้ในตัวแปร
        bool result = isRightServe();
        Debug.Log("isRightServe : " + result);

        // 2. ใช้ตัวแปรนั้นในการตัดสินใจ
        if (result)
        {
            nPC_Controller.PlayHappyFace();
            Debug.Log("Result: Success! NPC is happy.");
        }
        else
        {
            nPC_Controller.PlayAngryFace();
            Debug.Log("Result: Failed! NPC is angry.");
        }

        GameEvent.OnPlayerServed?.Invoke();

        Destroy(currentBeverage);
        Destroy(currentFood);

        currentOrder.food = null;
        currentOrder.beverage = null;
        currentOrder.topping = null;
        currentOrder.taste = taste.Normal;
        currentOrder.isSpecial = false;


        // chage UI tro default
        foreach (TasteBtnData tasteBtn in tasteButtons)
        {
            tasteBtn.button.GetComponent<Image>().color = Color.gray;
        }
        specialBtn.GetComponent<Image>().color = Color.grey;

        // if(currentOrder.food != )
    }

    public void UpdateServiceScore(int score)
    {
        gamesubmary.currentServiceScore += score;
        gamesubmary.UpdateSerciveScoreUI();
    }

    bool isRightServe()
    {
        bool isRightServe = true;
        if (currentOrder.food.name != newOrder.food)
        {
            UpdateServiceScore(-2);
            Debug.Log("-2 service score");
            isRightServe = false;
        }
        if (currentOrder.beverage.name != newOrder.beverage)
        {
            UpdateServiceScore(-2);
            Debug.Log("-2 service score");
            isRightServe = false;
        }
        if (currentOrder.topping.name != newOrder.topping)
        {
            UpdateServiceScore(-2);
            Debug.Log("-2 service score");
            isRightServe = false;
        }
        if (currentOrder.taste != newOrder.taste)
        {
            UpdateServiceScore(-2);
            Debug.Log("-2 service score");
            isRightServe = false;
        }
        if (currentOrder.isSpecial != newOrder.isSpecial)
        {
            UpdateServiceScore(-2);
            Debug.Log("-2 service score");
            isRightServe = false;
        }

        return isRightServe;
    }


    IEnumerator WaitforCooking()
    {
        Debug.Log("Starting Coroutine");

        float cookTime = 2f;
        float timeleft = cookTime;

        timerUI.fillAmount = 1f;

        while (timeleft > 0)
        {
            timeleft -= Time.deltaTime;
            timerUI.fillAmount = timeleft / cookTime;
            yield return null;
        }

        timerUI.fillAmount = 0f;

        Debug.Log("Coroutine resumed after 2 seconds");
        CookFinished();
        Debug.Log("cook finish");
    }



    public OrderData RandomOrder()
    {


        int ranFood = Random.Range(0, foods.Count);
        int ranBvg = Random.Range(0, beverages.Count);
        int ranTopping = Random.Range(0, toppings.Count);


        int ranTaste = Random.Range(0, 4);

        int ranIsSpecial = Random.Range(0, 2);



        switch (ranTaste)
        {
            case 0:
                thisTaste = taste.Sweet;
                break;
            case 1:
                thisTaste = taste.Normal;
                break;
            case 2:
                thisTaste = taste.Salty;
                break;
            case 3:
                thisTaste = taste.Spicy;
                break;
        }
        bool isSpecial = (ranIsSpecial == 0) ? isSpecial = true : isSpecial = false;

        List<int> randomNums = new List<int>();

        newOrder = new OrderData(foods[ranFood].name, beverages[ranBvg].name, toppings[ranTopping].name, thisTaste, isSpecial);
        converRulebase_Manager.setCurentOrder(newOrder);
        return newOrder;
    }
}
