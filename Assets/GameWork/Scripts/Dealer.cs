using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dealer random deal cards for player & banker.
/// </summary>
public class Dealer : MonoBehaviour
{

    public GameObject cardPrefab;


    public Dictionary<int, Card> cardsMap = new Dictionary<int, Card>();


    System.Random random = new System.Random();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Deal card.
    /// </summary>
    /// <returns>The deal.</returns>
    public Card Deal()
    {
        SoundManager.Instance.PlayFlip();
        Card card = this.GetRandomCard();
        //Debug.LogFormat("{0}:{1}, {2} :{3}", id, card.color, card.number,card.point);
        return card;
    }


    public void Recycle(Card card)
    {
        card.gameObject.SetActive(false);
        card.transform.SetParent(this.transform, false);
        card.transform.localPosition = Vector3.zero;
    }
    /// <summary>
    /// Get the random card index.
    /// </summary>
    /// <returns>The random identifier.</returns>
    Card GetRandomCard()
    {
        Card card = null;
        int id = random.Next(1, 53);
        if (cardsMap.ContainsKey(id))
        {
            if (cardsMap[id].isActiveAndEnabled)
                card = this.GetRandomCard();
            else
            {
                card = cardsMap[id];
                card.gameObject.SetActive(true);
                return card;
            }
        }
        else
        {
            GameObject go = Instantiate(cardPrefab, this.transform);
            card = go.GetComponent<Card>();
            card.ID = id;
            this.cardsMap[id] = card;
        }
        return card;
    }
}
