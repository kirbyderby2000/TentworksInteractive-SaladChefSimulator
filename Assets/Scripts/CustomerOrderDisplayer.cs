using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderDisplayer : MonoBehaviour
{
    [SerializeField] IngredientDisplayer saladUIDisplayer;
    

    [Header("Customer Slider References")]
    [SerializeField] UnityEngine.UI.Slider customerSliderUI;
    [SerializeField] UnityEngine.UI.Image sliderHandleImage;
    [SerializeField] UnityEngine.UI.Image sliderFillAreaImage;

    [Header("Angered State Settings")]
    [SerializeField] Color angeredStateSliderHandleColor; 
    [SerializeField] Color angeredStateSliderBackgroundColor;
    private Customer customersOrderToDisplay = null;

    private void Awake()
    {
        if(customersOrderToDisplay == null)
        {
            saladUIDisplayer.ToggleSaladUI(false);
        }
    }

    public void AssignCustomer(Customer customerToDisplay)
    {
        if (customersOrderToDisplay != null)
        {
            Debug.LogError("You can't assign more than one customer for this UI to display");
            return;
        }

        customersOrderToDisplay = customerToDisplay;
        UpdateCustomerOrder(customersOrderToDisplay);
        customersOrderToDisplay.OnCustomerOrderPlaced.AddListener(UpdateCustomerOrder);
        customersOrderToDisplay.OnCustomerLeaving.AddListener(OnCustomerLeaving);
        customersOrderToDisplay.OnCustomerTimerChanged.AddListener(UpdateCustomerTimer);
        customersOrderToDisplay.OnCustomerAngered.AddListener(CustomerAngered);
    }

    private void UpdateCustomerOrder(Customer customersOrder)
    {
        saladUIDisplayer.UpdateSaladIngredientsUI(customersOrder.CustomerOrder);
        RepositionUIOntoCustomer();
    }

    private void UpdateCustomerTimer(Customer customerOrder)
    {
        customerSliderUI.value = customerOrder.CustomerTimeLeft;
    }

    private void CustomerAngered(Customer customer)
    {
        sliderHandleImage.color = angeredStateSliderHandleColor;
        sliderFillAreaImage.color = angeredStateSliderBackgroundColor;
    }

    private void RepositionUIOntoCustomer()
    {
        Vector3 customerPosition = customersOrderToDisplay.transform.position;
        Vector3 positionToMoveTo = MainCamera.CameraInstance.WorldToScreenPoint(customerPosition);
        transform.position = positionToMoveTo;
    }

    private void OnCustomerLeaving(Customer customer)
    {
        customersOrderToDisplay.OnCustomerOrderPlaced.RemoveListener(UpdateCustomerOrder);
        customersOrderToDisplay.OnCustomerTimerChanged.RemoveListener(UpdateCustomerTimer);
        customersOrderToDisplay.OnCustomerLeaving.RemoveListener(OnCustomerLeaving);
        customersOrderToDisplay.OnCustomerAngered.RemoveListener(CustomerAngered);
        Destroy(this.gameObject);
    }
}
