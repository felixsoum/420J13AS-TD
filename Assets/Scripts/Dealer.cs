using TMPro;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    public TMP_Text pokerHandText;
    public CardVisual[] hand;
    Deck deck = new Deck();

    void Start()
    {
        deck.Shuffle();
        DrawHand();
    }

    public void DrawCard(int handIndex)
    {
        hand[handIndex].ApplyData(deck.Draw());
        UpdatePokerHandText();
    }

    public void DrawHand()
    {
        foreach (CardVisual cardVisual in hand)
        {
            cardVisual.ApplyData(deck.Draw());
        }
        UpdatePokerHandText();
    }

    void UpdatePokerHandText()
    {
        if (hand[0].data.suit == hand[1].data.suit
            && hand[0].data.suit == hand[2].data.suit
            && hand[0].data.suit == hand[3].data.suit
            && hand[0].data.suit == hand[4].data.suit)
        {
            pokerHandText.text = "FLUSH (ALL SAME SUIT) DETECTED";
            return;
        }

        // Try to detect a pair!

        pokerHandText.text = "NO HAND DETECTED";
    }
}
