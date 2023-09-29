using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BiancasBikes.Data;
using Microsoft.EntityFrameworkCore;
using BiancasBikes.Models;

namespace BiancasBikes.Controllers;

[ApiController] // ApiController attribute: enables many API-specific behaviors
[Route("api/[controller]")] // route attribute: tells framework to use first part of controller name ("Bike") to create route. case-insensitive
public class BikeController : ControllerBase
{
    private BiancasBikesDbContext _dbContext; // private because this variable should only be accessed inside the class.


    public BikeController(BiancasBikesDbContext context) // constructor; requires 1 param of type BiancasBikesDbContext to be 
    //passed in when instance of class is created. This B.B.D.C instance is saved as the value of *private* field _dbContext
    //enabling the GET method to use it to get bikes from database.
    {
        _dbContext = context;
    }

    // constructor above *injects* an already-existing instance of BiancasBikesDbContext class for controller to use.
    // decouples the dependencies of BiancasBikesDbContext from those of BikeController.
    // When framework creates instance of BikeController to handle a bike request,
    // it will pass in an instance of B.B.D.C for BikeController to use.

    [HttpGet] // attribute defining the endpoint method. "Get" is default, so technically not needed.
    [Authorize] // endpoint can only be accessed if user is logged in. Comment out to test in Postman without logging in
    public IActionResult Get() // endpoint: get collection of all bikes
    {
        return Ok(_dbContext.Bikes.Include(b => b.Owner).ToList());
    }

    [HttpGet("{id}")] // adds id as necessary part of this handler's route: api/bike/{id}
    [Authorize]
    public IActionResult GetById(int id) // name inside {} above MUST match param here
    {
        Bike bike = _dbContext
            .Bikes
            .Include(b => b.Owner)
            .Include(b => b.BikeType)
            .Include(b => b.WorkOrders)
            .SingleOrDefault(b => b.Id == id);

        if (bike == null)
        {
            return NotFound();
        }

        return Ok(bike);
    }

    [HttpGet("inventory")]
    [Authorize]
    public IActionResult Inventory()
    {
        int inventory = _dbContext
        .Bikes
        .Where(b => b.WorkOrders.Any(wo => wo.DateCompleted == null))
        .Count();

        return Ok(inventory);
    }

}