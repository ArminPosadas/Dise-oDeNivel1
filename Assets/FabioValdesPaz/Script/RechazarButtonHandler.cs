using UnityEngine;

public class RechazarButtonHandler : MonoBehaviour
{
    public PlayerGenerator playerGenerator;
    public RuleGenerator ruleGenerator;
    public DocumentGenerator documentGenerator;

    public void Rechazar()
    {
        var jugador = playerGenerator.jugadorActual;
        var reglas = ruleGenerator.reglas;
        var documento = documentGenerator.documentoActual;

        bool jugadorYDocumentoIguales = CompararReglas(jugador, documento);

        bool datosGeneralesCoincidenConReglas =
            jugador.fecha == reglas.fecha &&
            jugador.region == reglas.region &&
            jugador.trabajo == reglas.trabajo &&
            jugador.dinero == reglas.dinero;

        bool delitoYProfesionValidos =
            jugador.delito == documento.delito &&
            jugador.profesion == documento.profesion &&
            jugador.delito != reglas.delito &&
            jugador.profesion != reglas.profesion;

        if (jugadorYDocumentoIguales && datosGeneralesCoincidenConReglas && delitoYProfesionValidos)
        {
            Debug.Log(" Amonestación: rechazaste a alguien que sí podía pasar.");
        }
        else
        {
            Debug.Log(" Bien. Rechazaste correctamente a alguien con datos inválidos.");
        }
    }

    private bool CompararReglas(ReglasGeneradas a, ReglasGeneradas b)
    {
        return
            a.fecha == b.fecha &&
            a.region == b.region &&
            a.delito == b.delito &&
            a.profesion == b.profesion &&
            a.trabajo == b.trabajo &&
            a.dinero == b.dinero;
    }
}



