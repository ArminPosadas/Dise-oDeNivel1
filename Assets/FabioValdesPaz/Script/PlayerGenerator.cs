using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerGenerator : MonoBehaviour
{
    public RuleGenerator ruleGenerator;
    public RuleDatabase database;
    public ReglasGeneradas reglas;
    public TextMeshPro jugadorTexto;
    public DocumentGenerator documentGenerator;
    public SkeletonObjectManager skeletonManager;

    public ReglasGeneradas jugadorActual;

    [Range(0f, 1f)] public float probabilidadCoincidencia = 0.8f;
    [Range(0f, 1f)] public float probabilidadDelitoProfesion = 0.35f;

    //void Start()
    //{
    //    reglas = ruleGenerator.reglas;
    //    ReglasGeneradas jugador = GenerarJugadorConProbabilidad(reglas);
    //    MostrarJugador(jugador);
    //}
    
    List<string> GenerarObjetosProhibidos()
    {
        List<string> objetos = new List<string>();
        int cantidadObjetos = Random.Range(0, 3); // 0 to 2 objects
        
        for (int i = 0; i < cantidadObjetos; i++)
        {
            int randomIndex = Random.Range(2, database.objetosProhibidos.Count);
            objetos.Add(database.objetosProhibidos[randomIndex]);
        }
        
        return objetos;
    }

    public void GenerarNuevoJugador()
    {
        reglas = ruleGenerator.reglas;
        jugadorActual = GenerarJugadorConProbabilidad(reglas);
        MostrarJugador(jugadorActual);
        documentGenerator.GenerarDocumentos(jugadorActual);
        skeletonManager.SetupWithObjects(jugadorActual.objetosProhibidos);
    }

    ReglasGeneradas GenerarJugadorConProbabilidad(ReglasGeneradas reglas)
    {
        return new ReglasGeneradas
        {
            fecha = Coincide() ? reglas.fecha : ElegirDiferente(database.fechasVencimiento, reglas.fecha),
            region = Coincide() ? reglas.region : ElegirDiferente(database.regionesProhibidas, reglas.region),
            delito = CoincideConProbabilidad(probabilidadDelitoProfesion) ? reglas.delito : ElegirDiferente(database.delitosProhibidos, reglas.delito),
            profesion = CoincideConProbabilidad(probabilidadDelitoProfesion) ? reglas.profesion : ElegirDiferente(database.profesionesProhibidas, reglas.profesion),
            trabajo = Coincide() ? reglas.trabajo : ElegirDiferente(database.tiposDeTrabajoPermitidos, reglas.trabajo),
            dinero = Coincide() ? reglas.dinero : reglas.dinero - Random.Range(10, 50),
            objetosProhibidos = GenerarObjetosProhibidos()
        };
    }

    void MostrarJugador(ReglasGeneradas r)
    {
        Debug.Log("Mostrando jugador");
        jugadorTexto.text =
            " DATOS DEL JUGADOR:\n" +
            $"Pasaporte: {r.fecha}\n" +
            $"Región: {r.region}\n" +
            $"Delito: {r.delito}\n" +
            $"Dinero: ${r.dinero}\n" +
            $"Trabajo deseado: {r.trabajo}\n" +
            $"Profesión: {r.profesion}";
        
        if (r.objetosProhibidos.Count > 0)
        {
            Debug.Log($"Objetos que lleva el jugador: {string.Join(", ", r.objetosProhibidos)}");
        }
        else
        {
            Debug.Log("El jugador no lleva objetos prohibidos");
        }
    }

    string ElegirDiferente(System.Collections.Generic.List<string> lista, string excluido)
    {
        if (lista.Count <= 1) return excluido;
        string elegido;
        do
        {
            elegido = lista[Random.Range(0, lista.Count)];
        } while (elegido == excluido);
        return elegido;
    }

    bool Coincide()
    {
        return Random.value <= probabilidadCoincidencia;
    }

    bool CoincideConProbabilidad(float probabilidad)
    {
        return Random.value <= probabilidad;
    }
}
