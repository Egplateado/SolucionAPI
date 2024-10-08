﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Data;
using Modelo;
using static Data.EmpleadoData;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly EmpleadoData _empleadoData;

        public EmpleadoController(EmpleadoData empleadoData)
        {
            _empleadoData = empleadoData;
        }

        [HttpGet("Getall")]
        public async Task<IActionResult> GetAll()
        {
            List<Empleado> listaEmpleado = await _empleadoData.Getall();
            return StatusCode(StatusCodes.Status200OK, listaEmpleado);
        }


        [HttpPost]
        public async Task<IActionResult> Save([FromBody] EmpleadoDTO empleado)
        {
            bool insertar = await _empleadoData.InsertarEmpleado(empleado);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = insertar });
        }


        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Empleado empleado)
        {
            bool editar = await _empleadoData.EditarEmpleado(empleado);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = editar });
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            bool editar = await _empleadoData.Delete(Id);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = editar });
        }
    }
}
