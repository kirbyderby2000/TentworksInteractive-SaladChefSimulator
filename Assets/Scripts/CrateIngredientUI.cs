using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateIngredientUI : MonoBehaviour
{

    [SerializeField] IngredientItemSpawner spawnerRef;
    
    [SerializeField] UnityEngine.UI.Image ingredientImage;
    [SerializeField] UnityEngine.UI.Image rightArrowImage;
    private void Awake()
    {
        AdjustToSpawnerLocation();
        PositionUI();
        ingredientImage.sprite = spawnerRef.GetIngredientSprite();
    }


    private void AdjustToSpawnerLocation()
    {
        if(MainCamera.CameraInstance.WorldToViewportPoint(spawnerRef.transform.position).x > 0.5f)
        {
            AdjustPivot(0.0f);
            ingredientImage.transform.SetAsLastSibling();
            rightArrowImage.rectTransform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            AdjustPivot(1.0f);
            ingredientImage.transform.SetAsFirstSibling();
        }
    }

    private void AdjustPivot(float xAnchorPosition)
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        Vector2 pivot = rectTransform.pivot;
        pivot.x = xAnchorPosition;
        rectTransform.pivot = pivot;
    }



    private void PositionUI()
    {
        transform.position = MainCamera.CameraInstance.WorldToScreenPoint(spawnerRef.GetUITransformReference().position);
    }
}
