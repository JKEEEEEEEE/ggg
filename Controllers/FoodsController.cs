﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kursach_diplom_api.Models;

namespace kursach_diplom_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly KurcachDiplomContext _context;

        public FoodsController(KurcachDiplomContext context)
        {
            _context = context;
        }

        // GET: api/Foods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoods()
        {
          if (_context.Foods == null)
          {
              return NotFound();
          }
            return await _context.Foods.ToListAsync();
        }

        // GET: api/Foods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> GetFood(int? id)
        {
          if (_context.Foods == null)
          {
              return NotFound();
          }
            var food = await _context.Foods.FindAsync(id);

            if (food == null)
            {
                return NotFound();
            }

            return food;
        }

        // PUT: api/Foods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFood(int? id, Food food)
        {
            if (id != food.IdFood)
            {
                return BadRequest();
            }

            _context.Entry(food).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Foods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Food>> PostFood(Food food)
        {
          if (_context.Foods == null)
          {
              return Problem("Entity set 'KurcachDiplomContext.Foods'  is null.");
          }
            _context.Foods.Add(food);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFood", new { id = food.IdFood }, food);
        }

        // DELETE: api/Foods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood(int? id)
        {
            if (_context.Foods == null)
            {
                return NotFound();
            }
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FoodExists(int? id)
        {
            return (_context.Foods?.Any(e => e.IdFood == id)).GetValueOrDefault();
        }
		// GET: api/Foods/Search?Dish=...
		[HttpGet("Search")]
		public async Task<ActionResult<IEnumerable<Food>>> SearchFoods(string dish)
		{
			if (string.IsNullOrWhiteSpace(dish))
			{
				return BadRequest();
			}

			var foods = await _context.Foods.Where(f => f.DishFood.Contains(dish)).ToListAsync();

			return foods;
		}

		// GET: api/Foods/Sort?sortBy=price&sortOrder=asc
		[HttpGet("Sort")]
		public async Task<ActionResult<IEnumerable<Food>>> SortFoods(string sortBy)
		{
			IQueryable<Food> query = _context.Foods;

			// Сортировка
			if (!string.IsNullOrWhiteSpace(sortBy))
			{
				switch (sortBy.ToLower())
				{
					case "price":
						query =  query.OrderBy(f => f.PriceFood);
						break;
						// Добавьте другие поля, по которым можно сортировать, при необходимости
				}
			}

			return await query.ToListAsync();
		}

		// GET: api/Foods/SortDescending?sortBy=price
		[HttpGet("SortDescending")]
		public async Task<ActionResult<IEnumerable<Food>>> SortFoodsDescending(string sortBy)
		{
			IQueryable<Food> query = _context.Foods;

			// Сортировка по убыванию
			if (!string.IsNullOrWhiteSpace(sortBy))
			{
				switch (sortBy.ToLower())
				{
					case "price":
						query = query.OrderByDescending(f => f.PriceFood);
						break;
						// Добавьте другие поля, по которым можно сортировать по убыванию, при необходимости
				}
			}

			return await query.ToListAsync();
		}

	}
}
