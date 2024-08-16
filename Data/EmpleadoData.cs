using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Data
{
    public class EmpleadoData
    {
        public List<Empleado> ListaEmpleado { get; set; }
        public bool respuesta { get; set; }
        //Una variable del topo de la clase ConnectionStrings
        private readonly ConnectionStrings conexiones;

        //CONSTRUCTOR => Lo primero que se ejecuta al hacer una instancia de la clase
        public EmpleadoData(IOptions<ConnectionStrings> options)
        {
            conexiones = options.Value;
        }

        public async Task<List<Empleado>> Getall()
        {
            ListaEmpleado = new List<Empleado>();
            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                //Abrir conexion a la base de datos
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_ObtenerEmpleados", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    //Mientras haya mas filas
                    while (reader.Read())
                    {
                        //Convertir el json de Departamento a su objeto
                        var departamentoString = reader["Departamento"].ToString();

                        ListaEmpleado.Add(new Empleado
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            NombreCompleto = Convert.ToString(reader["NombreCompleto"]),
                            Sueldo = Convert.ToDecimal(reader["Sueldo"]),
                            FechaContrato = (DateTime)reader["FechaContrato"],
                            Departamento = JsonConvert.DeserializeObject<Departamento>(departamentoString)
                            //Departamento = new Departamento
                            //{
                            //    Nombre = reader["Departamento"].ToString()
                            //}
                        });
                    }
                }

            }
            return ListaEmpleado;
        }

        public async Task<bool> InsertarEmpleado(EmpleadoDTO empleado)
        {

            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                //Abrir conexion a la base de datos
                //await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_InsertarEmpleado", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", empleado.NombreCompleto);
                cmd.Parameters.AddWithValue("@IdDepartamento", empleado.IdDepartamento);
                cmd.Parameters.AddWithValue("@Sueldo", empleado.Sueldo);

                //utiliza el bloque try-catch para iniciar la operacion de insertar
                try
                {
                    await conexion.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }


        public async Task<bool> EditarEmpleado(Empleado empleado)
        {

            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                //Abrir conexion a la base de datos
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_ActualizarEmpleado", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", empleado.Id);
                cmd.Parameters.AddWithValue("@Nombre", empleado.NombreCompleto);
                cmd.Parameters.AddWithValue("@IdDepartamento", empleado.IdDepartamento);
                cmd.Parameters.AddWithValue("@Sueldo", empleado.Sueldo);
                cmd.Parameters.AddWithValue("@FechaContrato", empleado.FechaContrato);
                cmd.Parameters.AddWithValue("@Eliminado", empleado.Eliminado);

                //utiliza el bloque try-catch para iniciar la operacion de insertar
                try
                {
                    await conexion.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Delete(long Id)
        {

            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                //Abrir conexion a la base de datos
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_EliminarEmpleado", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);

                //utiliza el bloque try-catch para iniciar la operacion de insertar
                try
                {
                    await conexion.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        //Clase para Insertar un nuevo Empleado
        public class EmpleadoDTO
        {
            public long Id { get; set; }
            public string NombreCompleto { get; set; }
            public long IdDepartamento { get; set; }
            public decimal Sueldo { get; set; }
            public DateTime FechaContrato { get; set; }
            public bool Eliminado { get; set; }
            //public Departamento? Departamento { get; set; }

        }

    }
}
