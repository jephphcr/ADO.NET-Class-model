using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.CodeDom;
using System.Collections;
using System.Runtime.Remoting;

namespace ADO.NET_Class
{
    public class DataAccess
    {
        #region Static Objects

        //Objeto Connection para obter acesso ao SQL Server
        public static SqlConnection sqlconnection;

        //Objeto SQLCommand para executar os comandos
        public static SqlCommand command;

        //Objeto SQLParameter para executar os parâmetros necessários nas consultas
        public static SqlParameter parameter;

        #endregion

        #region Obtain SqlConnection

        public static SqlConnection Connection()
        {
            try
            {
                //We obtain configuration connection in WebConfig through configuration Manager    
                string dataConnection = ConfigurationManager
                .ConnectionStrings["NomeDaConnectionString"].ConnectionString;

                //Instancing SqlConnection object
                sqlconnection = new SqlConnection(dataConnection);

                //Verify if connection is closed
                if (sqlconnection.State == ConnectionState.Closed)
                {
                    sqlconnection.Open();
                }

                //Return sqlConnection
                return sqlconnection;
            }

            catch (SqlException ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Open Connection
        public void Open()
        {
            sqlconnection.Open();
        }
        #endregion

        #region Close Connection
        public void Close()
        {
            sqlconnection.Close();
        }
        #endregion

        #region Add Parameters
        public void AddParameters(string name, SqlDbType type, int size, object value)
        {
            //Create the instance and insert the values

            parameter.ParameterName = name;
            parameter.SqlDbType = type;
            parameter.Size = size;
            parameter.Value = value;

            //Add to SQLCommand the parameter
            command.Parameters.Add(parameter);
        }

        #endregion

        #region Add Parameters
        public void AddParameters(string name, SqlDbType type, object value)
        {
            //Create the instance and insert the values

            parameter.ParameterName = name;
            parameter.SqlDbType = type;
            parameter.Value = value;

            //Add to SQLCommand the parameter
            command.Parameters.Add(parameter);
        }

        #endregion

        #region

        public void RemoveParameters(string pName)
        {
            //Verify if parameters exist
            if (command.Parameters.Contains(pName))
            {
                command.Parameters.Remove(pName);
            }
        }

        #endregion

        #region Clear Parameters
        public void ClearParameters()
        {
            command.Parameters.Clear();
        }
        #endregion

        #region Execute Query
        public DataTable ExecuteQuery(string sql)
        {
            try
            {
                //Use SQL Server configuration
                command.Connection = Connection();
                //Insert SQL instruction
                command.CommandText = sql;
                //Execute the query
                command.ExecuteScalar();
                //Read the data and transfer for a DataTable
                IDataReader dtreader = command.ExecuteReader();
                DataTable dtresult = new DataTable();
                dtresult.Load(dtreader);
                //Close the connection
                sqlconnection.Close();
                //Return the DataTable with query data
                return dtresult;
            }

            catch (Exception ex)
            {
                if (ex.Message.ToString().Contains("Networking"))
                {
                    throw new Exception("Network error");
                }
                else
                {
                    throw ex;
                }
            }

        }

        #endregion

        #region Execute a SQL instruction: INSERT, UPDATE AND DELETE
        public int ExecuteUpdate(string sql)
        {
            try
            {
                command.Connection = Connection();
                command.CommandText = sql;

                int result = command.ExecuteNonQuery();
                sqlconnection.Close();
                return result;
            }

            catch (Exception ex)
            {
                throw ex;
            }


        }
        #endregion


        DataAccess dataAcess = new DataAccess();
        public DataTable EfetuarConsultaPorCodigo(int codigo)
        {
            // Limpando parãmetros existentes
            dataAcess.ClearParameters();
            string SQL = @"SELECT
            c.NR_MATRICULA,
            c.DS_NOMECLIENTE AS NOME,
            c.DS_ESTADO AS ESTADO,
            c.DS_CIDADE AS CIDADE,
            c.DS_ENDERECO AS ENDERECO,
            c.DT_NASCIMENTO
            FROM CLIENTES c WHERE c.NR_MATRICULA = @NR_MATRICULA";
            // Adicionando o parâmetro para filtrar pelo código
            dataAcess.AddParameters("@NR_MATRICULA", SqlDbType.Int, codigo);
            // Retorna um DataTable com os dados da consulta
            return dataAcess.ExecuteQuery(SQL);
        }


        //Criar outras classes de negócios



    }



}
