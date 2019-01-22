using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Chip panel UI logic
/// </summary>
public class UIChipsPanel : MonoBehaviour {

    public Button[] chips;

    Vector3 initPos;

	// Use this for initialization
	void Start () {
        this.initPos = this.transform.position;
        for (int i = 0; i <chips.Length; i++)
        {
            int id = i;
            chips[i].onClick.AddListener(() => { this.ChipClick(id); });
        }
	}
	
    /// <summary>
    /// Refreshs the chips.
    /// </summary>
    public void RefreshChips()
    {
        int money = Game.Instance.Money;
        int count = 0;
        for (int i = chips.Length - 1; i >=0;i--)
        {
            bool visible = money >= Math.Pow(10,i);
            if (visible) count++;
            if (count > 4)
                visible = false;
            chips[i].gameObject.SetActive(visible);
        }
        
    }

    /// <summary>
    /// Chips on clicked.
    /// </summary>
    /// <param name="id">Identifier.</param>
    void ChipClick(int id)
    {
        //Debug.LogFormat("ChipClick:{0}", id);
        SoundManager.Instance.PlayClick();

        Chip.Instance.ShowChips(this.chips[id].image, this.chips[id].transform.position);

        Game.Instance.AddChips((int)Math.Pow(10, id));

    }

	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Hides the panel.
    /// </summary>
    public void HidePanel()
    {
        LeanTween.move(this.gameObject, this.initPos + new Vector3(0, -500, 0), 0.5f);
    }

    /// <summary>
    /// Shows the panel.
    /// </summary>
    public void ShowPanel()
    {
        LeanTween.move(this.gameObject, this.initPos, 0.5f);
    }
}
