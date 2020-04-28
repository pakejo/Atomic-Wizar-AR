using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

/*
 * #########################################################################
 * #         CLASE ENCARGADA DE LAS CONSULTAS A LA BASE DE DATOS           #
 * #########################################################################
 */

public class SQLiteHelper
{
    private readonly IDbConnection dbConnection;

    /*
     * Constructor de la clase. Crea una conexion a la base de datos y la abre
     */

    public SQLiteHelper()
    {
        string filepath = Application.persistentDataPath + "/Test.db";

#if UNITY_EDITOR
        filepath = Application.streamingAssetsPath + "/Test.db";
#elif UNITY_ANDROID

        //if(!File.Exists(filepath))
       // {
            UnityWebRequest www = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/" + "Test.db");
            www.SendWebRequest();
            while(!www.isDone) { }

            File.WriteAllBytes(filepath, www.downloadHandler.data);
       // }
#endif

        string connection = "URI=file:" + filepath;
        dbConnection = new SqliteConnection(connection);
        dbConnection.Open();
    }

    /*
     * Destructor de la clase. Cierra la conexión existente
     */

    ~SQLiteHelper()
    {
        Debug.Log("Database connection finished");
        dbConnection.Close();
    }

    /*
     * Obtiene la informacion de la tabla asociada a un ID
     * Devuelve un string con la informacion asociada.
     * El string estará vacío si no hay ninguna entrada en la tabla con ese id
     */

    public List<string> GetInfoByID(string id)
    {
        List<string> info = new List<string>();
        IDbCommand dbcmd = dbConnection.CreateCommand();   // Para usar comandos SQL
        dbcmd.CommandText = "SELECT * FROM Elementos WHERE Simbolo = '" + id + "'";
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            info.Add(reader[0].ToString());   // Simbolo elemento
            info.Add(reader[1].ToString());   // Valencias
            info.Add(reader[2].ToString());   // Radio atomico
        }
        return info;
    }

    /*
     * Función que devuelve el material asociado a u elemento
     */

    public string GetMaterialOf(string element)
    {
        string material = "";
        IDbCommand dbcmd = dbConnection.CreateCommand();   // Para usar comandos SQL

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