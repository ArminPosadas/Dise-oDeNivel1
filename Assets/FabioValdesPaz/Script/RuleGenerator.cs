using UnityEngine;
using TMPro; 

public class RuleGenerator : MonoBehaviour
{
    public RuleDatabase database;
    public TextMeshPro hojaTexto;

    void Start()
    {
        GenerarHoja();
    }

    void GenerarHoja()
    {
        string region = ElegirAleatorio(database.regionesProhibidas);
        string delito = ElegirAleatorio(database.delitosProhibidos);
        string profesion = ElegirAleatorio(database.profesionesProhibidas);
        string trabajo = ElegirAleatorio(database.tiposDeTrabajoPermitidos);
        string fecha = ElegirAleatorio(database.fechasVencimiento);
        int dinero = database.dineroMinimoRequerido;

        hojaTexto.text =
            "Solo puede pasar la persona si sus papeles coinciden con los siguientes datos:\n\n" +
            $"- Fecha de vencimiento del pasaporte: antes del {fecha}\n" +
            $"- No debe venir de la región: {region}\n" +
            $"- No debe tener antecedentes de: {delito}\n" +
            $"- Debe tener al menos ${dinero}\n" +
            $"- Tipo de trabajo que busca: {trabajo}\n" +
            $"- Profesiones prohibidas: {profesion}";
    }

    string ElegirAleatorio(System.Collections.Generic.List<string> lista)
    {
        if (lista.Count == 0) return "N/A";
        return lista[Random.Range(0, lista.Count)];
    }
}

