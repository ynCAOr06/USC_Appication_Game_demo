using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeHit : MonoBehaviour
{
    public int maxHp = 10;
    [HideInInspector]
    public int hp;

    Slider hpSlider;
    Image fillImage;

    Color color1;

    void Start()
    {
        hp = maxHp;
        hpSlider = GetComponentInChildren<Slider>();

        if (hpSlider)
        {
            fillImage = hpSlider.transform.Find("Fill Area/Fill").GetComponent<Image>();
            color1 = fillImage.color;
            hpSlider.maxValue = maxHp;
            hpSlider.minValue = 0;
            hpSlider.value = hp;
        }
    }

    public void GetHit(int damage)
    {
        hp -= damage;
        if (hp < 0) { hp = 0; }

        Debug.Log($"{gameObject.name}.hp = {hp}");

        if (hpSlider)
        {
            hpSlider.value = hp;
            fillImage.color = Color.Lerp(Color.red, color1, ((float)hp) / maxHp);
        }

        if (hp == 0)
        {
            SendMessage("Die");
        }
    }

    public void ReviveHP()
    {
        hp = maxHp;
    }
}
