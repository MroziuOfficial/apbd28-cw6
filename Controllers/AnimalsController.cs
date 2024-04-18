using apbd_cw06.Models;
using apbd_cw06.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace apbd_cw06.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IConfiguration _config;
    public AnimalsController(IConfiguration config)
    {
        _config = config;
    }
    
    [HttpGet]
    public IActionResult GetAnimals()
    {
        //otwieramy polaczenie do bazy
        SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
        conn.Open();
        //definicja commanda
        SqlCommand command = new SqlCommand();
        command.Connection = conn;
        command.CommandText = "SELECT * FROM Animal";
        
        //wykonanie commanda
        var reader = command.ExecuteReader();

        var animals = new List<Animal>();

        int IdAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        int NameOrdinal = reader.GetOrdinal("Name");

        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(IdAnimalOrdinal),
                Name = reader.GetString(NameOrdinal)
            });
        }
        
        
        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal(AddAnimalReq animal)
    {
        //otwieramy polaczenie do bazy
        SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
        conn.Open();
        //definicja commanda
        SqlCommand command = new SqlCommand();
        command.Connection = conn;
        command.CommandText = $"INSERT INTO Animal VALUES(@animalName, @animalDescription, @animalCategory, @animalArea)";
        command.Parameters.AddWithValue("@animalName", animal.Name);
        command.Parameters.AddWithValue("@animalDescription", animal.Description);
        command.Parameters.AddWithValue("@animalCategory", animal.Category);
        command.Parameters.AddWithValue("@animalArea", animal.Area);
        //wykonanie commanda
        command.ExecuteNonQuery();
        return Created("", null);
    }
    
    [HttpPut("{idAnimal:int}")]
    public IActionResult EditAnimal(int idAnimal, EditAnimalReq animal)
    {
        //otwieramy polaczenie do bazy
        SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
        conn.Open();
        //definicja commanda
        SqlCommand command = new SqlCommand();
        command.Connection = conn;
        command.CommandText = $"UPDATE Animal SET Name = @animalName, Description = @animalDescription, Category = @animalCategory, Area = @animalArea WHERE IdAnimal = @idAnimal";
        command.Parameters.AddWithValue("@idAnimal", idAnimal);
        command.Parameters.AddWithValue("@animalName", animal.Name);
        command.Parameters.AddWithValue("@animalDescription", animal.Description);
        command.Parameters.AddWithValue("@animalCategory", animal.Category);
        command.Parameters.AddWithValue("@animalArea", animal.Area);
        //wykonanie commanda
        command.ExecuteNonQuery();
        return Ok();
    }

    [HttpDelete("{idAnimal:int}")]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
        conn.Open();
        //definicja commanda
        SqlCommand command = new SqlCommand();
        command.Connection = conn;
        command.CommandText = $"DELETE FROM Animal WHERE IdAnimal = @idAnimal";
        command.Parameters.AddWithValue("@idAnimal", idAnimal);
        //wykonanie commanda
        command.ExecuteNonQuery();
        return Ok();
    }
}