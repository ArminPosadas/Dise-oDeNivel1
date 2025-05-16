using UnityEngine;

public class GameManager : MonoBehaviour
{
    public RuleGenerator ruleGenerator;
    public PlayerGenerator playerGenerator;
    public DocumentGenerator documentGenerator;

    private int personasAtendidas = 0;

    public void PersonaAtendida()
    {
        personasAtendidas++;

        if (personasAtendidas >= 10)
        {
            personasAtendidas = 0;
            ruleGenerator.SendMessage("Start");
            Debug.Log("Nueva regla generada tras atender 10 personas.");
        }

        // Generar nuevo jugador y documento después de aceptar o rechazar
        playerGenerator.GenerarNuevoJugador();
        var nuevoJugador = playerGenerator.jugadorActual;
        documentGenerator.GenerarDocumentos(nuevoJugador);
    }
}

