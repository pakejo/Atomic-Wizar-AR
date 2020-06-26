using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

#if UNITY_ANDROID
using System.IO;
using UnityEngine.Networking;
#endif

/*
 * #########################################################################
 * #         CLASE ENCARGADA DE LAS CONSULTAS A LA BASE DE DATOS           #
 * #########################################################################
 */

public class SQLiteHelper
{
    private readonly IDbConnection conexionConLaBaseDeDatos;

    /*
     * Constructor de la clase. Crea una conexion a la base de datos y la abre
     */

    public SQLiteHelper()
    {
        string directorioDeLaBaseDeDatos = Application.persistentDataPath + "/Test.db";

#if UNITY_EDITOR
        directorioDeLaBaseDeDatos = Application.streamingAssetsPath + "/Test.db";

#elif UNITY_ANDROID

        //DateTime anteriorFechaModificacion = File.GetLastWriteTime(Application.persistentDataPath + "/Test.db");
        //DateTime ultimaFechaModificacion = File.GetLastWriteTime(Application.streamingAssetsPath + "/Test.db");

        //if (anteriorFechaModificacion < ultimaFechaModificacion)
        //{
            UnityWebRequest www = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/" + "Test.db");
            www.SendWebRequest();
            while(!www.isDone) { }

            File.WriteAllBytes(directorioDeLaBaseDeDatos, www.downloadHandler.data);
        //}
#endif

        string pathArchivoBaseDeDatos = "URI=file:" + directorioDeLaBaseDeDatos;
        conexionConLaBaseDeDatos = new SqliteConnection(pathArchivoBaseDeDatos);
        conexionConLaBaseDeDatos.Open();
    }

    /*
     * Destructor de la clase. Cierra la conexión existente
     */

    ~SQLiteHelper()
    {
        Debug.Log("Database connection finished");
        conexionConLaBaseDeDatos.Close();
    }

    /*
     * Obtiene la informacion de la tabla asociada a un ID
     * Devuelve un string con la informacion asociada.
     * El string estará vacío si no hay ninguna entrada en la tabla con ese id
     */

    public List<string> obtenerInformacionDeElementoConID(string id)
    {
        List<string> informacionRecogida = new List<string>();
        IDbCommand dbcmd = conexionConLaBaseDeDatos.CreateCommand();   // Para usar comandos SQL
        dbcmd.CommandText = "SELECT * FROM Elementos WHERE Simbolo = '" + id + "'";
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            informacionRecogida.Add(reader[0].ToString());   // Simbolo elemento
            informacionRecogida.Add(reader[1].ToString());   // Valencias
            informacionRecogida.Add(reader[2].ToString());   // Radio atomico
        }
        return informacionRecogida;
    }

    /*
     * Función que devuelve el material asociado a u elemento
     */

    public string ObtenerMaterialDelElemento(string element)
    {
        string material = "";
        IDbCommand dbcmd = conexionConLaBaseDeDatos.CreateCommand();   // Para usar comandos SQL

        if (element.Length > 2)
            dbcmd.CommandText = "SELECT material FROM Materiales WHERE ID = '" + element + "'";
        else
            dbcmd.CommandText = "SELECT material FROM Materiales WHERE Simbolo = '" + element + "'";

        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
            material += reader[0].ToString();

        return material;
    }
}