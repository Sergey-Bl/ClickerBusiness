using TMPro;
using UnityEngine;

public class ChangeText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _oldTextImp1;
    [SerializeField] private TextMeshProUGUI _oldTextImp2;
    [SerializeField] private string newTextImpr;

    public void newsTextImpr1()
    {
        _oldTextImp1.text = newTextImpr;
    }

    public void newsTextImpr2()
    {
        _oldTextImp2.text = newTextImpr;
    }
}