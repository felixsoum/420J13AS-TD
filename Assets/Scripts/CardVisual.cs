using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Suit
{
    Spade, // ♠ or S
    Heart, // ♥ or H
    Club, // ♣ or C
    Diamond // ♦ or D
}

public class Card
{
    public int value;
    public Suit suit;

    public Card(int value, Suit suit)
    {
        this.value = value;
        this.suit = suit;
    }

    public string GetIcon()
    {
        string[] icons = { "♠", "♥", "♣", "♦" };
        int suitIndex = (int)suit;
        string icon = icons[suitIndex];
        return icon;
    }

    public string GetValueLabel()
    {
        string valueString = value.ToString();
        switch (value)
        {
            case 1:
                valueString = "A";
                break;
            case 11:
                valueString = "J";
                break;
            case 12:
                valueString = "Q";
                break;
            case 13:
                valueString = "K";
                break;
        }
        return valueString;
    }

    public override string ToString()
    {
        return $"[{GetValueLabel()}{GetIcon()}]";
    }
}

public class Deck
{
    List<Card> cards = new List<Card>();

    public Deck()
    {
        AddStandardDeck();
    }

    private void AddStandardDeck()
    {
        for (int i = 1; i <= 13; i++)
        {
            cards.Add(new Card(i, Suit.Spade));
            cards.Add(new Card(i, Suit.Heart));
            cards.Add(new Card(i, Suit.Club));
            cards.Add(new Card(i, Suit.Diamond));
        }
    }

    public Card Draw()
    {
        if (cards.Count == 0)
        {
            AddStandardDeck();
            Shuffle();
        }

        Card cardToDraw = cards[0];
        cards.RemoveAt(0);
        return cardToDraw;
    }

    public void Shuffle()
    {
        List<Card> tempDeck = new List<Card>();

        while (cards.Count > 0)
        {
            int randomIndex = Random.Range(0, cards.Count);
            tempDeck.Add(cards[randomIndex]);
            cards.RemoveAt(randomIndex);
        }

        cards = tempDeck;
    }
}


public class CardVisual : MonoBehaviour
{
    public TMP_Text valueText;
    public TMP_Text suitText;
    public Card data;

    internal void ApplyData(Card card)
    {
        data = card;
        valueText.text = card.GetValueLabel();
        suitText.text = card.GetIcon();

        switch (card.suit)
        {
            case Suit.Spade:
            case Suit.Club:
                valueText.color = Color.black;
                suitText.color = Color.black;
                break;
            case Suit.Heart:
            case Suit.Diamond:
                valueText.color = Color.red;
                suitText.color = Color.red;
                break;
        }
    }
}
