using UnityEngine;
using TMPro;

public class DayCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    public void SetDay(int day) => text.text = day.ToString();
}