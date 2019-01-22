using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Chip class manage the chips player bets.
/// </summary>
public class Chip : RefSingleton<Chip> {

    public Image image;
    public Text text;
    public Image flyChip;

    Vector3 initPos;
	// Use this for initialization
	void Start () {
        this.Clear();
        initPos = this.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Show the chips.
    /// </summary>
    /// <param name="image">Image.</param>
    /// <param name="from">From.</param>
    public void ShowChips(Image image , Vector3 from)
    {
        this.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(true);
        this.flyChip.transform.position = from;
        if (image != null)
            this.flyChip.sprite = image.sprite;
        this.flyChip.enabled = true;
        LeanTween.move(this.flyChip.gameObject, this.image.transform, 0.3f).setOnComplete(() => {
            this.image.enabled = true;
            this.image.sprite = this.flyChip.sprite;
            this.flyChip.enabled = false;
            this.text.enabled = true;
            this.text.text = Game.Instance.chips.ToString();

        });
    }

    /// <summary>
    /// Clear chips display.
    /// </summary>
    public void Clear()
    {
        this.image.enabled = false;
        this.flyChip.enabled = false;
        this.text.enabled = false;
    }


    /// <summary>
    /// Fly to side.
    /// </summary>
    public void FlyToSide()
    {
        LeanTween.moveLocal(this.gameObject, new Vector3(300, 0, 0), 0.3f).setEase(LeanTweenType.easeInSine);
    }

    /// <summary>
    /// Fly to banker.
    /// </summary>
    public void FlyToBanker()
    {
        LeanTween.moveLocal(this.gameObject, new Vector3(0, 500, 0), 0.3f).setEase(LeanTweenType.easeInSine);
    }

    /// <summary>
    /// Fly to player.
    /// </summary>
    public void FlyToPlayer()
    {
        LeanTween.moveLocal(this.gameObject, new Vector3(0, -500, 0), 0.3f).setEase(LeanTweenType.easeInSine);
    }

    /// <summary>
    /// Reset the position.
    /// </summary>
    public void ResetPosition()
    {
        this.transform.localPosition = initPos;
    }
}
