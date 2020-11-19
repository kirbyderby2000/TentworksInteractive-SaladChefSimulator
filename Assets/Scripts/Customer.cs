using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Customer : MonoBehaviour
{
    public enum CustomerState { Default, Angry };

    [Header("Order Selection Settings")]
    [SerializeField] Ingredient requiredIngredient;
    [SerializeField] List<Ingredient> additionalSaladIngredients = new List<Ingredient>();
    [SerializeField] int minIngredientsOnOrder = 2;
    [SerializeField] int maxIngredientsOnOrder = 4;
    [Header("Timer Settings")]
    [SerializeField] float awaitingTimePerIngredientChosen;
    [Range(1.25f,3.0f)]
    [SerializeField] float angryStateTimeModifier = 2.0f;

    /// <summary>
    /// The customer's serving area reference
    /// </summary>
    CustomerServingArea servingArea = null;

    /// <summary>
    /// The time remaining for this customer's order to be fulfilled
    /// </summary>
    private float timeRemaining = 1.0f;
    /// <summary>
    /// The total time given for this customer's order to be fulfilled
    /// </summary>
    private float totalTimeAwaitted = 1.0f;

    /// <summary>
    /// Whether or not this customer has been served
    /// </summary>
    private bool customerServed = false;

    /// <summary>
    /// The customer logic handler (used to reward / punish the players)
    /// </summary>
    private ICustomerLogicHandler customerLogicHandler = null;

    /// <summary>
    /// This customer's order
    /// </summary>
    public List<Ingredient> CustomerOrder
    {
        private set;
        get;
    } = new List<Ingredient>();

    /// <summary>
    /// The fraction of time left for this customer's order to be fulfilled (0.0f - 1.0f) (Reaches 0.0f as the timer progresses)
    /// </summary>
    public float CustomerTimeLeft
    {
        get
        {
            return Mathf.Clamp01(timeRemaining / totalTimeAwaitted);
        }
    }

    /// <summary>
    /// The customers current state
    /// </summary>
    public CustomerState CurrentState
    {
        private set;
        get;
    } = CustomerState.Default;

    /// <summary>
    /// List of players that angered this customer
    /// </summary>
    public List<Players> AngeredByPlayers
    {
        private set;
        get;
    } = new List<Players>();


    

    /// <summary>
    /// Assigns the customer's serving area; this also starts the customer coroutine
    /// </summary>
    /// <param name="customerServingArea"></param>
    public void StartCustomerCoroutine(CustomerServingArea customerServingArea, ICustomerLogicHandler customerLogicHandler)
    {
        this.customerLogicHandler = customerLogicHandler;

        // If the serving area isn't already null, then debug a log error
        if (servingArea != null)
        {
            Debug.LogError("AssignServingArea was called but this customer already has a serving area assigned", this.gameObject);
            return;
        }
        // Otherwise, assign the customer serving area
        servingArea = customerServingArea;
        // Start the customer coroutine
        StartCoroutine(CustomerCoroutine());
    }

    IEnumerator CustomerCoroutine()
    {
        // Have the customer enter the restaurant
        yield return CustomerEntry();
        // Have the customer place the order
        SelectOrder();
        // Subscribe to the serving area's OnServed event
        servingArea.OnAreaServed.AddListener(CustomerAreaServed);
        // Have the customer await for the order
        yield return WaitForOrder();
        // Unsubscribe from the serving area's OnServed event
        servingArea.OnAreaServed.RemoveListener(CustomerAreaServed);
        // If the customer has not been served upon waiting for the complete time assigned,
        // then punish the players with the customer logic handler reference
        if(customerServed == false && customerLogicHandler != null)
        {
            customerLogicHandler.PunishPlayers(this);
        }
        // Call the customer leaving event
        OnCustomerLeaving.Invoke(this);
        // Have the customer exit the restaurant
        yield return CustomerExit();
    }

    /// <summary>
    /// Coroutine used for the customer's entry
    /// </summary>
    /// <returns></returns>
    IEnumerator CustomerEntry()
    {
        yield return null;
    }

    /// <summary>
    /// Coroutine used for the customer's order awaiting
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForOrder()
    {
        // While the customer is not served and the time remaining is more than 0,
        // keep waiting for the order
        while(customerServed == false && timeRemaining > Mathf.Epsilon)
        {
            // Modify the timer by Time.deltaTime
            ModifyTimer(Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// Coroutine used for the customer's exit
    /// </summary>
    /// <returns></returns>
    IEnumerator CustomerExit()
    {
        yield return null;
    }

    /// <summary>
    /// Method called to modify the customer's time remaining vlaue
    /// </summary>
    /// <param name="deltaTime">The time passed (deltaTime)</param>
    private void ModifyTimer(float deltaTime)
    {
        // If the customer is angry, then multiply the deltaTime by the angry state time modifier
        // and deduct the modified deltaTime
        if(CurrentState == CustomerState.Angry)
        {
            timeRemaining -= deltaTime * angryStateTimeModifier;
        }
        // Otherwise, simply deduct the modified deltaTime
        else
        {
            timeRemaining -= deltaTime;
        }
        // Invoke the OnCustomerTimerChanged event
        OnCustomerTimerChanged.Invoke(this);
    }


    /// <summary>
    /// Method called when the customer is served
    /// </summary>
    /// <param name="itemServed"></param>
    /// <param name="servingPlayer"></param>
    private void CustomerAreaServed(HoldableItem itemServed, PlayerController servingPlayer)
    {
        // Try to get the salad locator on the item served
        SaladLocator saladLocator = itemServed.GetComponent<SaladLocator>();
        // If the salad item is not null, then see if the salad ingredients match the order ingredients
        if(saladLocator != null)
        {
            // Check if the ingredient lists are the same
            if(AreIngredientListsTheSame(CustomerOrder, saladLocator.SaladBowlAttached.SaladIngredients))
            {
                // If yes, then the customer has been served
                customerServed = true;
                // Remove the listener from the on area served event
                servingArea.OnAreaServed.RemoveListener(CustomerAreaServed);
                // Consume the ingredients in the bowl
                saladLocator.SaladBowlAttached.ConsumeBowl();
                // Reward the corresponding player using the respective player logic handler
                if(customerLogicHandler != null)
                    customerLogicHandler.RewardPlayer(servingPlayer.Player, this);
                OnCustomerCorrectlyServed.Invoke(this);
            }
            // Otherwise, the player served the wrong order, call the player served wrong order
            else
            {
                PlayerServedWrongOder(servingPlayer);
            }
        }
        // If the item served is not a salad, make the customer angry lol
        else
        {
            PlayerServedWrongOder(servingPlayer);
        }
    }

    /// <summary>
    /// Method called when a player serves the wrong order to this customer
    /// </summary>
    /// <param name="servingPlayer"></param>
    private void PlayerServedWrongOder(PlayerController servingPlayer)
    {
        // If the customer's state isn't already set to angry, set it to angry
        if(CurrentState == CustomerState.Default)
        {
            CurrentState = CustomerState.Angry;
            OnCustomerAngered.Invoke(this);
        }

        // If the angered by players list doesn't contain the player that served the wrong order
        // then add that player to the list
        if(AngeredByPlayers.Contains(servingPlayer.Player) == false)
        {
            AngeredByPlayers.Add(servingPlayer.Player);
        }
    }

    /// <summary>
    /// Method called for the customer to place his order
    /// </summary>
    private void SelectOrder()
    {
        // Add the first required ingredient (likely chopped lettuce)
        CustomerOrder.Add(requiredIngredient);
        // Select additional random ingredient count (determined by the min / max ingredient order values)
        int ingredientOrderCount = Random.Range(minIngredientsOnOrder, maxIngredientsOnOrder + 1);
        // While the customer order ingredients is less than the required amount, 
        // keep adding random ingredients into the recipe order
        while(CustomerOrder.Count < ingredientOrderCount)
        {
            Ingredient randomIngredient = additionalSaladIngredients[Random.Range(0, additionalSaladIngredients.Count)];
            CustomerOrder.Add(randomIngredient);
        }
        // Initialize the timers on this customer's order
        InitializeTimers();

        OnCustomerOrderPlaced.Invoke(this);
    }

    /// <summary>
    /// Method called to initialize the timer values on this customer's order
    /// </summary>
    private void InitializeTimers()
    {
        totalTimeAwaitted = CustomerOrder.Count * awaitingTimePerIngredientChosen;
        timeRemaining = totalTimeAwaitted;
    }



    /// <summary>
    /// Returns whether or not the ingredient lists are the same (without regard to list order)
    /// </summary>
    /// <param name="list1">The first list to compare</param>
    /// <param name="list2">The second list to compare</param>
    /// <returns></returns>
    private bool AreIngredientListsTheSame(List<Ingredient> list1, List<Ingredient> list2)
    {
        // If the list counts aren't the same, then return false
        if (list1.Count != list2.Count)
            return false;

        // Otherwise, create shallow copy lists of the two lists
        // Order both lists by the ingredient name property, this will ensure 
        // both lists are in the same order when cross-checking the lists
        List<Ingredient> shallowSortedList1 = new List<Ingredient>(list1).OrderBy(x => { return x.IngredientName; }).ToList();
        List<Ingredient> shallowSortedList2 = new List<Ingredient>(list2).OrderBy(x => { return x.IngredientName; }).ToList();

        // Iterate through the lists
        for (int i = 0; i < shallowSortedList1.Count; i++)
        {
            // If the sorted first list index ingredient isn't the same as the 
            // second sort list index, then the ingredients are not the same
            if (shallowSortedList1[i].IngredientName != shallowSortedList2[i].IngredientName)
            {
                // return false
                return false;
            }
        }

        // Otherwise, all the ingredients are the same in the given lists,
        // return true
        return true;
    }

    [Header("Customer Events")]
    public CustomerEventHandler OnCustomerOrderPlaced;

    public CustomerEventHandler OnCustomerAngered;

    public CustomerEventHandler OnCustomerCorrectlyServed;

    public CustomerEventHandler OnCustomerTimerChanged;

    public CustomerEventHandler OnCustomerLeaving;

}

[System.Serializable]
public class CustomerEventHandler : UnityEngine.Events.UnityEvent<Customer>
{

}
