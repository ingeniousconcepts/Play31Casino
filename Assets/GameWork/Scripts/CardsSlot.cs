using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Card slot is a management class used to manage the cards.
/// </summary>
public class CardsSlot : MonoBehaviour {


    public List<Card> cards = new List<Card>();
    private List<Card> aList = new List<Card>();
    int count = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    /// <summary>
    /// Adds the card.
    /// </summary>
    /// <param name="card">Card.</param>
    /// <param name="flip">If set to <c>true</c> flip.</param>
    public void AddCard(Card card,bool flip = true)
    {
        this.cards.Add(card);
        if (card.number == 1)
            this.aList.Add(card);
        card.transform.SetParent(this.transform, true);

        int i = cards.Count - 1;
        float angel = i * -3f;
        float x = i * 20f;
        float y = 30f * Mathf.Cos(Mathf.Deg2Rad * angel * 5);

        //cards[i].transform.localPosition = new Vector3(x, y, 0f);
        //cards[i].transform.localRotation = Quaternion.Euler(0, 0, angel);

        LeanTween.moveLocal(card.gameObject, new Vector3(x, y, 0f), 0.5f);
        LeanTween.rotateLocal(card.gameObject, new Vector3(0, 0, angel), 0.5f).setOnComplete(Layout);

        if(flip)
        {
            LeanTween.delayedCall(card.gameObject, 0.25f, () => { card.Flip(); });
        }
    }

    /// <summary>
    /// Layout card slot.
    /// </summary>
    void Layout()
    {
        int num = cards.Count;
        LeanTween.rotateLocal(this.gameObject, new Vector3(0, 0, num / 2 * 3f),0.1f);
    }

    /// <summary>
    /// Get current point.
    /// </summary>
    /// <value>The point.</value>
    public int Point
    {
        get{
            return this.GetPoint();
        }
    }

    /// <summary>
    /// Get current point.
    /// </summary>
    /// <returns>The point.</returns>
    int GetPoint()
    {
        int point = this.CalcPoint();
        if(point>31 && aList.Count >0)
        {
            for (int i = 0; i < aList.Count; i++)
            {
                Card card = aList[i];

                card.point = 1;

                point = this.CalcPoint();
                if (point <= 31)
                    break;
            }
        }
        return point;
    }


    int CalcPoint()
    {
        int point = 0;
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            point += card.point;
        }
        return point;
    }

    /// <summary>
    /// Flip all cards.
    /// </summary>
    public void FlipAll()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            card.Flip();
        }
    }

    /// <summary>
    /// Remove all cards.
    /// </summary>
    public void RemoveAll()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            LeanTween.move(card.gameObject, new Vector3(800, 1000, 0), 0.5f).setOnComplete(()=>{
                Game.Instance.RecycleCard(card);
            });
        }
        this.cards.Clear();
    }
}
