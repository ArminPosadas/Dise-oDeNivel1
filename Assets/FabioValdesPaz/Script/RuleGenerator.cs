using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RuleGenerator : MonoBehaviour
{
    public RuleDatabase database;
    public TextMeshPro hojaTexto;
    public PlayerGenerator playerGenerator;


    [SerializeField] public ReglasGeneradas reglas;

    void Start()
    {
        GenerarNuevaRegla();
    }

    public void GenerarNuevaRegla()
    {
        GenerarHoja();
    }

    void GenerarHoja()
    {
        reglas = new ReglasGeneradas();

        string region = ElegirAleatorio(database.regionesProhibidas);
        string delito = ElegirAleatorio(database.delitosProhibidos);
        string profesion = ElegirAleatorio(database.profesionesProhibidas);
        string trabajo = ElegirAleatorio(database.tiposDeTrabajoPermitidos);
        string fecha = ElegirAleatorio(database.fechasVencimiento);
        int dinero = database.dineroMinimoRequerido;
        
        // Add this for prohibited objects
        List<string> objetosProhibidos = new List<string>();
        objetosProhibidos.Add(database.objetosProhibidos[0]); // Always include first item
        objetosProhibidos.Add(database.objetosProhibidos[1]); // Always include second item

        reglas.region = region;
        reglas.delito = delito;
        reglas.profesion = profesion;
        reglas.trabajo = trabajo;
        reglas.fecha = fecha;
        reglas.dinero = dinero;
        reglas.objetosProhibidos = objetosProhibidos; // Add this line

        hojaTexto.text =
            "Solo puede pasar la persona si sus papeles coinciden con los siguientes datos:\n\n" +
            $"- Fechas de pasaprte permitidas: {fecha}\n" +
            $"- Se haceptan de la Region: {region}\n" +
            $"- No debe tener antecedentes de: {delito}\n" +
            $"- Debe tener al menos ${dinero}\n" +
            $"- Tipo de trabajo que busca: {trabajo}\n" +
            $"- Profesiones prohibidas: {profesion}\n" + 
            $"- Objetos prohibidos: {string.Join(", ", objetosProhibidos)}"; // Add this line

        playerGenerator.GenerarNuevoJugador();
    }

    string ElegirAleatorio(System.Collections.Generic.List<string> lista)
    {
        if (lista.Count == 0) return "N/A";
        return lista[Random.Range(0, lista.Count)];
    }
}
