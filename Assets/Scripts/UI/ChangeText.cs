using TMPro;
using UnityEngine;

public class ChangeText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI oldTextImp1;
    [SerializeField] private TextMeshProUGUI oldTextImp2;
    [SerializeField] private string newTextImpr;

    public void newsTextImpr1()
    {
        oldTextImp1.text = newTextImpr;
    }

    public void newsTextImpr2()
    {
        oldTextImp2.text = newTextImpr;
    }
}