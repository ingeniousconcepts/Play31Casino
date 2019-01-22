using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Poker behaviour script
/// For card diaplsy & flap.
/// </summary>
public class Card : MonoBehaviour {

    public Sprite face;
    public Sprite back;

    private int id;
    public int ID
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
            this.color = (id - 1) / 13;
            this.number = id - this.color * 13;

            this.point = this.number;

            if (this.number > 10)
            {
                this.point = 10;
            }
            if (this.number == 1)
                this.point = 11;
            this.gameObject.name = "Card" + id;
        }
    }
    public int color;
    public int number;
    public int point;

    public bool fliped = false;

    public Image image;
    
	// Use this for initialization
	void Start () {

        string[] names = new string[]{
            "CA",
            "CB",
            "CC",
            "CD"
        };

        string path = names[this.color] + this.number.ToString();

        if (face == null)
            face = Resources.Load<Sprite>("cards/" + path);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Flip this card.
    /// </summary>
    public void Flip()
    {
        if (fliped == false)
        {
            
                fliped = true;
                this.image.sprite = this.face;
                LeanTween.scaleX(this.gameObject, 1, 0.2f);
            
        }
    }
}
