using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

public class FruitCounter : MonoBehaviour
{
    public static FruitCounter Instance { get; private set; }

    [Tooltip("Arrastra aquí tu UI Text para ver el conteo")]
    public Text counterText;

    //diccionario: tipo de fruta, cantidad cortada
    private readonly Dictionary<string,int> counts = new Dictionary<string,int>();

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        UpdateUI();
    }

    public void RegisterSlice(string fruitType)
    {
        if (string.IsNullOrEmpty(fruitType))
            fruitType = "Unknown";

        if (!counts.ContainsKey(fruitType))
            counts[fruitType] = 0;

        counts[fruitType]++;
        UpdateUI();
    }

    public void ResetCounts()
    {
        counts.Clear();
        UpdateUI();
    }

    //Texto para UI
    private void UpdateUI()
    {
        var sb = new StringBuilder();
        if (counts.Count == 0)
        {
            sb.Append(" ");
        }
        else
        {
            foreach (var kv in counts)
                sb.AppendLine($"  {kv.Key}: {kv.Value}"); //recorre el dictionario mostrando el tipo y la cantidad que se corto
        }
        counterText.text = sb.ToString();
    }
}