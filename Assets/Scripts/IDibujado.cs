using System.Collections;
using System.Collections.Generic;

public interface IDibujado
{
    void AsignarControlador(Controlador controlador);

    IEnumerator Dibuja(List<string> formula, ModoDibujado modoDibujado);
}
