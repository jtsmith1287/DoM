using UnityEngine;
using System.Collections;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class DatabaseManager : MonoBehaviour {

	// Use this for initialization
	void Start() {
		string conn = "URI=file:" + Application.dataPath + "/Databases/Craftables.db"; //Path to database.
		IDbConnection dbconn = (IDbConnection)new SqliteConnection(conn);
		dbconn.Open(); //Open connection to the database.
		IDbCommand dbcmd = dbconn.CreateCommand();
		string sqlQuery = "SELECT * " + "FROM Blueprints";
		dbcmd.CommandText = sqlQuery;
		IDataReader reader = dbcmd.ExecuteReader();
		while (reader.Read()) {
			string name = reader.GetString(0);
			int softwood = reader.GetInt32(1);

			Debug.LogError(string.Format("Name: {0}, Softwood: {1}", name, softwood));
		}
		reader.Close();
		reader = null;
		dbcmd.Dispose();
		dbcmd = null;
		dbconn.Close();
		dbconn = null;
	}
}