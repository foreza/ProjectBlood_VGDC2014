using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScrollableList : MonoBehaviour
{
    public GameObject itemPrefab;
    public int columnCount = 1;
	List<Objective> descs;
	public int itemCount;
    public void doTheGUI()
    {
		descs = this.GetComponent<UIMaster> ().descriptions;
		itemCount = descs.Count;
        RectTransform rowRectTransform = itemPrefab.GetComponent<RectTransform>();
        RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();

        //calculate the width and height of each child item.
        float width = containerRectTransform.rect.width / columnCount; //half screen width
        float ratio = width / rowRectTransform.rect.width;
        float height = rowRectTransform.rect.height * ratio; //distance between two rows
        int rowCount = itemCount / columnCount;
        //if (itemCount % rowCount > 0)
          //  rowCount++;

        //adjust the height of the container so that it will just barely fit all its children
        float scrollHeight = height * rowCount;
        containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
        containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);

        int j = 0;
		Debug.Log ("objectives has length "+descs.Count);
        for (int i = 0; i < itemCount; i++)
        {
            //this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
            if (i % columnCount == 0)
                j++;

            //create a new item, name it, and set the parent
            GameObject newItem = Instantiate(itemPrefab) as GameObject;
            newItem.name = gameObject.name + " item at (" + i + "," + j + ")";
            //newItem.transform.parent = gameObject.transform;
			newItem.transform.SetParent (gameObject.transform);
            //move and size the new item
            RectTransform rectTransform = newItem.GetComponent<RectTransform>();
			newItem.GetComponentInChildren<Text>().text = descs[i].name;
            float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
            float y = containerRectTransform.rect.height / 2 - height * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x + width;
            y = rectTransform.offsetMin.y + height;
            rectTransform.offsetMax = new Vector2(x, y);
        }


    }

}
