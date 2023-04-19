using TMPro;
using UnityEngine;

public class ChangeText : MonoBehaviour
{
    public TextMeshProUGUI oldTextImp1;
    public TextMeshProUGUI oldTextImp2;
    
    public string newTextImpr;

    public void newsTextImpr1()
    {
        oldTextImp1.text = newTextImpr;
    }

    public void newsTextImpr2()
    {
        oldTextImp2.text = newTextImpr;
    }
}