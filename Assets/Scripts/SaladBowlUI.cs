using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaladBowlUI : MonoBehaviour
{
    [SerializeField] SaladLocator saladBowl;

    [SerializeField] SaladUIDisplayer saladIngredientsUI;


    private void Awake()
    {
        saladBowl.SaladBowlAttached.OnSaladIngredientsChanged.AddListener(OnSaladBowlIngredientsChanged);
        OnSaladBowlIngredientsChanged(saladBowl.SaladBowlAttached);
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = MainCamera.CameraInstance.WorldToScreenPoint(saladBowl.transform.position);
    }


    private void OnSaladBowlIngredientsChanged(SaladBowl salad)
    {
        saladIngredientsUI.UpdateSaladIngredientsUI(salad.SaladIngredients);
        UpdatePosition();
        this.gameObject.SetActive(salad.SaladIngredients.Count > 0);
    }

}
