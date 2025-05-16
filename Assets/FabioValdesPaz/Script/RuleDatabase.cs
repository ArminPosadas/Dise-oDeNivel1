using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleDatabase", menuName = "GameData/RuleDatabase")]
public class RuleDatabase : ScriptableObject
{
    public List<string> regionesProhibidas;
    public List<string> delitosProhibidos;
    public List<string> profesionesProhibidas;
    public List<string> tiposDeTrabajoPermitidos;
    public List<string> fechasVencimiento;
    public int dineroMinimoRequerido = 500;
}

