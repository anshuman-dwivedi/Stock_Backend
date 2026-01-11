using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Mappers;
using api.Dtos.Comment.Stock;
using api.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using api.Helpers;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;

        public StockController(ApplicationDBContext context,IStockRepository stockRepo){

             _context = context;
             _stockRepo=stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]QueryObject query){
              if (!ModelState.IsValid)
                return BadRequest(ModelState);

             var stock =await  _stockRepo.GetAllAsync(query);
             var stocks=stock.Select(s=>s.ToStockDto());
            return  Ok(stocks);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
              if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var stock = await _stockRepo.GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateStockRequestDto stockDto)
        {
           if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = stockDto.ToStockFromCreate();

            await _stockRepo.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody]UpdateRequestDto updateDto)
        {
             if (!ModelState.IsValid)
                return BadRequest(ModelState); 

            var stockModel = await _stockRepo.UpdateAsync(id, updateDto);

            if (stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToStockDto());
        }
[HttpDelete]
[Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 

            var stockModel = await _stockRepo.DeleteAsync(id);

            if (stockModel == null)
            {
                return NotFound();
            }

            return NoContent();

        }
    
    }
}
/* "userName": "Anshuman",
  "email": "anshuman@example.com",
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImFuc2h1bWFuQGV4YW1wbGUuY29tIiwiZ2l2ZW5fbmFtZSI6IkFuc2h1bWFuIiwibmJmIjoxNzY3MDMzMjAyLCJleHAiOjE3Njc2MzgwMDIsImlhdCI6MTc2NzAzMzIwMn0.unZA5HFo9BdQ3_A5RjMrspFZDQ4ibDrdY2emHMxp7oGuNF9Azkxp8eEV2PF2kWvNxkzcwys-9rkqoISC2xYprQ" */