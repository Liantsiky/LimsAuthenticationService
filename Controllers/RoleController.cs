using Microsoft.AspNetCore.Mvc;
using LimsAuthenticationService.Models;
using Microsoft.EntityFrameworkCore;
using LimsAuthenticationService.Services;
using LimsAuthenticationService.Utils;

namespace LimsEmployeService.Controllers;

[ApiController]
[Route("api/role")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("all")]
    // [Route("/api/role/all")]
    public async Task<ActionResult<ApiResponse>> GetAllRoles()
    {
        List<Role> roles = await _roleService.GetRoles();
        return Ok(new ApiResponse
        {
            Data = roles,
            ViewBag = null,
            IsSuccess = true,
            Message = "Datas retrieved successfully.",
            StatusCode = 200
        });
    }
        //TODO: CRUD role
    [HttpGet]
    public async Task<ActionResult> GetRole(int position, int pageSize)
    {
        if (position == 0) position = 1;
        if (pageSize == 0) pageSize = 2;
        Dictionary<string, object> response = new Dictionary<string, object>();
        int totalRoleRows = await _roleService.CountRoles();
        response["nbrPerPage"] = pageSize;
        response["TotalCount"] = totalRoleRows;
        response["nbrLinks"] = Math.Ceiling((double)totalRoleRows / pageSize);

            response["position"] = position;
            int skiped = (position-1) * pageSize;
            List<Role> roles = await _roleService.GetRolesFrom(skiped, pageSize);
            return Ok(new ApiResponse
            {
                Data = roles,
                ViewBag = response,
                IsSuccess = true,
                Message = "Datas retrieved successfully.",
                StatusCode = 200
            });
    }

    // GET: api/role/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Role>> GetRoleDetails(int id)
    {
        Role role = await _roleService.GetRole(id);
        if(role == null) return NotFound();

        return Ok(new ApiResponse
        {
            Data = role,
            ViewBag = null,
            IsSuccess = true,
            Message = "Data retrieved successfully",
            StatusCode = 200
        });
    }

    // POST: api/role
    [HttpPost]
    public async Task<ActionResult<Role>> CreateRole(Role role)
    {
        Dictionary<string, object> response = new Dictionary<string, object>();
        Role createdEmploye = await _roleService.CreateRole(role);
        return CreatedAtAction(nameof(GetRole), new { id = createdEmploye}, new ApiResponse
        {
            Data = role,
            ViewBag = null,
            IsSuccess = true,
            Message = "Created successfully",
            StatusCode = 201
        });
    }

    // PUT: api/role/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(int? id, Role role)
    {
        // Console.Write(role);
        if(id == null) return NotFound();
        Role updatedRole =await _roleService.EditRole(role);
        return CreatedAtAction(nameof(GetRole), new { id = updatedRole.IdRole }, new ApiResponse
        {
            Data = updatedRole,
            ViewBag = null,
            IsSuccess = true,
            Message = "Created successfully",
            StatusCode = 201
        });
        
    }

    //On ne doit pas supprimer un role

    // DELETE: api/role/5
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeleteRole(int id)
    // {
    //     await _roleService.DeleteRole(id);
    //     return NoContent();
    // }
}
