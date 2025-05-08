using UnityEngine;
using TMPro;

public class DocumentGenerator : MonoBehaviour
{
    public RuleDatabase database;
    public TextMeshPro documentosTexto;
    [Range(0f, 1f)] public float probabilidadDocumentosCorrectos = 0.95f;

    public void GenerarDocumentos(ReglasGeneradas jugadorReal)
    {
        ReglasGeneradas documentos = new ReglasGeneradas
        {
            fecha = Coincide() ? jugadorReal.fecha : ElegirDiferente(database.fechasVencimiento, jugadorReal.fecha),
            region = Coincide() ? jugadorReal.region : ElegirDiferente(database.regionesProhibidas, jugadorReal.region),
            delito = Coincide() ? jugadorReal.delito : ElegirDiferente(database.delitosProhibidos, jugadorReal.delito),
            profesion = Coincide() ? jugadorReal.profesion : ElegirDiferente(database.profesionesProhibidas, jugadorReal.profesion),
            trabajo = Coincide() ? jugadorReal.trabajo : ElegirDiferente(database.tiposDeTrabajoPermitidos, jugadorReal.trabajo),
            dinero = Coincide() ? jugadorReal.dinero : jugadorReal.dinero - Random.Range(5, 20)
        };

        MostrarDocumentos(documentos);
    }

    void MostrarDocumentos(ReglasGeneradas d)
    {
        documentosTexto.text =
            " DOCUMENTOS PRESENTADOS:\n" +
            $"Pasaporte: {d.fecha}\n" +
            $"Región: {d.region}\n" +
            $"Delito: {d.delito}\n" +
            $"Dinero: ${d.dinero}\n" +
            $"Trabajo deseado: {d.trabajo}\n" +
            $"Profesión: {d.profesion}";
    }

    bool Coincide()
    {
        return Random.value <= probabilidadDocumentosCorrectos;
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
}

