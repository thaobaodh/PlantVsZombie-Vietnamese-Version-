using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardHolderManager : MonoBehaviour
{
    [Header("Card Holder Parameter")]
    [SerializeField] private Transform _cardHolderPosition;
    [SerializeField] private GameObject _card;
    [SerializeField] private Card[] _cardSO;


    [Header("Card Parameter")]
    private int _cardAmount;
    [SerializeField] private GameObject[] _plantedCards;
    private int _cost;
    private Sprite _icon;

    private void Start() 
    {
        _cardAmount = _cardSO.Length;
        _plantedCards = new GameObject[_cardAmount];
        for (int i = 0; i< _cardAmount; i ++)
        {
            CreateCard(i);
        }
    }

    private void CreateCard(int index)
    {
        var card = Instantiate(_card, _cardHolderPosition);
        CardManager cardManager = card.GetComponent<CardManager>();

        cardManager.CardSO = _cardSO[index];

        _plantedCards[index] = card;
        _icon = _cardSO[index].icon;
        _cost = _cardSO[index].cost;

        card.GetComponentInChildren<SpriteRenderer>().sprite = _icon;
        card.GetComponentInChildren<TMP_Text>().text = _cost.ToString();
    }
}
