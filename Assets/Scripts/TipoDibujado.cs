using System.Collections;
using System.Collections.Generic;

public abstract class TipoDibujado
{
    protected Controlador controlador;

    public TipoDibujado(Controlador controlador)
    {
        this.controlador = controlador;
    }

    public abstract IEnumerator Dibuja(List<string> formula, ModoDibujado modoDibujado);
}
