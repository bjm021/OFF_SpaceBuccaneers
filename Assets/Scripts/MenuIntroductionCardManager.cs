using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class MenuIntroductionCardManager : MonoBehaviour
{
   [SerializeField] public Image cardImage;
   [SerializeField] public Image unitPreviewImage;
   [SerializeField] public TMP_Text unitDescription;
   [SerializeField] public TMP_Text unitCost;
   [SerializeField] public GameObject nextButton;
   [SerializeField] public GameObject previousButton;
   [SerializeField] public Sprite minerCard;
   [SerializeField] public Sprite unitCard;
   [SerializeField] public Sprite abilityCard;
   [SerializeField] public List<Sprite> unitPreviews;
   [SerializeField] public List<string> costs;
   [SerializeField] public List<string> descriptions;

   int _currentCardIndex = 1;

   public void OnClickNext()
   {
      _currentCardIndex++;
      UpdateCard();
   }
   
   public void OnClickPrevious()
   {
      _currentCardIndex--;
      UpdateCard();
   }
   
   private void UpdateCard()
   {
      if (_currentCardIndex == 10) nextButton.SetActive(false);
      if (_currentCardIndex == 9) nextButton.SetActive(true);
      
      if (_currentCardIndex == 2) previousButton.SetActive(true);
      if (_currentCardIndex == 1) previousButton.SetActive(false);
      
      unitPreviewImage.sprite = unitPreviews[_currentCardIndex - 1];
      unitDescription.text = descriptions[_currentCardIndex - 1];
      unitCost.text = costs[_currentCardIndex - 1];
      
      cardImage.sprite = _currentCardIndex switch
      {
         1 => minerCard,
         > 1 and < 7 => unitCard,
         > 6 and < 11 => abilityCard,
         _ => cardImage.sprite
      };
   }
}
