using UnityEngine;

public class AceptarButtonHandler : MonoBehaviour
{
    public PlayerGenerator playerGenerator;
    public RuleGenerator ruleGenerator;
    public DocumentGenerator documentGenerator;
    public GameManager gameManager;

    public void Aceptar()
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
        
        bool objetosValidos = true;
        foreach (var objeto in jugador.objetosProhibidos)
        {
            if (reglas.objetosProhibidos.Contains(objeto))
            {
                objetosValidos = false;
                Debug.Log($"Objeto prohibido encontrado: {objeto}");
                break;
            }
        }

        if (jugadorYDocumentoIguales && datosGeneralesCoincidenConReglas && delitoYProfesionValidos && objetosValidos)
        {
            Debug.Log(" Todo bien. Puede pasar.");
        }
        else
        {
            Debug.Log(" Amonestaciˇn: dejˇ pasar a alguien con datos incorrectos.");
        }
        playerGenerator.GenerarNuevoJugador();
        documentGenerator.GenerarDocumentos(jugador);
        gameManager.PersonaAtendida();
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


