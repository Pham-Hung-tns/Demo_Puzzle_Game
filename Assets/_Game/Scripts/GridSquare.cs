using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    public Image normalImage;
    public List<Sprite> normalImages;
    public Image overImage; // when players drag jewels, players can know which grid they will place it in
    public Image activeImage;
    public bool IsSelected { get; set; }
    public gridColor gridColor;

    // Start is called before the first frame update
    private void Start()
    {
        IsSelected = false;
        gridColor = gridColor.Nothing;
    }

    public bool CanUseThisGrid()
    {
        return overImage.gameObject.activeSelf;
    }

    /// <summary>
    /// show jewel on the grid which play drag on
    /// </summary>
    /// <param name="_sprite"></param>
    /// <param name="jewels"></param>
    /// <param name="color"></param>
    public void ActiveGrid(Sprite _sprite, JewelsController jewels, string color)
    {
        overImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        activeImage.sprite = _sprite;
        Enum.TryParse(color, out gridColor);
        IsSelected = true;
        Pool.Instance.ReturnJewels(jewels.gridColor.ToString(), jewels.gameObject);
        // calculate amount of jewel to spawn.
        JewelsStorage.amountOfJewel--;
        if (JewelsStorage.amountOfJewel == 0)
        {
            EventsInGame.CreateJewels();
        }
    }

    /// <summary>
    /// Deactive image on grid when them have neighbors
    /// </summary>
    public void DeActiveGrid()
    {
        overImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(false);
        gridColor = gridColor.Nothing;
        activeImage.sprite = null;
        IsSelected = false;
    }

    public void SetImage(bool setFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = setFirstImage ? normalImages[1] : normalImages[0];
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsSelected)
            overImage.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        overImage.gameObject.SetActive(false);
    }
}